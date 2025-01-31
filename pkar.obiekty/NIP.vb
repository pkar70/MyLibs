Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq

' były minusy: fizyczne 123-456-78-19, firmy 123-45-67-819


Public Class NIP
    Inherits ObiektyBaseString
    Implements IObiekty

    Public Sub New(tekst As String)
        ' teraz jest bez minusów
        MyBase.New(tekst.Replace("-", ""))

    End Sub


    Public Function GetDictionaryType() As DictionaryType Implements IObiekty.GetDictionaryType
        Return DictionaryType.OnlineVerifiable
    End Function

    Public Overrides Function IsValid(Optional useonline As Boolean = False) As Boolean Implements IObiekty.IsValid
        Return CheckChecksum()
    End Function


    Public Overrides Function IsCacheable() As Boolean Implements IObiekty.IsCacheable
        Throw New NotImplementedException()
    End Function

    Public Overrides Function HasRegexp() As Boolean Implements IObiekty.HasRegexp
        Return True
    End Function

    Public Overrides Function GetRegexp() As String Implements IObiekty.GetRegexp
        ' bez minusów, 10 cyfr
        Return "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
    End Function

    Private Function CheckChecksum() As Boolean
        If v.Length <> 10 Then Return False
        Return v.Substring(9, 1) = CalculateChecksum()
    End Function

    Private Function CalculateChecksum() As String
        If Not Regex.IsMatch(v, GetRegexp) Then Return " "

        Dim suma As Integer = 0
        suma += CyfraWaga(0, 6)
        suma += CyfraWaga(1, 5)
        suma += CyfraWaga(2, 7)
        suma += CyfraWaga(3, 2)
        suma += CyfraWaga(4, 3)
        suma += CyfraWaga(5, 4)
        suma += CyfraWaga(6, 5)
        suma += CyfraWaga(7, 6)
        suma += CyfraWaga(8, 7)

        If suma < 0 Then Return ""

        Dim modulo As Integer = suma Mod 11
        If modulo = 10 Then Return " "  ' error, taki nie ma prawa się zdarzyć

        Return modulo

    End Function

    Private Function CyfraWaga(pos As Integer, waga As Integer) As Integer
        Dim cyfra As String = v.Substring(pos, 1)
        Dim num As Integer
        If Not Integer.TryParse(cyfra, num) Then Return -1000
        Return num * waga
    End Function

    Public Function GetUrzSkarb() As String
        ' z pierwszych trzech cyfr
    End Function

    Public Function AsFirma() As Firma
        Dim ret As New Firma
        ret.nip = v


    End Function



End Class
