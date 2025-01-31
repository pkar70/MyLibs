
Imports System.ComponentModel
Imports System.Net.Http
Imports System.Text.RegularExpressions
Imports pkar.DotNetExtensions

Public Class KodPocztowy
    Inherits ObiektyBaseString
    Implements IObiekty, IObiektCache

#Region "to co zawsze"

    Public Sub New(tekst As String)
        MyBase.New(tekst)
        'If Text.RegularExpressions.Regex.IsMatch(v, "[0-9][0-9][0-9][0-9][0-9]") Then
        'End If
    End Sub

    Public Overrides Function HasRegexp() As Boolean Implements IObiekty.HasRegexp
        Return True
    End Function

    Public Overrides Function GetRegexp() As String Implements IObiekty.GetRegexp
        Return "[0-9][0-9]-[0-9][0-9][0-9]"
    End Function


    ''' <summary>
    ''' zwraca pierwszy znaleziony kod pocztowy (musi być poprawny), albo NULL
    ''' </summary>
    ''' <param name="tekst">tekst w którym trzeba szukać kodu</param>
    Public Shared Function TryParse(tekst As String) As KodPocztowy
        Dim macze As MatchCollection = Regex.Matches(tekst, New KodPocztowy("").GetRegexp)

        For Each macz As Match In macze
            Dim ret As New KodPocztowy(macz.Value)
            If ret.IsValid Then Return ret
        Next

        Return Nothing
    End Function


