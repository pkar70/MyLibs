

Imports System.Text.RegularExpressions

Public Class PESEL
    Inherits ObiektyBaseString
    Implements IObiekty

    Public Sub New(tekst As String)
        MyBase.New(tekst)
    End Sub

    ''' <summary>
    ''' sprawdza długość, cyfrowość, poprawność daty, oraz poprawność sumy kontrolnej
    ''' </summary>
    Public Overrides Function IsValid(Optional useonline As Boolean = False) As Boolean Implements IObiekty.IsValid

        If Not MyBase.IsValid Then Return False
        If BirthDate() = Date.MinValue Then Return False
        Return VerifyChecksum()
    End Function

    Public Overrides Function HasRegexp() As Boolean Implements IObiekty.HasRegexp
        Return True
    End Function

    Public Overrides Function GetRegexp() As String Implements IObiekty.GetRegexp
        Return "[0-9][0-9][0-9][0-9][0-3][0-9][0-9][0-9][0-9][0-9][0-9]"
    End Function



    ''' <summary>
    ''' zwraca datę urodzenia, albo MinValue przy błędzie
    ''' </summary>
    Public Function BirthDate() As Date
        If v.Length < 6 Then Return Date.MinValue

        Dim syr As String = v.Substring(0, 2)
        Dim smn As String = v.Substring(2, 2)
        Dim sdy As String = v.Substring(4, 2)

        Dim yr, mn, dy As Integer
        If Not Integer.TryParse(syr, yr) Then Return Date.MinValue
        If Not Integer.TryParse(smn, mn) Then Return Date.MinValue
        If Not Integer.TryParse(sdy, dy) Then Return Date.MinValue

        yr += 1900

        ' tylko XIX wiek
        While mn > 80
            mn += 20
            yr -= 100
        End While

        ' wiek XX-XXII
        While mn > 20
            mn -= 20
            yr += 100
        End While

        ' dwie własne próby, choć i tak one potem są powtarzane w new date
        If mn < 1 OrElse mn > 13 Then Return Date.MinValue
        If dy < 1 OrElse dy > 31 Then Return Date.MinValue

        Try
            Return New Date(yr, mn, dy)
        Catch ex As Exception
            ' także gdy 30 lutego lub tym podobne
        End Try

        Return Date.MinValue

    End Function

    Private Function VerifyChecksum() As Boolean
        If v.Length <> 11 Then Return False
        Dim chksm As String = GenChksum()
        If chksm = "" Then Return False
        Return chksm = v.Substring(10)
    End Function

    Private Function GenChksum() As String
        If v.Length <> 11 Then Return ""

        ' *TODO* kontrola sumy kontrolnej
        Dim suma As Integer = 0
        suma += CyfraWaga(0, 1)
        suma += CyfraWaga(1, 3)
        suma += CyfraWaga(2, 7)
        suma += CyfraWaga(3, 9)
        suma += CyfraWaga(4, 1)
        suma += CyfraWaga(5, 3)
        suma += CyfraWaga(6, 7)
        suma += CyfraWaga(7, 9)
        suma += CyfraWaga(8, 1)
        suma += CyfraWaga(9, 3)

        If suma < 0 Then Return ""

        Dim modulo As Integer = suma Mod 10
        If modulo = 0 Then Return "0"

        Return (10 - modulo).ToString

    End Function

    Private Function CyfraWaga(pos As Integer, waga As Integer) As Integer
        Dim cyfra As String = v.Substring(pos, 1)
        Dim num As Integer
        If Not Integer.TryParse(cyfra, num) Then Return -1000
        Return num * waga
    End Function


    ''' <summary>
    ''' dla błędnego PESELa zwraca false
    ''' </summary>
    Public Function IsMale() As Boolean
        If v.Length < 10 Then Return False

        Dim sex As String = v.Substring(9, 1)
        Return sex = "1" OrElse sex = "3" OrElse sex = "5" OrElse sex = "7" OrElse sex = "9"

    End Function

    ''' <summary>
    ''' dla błędnego PESELa zwraca false
    ''' </summary>
    Public Function IsFemale() As Boolean
        If v.Length < 10 Then Return False
        Return Not IsMale()
    End Function

    ''' <summary>
    ''' zwraca symbol, domyślnie planetarny ♂/♀
    ''' </summary>
    Public Function SexIcon(Optional typ As IconType = IconType.Planet) As String

        Select Case typ
            Case IconType.Head
                Return If(IsMale(), "👨", "👩") ' 1F468, 1F469
            Case IconType.Restroom
                Return If(IsMale(), "🚹", "🚺") ' 1F6B9, 1F6BA
            Case IconType.WC
                Return If(IsMale(), "⛛", "⭘") ' 26DB, 2B58
            Case Else
                Return If(IsMale(), "♂", "♀") ' 2640, 2642

        End Select

    End Function

    ''' <summary>
    ''' zwraca pierwszy znaleziony PESEL (musi być poprawny), albo NULL
    ''' </summary>
    ''' <param name="tekst">tekst w którym trzeba szukać PESELa</param>
    Public Shared Function TryParse(tekst As String) As PESEL
        Dim macze As MatchCollection = Regex.Matches(tekst, New PESEL("").GetRegexp)

        For Each macz As Match In macze
            Dim ret As New PESEL(macz.Value)
            If ret.IsValid Then Return ret
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' zwraca wiek osoby w latach (z dni/365.24) 
    ''' </summary>
    Public Function GetAgeYears() As Double
        Return GetAge().TotalDays / 365.24 ' uwzględniając przestępne
    End Function

    ''' <summary>
    ''' zwraca wiek osoby 
    ''' </summary>
    Public Function GetAge() As TimeSpan
        Dim urodz As Date = BirthDate()
        If urodz = Date.MinValue Then Return TimeSpan.Zero
        Return (Date.Now - BirthDate())
    End Function



    ''' <summary>
    ''' sprawdza czy do NOW minęla podana liczba lat
    ''' </summary>
    Public Function IsAdult(Optional lat As Double = 18) As Boolean
        Dim urodz As Date = BirthDate()
        If urodz = Date.MinValue Then Return False

        Return (urodz.AddYears(lat) < Date.Now)

    End Function

    Public Overrides Function ToString() As String
        Return v
    End Function

    ''' <summary>
    ''' zwraca TRUE gdy są tej samej płci
    ''' </summary>
    Public Function SameSex(drugi As PESEL) As Boolean
        Return drugi.IsMale = IsMale()
    End Function

    ''' <summary>
    ''' zwraca TRUE gdy w tym samym dniu mają urodziny (miesiąc i dzień, bez roku)
    ''' </summary>
    Public Function SameBirthday(drugi As PESEL) As Boolean
        Dim d1 As Date = BirthDate()
        Dim d2 As Date = drugi.BirthDate
        If d1.Month <> d2.Month Then Return False
        Return d1.Day = d2.Day
    End Function


    Public Enum IconType
        ''' <summary>
        ''' Mars/Wenus "♂", "♀"
        ''' </summary>
        Planet
        ''' <summary>
        ''' Żelazo/Miedź "♂", "♀"
        ''' </summary>
        Metal
        ''' <summary>
        ''' główka 👨 / 👩
        ''' </summary>
        Head
        ''' <summary>
        ''' wypoczynek 🚹 / 🚺
        ''' </summary>
        Restroom
        ''' <summary>
        ''' ⛛ / ⭘ 
        ''' </summary>
        WC
    End Enum


End Class

Partial Public Module ObiektyExtensions


    ''' <summary>
    ''' stwórz obiekt PESEL z całości string - może wyjść niepoprawny
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    Public Function AsPESEL(ByVal s As String) As PESEL
        Return New PESEL(s)
    End Function
End Module
