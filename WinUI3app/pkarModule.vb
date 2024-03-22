' (...)
'            AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed
'
'            ' PKAR added wedle https://stackoverflow.com/questions/39262926/uwp-hardware-back-press-work-correctly-in-mobile-but-error-with-pc
'            AddHandler rootFrame.Navigated, AddressOf OnNavigatedAddBackButton
'            AddHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested, AddressOf OnBackButtonPressed
' (...)


' 2022.04.03: sync z uzupelnionym pkarlibmodule, przerzucenie czesci rzeczy do Extensions

' PLIK DOŁĄCZANY
' założenie: jest VBlib z pkarlibmodule
' mklink pkarModule.vb ..\..\_mojeSuby\pkarModuleWithLib.vb
' PLIK DOŁACZANY

' historia:
' historia.pkarmodule.vb

' 2022.05.02: NetIsIPavail param bMsg jest teraz optional (default: bez pytania)

' 2024.01.13  + FrameworkElement.SetUiPropertiesFromLang, FrameworkElementSetFromResourcesTree, TextBlock.SetLangText

Imports pkar
Imports pkar.UI.Extensions
Imports pkar.UI.Triggers

Imports MsExtConfig = Microsoft.Extensions.Configuration
Imports MsExtPrim = Microsoft.Extensions.Primitives

Imports WinAppData = Windows.Storage.ApplicationData

#If Not NETFX_CORE Then
Imports Microsoft.UI.Xaml
Imports Microsoft.UI.Xaml.Data ' dla IValueConverter
Imports System.IO   ' dla Stream oraz OpenStreamForWrite
Imports System.Runtime.CompilerServices
Imports Windows.Foundation.Collections  ' dla IPropertySet
Imports Windows.ApplicationModel    ' dla DataTransfer
#End If



Partial Public Class App
    Inherits Application

#Region "Back button"
#If NETFX_CORE Then
    ' PKAR added wedle https://stackoverflow.com/questions/39262926/uwp-hardware-back-press-work-correctly-in-mobile-but-error-with-pc
    Private Sub OnNavigatedAddBackButton(sender As Object, e As NavigationEventArgs)
        Try
            Dim oFrame As Frame = TryCast(sender, Frame)
            If oFrame Is Nothing Then Exit Sub

            Dim oNavig As Windows.UI.Core.SystemNavigationManager = Windows.UI.Core.SystemNavigationManager.GetForCurrentView

            If oFrame.CanGoBack Then
                oNavig.AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible
            Else
                oNavig.AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed
            End If

            Return

        Catch ex As Exception
            pkar.CrashMessageExit("@OnNavigatedAddBackButton", ex.Message)
        End Try

    End Sub

    Private Sub OnBackButtonPressed(sender As Object, e As Windows.UI.Core.BackRequestedEventArgs)
        Try
            TryCast(Window.Current.Content, Controls.Frame)?.GoBack()
            e.Handled = True
        Catch ex As Exception
        End Try
    End Sub
#Else
    Private Sub OnNavigatedAddBackButton(sender As Object, e As Object)
        ' tak naprawdę e to NavigationEventArgs, ale do tego trzeba imports Microsoft.UI.Xaml.Navigation (na WinUI3, bo na UWP nie trzeba)
        Try
            Dim oFrame As Controls.Frame = TryCast(sender, Controls.Frame)
            If oFrame Is Nothing Then Exit Sub
            If Not oFrame.CanGoBack Then Return

            Dim oPage As Controls.Page = TryCast(oFrame.Content, Controls.Page)
            If oPage Is Nothing Then Return

            Dim oGrid As Controls.Grid = TryCast(oPage.Content, Controls.Grid)
            If oGrid Is Nothing Then Return

            Dim oButton As New Controls.Button With {
            .Content = New Controls.SymbolIcon(Controls.Symbol.Back),
            .Name = "uiPkAutoBackButton",
                    .VerticalAlignment = VerticalAlignment.Top,
                    .HorizontalAlignment = HorizontalAlignment.Left}
            AddHandler oButton.Click, AddressOf OnBackButtonPressed

            Dim iCols As Integer = 0
            If oGrid.ColumnDefinitions IsNot Nothing Then iCols = oGrid.ColumnDefinitions.Count ' może być 0
            Dim iRows As Integer = 0
            If oGrid.RowDefinitions IsNot Nothing Then iRows = oGrid.RowDefinitions.Count ' może być 0
            If iRows > 1 Then
                Controls.Grid.SetRow(oButton, 0)
                Controls.Grid.SetRowSpan(oButton, iRows)
            End If
            If iCols > 1 Then
                Controls.Grid.SetColumn(oButton, 0)
                Controls.Grid.SetColumnSpan(oButton, iCols)
            End If
            oGrid.Children.Add(oButton)


        Catch ex As Exception
            pkar.CrashMessageExit("@OnNavigatedAddBackButton", ex.Message)
        End Try

    End Sub

    Private Sub OnBackButtonPressed(sender As Object, e As RoutedEventArgs)
        Dim oFE As FrameworkElement = sender
        Dim oPage As Controls.Page = Nothing

        While True
            oPage = TryCast(oFE, Controls.Page)
            If oPage IsNot Nothing Then Exit While
            oFE = oFE.Parent
            If oFE Is Nothing Then Return
        End While

        oPage.GoBack

    End Sub
#End If
#End Region