#End Region


    Private _mozliwosci As List(Of CacheEntry)

    ''' <summary>
    ''' spróbuj rozwiązać zamienić na adres - TRUE gdy coś wiadomo, FALSE gdy nie
    ''' </summary>
    ''' <param name="useonline">Zgoda na użycie Internet service</param>
    ''' <returns></returns>
    Public Async Function TryResolveAsync(Optional useonline As Boolean = False) As Task(Of Boolean)

        _mozliwosci = New List(Of CacheEntry)
        If CacheIsLoaded() Then
            _mozliwosci = _cacheZipow.Where(Function(x) x.kod = v AndAlso x.CachedAt > GetOldestDateLimit()).ToList
        End If

        If _mozliwosci.Count < 1 AndAlso useonline Then _mozliwosci = Await GetFromInternetTask()

        Return _mozliwosci.Count > 0
    End Function

    Public Function GetSingleWojewodztwo() As String
        Dim cts = GetPossibleWojewodztwo()
        If cts.Count <> 1 Then Return ""
        Return cts(0)
    End Function

    Public Function GetPossibleWojewodztwo() As String()
        Return _mozliwosci.Select(Of String)(Function(x) x.wojewodztwo).Distinct.ToArray
    End Function

    Public Function GetSinglePowiat(Optional woj As String = "") As String
        Dim cts = GetPossiblePowiat(woj)
        If cts.Count <> 1 Then Return ""
        Return cts(0)
    End Function

    Public Function GetPossiblePowiat(Optional woj As String = "") As String()
        If woj <> "" Then
            Return _mozliwosci.Where(Function(x) x.wojewodztwo.EqualsCI(woj)).Select(Of String)(Function(x) x.powiat).Distinct.ToArray
        Else
            Return _mozliwosci.Select(Of String)(Function(x) x.powiat).Distinct.ToArray
        End If
    End Function

    Public Function GetSingleGmina(Optional pow As String = "") As String
        Dim cts = GetPossibleGmina(pow)
        If cts.Count <> 1 Then Return ""
        Return cts(0)
    End Function

    Public Function GetPossibleGmina(Optional pow As String = "") As String()
        If pow <> "" Then
            Return _mozliwosci.Where(Function(x) x.powiat.EqualsCI(pow)).Select(Of String)(Function(x) x.gmina).Distinct.ToArray
        Else
            Return _mozliwosci.Select(Of String)(Function(x) x.gmina).Distinct.ToArray
        End If
    End Function

    Public Function GetSingleMiejscowosc(Optional gmi As String = "") As String
        Dim cts = GetPossibleMiejscowosc(gmi)
        If cts.Count <> 1 Then Return ""
        Return cts(0)
    End Function

    Public Function GetPossibleMiejscowosc(Optional gmi As String = "") As String()
        If gmi <> "" Then
            Return _mozliwosci.Where(Function(x) x.gmina.EqualsCI(gmi)).Select(Of String)(Function(x) x.miejscowosc).Distinct.ToArray
        Else
            Return _mozliwosci.Select(Of String)(Function(x) x.miejscowosc).Distinct.ToArray
        End If

    End Function

    Public Function GetSingleUlica(Optional msc As String = "") As String
        Dim cts = GetPossibleUlica(msc)
        If cts.Count <> 1 Then Return ""
        Return cts(0)
    End Function

    Public Function GetPossibleUlica(Optional msc As String = "") As String()
        If msc <> "" Then
            Return _mozliwosci.Where(Function(x) x.miejscowosc.EqualsCI(msc)).Select(Of String)(Function(x) x.ulica).Distinct.ToArray
        Else
            Return _mozliwosci.Select(Of String)(Function(x) x.ulica).Distinct.ToArray
        End If

    End Function


    ''' <summary>
    ''' Zamień KodPocztowy na AdresPL
    ''' </summary>
    ''' <param name="onlyFirst">TRUE użyj pierwszego match; FALSE wypełnij tylko pola pewne</param>
    ''' <param name="useonline">TRUE można skorzystać z Internet</param>
    ''' <returns>AdrePL wypełniony kodem, woj, pow, gm, msc, ul</returns>
    Public Async Function AsAdresPL(onlyFirst As Boolean, Optional useonline As Boolean = False) As Task(Of AdresPL)
        If Not IsValid() Then Return Nothing

        If Not Await TryResolveAsync(useonline) Then Return Nothing    ' _mozliwosci Is Nothing OrElse _mozliwosci.Count < 1 Then Return Nothing

        If onlyFirst Then Return _mozliwosci(0).AsAdresPL

        Dim ret As New AdresPL With
            {
            .Zip = v,
            .Wojewodztwo = GetSingleWojewodztwo(),
            .Powiat = GetSinglePowiat(),
            .Gmina = GetSingleGmina(),
            .Miejscowosc = GetSingleMiejscowosc(),
            .Ulica = GetSingleUlica()
        }

        Return ret

    End Function


    Private Function GetFromInternet() As BaseList(Of CacheEntry)
        Dim robSe As Task(Of BaseList(Of CacheEntry)) = GetFromInternetTask()
        robSe.Wait()
        Return robSe.Result
    End Function

    Private Async Function GetFromInternetTask() As Task(Of BaseList(Of CacheEntry))

        ' curl -H 'Accept:application/json' 'http://kodpocztowy.intami.pl/api/01-111'

        Dim webcln As New HttpClient
        webcln.DefaultRequestHeaders.Accept.Clear()
        webcln.DefaultRequestHeaders.Accept.TryParseAdd("application/json")

        Dim str As String = Await webcln.GetStringAsync("http://kodpocztowy.intami.pl/api/" & v)

        Dim nowe As New BaseList(Of CacheEntry)(Nothing)
        Try
            nowe.Import(str)
        Catch ex As Exception
        End Try

        If nowe.Count < 1 Then Return Nothing

        If _cacheZipow IsNot Nothing Then
            nowe.ForEach(Sub(x)
                             x.CachedAt = Date.Now
                             _cacheZipow.Add(x)
                         End Sub)

            _cacheZipow.Save()
        End If

        Return nowe

    End Function

#Region "obsługa cache"

    Public Overrides Function IsCacheable() As Boolean Implements IObiekty.IsCacheable
        Return True
    End Function

    Private Shared _cacheZipow As BaseList(Of CacheEntry)
    ' 30-147 to 2 kB!
    Private Const CACHE_MAXSIZE As Integer = 100

    Public Sub CacheInit(fold As String) Implements IObiektCache.CacheInit
        _cacheZipow = New BaseList(Of CacheEntry)(fold, "kodpocztowy.json")
    End Sub

    ''' <summary>
    ''' automatycznie usunie z cache te które są przeterminowane
    ''' </summary>
    Public Sub CacheLoad() Implements IObiektCache.CacheLoad
        If _cacheZipow Is Nothing Then Throw New NotSupportedException("Użyj CacheInit przed CacheLoad - bo niby co mam władować?")

        _cacheZipow.Load()

        ' najpierw usuwamy stare
        Dim oldestOk As Date = GetOldestDateLimit()
        Dim zaStare As List(Of CacheEntry) = _cacheZipow.Where(Function(x) x.CachedAt < oldestOk).ToList

        For Each oStary As CacheEntry In zaStare
            _cacheZipow.Remove(oStary)
        Next

        '_cacheZipow = _cacheZipow.Where(Function(x) x.CachedAt > oldestOk).ToList

    End Sub

    Public Sub CacheSave(Optional autoTrim As Boolean = True) Implements IObiektCache.CacheSave
        If _cacheZipow Is Nothing Then Throw New NotSupportedException("Użyj CacheInit przed CacheSave - bo niby co mam zapisać?")

        If autoTrim Then CacheTrim()
        _cacheZipow.Save()
    End Sub

    Public Function CacheIsLoaded() As Boolean Implements IObiektCache.CacheIsLoaded
        If _cacheZipow Is Nothing Then Return False
        Return _cacheZipow.Count > 0
    End Function

    Public Sub CacheTrim() Implements IObiektCache.CacheTrim
        If _cacheZipow Is Nothing Then Return

        If _cacheZipow.Count < CACHE_MAXSIZE Then Return

        ' usuwamy, te które są najwcześniej dopisane
        Dim usunac As Integer = _cacheZipow.Count - CACHE_MAXSIZE

        While _cacheZipow.Count - CACHE_MAXSIZE > 0 AndAlso _cacheZipow.Count > 0
            ' musimy wszystkie wystąpienia danego kodu usunąć
            Dim kod As String = _cacheZipow.ElementAt(0).kod
            RemoveFromCache(kod)
        End While

    End Sub

    ''' <summary>
    ''' usuń podany kod z cache (np. gdy podejrzewamy że się zmieniło, a mamy świeżo w cache)
    ''' </summary>
    ''' <param name="kod"></param>
    Public Sub RemoveFromCache(kod As String)
        _cacheZipow = _cacheZipow.Where(Function(x) x.kod <> kod)
    End Sub


    Public Function CacheGetSize() As Integer Implements IObiektCache.CacheGetSize
        If _cacheZipow Is Nothing Then Return 0

        Return _cacheZipow.GetFileSize
    End Function

    Private Function GetOldestDateLimit() As Date
        Return Date.Now.AddYears(-1)
    End Function

#End Region
    Private Class CacheEntry
        Inherits BaseStruct

        Public Property kod As String
        ' Public Property nazwa As String
        Public Property miejscowosc As String
        Public Property ulica As String
        Public Property gmina As String
        Public Property powiat As String
        Public Property wojewodztwo As String
        Public Property dzielnica As String
        Public Property numeracja() As List(Of CacheZipNumeracja)

        Public Property CachedAt As Date


        Public Function AsAdresPL() As AdresPL
            Return New AdresPL With {
                    .Wojewodztwo = wojewodztwo,
                    .Miejscowosc = miejscowosc,
                    .Ulica = ulica,
                    .Zip = kod,
                    .Powiat = powiat,
                    .Gmina = gmina
                    }
        End Function
    End Class

    Private Class CacheZipNumeracja
            Public Property od As String
            Public Property _do As String
            Public Property parzystosc As String
        End Class



End Class
