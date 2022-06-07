
' pkarlibmodule,
' dodatki które są w .Net Standard 2.0
' Dla UWP 16299+ (czyli app tylko desktop), także w Uno oraz w MAUI

Partial Public Module pkarlibmodule20

#Region "WinVer, AppVer"
    Public Function GetAppVers() As String
        Return System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString()
    End Function


#End Region

#Region "testy sieciowe"

    Public Function NetIsIPavailable(bMsg As Boolean) As Boolean
        If GetSettingsBool("offline") Then Return False

        If Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() Then Return True
        If bMsg Then
            DialogBox("ERROR: no IP network available")
        End If
        Return False
    End Function

    ''' <summary>
    ''' o tyle bez sensu, że to lib 2.0, a więc nie dla telefonu :)
    ''' </summary>
    Public Function NetIsCellInet() As Boolean
        ' można tak sprawdzić wszystkie, i jeśli jes 
        For Each oNIC As Net.NetworkInformation.NetworkInterface In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()

            ' jeśli nie jest to link aktywny, to ignorujemy go
            If oNIC.OperationalStatus <> Net.NetworkInformation.OperationalStatus.Up Then Continue For

            ' nie jest to pełna logika, bo mogą być dodatkowe typy kiedyś...
            Select Case oNIC.NetworkInterfaceType
                Case Net.NetworkInformation.NetworkInterfaceType.Wman
                    Return True
                Case Net.NetworkInformation.NetworkInterfaceType.Wwanpp
                    Return True
                Case Net.NetworkInformation.NetworkInterfaceType.Wwanpp2
                    Return True
                Case Net.NetworkInformation.NetworkInterfaceType.GenericModem
                    Return True
                Case Net.NetworkInformation.NetworkInterfaceType.HighPerformanceSerialBus
                    Return True
                Case Net.NetworkInformation.NetworkInterfaceType.Ppp
                    Return True
                Case Net.NetworkInformation.NetworkInterfaceType.Slip
                    Return True
            End Select
        Next

        Return False
    End Function

    Public Function GetHostName() As String
        Return Net.Dns.GetHostName()
    End Function

    Public Function IsThisMoje() As Boolean
        Dim sTmp As String = GetHostName.ToLower
        If sTmp.StartsWith("home-pkar") Then Return True
        If sTmp = "lumia_pkar" Then Return True
        If sTmp = "kuchnia_pk" Then Return True
        If sTmp = "ppok_pk" Then Return True
        'If sTmp.Contains("pkar") Then Return True
        'If sTmp.EndsWith("_pk") Then Return True
        Return False
    End Function

#End Region


End Module

Partial Module Extensions

    ''' <summary>
    ''' Tylko dla .Net Standard 2.0
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function AddCommandLineRO(ByVal configurationBuilder As Microsoft.Extensions.Configuration.IConfigurationBuilder) As Microsoft.Extensions.Configuration.IConfigurationBuilder
        configurationBuilder.Add(New CommandLineROConfigurationSource(Environment.GetCommandLineArgs.ToList))
        Return configurationBuilder
    End Function


    ''' <summary>
    ''' Tylko dla .Net Standard 2.0
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function AddEnvironmentVariablesROConfigurationSource(ByVal configurationBuilder As Microsoft.Extensions.Configuration.IConfigurationBuilder, sPrefix As String) As Microsoft.Extensions.Configuration.IConfigurationBuilder
        configurationBuilder.Add(New EnvironmentVariablesROConfigurationSource(sPrefix, Environment.GetEnvironmentVariables))
        Return configurationBuilder
    End Function


    ' _settingsGlobal.Providers nie ma w Std1.4!
    Private Function DumpSettings() As String
        ' GetDebugView(IConfigurationRoot) - ale to od późniejszych .Net, od platform extension 3

        Dim sRet As String = "Dump settings (VBlib version)" & vbCrLf

        For Each oProvider As Microsoft.Extensions.Configuration.IConfigurationProvider In _settingsGlobal.Providers
            sRet = sRet & vbCrLf & "Settings known by provider " & oProvider.ToString & vbCrLf

            For Each sKey As String In oProvider.GetChildKeys(New List(Of String), Nothing)
                Dim sVal As String = ""
                oProvider.TryGet(sKey, sVal)
                sRet = sRet & sKey & vbTab & sVal
            Next

        Next

        Return sRet
    End Function

End Module