#Region "RemoteSystem/Background"
    Private moTaskDeferal As Windows.ApplicationModel.Background.BackgroundTaskDeferral = Nothing
    Private moAppConn As Windows.ApplicationModel.AppService.AppServiceConnection
    Private msLocalCmdsHelp As String = ""

    Private Sub RemSysOnServiceClosed(appCon As Windows.ApplicationModel.AppService.AppServiceConnection, args As Windows.ApplicationModel.AppService.AppServiceClosedEventArgs)
        If appCon IsNot Nothing Then appCon.Dispose()
        If moTaskDeferal IsNot Nothing Then
            moTaskDeferal.Complete()
            moTaskDeferal = Nothing
        End If
    End Sub

    Private Sub RemSysOnTaskCanceled(sender As Windows.ApplicationModel.Background.IBackgroundTaskInstance, reason As Windows.ApplicationModel.Background.BackgroundTaskCancellationReason)
        If moTaskDeferal IsNot Nothing Then
            moTaskDeferal.Complete()
            moTaskDeferal = Nothing
        End If
    End Sub

    ''' <summary>
    ''' do sprawdzania w OnBackgroundActivated
    ''' jak zwróci True, to znaczy że nie wolno zwalniać moTaskDeferal !
    ''' sLocalCmdsHelp: tekst do odesłania na HELP
    ''' </summary>
    Public Function RemSysInit(args As Activation.BackgroundActivatedEventArgs, sLocalCmdsHelp As String) As Boolean
        Dim oDetails As Windows.ApplicationModel.AppService.AppServiceTriggerDetails =
                TryCast(args.TaskInstance.TriggerDetails, Windows.ApplicationModel.AppService.AppServiceTriggerDetails)
        If oDetails Is Nothing Then Return False

        msLocalCmdsHelp = sLocalCmdsHelp

        AddHandler args.TaskInstance.Canceled, AddressOf RemSysOnTaskCanceled
        moAppConn = oDetails.AppServiceConnection
        AddHandler moAppConn.RequestReceived, AddressOf RemSysOnRequestReceived
        AddHandler moAppConn.ServiceClosed, AddressOf RemSysOnServiceClosed
        Return True

    End Function

    Public Async Function CmdLineOrRemSys(sCommand As String) As Task(Of String)
        Dim sResult As String = AppServiceStdCmd(sCommand, msLocalCmdsHelp)
        If String.IsNullOrEmpty(sResult) Then
#If NETFX_CORE Then
            sResult = Await AppServiceLocalCommand(sCommand)
#End If
        End If

        Return sResult
    End Function

    Public Async Function ObsluzCommandLine(sCommand As String) As Task

        Dim oFold As Windows.Storage.StorageFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder
        If oFold Is Nothing Then Return

        Dim sLockFilepathname As String = IO.Path.Combine(oFold.Path, "cmdline.lock")
        Dim sResultFilepathname As String = IO.Path.Combine(oFold.Path, "stdout.txt")

        Try
            IO.File.WriteAllText(sLockFilepathname, "lock")
        Catch ex As Exception
            Return
        End Try

        Dim sResult = Await CmdLineOrRemSys(sCommand)
        If String.IsNullOrEmpty(sResult) Then
            sResult = "(empty - probably unrecognized command)"
        End If

        IO.File.WriteAllText(sResultFilepathname, sResult)

        IO.File.Delete(sLockFilepathname)

    End Function

    Private Async Sub RemSysOnRequestReceived(sender As Windows.ApplicationModel.AppService.AppServiceConnection, args As Windows.ApplicationModel.AppService.AppServiceRequestReceivedEventArgs)
        '// 'Get a deferral so we can use an awaitable API to respond to the message 

        Dim sStatus As String
        Dim sResult As String = ""
        Dim messageDeferral As Windows.ApplicationModel.AppService.AppServiceDeferral = args.GetDeferral()

        If VBlib.GetSettingsBool("remoteSystemDisabled") Then
            sStatus = "No permission"
        Else

            Dim oInputMsg As Windows.Foundation.Collections.ValueSet = args.Request.Message

            sStatus = "ERROR while processing command"

            If oInputMsg.ContainsKey("command") Then

                Dim sCommand As String = oInputMsg("command")
                sResult = Await CmdLineOrRemSys(sCommand)
            End If

            If sResult <> "" Then sStatus = "OK"

        End If

        Dim oResultMsg As New Windows.Foundation.Collections.ValueSet()
        oResultMsg.Add("status", sStatus)
        oResultMsg.Add("result", sResult)

        Await args.Request.SendResponseAsync(oResultMsg)

        messageDeferral?.Complete()
        moTaskDeferal?.Complete()

    End Sub


#End Region

    Public Shared Sub OpenRateIt()
        Dim sUri As New Uri("ms-windows-store://review/?PFN=" & Package.Current.Id.FamilyName)
        sUri.OpenBrowser
    End Sub

End Class

Public Module pkar

#Region "import settings"

    ' nie dla UWP
#If Not NETFX_CORE Then

    ''' <summary>
    ''' import settingsów JSON z UWP, o ile tam są a tutaj nie ma - wywoływać przed InitLib!
    ''' </summary>
    ''' <param name="packageName">Zobacz w Manifest, Packaging, Package Name</param>
    Public Sub TryImportSettingsFromUwp(packageName As String)
        Dim sPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)

        Dim uwpPath As String = IO.Path.Combine(sPath, packageName)
        Dim wpfPath As String = IO.Path.Combine(sPath, GetAppName)

        ' normalne
        ' UWP = C:\Users\xxx\AppData\Local\Packages\xxx\LocalState)
        ' WPF = WinUI3 = MAUI = C:\Users\xxx\AppData\Local
        TryImportSettingsFromDir(IO.Path.Combine(uwpPath, "LocalState"), wpfPath)

        ' roaming
        ' UWP = C:\Users\xxx\AppData\Local\Packages\xxx\RoamingState)
        ' WPF = WinUI3 = MAUI = C:\Users\xxx\AppData\Roaming
        Dim dirsep As String = IO.Path.DirectorySeparatorChar
        wpfPath = wpfPath.Replace(dirsep & "Local", dirsep & "Roaming")
        TryImportSettingsFromDir(IO.Path.Combine(uwpPath, "RoamingState"), wpfPath)
    End Sub

    Private Sub TryImportSettingsFromDir(srcDir As String, dstDir As String)
        Dim JSON_FILENAME As String = "AppSettings.json"

        Dim srcFile As String = IO.Path.Combine(srcDir, JSON_FILENAME)
        If Not IO.File.Exists(srcFile) Then Return

        Dim dstFile As String = IO.Path.Combine(dstDir, JSON_FILENAME)
        If IO.File.Exists(dstFile) Then Return

        If Not IO.Directory.Exists(dstDir) Then IO.Directory.CreateDirectory(dstDir)

        IO.File.Copy(srcFile, dstFile)
    End Sub

    Private Function GetAppName() As String
        Dim sAssemblyFullName = Reflection.Assembly.GetEntryAssembly().FullName
        Dim oAss As New Reflection.AssemblyName(sAssemblyFullName)
        Return oAss.Name
    End Function


#End If
#End Region


    ''' <summary>
    ''' dla starszych: InitLib(Nothing)
    ''' dla nowszych:  InitLib(Environment.GetCommandLineArgs)
    ''' </summary>
    Public Sub InitLib(aCmdLineArgs As List(Of String), Optional bUseOwnFolderIfNotSD As Boolean = True)
#If NETFX_CORE Then
        UI.Configs.InitSettings(VBlib.IniLikeDefaults.sIniContent, False, aCmdLineArgs)
#Else
        UI.Configs.InitSettings(VBlib.IniLikeDefaults.sIniContent, False)
#End If

        VBlib.LibInitToast(AddressOf FromLibMakeToast)
        VBlib.LibInitDialogBox(AddressOf FromLibDialogBoxAsync, AddressOf FromLibDialogBoxYNAsync, AddressOf FromLibDialogBoxInputAllDirectAsync)

        VBlib.LibInitClip(AddressOf FromLibClipPut, AddressOf FromLibClipPutHtml)
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
        ' InitDatalogFolder(bUseOwnFolderIfNotSD)
#Enable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
    End Sub

#Region "CrashMessage"
    ' większość w VBlib

    ''' <summary>
    ''' DialogBox z dotychczasowym logiem i skasowanie logu
    ''' </summary>
    Public Async Function CrashMessageShowAsync() As Task
        Dim sTxt As String = VBlib.GetSettingsString("appFailData")
        If sTxt = "" Then Return
        Await VBlib.DialogBoxAsync("FAIL messages:" & vbCrLf & sTxt)
        VBlib.SetSettingsString("appFailData", "")
    End Function

    ''' <summary>
    ''' Dodaj do logu, ewentualnie toast, i zakończ App
    ''' </summary>
    Public Sub CrashMessageExit(sTxt As String, exMsg As String)
        VBlib.CrashMessageAdd(sTxt, exMsg)
        TryCast(Application.Current, App)?.Exit()
    End Sub

#End Region

    ' -- CLIPBOARD ---------------------------------------------

#Region "ClipBoard"
    Private Sub FromLibClipPut(sTxt As String)
        Try

            Dim oClipCont As New DataTransfer.DataPackage With {
                .RequestedOperation = DataTransfer.DataPackageOperation.Copy
            }
            oClipCont.SetText(sTxt)
            DataTransfer.Clipboard.SetContent(oClipCont)
        Catch ex As Exception
            ' czasem daje "Not enough memory resources are available to process this command. (Exception from HRESULT: 0x80070008)"
        End Try
    End Sub

    Private Sub FromLibClipPutHtml(sHtml As String)
        Dim oClipCont As New DataTransfer.DataPackage With {
            .RequestedOperation = DataTransfer.DataPackageOperation.Copy
        }
        oClipCont.SetHtmlFormat(sHtml)
        DataTransfer.Clipboard.SetContent(oClipCont)
    End Sub

    ''' <summary>
    ''' w razie Catch() zwraca ""
    ''' </summary>
    Public Async Function ClipGetAsync() As Task(Of String)
        Dim oClipCont As DataTransfer.DataPackageView = DataTransfer.Clipboard.GetContent
        Try
            Return Await oClipCont.GetTextAsync()
        Catch ex As Exception
            Return ""
        End Try
    End Function
