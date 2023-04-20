
This Nuget fulfills two goals:
1) add .Net configuration provider as translator from UWP ApplicationData
2) add UI extensions helpers to transfer data between UI elements and .Net configuration.

It uses my Nuget pkar.NetConfigs, and its definition of various config providers.
Used config sources:
1) INI source;
2) Environment variables (which are prefixed with appname);
3) UWP ApplicationData.Current roaming and local;
4) JSON source, also roaming and local
5) command line arguments.

You can init config library using direct pkar.NetConfigs.InitSettings, or via this Nuget:

# initialization

        Sub InitSettings(sIniContent As String, bIniUseDebug As Boolean,
                         cmdLineArgs As IList(Of String))

Using string with Ini file content, not filename as parameter solves problem with same library used in Platform Uno (and Xamarin) contexts - Android doesn't unpack install files, so files cannot be accessed in standard way.
Command line arguments can be passed from OnActivated.CommandLineActivatedEventArgs (in UWP 16299+).

# helpers

 For all [Get|Set]Settings*, you can provide setting name (key), or use default (same as UI element name).
 For GetSettings*, you can provide default value, and for SetSettings*, you can specify if setting should be placed also in roaming (it is always placed in local setting).

        TextBlock.[Get|Set]SettingsString()
        TextBox.[Get|Set]SettingsString()
        TextBox.[Get|Set]SettingsInt()  // this Int can be scaled (int is saved, but UI get double)
        ToggleSwitch.[Get|Set]SettingsBool
        ToggleButton.[Get|Set]SettingsBool
        AppBarToggleButton.[Get|Set]SettingsBool
        Slider.[Get|Set]SettingsInt()
        ComboBox.[Get|Set]SettingsInt() // selected item index 
        CalendarDatePicker.[Get|Set]SettingsDate
