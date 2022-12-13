
Imports MsExtConf = Microsoft.Extensions.Configuration

' Difference with Microsoft's implementation:
' a) ctor uses not file name, but file content - e.g. on Android we have no files extracted from installation package
' b) .Set is converted to .Remove, important escpecially when used in pack with my others ConfigurationProviders which react for [roam] prefix

Public Class IniDefaultsConfigurationProvider
    Inherits MsExtConf.ConfigurationProvider

    Private _sIniContent As String
    Private _bUseDebug As Boolean

    Public Overrides Sub Load()
        ' load settings
        If _sIniContent = "" Then Return ' nie ma pliku, pewnie Android (wersja bez Init)

        Dim aFileContent As String() = _sIniContent.Split(vbCrLf, options:=StringSplitOptions.RemoveEmptyEntries)
        LoadSection(aFileContent, "main")
        If _bUseDebug Then LoadSection(aFileContent, "debug")

    End Sub

    Public Overrides Sub [Set](key As String, value As String)
        ' specjalnie nie zapisuje nowej wartosci, a nawet usuwa - żeby nie śmiecić
        Data.Remove(key)
    End Sub

    Private Sub LoadSection(ByRef aArray As String(), sSection As String)
        Dim bInSection As Boolean = False

        sSection = "[" & sSection.ToLower & "]"

        For Each sLine In aArray
            Dim sLineTrim As String = sLine.Trim

            If sLineTrim.StartsWith("[", StringComparison.Ordinal) AndAlso
                sLineTrim.EndsWith("]", StringComparison.Ordinal) Then

                If sLineTrim.ToLower = sSection Then
                    bInSection = True
                Else
                    bInSection = False
                End If
            Else
                If bInSection Then
                    If sLineTrim.StartsWith("#", StringComparison.Ordinal) Then Continue For
                    If sLineTrim.StartsWith("'", StringComparison.Ordinal) Then Continue For
                    If sLineTrim.StartsWith(";", StringComparison.Ordinal) Then Continue For
                    If sLineTrim.StartsWith("//", StringComparison.Ordinal) Then Continue For

                    Dim iInd As Integer = sLineTrim.IndexOf(" # ")
                    If iInd > 0 Then sLineTrim = sLineTrim.Substring(0, iInd)

                    iInd = sLineTrim.IndexOf(" ' ")
                    If iInd > 0 Then sLineTrim = sLineTrim.Substring(0, iInd)

                    iInd = sLineTrim.IndexOf(" ; ")
                    If iInd > 0 Then sLineTrim = sLineTrim.Substring(0, iInd)

                    iInd = sLineTrim.IndexOf(" // ")
                    If iInd > 0 Then sLineTrim = sLineTrim.Substring(0, iInd)

                    iInd = sLineTrim.IndexOf(vbTab & "# ")
                    If iInd > 0 Then sLineTrim = sLineTrim.Substring(0, iInd)

                    iInd = sLineTrim.IndexOf(vbTab & "' ")
                    If iInd > 0 Then sLineTrim = sLineTrim.Substring(0, iInd)

                    iInd = sLineTrim.IndexOf(vbTab & "; ")
                    If iInd > 0 Then sLineTrim = sLineTrim.Substring(0, iInd)

                    iInd = sLineTrim.IndexOf(vbTab & "// ")
                    If iInd > 0 Then sLineTrim = sLineTrim.Substring(0, iInd)

                    Dim aKeyVal As String() = sLineTrim.Split("=")
                    If aKeyVal.Length = 2 Then
                        Data(aKeyVal(0).Trim) = aKeyVal(1).Trim
                    Else
                        Debug.WriteLine("IniDefaultsConfigurationProvider.LoadSection: unrecognized line: " & sLineTrim)
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub New(sIniContent As String, bUseDebug As Boolean)
        _sIniContent = sIniContent
        _bUseDebug = bUseDebug
    End Sub

End Class

Public Class IniDefaultsConfigurationSource
    Implements MsExtConf.IConfigurationSource

    Private _sIniContent As String
    Private _bUseDebug As Boolean

    Public Function Build(builder As MsExtConf.IConfigurationBuilder) As MsExtConf.IConfigurationProvider Implements MsExtConf.IConfigurationSource.Build
        Return New IniDefaultsConfigurationProvider(_sIniContent, _bUseDebug)
    End Function

    Public Sub New(sIniContent As String, bUseDebug As Boolean)
        _sIniContent = sIniContent
        _bUseDebug = bUseDebug
    End Sub
End Class

