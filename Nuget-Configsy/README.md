
 This package contains four of my Microsoft.Extensions.Configuration.ConfigurationProvider / ConfigurationSource pairs, and some convenient methods to Get/Set values.

 I was "forced" to make these version when I started to porting my UWP apps to multiplatform, and I wanted analogs of Windows.Storage.ApplicationData.Current..RoamingSettings and .LocalSettings. Also, I want it to work on phones (so it is limited to .Net Standard 1.4, last that works on Windows 10.15063)

 So, I have these requirements:
 * work in Plarform Uno, phones, and Win7 => .Net Standard 1.4
 * have both roam and local settings
 * can use only runtime files, not files in installation package (Android doesn't extract files from package)

  Basic idea: when .Set(variableName, value), and value is prefixed with "[roam]", it sets roaming setting; else sets local setting.

  See also my pkar.uwp.configs Nuget, as a wrapper with many extensions for UI elements


# settings methods

## initialization

Without init, you get only read/write config in temporaty JSON file (in user temp directory).

You can init library by using fully customizable IConfigurationRoot

> Sub InitSettings(settings As MsExtConf.IConfigurationRoot)

where settings is result of something like this:

        Dim sAppName As String = Windows.ApplicationModel.Package.Current.DisplayName
        Dim oBuilder As New Microsoft.Extensions.Configuration.ConfigurationBuilder()
        Dim oDict As IDictionary = Environment.GetEnvironmentVariables()
        oBuilder = oBuilder.AddEnvironmentVariablesROConfigurationSource(sAppName, oDict) 
        oBuilder = oBuilder.AddUwpSettings()
        oBuilder = oBuilder.AddJsonRwSettings(Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                        Windows.Storage.ApplicationData.Current.RoamingFolder.Path)
        If aCmdLineArgs IsNot Nothing Then oBuilder = oBuilder.AddCommandLineRO(aCmdLineArgs)
        Dim settings As Microsoft.Extensions.Configuration.IConfigurationRoot = oBuilder.Build
        pkar.NetConfigs.InitSettings(settings As MsExtConf.IConfigurationRoot)

or use simplified form:

        Sub InitSettings(sINIcontent As String, bIniUseDebug As Boolean,
                            applicationName As String, dictionaryOfEnvVars As System.Collections.IDictionary,
                            configSource As MsExtConf.IConfigurationSource,
                            localJSONdirName As String, roamJSONdirNname As String, bJSONreadOnly As Boolean,
                            cmdLineArgs As List(Of String))

as in this call (from UWP):

        #if DEBUG
        pkar.NetConfigs.InitSettings(sINIcontent, True,
        #else
        pkar.NetConfigs.InitSettings(sINIcontent, False,
        #end if
                    Windows.ApplicationModel.Package.Current.DisplayName, Environment.GetEnvironmentVariables(),
                    new UwpConfigurationSource,
                    Windows.Storage.ApplicationData.Current.LocalFolder.Path, Windows.Storage.ApplicationData.Current.RoamingFolder.Path, False,
                    Environment.GetCommandLineArgs.ToList)

or from WPF:

        Dim sAssemblyFullName = System.Reflection.Assembly.GetEntryAssembly().FullName
        Dim oAss As New AssemblyName(sAssemblyFullName)
        Dim sAppName = oAss.Name

        Dim sPathLocal As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), sAppName)
        Dim sPathRoam As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), sAppName)

        Vblib.InitSettings(
                sAppName, Environment.GetEnvironmentVariables(),
                Nothing,
                sPathLocal, sPathRoam,
                Environment.GetCommandLineArgs.ToList)


You can use NULLs if you don't want particular ConfigurationSource


For .Net Standard 2.0 and above, you can also use simpler form:

    Public Sub InitSettings(sINIcontent As String, bIniUseDebug As Boolean,
                            bUseEnvVars As Boolean,
                            configSource As IConfigurationSource,
                            JsonUseTemp As Boolean, JsonUseLocal As Boolean, JsonUseRoam As Boolean,
                            Optional JsonReadOnly As Boolean = False,
                            Optional bUseCmdLineArgs As Boolean = True)

It can use three JSON sources: 
* temporary (per session) file
* local file (UWP: C:\Users\xxx\AppData\Local\Packages\xxx\LocalState, WPF: C:\Users\xxx\AppData\Local\xxx)
* roaming file (UWP: C:\Users\xxx\AppData\Local\Packages\xxx\RoamingState, WPF: C:\Users\xxx\AppData\Roaming\xxx)


