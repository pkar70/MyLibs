

Imports Windows.ApplicationModel

Public Module UnoConfig

    ''' <summary>
    ''' initialization of library - as a wrapper to pkar.NetConfigs. Used sources: (1) INI source; (2) Environment variables, prefixed with appname; (3) JSON source, roaming and local; (4) command line arguments
    ''' </summary>
    ''' <param name="sINIcontent">INI source; Content of INI file, NULL or "" if you don't want to use it</param>
    ''' <param name="bIniUseDebug">True if [debug] section is to be used ([main] is used always)</param>
    Public Sub InitSettings(sIniContent As String, bIniUseDebug As Boolean)

        ' as GetEntryAssembly() returns NULL, we have to override it
        ' https://github.com/dotnet/sdk/issues/7976

        Dim sAppName As String = Package.Current.DisplayName
        ' ale i tak jest Empty
        Dim oDict As IDictionary = Environment.GetEnvironmentVariables()    ' że, w 1.4, zwraca HashTable?
        Dim cmdLineArgs As List(Of String) = Environment.GetCommandLineArgs.ToList

        pkar.NetConfigs.InitSettings(sIniContent, bIniUseDebug, sAppName, oDict, Nothing,
                           Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                            Windows.Storage.ApplicationData.Current.RoamingFolder.Path, False, cmdLineArgs)


        ' use cmdline, envVars, JSON local+roam (no temp)
        'pkar.NetConfigs.InitSettings(sIniContent, bIniUseDebug, True, Nothing, False, True, True)

    End Sub

End Module

