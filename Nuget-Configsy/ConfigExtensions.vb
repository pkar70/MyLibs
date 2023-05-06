Imports MsExtConf = Microsoft.Extensions.Configuration

Public Module Extensions
    ''' <summary>
    ''' Add JSON read/write configuration source
    ''' </summary>
    ''' <param name="configurationBuilder"></param>
    ''' <param name="sPathnameLocal">filename for local settings</param>
    ''' <param name="sPathnameRoam">filename for roaming settings (can be null)</param>
    ''' <param name="bReadOnly">True if .Set should be ignored (as in Microsoft implementation)</param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddJsonRwSettings(ByVal configurationBuilder As MsExtConf.IConfigurationBuilder, sPathnameLocal As String, sPathnameRoam As String, Optional bReadOnly As Boolean = False) As MsExtConf.IConfigurationBuilder
        configurationBuilder.Add(New JsonRwConfigurationSource(sPathnameLocal, sPathnameRoam, bReadOnly))
        Return configurationBuilder
    End Function

#If NETSTANDARD2_0_OR_GREATER Then
    ''' <summary>
    ''' Add JSON read/write configuration source
    ''' </summary>
    ''' <param name="configurationBuilder"></param>
    ''' <param name="bUseTemp">True if temporary (per session) file should be used</param>
    ''' <param name="bUseLocal">True if local file should be used (UWP: C:\Users\xxx\AppData\Local\Packages\xxx\LocalState, WPF: C:\Users\xxx\AppData\Local\xxx)</param>
    ''' <param name="bUseRoam">True if roaming file should be used (UWP: C:\Users\xxx\AppData\Local\Packages\xxx\RoamingState, WPF: C:\Users\xxx\AppData\Roaming\xxx)</param>
    ''' <param name="bReadOnly">True if .Set should be ignored (as in Microsoft implementation)</param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddJsonRwSettings(ByVal configurationBuilder As MsExtConf.IConfigurationBuilder, bUseTemp As Boolean, bUseLocal As Boolean, bUseRoam As Boolean, Optional bReadOnly As Boolean = False) As MsExtConf.IConfigurationBuilder
        configurationBuilder.Add(New JsonRwConfigurationSource(bUseTemp, bUseLocal, bUseRoam, bReadOnly))
        Return configurationBuilder
    End Function
#End If


    ''' <summary>
    ''' Add INI (read-only) configuration source
    ''' </summary>
    ''' <param name="configurationBuilder"></param>
    ''' <param name="sIniContent">Content of INI file</param>
    ''' <param name="bUseDebug">True if [debug] section is to be used ([main] is used always)</param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddIniReleaseDebugSettings(ByVal configurationBuilder As MsExtConf.IConfigurationBuilder, sIniContent As String, bUseDebug As Boolean) As MsExtConf.IConfigurationBuilder
        configurationBuilder.Add(New IniDefaultsConfigurationSource(sIniContent, bUseDebug))
        Return configurationBuilder
    End Function

    ''' <summary>
    ''' Add CommandLine (read-only) configuration source
    ''' </summary>
    ''' <param name="configurationBuilder"></param>
    ''' <param name="aArgs">Array of command line arguments (output from Environment.GetCommandLineArgs.ToList, unavailable in .Net Standard 1.4)</param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddCommandLineRO(ByVal configurationBuilder As MsExtConf.IConfigurationBuilder, aArgs As List(Of String)) As MsExtConf.IConfigurationBuilder
        configurationBuilder.Add(New CommandLineROConfigurationSource(aArgs))
        Return configurationBuilder
    End Function

    ''' <summary>
    ''' Add EnvironmentVariables (read-only) configuration source
    ''' </summary>
    ''' <param name="configurationBuilder"></param>
    ''' <param name="sPrefix">Only variables with names with this prefix would be used</param>
    ''' <param name="oDict">Dictionary of variables (output from Environment.GetEnvironmentVariables(), unavailable in .Net Standard 1.4)</param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function AddEnvironmentVariablesROConfigurationSource(ByVal configurationBuilder As MsExtConf.IConfigurationBuilder, sPrefix As String, oDict As System.Collections.IDictionary) As MsExtConf.IConfigurationBuilder
        configurationBuilder.Add(New EnvironmentVariablesROConfigurationSource(sPrefix, oDict))
        Return configurationBuilder
    End Function

End Module