#End Region


    ' -- Testy sieciowe ---------------------------------------------

#Region "testy sieciowe"

    Public Function IsFamilyMobile() As Boolean
        Return (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily = "Windows.Mobile")
    End Function

    Public Function IsFamilyDesktop() As Boolean
        Return (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily = "Windows.Desktop")
    End Function


    ' <Obsolete("Jest w .Net Standard 2.0 (lib)")>
    Public Function NetIsIPavailable(Optional bMsg As Boolean = False) As Boolean
        If VBlib.GetSettingsBool("offline") Then Return False

        If Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() Then Return True
        If bMsg Then
            VBlib.DialogBox("ERROR: no IP network available")
        End If
        Return False
    End Function

    ' <Obsolete("Jest w .Net Standard 2.0 (lib), ale on jest nie do telefonu :)")>
    Public Function NetIsCellInet() As Boolean
        Return Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile().IsWwanConnectionProfile
    End Function


    ' <Obsolete("Jest w .Net Standard 2.0 (lib)")>
    Public Function GetHostName() As String
        Dim hostNames As IReadOnlyList(Of Windows.Networking.HostName) =
                Windows.Networking.Connectivity.NetworkInformation.GetHostNames()
        For Each oItem As Windows.Networking.HostName In hostNames
            If oItem.DisplayName.Contains(".local") Then
                Return oItem.DisplayName.Replace(".local", "")
            End If
        Next
        Return ""
    End Function

    ' <Obsolete("Jest w .Net Standard 2.0 (lib)")>
    ''' <summary>
    ''' Ale to chyba przestało działać...
    ''' </summary>
    Public Function IsThisMoje() As Boolean
        Dim sTmp As String = GetHostName.ToLower
        If sTmp = "home-pkar" Then Return True
        If sTmp = "lumia_pkar" Then Return True
        If sTmp = "kuchnia_pk" Then Return True
        If sTmp = "ppok_pk" Then Return True
        'If sTmp.Contains("pkar") Then Return True
        'If sTmp.EndsWith("_pk") Then Return True
        Return False
    End Function

    ''' <summary>
    ''' w razie Catch() zwraca false
    ''' </summary>
    Public Async Function NetWiFiOffOnAsync() As Task(Of Boolean)

        Try
            ' https://social.msdn.microsoft.com/Forums/ie/en-US/60c4a813-dc66-4af5-bf43-e632c5f85593/uwpbluetoothhow-to-turn-onoff-wifi-bluetooth-programmatically?forum=wpdevelop
            Dim result222 As Windows.Devices.Radios.RadioAccessStatus = Await Windows.Devices.Radios.Radio.RequestAccessAsync()
            If result222 <> Windows.Devices.Radios.RadioAccessStatus.Allowed Then Return False

            Dim radios As IReadOnlyList(Of Windows.Devices.Radios.Radio) = Await Windows.Devices.Radios.Radio.GetRadiosAsync()

            For Each oRadio In radios
                If oRadio.Kind = Windows.Devices.Radios.RadioKind.WiFi Then
                    Dim oStat As Windows.Devices.Radios.RadioAccessStatus =
                    Await oRadio.SetStateAsync(Windows.Devices.Radios.RadioState.Off)
                    If oStat <> Windows.Devices.Radios.RadioAccessStatus.Allowed Then Return False
                    Await Task.Delay(3 * 1000)
                    oStat = Await oRadio.SetStateAsync(Windows.Devices.Radios.RadioState.On)
                    If oStat <> Windows.Devices.Radios.RadioAccessStatus.Allowed Then Return False
                End If
            Next

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub OpenBrowser(sLink As String)
        Dim oUri As New Uri(sLink)
        oUri.OpenBrowser
    End Sub

