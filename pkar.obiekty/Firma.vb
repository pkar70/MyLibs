
Public Class Firma
    Inherits BaseStruct
    Implements IObiekty

    Public Property adres As AdresPL
    Public Property nip As String

    Public Property nazwa As String
    Public Property www As String
    Public Property krs As String



    Public Function GetDictionaryType() As DictionaryType Implements IObiekty.GetDictionaryType
        Return DictionaryType.OnlineVerifiable
    End Function

    Public Function IsValid(Optional useonline As Boolean = False) As Boolean Implements IObiekty.IsValid
        Throw New NotImplementedException()
    End Function



    Public Function IsCacheable() As Boolean Implements IObiekty.IsCacheable
        Throw New NotImplementedException()
    End Function

    Public Function HasRegexp() As Boolean Implements IObiekty.HasRegexp
        Throw New NotImplementedException()
    End Function

    Public Function GetRegexp() As String Implements IObiekty.GetRegexp
        Throw New NotImplementedException()
    End Function
End Class
