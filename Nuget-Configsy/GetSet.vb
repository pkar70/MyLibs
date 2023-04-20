
Imports Microsoft.Extensions.Configuration

Public Module GetSet

    Private _settingsGlobal As IConfigurationRoot
    Private Const ScopePrefix As String = "Pkar.Scope:"

#Region "Inits"

    ''' <summary>
    ''' initialization of library - fully customisable IConfigurationRoot
    ''' </summary>
    ''' <param name="settings"></param>
    <Obsolete("Use InitSettings (without 'Lib' prefix)")>
    Public Sub LibInitSettings(settings As IConfigurationRoot)
        _settingsGlobal = settings
    End Sub

    ''' <summary>
    ''' initialization of library - fully customisable IConfigurationRoot
    ''' </summary>
    ''' <param name="settings"></param>
    Public Sub InitSettings(settings As IConfigurationRoot)
        _settingsGlobal = settings
    End Sub


    ''' <summary>
    ''' initialization of library - my order of ConfigurationSources. Some parameters are required as these data is not available in .Net Standard 1.4
    ''' </summary>
    ''' <param name="sINIcontent">(1) INI source; Content of INI file, NULL or "" if you don't want to use it</param>
    ''' <param name="bIniUseDebug">True if [debug] section is to be used ([main] is used always)</param>
    ''' <param name="applicationName">(2) Environment variables; prefix for variables (for UWP, it should be Windows.ApplicationModel.Package.Current.DisplayName), NULL if you don't want to use this source</param>
    ''' <param name="dictionaryOfEnvVars">Dictionary of variables (output from Environment.GetEnvironmentVariables(), unavailable in .Net Standard 1.4)</param>
    ''' <param name="configSource">(3) additional source, can be UWPConfigurationSource; use NULL if you don't want it</param>
    ''' <param name="localJSONdirName">(4) JSON source; folder for local JSON file</param>
    ''' <param name="roamJSONdirNname">folder for roaming JSON file</param>
    ''' <param name="bJSONreadOnly">True if .Set should be ignored (as in Microsoft implementation)</param>
    ''' <param name="cmdLineArgs">(5) CmdLine source, Array of command line arguments (output from Environment.GetCommandLineArgs.ToList, unavailable in .Net Standard 1.4)</param>
    Public Sub InitSettings(sINIcontent As String, bIniUseDebug As Boolean,
                            applicationName As String, dictionaryOfEnvVars As System.Collections.IDictionary,
                            configSource As IConfigurationSource,
                            localJSONdirName As String, roamJSONdirNname As String, bJSONreadOnly As Boolean,
                            cmdLineArgs As List(Of String))

        Dim oBuilder As New ConfigurationBuilder()

        If Not String.IsNullOrWhiteSpace(sINIcontent) Then
            oBuilder = oBuilder.AddIniReleaseDebugSettings(sINIcontent, bIniUseDebug)
        End If

        If Not String.IsNullOrWhiteSpace(applicationName) AndAlso dictionaryOfEnvVars IsNot Nothing Then
            oBuilder = oBuilder.AddEnvironmentVariablesROConfigurationSource(applicationName, dictionaryOfEnvVars) ' Environment.GetEnvironmentVariables, Std 2.0
        End If

        If configSource IsNot Nothing Then
            oBuilder = oBuilder.Add(configSource)
        End If

        If Not String.IsNullOrWhiteSpace(localJSONdirName) OrElse Not String.IsNullOrWhiteSpace(roamJSONdirNname) Then
            oBuilder = oBuilder.AddJsonRwSettings(localJSONdirName, roamJSONdirNname, bJSONreadOnly)
        End If

        If cmdLineArgs IsNot Nothing Then oBuilder = oBuilder.AddCommandLineRO(cmdLineArgs)

        Dim settings As Microsoft.Extensions.Configuration.IConfigurationRoot = oBuilder.Build

        InitSettings(settings)
    End Sub