## setting values

    Sub SetSettingsString(sName As String, value As String, Optional bRoam As Boolean = False)
    Sub SetSettingsInt(sName As String, value As Integer, Optional bRoam As Boolean = False)
    Sub SetSettingsBool(sName As String, value As Boolean, Optional bRoam As Boolean = False)
    Sub SetSettingsString(sName, value.ToString(System.Globalization.CultureInfo.InvariantCulture), bRoam)
    Sub SetSettingsDate(sName As String, value As DateTimeOffset, Optional bRoam As Boolean = False)
    Sub SetSettingsCurrentDate(sName As String, Optional bRoam As Boolean = False)

## getting values

    Function GetSettingsString(sName As String, Optional sDefault As String = "") As String
    Function GetSettingsInt(sName As String, Optional iDefault As Integer = 0) As Integer
    Function GetSettingsBool(sName As String, Optional bDefault As Boolean = False) As Boolean
    Function GetSettingsLong(sName As String, Optional iDefault As Long = 0) As Long
    Function GetSettingsDate(sName As String, Optional sDefault As String = "") As DateTimeOffset
    Function GetSettingsDate(sName As String, dDefault As DateTimeOffset) As DateTimeOffset


# providers

## JsonRwConfiguration
Difference with Microsoft's implementation:
* Microsoft's implementation doesn't work in .Net Standard 1.4, so it cannot be used on phones
* Ms version is read-only
* limit of this implementation: no tree of values, only flat version
* we can have two files for values - local and roaming
* on reading, local values overrrides roaming values

        IConfigurationBuilder.AddJsonRwSettings(sPathnameLocal As String, sPathnameRoam As String, Optional bReadOnly As Boolean = False)

At least one pathname should be not null.

e.g.
    
        oBuilder.AddJsonRwSettings(Windows.Storage.ApplicationData.Current.LocalFolder.Path, Windows.Storage.ApplicationData.Current.RoamingFolder.Path);

For .Net Standard 2.0 and above, you can also use simpler form:

       oBuilder.AddJsonRwSettings(bUseTemp As Boolean, bUseLocal As Boolean, bUseRoam As Boolean, Optional bReadOnly As Boolean = False);


## IniDefaultsConfigurationProvider
Difference with Microsoft's implementation:
* ctor uses not file name, but file content - e.g. on Android we have no files extracted from installation package
* .Set is converted to .Remove, important escpecially when used in pack with my others ConfigurationProviders which react for [roam] prefix

        IConfigurationBuilder.AddIniReleaseDebugSettings(sIniContent As String, bUseDebug As Boolean)

e.g.

    #if DEBUG
    oBuilder = oBuilder.AddIniRelDebugSettings("...", true)
    #else
    oBuilder = oBuilder.AddIniRelDebugSettings("...", false)
    #endif


## EnvironmentVariablesROConfigurationProvider
Difference with Microsoft's implementation:
* Microsoft's implementation doesn't work in .Net Standard 1.4, so it cannot be used on phones
* .Set is converted to .Remove, important escpecially when used in pack with my others ConfigurationProviders which react for [roam] prefix

        IConfigurationBuilder.AddEnvironmentVariablesROConfigurationSource(sPrefix As String, oDict As System.Collections.IDictionary)

e.g.

    oBuilder.AddEnvironmentVariablesROConfigurationSource(Windows.ApplicationModel.Package.Current.DisplayName, Environment.GetEnvironmentVariables())

(Environment.GetEnvironmentVariables() is visible in UWP app, but not in .Net Standard 1.4)

## CommandLineROConfigurationProvider
Difference with Microsoft's implementation:
* no tree of values
* .Set is converted to .Remove, important escpecially when used in pack with my others ConfigurationProviders which react for [roam] prefix

        IConfigurationBuilder.AddCommandLineRO(aArgs As List(Of String))

e.g.

    oBuilder = oBuilder.AddCommandLineRO(Environment.GetCommandLineArgs.ToList)

(Environment.GetCommandLineArgs() is visible in UWP app, but not in .Net Standard 1.4)


For UWP, you can use Nuget pkar.UWP.config (which can use settings from UWP ApplicationData, and provides nice extensions for storing/retrieving UI data)