#Region "Bluetooth"
    ''' <summary>
    ''' Zwraca -1 (no radio), 0 (off), 1 (on), ale gdy bMsg to pokazuje dokładniej błąd (nie włączony, albo nie ma radia Bluetooth) - wedle stringów podanych, które mogą być jednak identyfikatorami w Resources
    ''' </summary>
    Public Async Function NetIsBTavailableAsync(bMsg As Boolean,
                                    Optional bRes As Boolean = False,
                                    Optional sBtDisabled As String = "ERROR: Bluetooth is not enabled",
                                    Optional sNoRadio As String = "ERROR: Bluetooth radio not found") As Task(Of Integer)


        'Dim result222 As Windows.Devices.Radios.RadioAccessStatus = Await Windows.Devices.Radios.Radio.RequestAccessAsync()
        'If result222 <> Windows.Devices.Radios.RadioAccessStatus.Allowed Then Return -1

        Dim oRadios As IReadOnlyList(Of Windows.Devices.Radios.Radio) = Await Windows.Devices.Radios.Radio.GetRadiosAsync()

        Dim bHasBT As Boolean = False

        For Each oRadio As Windows.Devices.Radios.Radio In oRadios
            If oRadio.Kind = Windows.Devices.Radios.RadioKind.Bluetooth Then
                If oRadio.State = Windows.Devices.Radios.RadioState.On Then Return 1
                bHasBT = True
            End If
        Next

        If bHasBT Then
            If bMsg Then
                If bRes Then
                    Await VBlib.DialogBoxResAsync(sBtDisabled)
                Else
                    Await VBlib.DialogBoxAsync(sBtDisabled)
                End If
            End If
            Return 0
        Else
            If bMsg Then
                If bRes Then
                    Await VBlib.DialogBoxResAsync(sNoRadio)
                Else
                    Await VBlib.DialogBoxAsync(sNoRadio)
                End If
            End If
            Return -1
        End If


    End Function

#End Region

#End Region


    ' -- DialogBoxy - tylko jako wskok z VBLib ---------------------------------------------

#Region "DialogBoxy"

    Public Async Function FromLibDialogBoxAsync(sMsg As String) As Task
        Dim oMsg As New Windows.UI.Popups.MessageDialog(sMsg)
        Await oMsg.ShowAsync
    End Function

    ''' <summary>
    ''' Dla Cancel zwraca ""
    ''' </summary>
    Public Async Function FromLibDialogBoxYNAsync(sMsg As String, Optional sYes As String = "Tak", Optional sNo As String = "Nie") As Task(Of Boolean)
        Dim oMsg As New Windows.UI.Popups.MessageDialog(sMsg)
        Dim oYes As New Windows.UI.Popups.UICommand(sYes)
        Dim oNo As New Windows.UI.Popups.UICommand(sNo)
        oMsg.Commands.Add(oYes)
        oMsg.Commands.Add(oNo)
        oMsg.DefaultCommandIndex = 1    ' default: No
        oMsg.CancelCommandIndex = 1
        Dim oCmd As Windows.UI.Popups.IUICommand = Await oMsg.ShowAsync
        If oCmd Is Nothing Then Return False
        If oCmd.Label = sYes Then Return True

        Return False
    End Function

    Public Async Function FromLibDialogBoxInputAllDirectAsync(sMsg As String, Optional sDefault As String = "", Optional sYes As String = "Continue", Optional sNo As String = "Cancel") As Task(Of String)
        Dim oInputTextBox = New Controls.TextBox With {
            .AcceptsReturn = False,
            .Text = sDefault,
            .IsSpellCheckEnabled = False
        }

        Dim oDlg As New Controls.ContentDialog With {
            .Content = oInputTextBox,
            .PrimaryButtonText = sYes,
            .SecondaryButtonText = sNo,
            .Title = sMsg
        }

        Dim oCmd = Await oDlg.ShowAsync
        If oCmd <> Controls.ContentDialogResult.Primary Then Return ""

        Return oInputTextBox.Text

    End Function


#End Region


    ' --- INNE FUNKCJE ------------------------