#If NETSTANDARD2_0_OR_GREATER Then
    ''' <summary>
    ''' initialization of library - my order of ConfigurationSources.
    ''' </summary>
    ''' <param name="sINIcontent">(1) INI source; Content of INI file, NULL or "" if you don't want to use it</param>
    ''' <param name="bIniUseDebug">True if [debug] section is to be used ([main] is used always)</param>
    ''' <param name="applicationName">(2) Environment variables; prefix for variables (for UWP, it should be Windows.ApplicationModel.Package.Current.DisplayName), NULL if you don't want to use this source</param>
    ''' <param name="configSource">(3) additional source, can be UWPConfigurationSource; use NULL if you don't want it</param>
    ''' <param name="localJSONdirName">(4) JSON source; folder for local JSON file</param>
    ''' <param name="roamJSONdirNname">folder for roaming JSON file</param>
    ''' <param name="bJSONreadOnly">True if .Set should be ignored (as in Microsoft implementation)</param>
    ''' <param name="bUseCmdLineArgs">(5) CmdLine source, True if should be used, False if not</param>
    Public Sub InitSettings(sINIcontent As String, bIniUseDebug As Boolean,
                            applicationName As String,
                            configSource As IConfigurationSource,
                            localJSONdirName As String, roamJSONdirNname As String, Optional bJSONreadOnly As Boolean = False,
                            Optional bUseCmdLineArgs As Boolean = True)

        Dim dictionaryOfEnvVars As System.Collections.IDictionary = Nothing
        If Not String.IsNullOrWhiteSpace(applicationName) Then dictionaryOfEnvVars = Environment.GetEnvironmentVariables()

        Dim cmdLineArgs As List(Of String) = Nothing
        If bUseCmdLineArgs Then cmdLineArgs = Environment.GetCommandLineArgs.ToList

        InitSettings(sINIcontent, bIniUseDebug,
                            applicationName, dictionaryOfEnvVars,
                            configSource,
                            localJSONdirName, roamJSONdirNname, bJSONreadOnly,
                            cmdLineArgs)

    End Sub

    Private Sub SettingsCheckInit()
        If _settingsGlobal IsNot Nothing Then Return

        Debug.WriteLine("Settings calls without LibInitSettings, using only temporary values")

        Dim cmdLineArgs As List(Of String) = Environment.GetCommandLineArgs.ToList
        InitSettings("", False, "", Nothing, Nothing, IO.Path.GetTempFileName, Nothing, False, cmdLineArgs)


    End Sub
#Else

    Private Sub SettingsCheckInit()
        If _settingsGlobal IsNot Nothing Then Return

        Debug.WriteLine("Nuget.Configs call without InitSettings, using only temporary values")
        InitSettings("", False, "", Nothing, Nothing, IO.Path.GetTempFileName, Nothing, False, Nothing)

    End Sub
#End If

#End Region

#Region "getset"

    ''' <summary>
    ''' set variable sName to value, Roam or local
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="value">new value</param>
    ''' <param name="bRoam">True if roaming, False if local</param>
    Public Sub SetSettingsString(sName As String, value As String, Optional bRoam As Boolean = False)
        SettingsCheckInit()

        If bRoam Then value = "[ROAM]" & value

        _settingsGlobal(sName) = value

    End Sub

    ''' <summary>
    ''' set variable sName to value, Roam or local
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="value">new value</param>
    ''' <param name="bRoam">True if roaming, False if local</param>
    Public Sub SetSettingsInt(sName As String, value As Integer, Optional bRoam As Boolean = False)
        SetSettingsString(sName, value.ToString(System.Globalization.CultureInfo.InvariantCulture), bRoam)
    End Sub

    ''' <summary>
    ''' set variable sName to value, Roam or local
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="value">new value</param>
    ''' <param name="bRoam">True if roaming, False if local</param>
    Public Sub SetSettingsBool(sName As String, value As Boolean, Optional bRoam As Boolean = False)
        SetSettingsString(sName, If(value, "True", "False"), bRoam)
    End Sub

    ''' <summary>
    ''' set variable sName to value, Roam or local
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="value">new value</param>
    ''' <param name="bRoam">True if roaming, False if local</param>
    Public Sub SetSettingsLong(sName As String, value As Long, Optional bRoam As Boolean = False)
        SetSettingsString(sName, value.ToString(System.Globalization.CultureInfo.InvariantCulture), bRoam)
    End Sub

    ''' <summary>
    ''' set variable sName to value, Roam or local
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="value">new value</param>
    ''' <param name="bRoam">True if roaming, False if local</param>
    Public Sub SetSettingsDate(sName As String, value As DateTimeOffset, Optional bRoam As Boolean = False)
        SetSettingsString(sName, value.ToString("yyyy.MM.dd HH:mm:ss"), bRoam)
    End Sub

    ''' <summary>
    ''' set variable sName to current date, Roam or local
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="bRoam">True if roaming, False if local</param>
    Public Sub SetSettingsCurrentDate(sName As String, Optional bRoam As Boolean = False)
        SetSettingsDate(sName, DateTimeOffset.Now, bRoam)
    End Sub


    Private Function GetSettingsNet(sName As String, sDefault As String)
        SettingsCheckInit()
        Dim sRetVal As String = _settingsGlobal(sName)
        If sRetVal IsNot Nothing Then Return sRetVal

        Return sDefault
        ' https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration/src/ConfigurationRoot.cs
        ' widać że zwraca NULL gdy nie trafi na zmienną nigdzie
    End Function

    ''' <summary>
    ''' get variable value (from first source with such variable, counting from last source added to Configuration)
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="sDefault">default value</param>
    ''' <returns></returns>
    Public Function GetSettingsString(sName As String, Optional sDefault As String = "") As String
        Return GetSettingsNet(sName, sDefault)
    End Function

    ''' <summary>
    ''' get variable value (from first source with such variable, counting from last source added to Configuration)
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="iDefault">default value</param>
    ''' <returns></returns>
    Public Function GetSettingsInt(sName As String, Optional iDefault As Integer = 0) As Integer
        Dim sRetVal As String = GetSettingsNet(sName, iDefault.ToString(System.Globalization.CultureInfo.InvariantCulture))
        Dim iRetVal As Integer = 0
        If Integer.TryParse(sRetVal, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, iRetVal) Then
            Return iRetVal
        End If
        Return iDefault
    End Function

    ''' <summary>
    ''' get variable value (from first source with such variable, counting from last source added to Configuration)
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="bDefault">default value</param>
    ''' <returns></returns>
    Public Function GetSettingsBool(sName As String, Optional bDefault As Boolean = False) As Boolean
        Dim sRetVal As String = GetSettingsNet(sName, If(bDefault, "True", "False"))
        If sRetVal.ToLower = "true" Then Return True
        Return False
    End Function

    ''' <summary>
    ''' get variable value (from first source with such variable, counting from last source added to Configuration)
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="iDefault">default value</param>
    ''' <returns></returns>
    Public Function GetSettingsLong(sName As String, Optional iDefault As Long = 0) As Long
        Dim sRetVal As String = GetSettingsNet(sName, iDefault.ToString(System.Globalization.CultureInfo.InvariantCulture))
        Dim iRetVal As Long = 0
        If Long.TryParse(sRetVal, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, iRetVal) Then
            Return iRetVal
        End If
        Return iDefault
    End Function

    ''' <summary>
    ''' get variable value (from first source with such variable, counting from last source added to Configuration)
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="sDefault">default value (as string yyyy.MM.dd HH:mm:ss), "" means current date</param>
    ''' <returns></returns>
    Public Function GetSettingsDate(sName As String, Optional sDefault As String = "") As DateTimeOffset
        If sDefault = "" Then sDefault = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")
        Dim sRetVal As String = GetSettingsNet(sName, sDefault)
        Dim dRetVal As DateTimeOffset
        If DateTimeOffset.TryParseExact(sRetVal, {"yyyy.MM.dd HH:mm:ss"},
                             Globalization.CultureInfo.InvariantCulture.DateTimeFormat,
                             Globalization.DateTimeStyles.AllowWhiteSpaces, dRetVal) Then
            Return dRetVal
        End If

        Return DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")
    End Function


    ''' <summary>
    ''' get variable value (from first source with such variable, counting from last source added to Configuration)
    ''' </summary>
    ''' <param name="sName">name of variable</param>
    ''' <param name="dDefault">default value</param>
    ''' <returns></returns>
    Public Function GetSettingsDate(sName As String, dDefault As DateTimeOffset) As DateTimeOffset
        Dim sRetVal As String = GetSettingsNet(sName, "")
        If String.IsNullOrWhiteSpace(sRetVal) Then Return dDefault

        Dim dRetVal As DateTimeOffset
        If DateTimeOffset.TryParseExact(sRetVal, {"yyyy.MM.dd HH:mm:ss"},
                             Globalization.CultureInfo.InvariantCulture.DateTimeFormat,
                             Globalization.DateTimeStyles.AllowWhiteSpaces, dRetVal) Then
            Return dRetVal
        End If

        Return DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")
    End Function

#End Region

    Public Function DumpSettings() As String
        ' GetDebugView(IConfigurationRoot) - ale to od późniejszych .Net, od platform extension 3

        Dim sRet As String = "Dump settings (Nuget pkar.NetConfigs)" & vbCrLf

        For Each oSett In _settingsGlobal.AsEnumerable
            sRet = sRet & oSett.Key & vbTab & oSett.Value & vbCrLf
        Next

        Return sRet
    End Function

End Module

Public Enum ConfigScope
    temp
    local
    roam
    machine
    onedrive
End Enum