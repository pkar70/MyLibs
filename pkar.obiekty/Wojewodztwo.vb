
Imports pkar.DotNetExtensions

Public Class Wojewodztwo
    Inherits ObiektyBaseString
    Implements IObiekty

    Public Sub New(tekst As String)
        MyBase.New(tekst)
    End Sub

    Public Overrides Function GetDictionaryType() As DictionaryType
        Return DictionaryType.Hardcoded
    End Function

    Public Overrides Function GetDictionary(Optional searchTerm As String = "") As String()
        Return _
        {
        "dolnośląskie",
        "kujawsko-pomorskie",
        "lubelskie",
        "lubuskie",
        "łódzkie",
        "małopolskie",
        "mazowieckie",
        "opolskie",
        "podkarpackie",
        "podlaskie",
        "pomorskie",
        "śląskie",
        "świętokrzyskie",
        "warmińsko-mazurskie",
        "wielkopolskie",
        "zachodniopomorskie"
        }.Where(Function(x) x.ContainsCI(searchTerm))
    End Function


    Public Shared Function FromRejestracja(rej As String) As Wojewodztwo
        If String.IsNullOrWhiteSpace(rej) Then Return New Wojewodztwo("UNKNOWN")

        Select Case rej.ToUpperInvariant
            Case "B"
                Return New Wojewodztwo("podlaskie")
            Case "C"
                Return New Wojewodztwo("kujawsko-pomorskie")
            Case "D", "V"
                Return New Wojewodztwo("dolnośląskie")
            Case "E"
                Return New Wojewodztwo("łódzkie")
            Case "F"
                Return New Wojewodztwo("lubuskie")
            Case "G", "X"
                Return New Wojewodztwo("pomorskie")
            Case "K", "J"
                Return New Wojewodztwo("małopolskie")
            Case "L"
                Return New Wojewodztwo("lubelskie")
            Case "N"
                Return New Wojewodztwo("warmińsko-mazurskie")
            Case "O"
                Return New Wojewodztwo("opolskie")
            Case "P", "M"
                Return New Wojewodztwo("wielkopolskie")
            Case "R", "Y"
                Return New Wojewodztwo("podkarpackie")
            Case "S", "I"
                Return New Wojewodztwo("śląskie")
            Case "T"
                Return New Wojewodztwo("świętokrzyskie")
            Case "W", "A"
                Return New Wojewodztwo("mazowieckie")
            Case "Z"
                Return New Wojewodztwo("zachodniopomorskie")
        End Select

        Return New Wojewodztwo("UNKNOWN")
    End Function


    Public Shared Function TryParse(str As String) As Wojewodztwo
        ' próba rozpoznania z
        ' PicSort "małopolskie » Kraków » Śródmieście-część miasta Kraków » Kraków"
        ' PicSort "Plac Dominikański, Stare Miasto, Kraków, województwo małopolskie, 31-006, Polska"

        For Each woj As String In New Wojewodztwo("").GetDictionary
            If str.ContainsCI(woj) Then Return New Wojewodztwo(woj)
        Next

        Return Nothing
    End Function

End Class


Partial Public Module ObiektyExtensions


    ''' <summary>
    ''' stwórz obiekt PESEL z całości string - może wyjść niepoprawny
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    Public Function AsWojewodztwo(ByVal s As String) As Wojewodztwo
        Return New Wojewodztwo(s)
    End Function
End Module