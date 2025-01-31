
Public Class AdresPL
    Inherits BaseStruct
    Implements IObiekty

    Public Property Dated As Date
    Public Property Wojewodztwo As String
    Public Property Powiat As String
    Public Property Gmina As String
    Public Property Miejscowosc As String
    Public Property Ulica As String
    Public Property Dom As String
    Public Property Lokal As String
    Public Property Zip As String

    ''' <summary>
    ''' np. "klatka 3", "wejście od podwórka" itp.
    ''' </summary>
    ''' <returns></returns>
    Public Property InfoDod As String

    Private Shared Property _emptyZip As New KodPocztowy("")

    'Public Sub LoadCache(dir As String) Implements IObiekty.LoadCache
    '    _emptyZip.LoadCache(dir)
    'End Sub

    ''' <summary>
    ''' sprawdza czy jest podany kod, oraz gdy może - to czy kod odpowiada rzeczywistości (z dokładnością do ulicy). Używa cache z KodPocztowy
    ''' </summary>
    ''' <param name="useonline"></param>
    ''' <returns></returns>
    Public Function IsValid(Optional useonline As Boolean = False) As Boolean Implements IObiekty.IsValid
        If String.IsNullOrWhiteSpace(Zip) Then Return False
        ' województwo mamy zesłownikowane, więc możemy sprawdzić czy jest w słowniku
        If Not New Wojewodztwo(Wojewodztwo).IsValid Then Return False

        Dim kod As New KodPocztowy(Zip)

        Dim bck = kod.AsAdresPL(True, False)
        bck.Wait()
        Dim adres As AdresPL = bck.Result

        If adres Is Nothing Then
            ' jeśli nie ma w cache, i nie wolno sprawdzać online, to załóż że OK
            If Not useonline Then Return True

            bck = kod.AsAdresPL(True, True)
            bck.Wait()
            adres = bck.Result
        End If

        If Wojewodztwo <> adres.Wojewodztwo Then Return False
        If Miejscowosc <> adres.Miejscowosc Then Return False

        If String.IsNullOrWhiteSpace(adres.Ulica) Then Return True

        Return Ulica = adres.Ulica
    End Function



    Public Function IsCacheable() As Boolean Implements IObiekty.IsCacheable
        Return True
    End Function

    Public Function HasRegexp() As Boolean Implements IObiekty.HasRegexp
        Return False
    End Function

    Public Function GetRegexp() As String Implements IObiekty.GetRegexp
        Throw New NotImplementedException()
    End Function

    Public Shared Operator =(ByVal val1 As AdresPL, ByVal val2 As AdresPL) As Boolean
        If Not val1.Wojewodztwo.EqualsCI(val2.Wojewodztwo) Then Return False
        If Not val1.Miejscowosc.EqualsCI(val2.Miejscowosc) Then Return False
        If Not val1.Ulica.EqualsCI(val2.Ulica) Then Return False
        If Not val1.Dom.EqualsCI(val2.Dom) Then Return False
        If Not val1.Lokal.EqualsCI(val2.Lokal) Then Return False

        If Not String.IsNullOrWhiteSpace(val1.Zip) AndAlso Not String.IsNullOrWhiteSpace(val1.Zip) Then
            If Not val1.Zip.EqualsCI(val2.Zip) Then Return False
        End If

        If Not String.IsNullOrWhiteSpace(val1.Powiat) AndAlso Not String.IsNullOrWhiteSpace(val1.Powiat) Then
            If Not val1.Powiat.EqualsCI(val2.Powiat) Then Return False
        End If

        If Not String.IsNullOrWhiteSpace(val1.Gmina) AndAlso Not String.IsNullOrWhiteSpace(val1.Gmina) Then
            If Not val1.Gmina.EqualsCI(val2.Gmina) Then Return False
        End If


        Return True
    End Operator

    Public Shared Operator <>(ByVal val1 As AdresPL, ByVal val2 As AdresPL) As Boolean
        Return Not val1 = val2
    End Operator

    Public Function GetDictionaryType() As DictionaryType Implements IObiekty.GetDictionaryType
        Return DictionaryType.OnlineVerifiable
    End Function
End Class
