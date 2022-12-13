

Imports MsExtConf = Microsoft.Extensions.Configuration



' Difference with Microsoft's implementation:
' a) no tree of values
' b) .Set is converted to .Remove, important escpecially when used in pack with my others ConfigurationProviders which react for [roam] prefix


Public Class CommandLineROConfigurationProvider
    Inherits MsExtConf.ConfigurationProvider

    Private ReadOnly _aArgs As List(Of String)

    Public Overrides Sub Load()
        Dim key, value As String

        Using enumerator As IEnumerator(Of String) = _aArgs.GetEnumerator()
            ' key1=value1 --key2=value2 /key3=value3 --key4 value4 /key5 value5
            While enumerator.MoveNext()

                Dim currentArg As String = enumerator.Current
                Dim bWasKeyPrefix As Boolean = True

                If currentArg.StartsWith("--", StringComparison.Ordinal) Then
                    currentArg = currentArg.Substring(2)
                ElseIf currentArg.StartsWith("-", StringComparison.Ordinal) Then
                    currentArg = currentArg.Substring(1)
                ElseIf currentArg.StartsWith("/", StringComparison.Ordinal) Then
                    currentArg = currentArg.Substring(1)
                Else
                    bWasKeyPrefix = False
                End If

                Dim separator = currentArg.IndexOf("=", StringComparison.Ordinal)

                If separator < 0 Then
                    ' nie ma '=', a więc następne powinno być wartością (--key4 value4 /key5 value5)
                    If Not bWasKeyPrefix Then
                        ' If there is neither equal sign nor prefix in current argument, it is an invalid format
                        ' Ignore invalid formats
                        Continue While
                    End If

                    key = currentArg

                    If Not enumerator.MoveNext() Then
                        ' ignore missing values
                        Continue While
                    End If

                    value = enumerator.Current

                Else
                    ' jest '=', a więc: key1=value1 --key2=value2 /key3=value3
                    key = currentArg.Substring(0, separator)
                    value = currentArg.Substring(separator + 1)
                End If

                ' Override value when key is duplicated. So we always have the last argument win.
                Data(key) = value
            End While
        End Using

    End Sub


    Public Overrides Sub [Set](key As String, value As String)
        ' specjalnie nie zapisuje nowej wartosci, a nawet usuwa - żeby nie śmiecić
        Data.Remove(key)
    End Sub

    Public Sub New(aArgs As List(Of String))
        _aArgs = aArgs
    End Sub

End Class

Public Class CommandLineROConfigurationSource
    Implements MsExtConf.IConfigurationSource

    Private ReadOnly _aArgs As List(Of String)

    Public Function Build(builder As MsExtConf.IConfigurationBuilder) As MsExtConf.IConfigurationProvider Implements MsExtConf.IConfigurationSource.Build
        Return New CommandLineROConfigurationProvider(_aArgs)
    End Function

    Public Sub New(aArgs As List(Of String))
        _aArgs = aArgs
    End Sub

End Class

