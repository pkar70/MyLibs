Imports MsExtConf = Microsoft.Extensions.Configuration

Public Module Extensions
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddJsonRwSettings(ByVal configurationBuilder As MsExtConf.IConfigurationBuilder, sPathnameLocal As String, sPathnameRoam As String, Optional bReadOnly As Boolean = False) As MsExtConf.IConfigurationBuilder
        configurationBuilder.Add(New JsonRwConfigurationSource(sPathnameLocal, sPathnameRoam, bReadOnly))
        Return configurationBuilder
    End Function

    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddIniReleaseDebugSettings(ByVal configurationBuilder As MsExtConf.IConfigurationBuilder, sIniContent As String, bUseDebug As Boolean) As MsExtConf.IConfigurationBuilder
        configurationBuilder.Add(New IniDefaultsConfigurationSource(sIniContent, bUseDebug))
        Return configurationBuilder
    End Function

    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddCommandLineRO(ByVal configurationBuilder As MsExtConf.IConfigurationBuilder, aArgs As List(Of String)) As MsExtConf.IConfigurationBuilder
        configurationBuilder.Add(New CommandLineROConfigurationSource(aArgs))
        Return configurationBuilder
    End Function


    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddEnvironmentVariablesROConfigurationSource(ByVal configurationBuilder As MsExtConf.IConfigurationBuilder, sPrefix As String, oDict As System.Collections.IDictionary) As MsExtConf.IConfigurationBuilder
        configurationBuilder.Add(New EnvironmentVariablesROConfigurationSource(sPrefix, oDict))
        Return configurationBuilder
    End Function

End Module

