

Public Module WpfConfig

    ''' <summary>
    ''' initialization of library - as a wrapper to pkar.NetConfigs. Used sources: (1) INI source; (2) Environment variables, prefixed with appname; (3) JSON source, roaming and local; (4) command line arguments
    ''' </summary>
    ''' <param name="sINIcontent">INI source; Content of INI file, NULL or "" if you don't want to use it</param>
    ''' <param name="bIniUseDebug">True if [debug] section is to be used ([main] is used always)</param>
    Public Sub InitSettings(sIniContent As String, bIniUseDebug As Boolean)

        ' use cmdline, envVars, JSON local+roam (no temp)
        pkar.NetConfigs.InitSettings(sIniContent, bIniUseDebug, True, Nothing, False, True, True)

    End Sub

End Module

