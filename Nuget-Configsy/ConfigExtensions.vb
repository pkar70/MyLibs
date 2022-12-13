Imports Microsoft.Extensions.Configuration

Partial Module Extensions
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddJsonRwSettings(ByVal configurationBuilder As IConfigurationBuilder, sPathnameLocal As String, sPathnameRoam As String, Optional bReadOnly As Boolean = False) As IConfigurationBuilder
        configurationBuilder.Add(New JsonRwConfigurationSource(sPathnameLocal, sPathnameRoam, bReadOnly))
        Return configurationBuilder
    End Function

    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddIniRelDebugSettings(ByVal configurationBuilder As IConfigurationBuilder, sIniContent As String, bUseDebug As Boolean) As IConfigurationBuilder
        configurationBuilder.Add(New IniDefaultsConfigurationSource(sIniContent, bUseDebug))
        Return configurationBuilder
    End Function

    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddCommandLineRO(ByVal configurationBuilder As IConfigurationBuilder, aArgs As List(Of String)) As IConfigurationBuilder
        configurationBuilder.Add(New CommandLineROConfigurationSource(aArgs))
        Return configurationBuilder
    End Function


    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddEnvironmentVariablesROConfigurationSource(ByVal configurationBuilder As IConfigurationBuilder, sPrefix As String, oDict As System.Collections.IDictionary) As IConfigurationBuilder
        configurationBuilder.Add(New EnvironmentVariablesROConfigurationSource(sPrefix, oDict))
        Return configurationBuilder
    End Function

End Module