#Region "Toasty itp"
    Public Sub SetBadgeNo(iInt As Integer)
        ' https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/tiles-and-notifications-badges

        Dim oXmlBadge As Windows.Data.Xml.Dom.XmlDocument
        oXmlBadge = Windows.UI.Notifications.BadgeUpdateManager.GetTemplateContent(
                Windows.UI.Notifications.BadgeTemplateType.BadgeNumber)

        Dim oXmlNum As Windows.Data.Xml.Dom.XmlElement
        oXmlNum = CType(oXmlBadge.SelectSingleNode("/badge"), Windows.Data.Xml.Dom.XmlElement)
        oXmlNum.SetAttribute("value", iInt.ToString)

        Windows.UI.Notifications.BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(
                New Windows.UI.Notifications.BadgeNotification(oXmlBadge))
    End Sub

    <Obsolete("Czy na pewno ma być GetSettingsString a nie GetLangString?")>
    Public Function ToastAction(sAType As String, sAct As String, sGuid As String, sContent As String) As String
        Dim sTmp As String = sContent
        If sTmp <> "" Then sTmp = VBlib.GetSettingsString(sTmp, sTmp)

        Dim sTxt As String = "<action " &
            "activationType=""" & sAType & """ " &
            "arguments=""" & sAct & sGuid & """ " &
            "content=""" & sTmp & """/> "
        Return sTxt
    End Function

    Private Sub FromLibMakeToast(sMsg As String, sMsg1 As String)
        Dim sXml = "<visual><binding template='ToastGeneric'><text>" & VBlib.XmlSafeStringQt(sMsg)
        If sMsg1 <> "" Then sXml = sXml & "</text><text>" & VBlib.XmlSafeStringQt(sMsg1)
        sXml &= "</text></binding></visual>"
        Dim oXml = New Windows.Data.Xml.Dom.XmlDocument
        oXml.LoadXml("<toast>" & sXml & "</toast>")
        Dim oToast = New Windows.UI.Notifications.ToastNotification(oXml)
        Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(oToast)
    End Sub

    ''' <summary>
    ''' dwa kolejne teksty, sMsg oraz sMsg1
    ''' </summary>
    Public Sub MakeToast(sMsg As String, Optional sMsg1 As String = "")
        FromLibMakeToast(sMsg, sMsg1)
    End Sub
    Public Sub MakeToast(oDate As DateTime, sMsg As String, Optional sMsg1 As String = "")
        Dim sXml = "<visual><binding template='ToastGeneric'><text>" & VBlib.XmlSafeStringQt(sMsg)
        If sMsg1 <> "" Then sXml = sXml & "</text><text>" & VBlib.XmlSafeStringQt(sMsg1)
        sXml &= "</text></binding></visual>"
        Dim oXml = New Windows.Data.Xml.Dom.XmlDocument
        oXml.LoadXml("<toast>" & sXml & "</toast>")
        Try
            ' Dim oToast = New Windows.UI.Notifications.ScheduledToastNotification(oXml, oDate, TimeSpan.FromHours(1), 10)
            Dim oToast = New Windows.UI.Notifications.ScheduledToastNotification(oXml, oDate)
            Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().AddToSchedule(oToast)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub RemoveScheduledToasts()
        Try
            While Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().Count > 0
                Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().Item(0))
            End While
        Catch ex As Exception
            ' ponoc na desktopm nie dziala
        End Try

    End Sub

    Public Sub RemoveCurrentToasts()
        Windows.UI.Notifications.ToastNotificationManager.History.Clear()
    End Sub

#End Region

#Region "WinVer, AppVer"


    Public Function WinVer() As Integer
        'Unknown = 0,
        'Threshold1 = 1507,   // 10240
        'Threshold2 = 1511,   // 10586
        'Anniversary = 1607,  // 14393 Redstone 1
        'Creators = 1703,     // 15063 Redstone 2
        'FallCreators = 1709 // 16299 Redstone 3
        'April = 1803		// 17134
        'October = 1809		// 17763
        '? = 190?		// 18???

        'April  1803, 17134, RS5

        Dim u As ULong = ULong.Parse(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion)
        u = (u And &HFFFF0000L) >> 16
        Return u
        'For i As Integer = 5 To 1 Step -1
        '    If Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", i) Then Return i
        'Next

        'Return 0
    End Function

    ' <Obsolete("Jest w .Net Standard 2.0 (lib)")>
    Public Function GetAppVers() As String

        Return Windows.ApplicationModel.Package.Current.Id.Version.Major & "." &
        Windows.ApplicationModel.Package.Current.Id.Version.Minor & "." &
        Windows.ApplicationModel.Package.Current.Id.Version.Build

    End Function

    Public Function GetBuildTimestamp() As String
        Dim install_folder As String = Windows.ApplicationModel.Package.Current.InstalledLocation.Path
        Dim sManifestPath As String = Path.Combine(install_folder, "AppxManifest.xml")

        If File.Exists(sManifestPath) Then
            Return File.GetLastWriteTime(sManifestPath).ToString("yyyy.MM.dd HH:mm")
        End If

        Return ""
    End Function


#End Region


#Region "triggers"
#Region "zwykłe"

#If TRIGGERSHERENOTNUGET Then

    Public Function IsTriggersRegistered(sNameMask As String) As Boolean
        sNameMask = sNameMask.Replace(" ", "").Replace("'", "")

        Try
            For Each oTask As KeyValuePair(Of Guid, Background.IBackgroundTaskRegistration) In Background.BackgroundTaskRegistration.AllTasks
                If oTask.Value.Name.ToLower.Contains(sNameMask.ToLower) Then Return True
            Next
        Catch ex As Exception
            ' np. gdy nie ma permissions, to może być FAIL
        End Try

        Return False
    End Function

    ''' <summary>
    ''' jakikolwiek z prefixem Package.Current.DisplayName
    ''' </summary>
    Public Function IsTriggersRegistered() As Boolean
        Return IsTriggersRegistered(Windows.ApplicationModel.Package.Current.DisplayName)
    End Function

    ''' <summary>
    ''' wszystkie z prefixem Package.Current.DisplayName
    ''' </summary>
    Public Sub UnregisterTriggers()
        UnregisterTriggers(Windows.ApplicationModel.Package.Current.DisplayName)
    End Sub



    Public Sub UnregisterTriggers(sNamePrefix As String)
        sNamePrefix = sNamePrefix.Replace(" ", "").Replace("'", "")

        Try
            For Each oTask As KeyValuePair(Of Guid, Background.IBackgroundTaskRegistration) In Background.BackgroundTaskRegistration.AllTasks
                If String.IsNullOrEmpty(sNamePrefix) OrElse oTask.Value.Name.ToLower.Contains(sNamePrefix.ToLower) Then oTask.Value.Unregister(True)
            Next
        Catch ex As Exception
            ' np. gdy nie ma permissions, to może być FAIL
        End Try

        ' z innego wyszlo, ze RemoveAccess z wnetrza daje Exception
        ' If bAll Then BackgroundExecutionManager.RemoveAccess()

    End Sub

    Public Async Function CanRegisterTriggersAsync() As Task(Of Boolean)

        Dim oBAS As Background.BackgroundAccessStatus
        oBAS = Await Background.BackgroundExecutionManager.RequestAccessAsync()

        If oBAS = Windows.ApplicationModel.Background.BackgroundAccessStatus.AlwaysAllowed Then Return True
        If oBAS = Windows.ApplicationModel.Background.BackgroundAccessStatus.AllowedSubjectToSystemPolicy Then Return True

        Return False

    End Function

    Public Function RegisterTimerTrigger(sName As String, iMinutes As Integer, Optional bOneShot As Boolean = False, Optional oCondition As Windows.ApplicationModel.Background.SystemCondition = Nothing) As Windows.ApplicationModel.Background.BackgroundTaskRegistration

        Try
            Dim builder As New Background.BackgroundTaskBuilder
            Dim oRet As Background.BackgroundTaskRegistration

            builder.SetTrigger(New Windows.ApplicationModel.Background.TimeTrigger(iMinutes, bOneShot))
            builder.Name = sName
            If oCondition IsNot Nothing Then builder.AddCondition(oCondition)
            oRet = builder.Register()
            Return oRet
        Catch ex As Exception
            ' brak możliwości rejestracji (na przykład)
        End Try

        Return Nothing
    End Function

    Public Function RegisterUserPresentTrigger(Optional sName As String = "", Optional bOneShot As Boolean = False) As Windows.ApplicationModel.Background.BackgroundTaskRegistration

        Try
            Dim builder As New Background.BackgroundTaskBuilder
            Dim oRet As Background.BackgroundTaskRegistration

            Dim oTrigger As Windows.ApplicationModel.Background.SystemTrigger
            oTrigger = New Background.SystemTrigger(Background.SystemTriggerType.UserPresent, bOneShot)

            builder.SetTrigger(oTrigger)
            builder.Name = sName
            If String.IsNullOrEmpty(sName) Then builder.Name = GetTriggerNamePrefix() & "_userpresent"

            oRet = builder.Register()
            Return oRet
        Catch ex As Exception
            ' brak możliwości rejestracji (na przykład)
        End Try

        Return Nothing
    End Function

    Private Function GetTriggerNamePrefix() As String
        Dim sName As String = Windows.ApplicationModel.Package.Current.DisplayName
        sName = sName.Replace(" ", "").Replace("'", "")
        Return sName
    End Function

    Private Function GetTriggerPolnocnyName() As String
        Return GetTriggerNamePrefix() & "_polnocny"
    End Function


    ''' <summary>
    ''' Tak naprawdę powtarzalny - w OnBackgroundActivated wywołaj IsThisTriggerPolnocny
    ''' </summary>
    Public Async Function DodajTriggerPolnocny() As System.Threading.Tasks.Task
        If Not Await CanRegisterTriggersAsync() Then Return

        Dim oDateNew As New DateTime(Date.Now.Year, Date.Now.Month, Date.Now.Day, 23, 40, 0)
        If Date.Now.Hour > 21 Then oDateNew = oDateNew.AddDays(1)

        Dim iMin As Integer = (oDateNew - DateTime.Now).TotalMinutes
        Dim sName As String = GetTriggerPolnocnyName()

        RegisterTimerTrigger(sName, iMin, False)
    End Function

    ''' <summary>
    ''' para z DodajTriggerPolnocny
    ''' </summary>
    Public Function IsThisTriggerPolnocny(args As Windows.ApplicationModel.Activation.BackgroundActivatedEventArgs) As Boolean

        Dim sName As String = GetTriggerPolnocnyName()
        If args.TaskInstance.Task.Name <> sName Then Return False

        Dim sCurrDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        VBlib.SetSettingsString("lastPolnocnyTry", sCurrDate)

        Dim bRet As Boolean '= False
        Dim oDateNew As New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 40, 0)

        If (DateTime.Now.Hour = 23 AndAlso DateTime.Now.Minute > 20) Then
            ' tak, to jest północny o północy
            bRet = True
            oDateNew = oDateNew.AddDays(1)
            VBlib.SetSettingsString("lastPolnocnyOk", sCurrDate)
        Else
            ' północny, ale nie o północy
            bRet = False
        End If
        Dim iMin As Integer = (oDateNew - DateTime.Now).TotalMinutes

        ' Usuwamy istniejący, robimy nowy
        UnregisterTriggers(sName)
        RegisterTimerTrigger(sName, iMin, False)

        Return bRet

    End Function



    Public Function RegisterServicingCompletedTrigger(sName As String) As Background.BackgroundTaskRegistration

        Try
            Dim builder As New Background.BackgroundTaskBuilder

            builder.SetTrigger(New Background.SystemTrigger(Background.SystemTriggerType.ServicingComplete, True))
            builder.Name = sName

            Dim oRet As Windows.ApplicationModel.Background.BackgroundTaskRegistration
            oRet = builder.Register()
            Return oRet
        Catch ex As Exception
            ' brak możliwości rejestracji (na przykład)
        End Try

        Return Nothing
    End Function

    Public Function RegisterToastTrigger(sName As String) As Background.BackgroundTaskRegistration

        Try
            Dim builder As New Background.BackgroundTaskBuilder
            Dim oRet As Windows.ApplicationModel.Background.BackgroundTaskRegistration

            builder.SetTrigger(New Windows.ApplicationModel.Background.ToastNotificationActionTrigger)
            builder.Name = sName
            oRet = builder.Register()
            Return oRet
        Catch ex As Exception
            ' brak możliwości rejestracji (na przykład)
        End Try

        Return Nothing
    End Function

        Private Function DumpTriggers() As String
        Dim sRet As String = "Dumping Triggers" & vbCrLf & vbCrLf
        Try
            For Each oTask In Windows.ApplicationModel.Background.BackgroundTaskRegistration.AllTasks
                sRet &= oTask.Value.Name & vbCrLf ' //GetType niestety nie daje rzeczywistego typu
            Next
        Catch
        End Try


        Return sRet
    End Function


#End If

#End Region
#Region "RemoteSystem"


    ''' <summary>
    ''' jeśli na wejściu jest jakaś standardowa komenda, to na wyjściu będzie jej rezultat. Else = ""
    ''' </summary>
    Public Function AppServiceStdCmd(sCommand As String, sLocalCmds As String) As String
        Dim sTmp As String = VBlib.LibAppServiceStdCmd(sCommand, sLocalCmds)
        If sTmp <> "" Then Return sTmp

        ' If sCommand.StartsWith("debug loglevel") Then - vbLib

        Select Case sCommand.ToLower()
            ' Case "ping" - vblib
            Case "ver"
                Return GetAppVers()
            Case "localdir"
                Return Windows.Storage.ApplicationData.Current.LocalFolder.Path
            ' Case "appdir" - vblib
            Case "installeddate"
                Return Windows.ApplicationModel.Package.Current.InstalledDate.ToString("yyyy.MM.dd HH:mm:ss")
            ' Case "help" - vblib

            ' Case "debug vars" - vblib
            Case "debug triggers"
                Return DumpTriggers()
            Case "debug toasts"
                Return DumpToasts()
            Case "debug memsize"
                Return Windows.System.MemoryManager.AppMemoryUsage.ToString() & "/" & Windows.System.MemoryManager.AppMemoryUsageLimit.ToString()
            Case "debug rungc"
                sTmp = "Memory usage before Global Collector call: " & Windows.System.MemoryManager.AppMemoryUsage.ToString() & vbCrLf
                GC.Collect()
                GC.WaitForPendingFinalizers()
                sTmp = sTmp & "After: " & Windows.System.MemoryManager.AppMemoryUsage.ToString() & "/" & Windows.System.MemoryManager.AppMemoryUsageLimit.ToString()
                Return sTmp
            ' Case "debug crashmsg"
            ' Case "debug crashmsg clear"

            Case "lib unregistertriggers"
                sTmp = DumpTriggers()
                UnregisterTriggers("") ' // całkiem wszystkie
                Return sTmp
            Case "lib isfamilymobile"
                Return IsFamilyMobile().ToString()
            Case "lib isfamilydesktop"
                Return IsFamilyDesktop().ToString()
            Case "lib netisipavailable"
                Return NetIsIPavailable(False).ToString()
            Case "lib netiscellinet"
                Return NetIsCellInet().ToString()
            Case "lib gethostname"
                Return GetHostName()
            Case "lib isthismoje"
                Return IsThisMoje().ToString()
            Case "lib istriggersregistered"
                Return IsTriggersRegistered().ToString()

                'Case "lib pkarmode 1"
                'Case "lib pkarmode 0"
                'Case "lib pkarmode"
        End Select

        Return ""  ' oznacza: to nie jest standardowa komenda
    End Function


    Private Function DumpToasts() As String

        Dim sResult As String = ""
        For Each oToast As Windows.UI.Notifications.ScheduledToastNotification
            In Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications()

            sResult = sResult & oToast.DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss") & vbCrLf
        Next

        If sResult = "" Then
            sResult = "(no toasts scheduled)"
        Else
            sResult = "Toasts scheduled for dates: " & vbCrLf & sResult
        End If

        Return sResult
    End Function

#End Region


#End Region

#Region "DataLog folder support"
#If False Then

    Private Async Function GetSDcardFolderAsync() As Task(Of Windows.Storage.StorageFolder)
        ' uwaga: musi być w Manifest RemoteStorage oraz fileext!

        Dim oRootDir As Windows.Storage.StorageFolder

        Try
            oRootDir = Windows.Storage.KnownFolders.RemovableDevices
        Catch ex As Exception
            Return Nothing ' brak uprawnień, może być także THROW
        End Try

        Try
            Dim oCards As IReadOnlyList(Of Windows.Storage.StorageFolder) = Await oRootDir.GetFoldersAsync()
            If oCards.Count < 1 Then Return Nothing
            Return oCards(0)
        Catch ex As Exception
            ' nie udało się folderu SD
        End Try

        Return Nothing


    End Function

    Public Async Function GetLogFolderRootAsync(Optional bUseOwnFolderIfNotSD As Boolean = True) As Task(Of Windows.Storage.StorageFolder)
#Disable Warning IDE0059 ' Unnecessary assignment of a value
        Dim oSdCard As Windows.Storage.StorageFolder = Nothing
#Enable Warning IDE0059 ' Unnecessary assignment of a value
        Dim oFold As Windows.Storage.StorageFolder

        If IsFamilyMobile() Then
            oSdCard = Await GetSDcardFolderAsync()

            If oSdCard IsNot Nothing Then
                oFold = Await oSdCard.CreateFolderAsync("DataLogs", Windows.Storage.CreationCollisionOption.OpenIfExists)
                If oFold Is Nothing Then Return Nothing

                Dim sAppName As String = Package.Current.DisplayName
                sAppName = sAppName.Replace(" ", "").Replace("'", "")

                oFold = Await oFold.CreateFolderAsync(sAppName, Windows.Storage.CreationCollisionOption.OpenIfExists)
                If oFold Is Nothing Then Return Nothing
            Else
                If Not bUseOwnFolderIfNotSD Then Return Nothing
                oSdCard = Windows.Storage.ApplicationData.Current.LocalFolder
                oFold = Await oSdCard.CreateFolderAsync("DataLogs", Windows.Storage.CreationCollisionOption.OpenIfExists)
                If oFold Is Nothing Then Return Nothing
            End If
        Else
            oSdCard = Windows.Storage.ApplicationData.Current.LocalFolder
            oFold = Await oSdCard.CreateFolderAsync("DataLogs", Windows.Storage.CreationCollisionOption.OpenIfExists)
            If oFold Is Nothing Then Return Nothing
        End If

        Return oFold
    End Function


    ''' <summary>
    ''' do wywolania raz, na poczatku - inicjalizacja zmiennych w VBlib (sciezki root)
    ''' </summary>
    Public Async Function InitDatalogFolder(Optional bUseOwnFolderIfNotSD As Boolean = True) As Task
        Dim oFold As Windows.Storage.StorageFolder = Await GetLogFolderRootAsync(bUseOwnFolderIfNotSD)
        If oFold Is Nothing Then Return
        VBlib.LibInitDataLog(oFold.Path)
    End Function
#End If

#End Region


    Public Async Function IsFullVersion() As Task(Of Boolean)
#If DEBUG Then
        Return True
#End If

        If IsThisMoje() Then Return True

        ' Windows.Services.Store.StoreContext: min 14393 (1607)
        Dim oLicencja = Await Windows.Services.Store.StoreContext.GetDefault().GetAppLicenseAsync()
        If Not oLicencja.IsActive Then Return False ' bez licencji? jakżeż to możliwe?

        If oLicencja.IsTrial Then Return False

        Return True

    End Function


End Module


#Region ".Net configuration - UWP settings"

Public Class UwpConfigurationProvider
    ' Inherits MsExtConfig.ConfigurationProvider
    Implements MsExtConfig.IConfigurationProvider

    Private ReadOnly _roamPrefix1 As String = Nothing
    Private ReadOnly _roamPrefix2 As String = Nothing

    ''' <summary>
    ''' Create Configuration Provider, for LocalSettings and RoamSettings
    ''' </summary>
    ''' <param name="sRoamPrefix1">prefix for RoamSettings, use NULL if want only LocalSettings</param>
    ''' <param name="sRoamPrefix2">prefix for RoamSettings, use NULL if want only LocalSettings</param>
    Public Sub New(Optional sRoamPrefix1 As String = "[ROAM]", Optional sRoamPrefix2 As String = Nothing)
        Data = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        _roamPrefix1 = sRoamPrefix1
        _roamPrefix2 = sRoamPrefix2
    End Sub

    Private Sub LoadData(settSource As IPropertySet)
        For Each oItem In settSource
            Data(oItem.Key) = oItem.Value
        Next
    End Sub

    ''' <summary>
    ''' read current state of settings (all values); although it is not used in TryGet, but we should have Data property set for other reasons (e.g. for listing all variables)...
    ''' </summary>
    Public Sub Load() Implements MsExtConfig.IConfigurationProvider.Load
        LoadData(WinAppData.Current.RoamingSettings.Values)
        LoadData(WinAppData.Current.LocalSettings.Values)
    End Sub


    ''' <summary>
    ''' always set LocalSettings, and if value is prefixed with Roam prefix, also RoamSettings (prefix is stripped)
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    Public Sub [Set](key As String, value As String) Implements MsExtConfig.IConfigurationProvider.Set
        If value Is Nothing Then value = ""

        If _roamPrefix1 IsNot Nothing AndAlso value.ToUpperInvariant().StartsWith(_roamPrefix1, StringComparison.Ordinal) Then
            value = value.Substring(_roamPrefix1.Length)
            Try
                WinAppData.Current.RoamingSettings.Values(key) = value
            Catch
                ' probably length is too big
            End Try
        End If

        If _roamPrefix2 IsNot Nothing AndAlso value.ToUpperInvariant().StartsWith(_roamPrefix2, StringComparison.Ordinal) Then
            value = value.Substring(_roamPrefix2.Length)
            Try
                WinAppData.Current.RoamingSettings.Values(key) = value
            Catch
                ' probably length is too big
            End Try
        End If

        Data(key) = value
        Try
            WinAppData.Current.LocalSettings.Values(key) = value
        Catch
            ' probably length is too big
        End Try

    End Sub

    ''' <summary>
    ''' this is used only for iterating keys, not for Get/Set
    ''' </summary>
    ''' <returns></returns>
    Protected Property Data As IDictionary(Of String, String)

    ''' <summary>
    ''' gets current Value of Key; local value overrides roaming value
    ''' </summary>
    ''' <returns>True if Key is found (and Value is set)</returns>
    Public Function TryGet(key As String, ByRef value As String) As Boolean Implements MsExtConfig.IConfigurationProvider.TryGet

        Dim bFound As Boolean = False

        If WinAppData.Current.RoamingSettings.Values.ContainsKey(key) Then
            value = WinAppData.Current.RoamingSettings.Values(key).ToString
            bFound = True
        End If

        If WinAppData.Current.LocalSettings.Values.ContainsKey(key) Then
            value = WinAppData.Current.LocalSettings.Values(key).ToString
            bFound = True
        End If

        Return bFound

    End Function

    Public Function GetReloadToken() As MsExtPrim.IChangeToken Implements MsExtConfig.IConfigurationProvider.GetReloadToken
        Return New MsExtConfig.ConfigurationReloadToken
    End Function

    Public Function GetChildKeys(earlierKeys As IEnumerable(Of String), parentPath As String) As IEnumerable(Of String) Implements MsExtConfig.IConfigurationProvider.GetChildKeys
        ' in this configuration, we don't have structure - so just return list

        Dim results As New List(Of String)
        For Each kv As KeyValuePair(Of String, String) In Data
            results.Add(kv.Key)
        Next

        results.Sort()

        Return results
    End Function

End Class

Public Class UwpConfigurationSource
    Implements MsExtConfig.IConfigurationSource

    Private ReadOnly _roamPrefix1 As String = Nothing
    Private ReadOnly _roamPrefix2 As String = Nothing

    Public Function Build(builder As MsExtConfig.IConfigurationBuilder) As MsExtConfig.IConfigurationProvider Implements MsExtConfig.IConfigurationSource.Build
        Return New UwpConfigurationProvider(_roamPrefix1, _roamPrefix2)
    End Function

    Public Sub New(Optional sRoamPrefix1 As String = "[ROAM]", Optional sRoamPrefix2 As String = Nothing)
        _roamPrefix1 = sRoamPrefix1
        _roamPrefix2 = sRoamPrefix2
    End Sub
End Class

Partial Module Extensions
    <Runtime.CompilerServices.Extension()>
    Public Function AddUwpSettings(ByVal configurationBuilder As MsExtConfig.IConfigurationBuilder, Optional sRoamPrefix1 As String = "[ROAM]", Optional sRoamPrefix2 As String = Nothing) As MsExtConfig.IConfigurationBuilder
        configurationBuilder.Add(New UwpConfigurationSource(sRoamPrefix1, sRoamPrefix2))
        Return configurationBuilder
    End Function
End Module


#End Region

Partial Public Module Extensions

    ''' <summary>
    ''' ustaw wszystkie Properties według resources, jeśli są zdefiniowane dla tego elementu
    ''' </summary>
    <Extension>
    Public Sub SetFromResources(uiElement As FrameworkElement)
        Vblib.SetUiPropertiesFromLang(uiElement)
    End Sub

    ''' <summary>
    ''' ustaw wszystkie Properties według resources, jeśli są zdefiniowane dla tego elementu bądź jego dzieci
    ''' </summary>
    <Extension>
    Public Sub SetFromResourcesTree(uiElement As FrameworkElement)

        uiElement.SetFromResources

        Dim iMax As Integer = Media.VisualTreeHelper.GetChildrenCount(uiElement)
        For iLp = 0 To iMax - 1
            Dim depObj = Media.VisualTreeHelper.GetChild(uiElement, iLp)
            Dim frmwrkEl As FrameworkElement = TryCast(depObj, FrameworkElement)
            frmwrkEl?.SetFromResourcesTree
        Next

    End Sub

    ''' <summary>
    ''' ustaw .Text używając podanego stringu z resources
    ''' </summary>
    <Extension>
    Public Sub SetLangText(uiElement As Controls.TextBlock, stringId As String)
        uiElement.Text = VBlib.GetLangString(stringId)
    End Sub

    ''' <summary>
    ''' ustaw .Content używając podanego stringu z resources
    ''' </summary>
    <Extension>
    Public Sub SetLangText(uiElement As Controls.Button, stringId As String)
        uiElement.Content = VBlib.GetLangString(stringId)
    End Sub



End Module


#Region "Konwertery Bindings XAML"
' nie mogą być w VBlib, bo Implements Microsoft.UI.Xaml.Data.IValueConverter

#Region "to co dla innych UI może być w Nuget, a w UWP być nie może"
#If NETFX_CORE Then

' WinRT nie może mieć "mustoverride"

Public MustInherit Class ValueConverterOneWay
    Implements IValueConverter

    Public MustOverride Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class

''' <summary>
''' this class should be used to define your own ValueConverters; but it frees you from writing ConvertBack method, and simplyfies Convert method
''' </summary>
Public MustInherit Class ValueConverterOneWaySimple
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Return Convert(value)
    End Function

    Protected MustOverride Function Convert(value As Object) As Object


    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class

#End If

#End Region

' parameter = NEG robi negację
Public Class KonwersjaVisibility
    Inherits ValueConverterOneWay

    Public Overrides Function Convert(ByVal value As Object,
    ByVal targetType As Type, ByVal parameter As Object,
    ByVal language As System.String) As Object

        Dim bTemp As Boolean = CType(value, Boolean)
        If parameter IsNot Nothing Then
            Dim sParam As String = CType(parameter, String)
            If sParam.ToUpperInvariant = "NEG" Then bTemp = Not bTemp
        End If
        If bTemp Then Return Visibility.Visible

        Return Visibility.Collapsed

    End Function

End Class

' ULONG to String
Public Class KonwersjaMAC
    Inherits ValueConverterOneWaySimple

    ' Define the Convert method to change a DateTime object to
    ' a month string.
    Protected Overrides Function Convert(ByVal value As Object) As Object

        ' value is the data from the source object.

        Dim uMAC As ULong = CType(value, ULong)
        If uMAC = 0 Then Return ""

        Return uMAC.ToHexBytesString()

    End Function

End Class

Public Class KonwersjaVal2StringFormat
    Inherits ValueConverterOneWay

    ' Define the Convert method to change a DateTime object to
    ' a month string.
    Public Overrides Function Convert(ByVal value As Object,
            ByVal targetType As Type, ByVal parameter As Object,
            ByVal language As System.String) As Object

        Dim sFormat As String = ""
        If parameter IsNot Nothing Then
            sFormat = CType(parameter, String)
        End If

        ' value is the data from the source object.
        If value.GetType Is GetType(Integer) Then
            Dim temp = CType(value, Integer)
            If sFormat = "" Then
                Return temp.ToString
            Else
                Return temp.ToString(sFormat)
            End If
        End If

        If value.GetType Is GetType(Long) Then
            Dim temp = CType(value, Long)
            If sFormat = "" Then
                Return temp.ToString
            Else
                Return temp.ToString(sFormat)
            End If
        End If

        If value.GetType Is GetType(Double) Then
            Dim temp = CType(value, Double)
            If sFormat = "" Then
                Return temp.ToString
            Else
                Return temp.ToString(sFormat)
            End If
        End If

        If value.GetType Is GetType(String) Then
            Dim temp = CType(value, String)
            If sFormat = "" Then
                Return temp.ToString
            Else
                Return String.Format(sFormat, temp)
            End If
        End If

        Return "???"

    End Function

End Class


#End Region
