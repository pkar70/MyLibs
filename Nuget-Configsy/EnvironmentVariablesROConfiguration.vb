

Imports MsExtConf = Microsoft.Extensions.Configuration

' Difference with Microsoft's implementation:
' a) Microsoft requires .Net Standard 2.0, so it cannot be used on phones
' b) here, .Set is converted to .Remove, important escpecially when used in pack with my others ConfigurationProviders which react for [roam] prefix

Public Class EnvironmentVariablesROConfigurationProvider
    Inherits MsExtConf.ConfigurationProvider

    Private ReadOnly _sPrefix As String
    Private ReadOnly _oDict As System.Collections.IDictionary

    ' używa tylko tych z prefiksem (dla app), oraz tu podane
    Private ReadOnly _AlwaysCopy As String = "|COMPUTERNAME|USERNAME|"

    Public Overrides Sub Load()

        For Each sVariable As DictionaryEntry In _oDict
            Dim sKey As String = sVariable.Key.ToString.ToLower
            Dim sVal As String = sVariable.Value.ToString
            If sKey.StartsWith(_sPrefix, StringComparison.Ordinal) Then
                sKey = sKey.Substring(_sPrefix.Length)
                Data(sKey) = sVal
            ElseIf sKey.StartsWith("_") Then
                Data(sKey.Substring(1)) = sVal
            ElseIf _AlwaysCopy.Contains("|" & sKey & "|") Then
                Data(sKey) = sVal
            End If
        Next

    End Sub


    Public Overrides Sub [Set](key As String, value As String)
        ' specjalnie nie zapisuje nowej wartosci, a nawet usuwa - żeby nie śmiecić
        Data.Remove(key)
    End Sub

    Public Sub New(sPrefix As String, oDict As System.Collections.IDictionary)
        _sPrefix = sPrefix.ToLower
        _oDict = oDict
    End Sub

End Class

Public Class EnvironmentVariablesROConfigurationSource
    Implements MsExtConf.IConfigurationSource

    Private ReadOnly _sPrefix As String
    Private ReadOnly _oDict As IDictionary(Of String, String)

    Public Function Build(builder As MsExtConf.IConfigurationBuilder) As MsExtConf.IConfigurationProvider Implements MsExtConf.IConfigurationSource.Build
        Return New EnvironmentVariablesROConfigurationProvider(_sPrefix, _oDict)
    End Function

    Public Sub New(sPrefix As String, oDict As System.Collections.IDictionary)
        _sPrefix = sPrefix

        _oDict = New Dictionary(Of String, String)
        For Each oItem As DictionaryEntry In oDict
            _oDict(oItem.Key) = oItem.Value
        Next

    End Sub

End Class


