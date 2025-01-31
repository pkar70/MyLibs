
Imports pkar.Localize
Imports System.Windows.Markup

'Imports System.Runtime.InteropServices.WindowsRuntime
'Imports System.Net.Mime.MediaTypeNames



#If PK_WPF Then
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Input
Imports System.Windows.Interop
Imports System.Windows.Threading
#Else
    ' WinUI, UWP
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.UI ' dla Color
Imports Windows.Storage


#If Not NETFX_CORE Then
' WinUI
Imports System
Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.UI.Xaml
Imports Microsoft.UI.Xaml.Controls
Imports Windows.Foundation
Imports Microsoft.UI.Xaml.Media
#End If

#End If

#If NETFX_CORE Then
Imports Windows.Foundation.Metadata
#Else
' to jest pusta definicja, żeby mógł być <DefaultOverload> podany - nie można go #IF zrobić, bo to nie działa
<AttributeUsage(AttributeTargets.Method)>
Public Class DefaultOverloadAttribute
    Inherits Attribute
End Class
#End If


Partial Public Module extensions

#Region "UI related"

#If PK_WPF Then
    ''' <summary>
    ''' Open Explorer on given folder
    ''' </summary>
    <Extension()>
    Public Sub OpenExplorer(ByVal folder As String)
        Dim proc As New Process()
        proc.StartInfo.UseShellExecute = True
        proc.StartInfo.FileName = folder
        proc.Start()
    End Sub

#Else
#Region "StorageFolder"

    ''' <summary>
    ''' Open Explorer on given folder
    ''' </summary>
    <Extension()>
    Public Sub OpenExplorer(ByVal oFold As Windows.Storage.StorageFolder)
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
        Windows.System.Launcher.LaunchFolderAsync(oFold)
#Enable Warning BC42358
    End Sub

    <Extension()>
    Private Async Function LaunchFileAsyncTask(ByVal oFold As Windows.Storage.StorageFolder, filename As String) As Task(Of Boolean)
        Dim oFile As Windows.Storage.StorageFile = Await oFold.GetFileAsync(filename)
        Return Await oFile.LaunchAsync
    End Function

    ''' <summary>
    ''' Run given file
    ''' </summary>
    <Extension()>
    Public Function LaunchFileAsync(ByVal oFold As Windows.Storage.StorageFolder, filename As String) As IAsyncOperation(Of Boolean)
        Return oFold.LaunchFileAsyncTask(filename).AsAsyncOperation
    End Function

    ''' <summary>
    ''' Run given file
    ''' </summary>
    <Extension()>
    Public Sub LaunchFile(ByVal oFold As Windows.Storage.StorageFolder, filename As String)
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
        oFold.LaunchFileAsyncTask(filename)
#Enable Warning BC42358
    End Sub

    ''' <summary>
    ''' Add/Replace this folder in StorageApplicationPermissions.FutureAccessList
    ''' </summary>
    <Extension()>
    <DefaultOverload>
    Public Sub FutureAccessListAddOrReplace(ByVal oFold As Windows.Storage.StorageFolder, token As String)
        Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, oFold)
    End Sub


#End Region

#Region "StorageFile"
    ''' <summary>
    ''' Run given file
    ''' </summary>
    <Extension()>
    Public Function LaunchAsync(ByVal oFile As Windows.Storage.StorageFile) As IAsyncOperation(Of Boolean)
        Return Windows.System.Launcher.LaunchUriAsync(New Uri(oFile.Path))
    End Function

    ''' <summary>
    ''' Run given file
    ''' </summary>
    <Extension()>
    Public Sub Launch(ByVal oFile As Windows.Storage.StorageFile)
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
        Windows.System.Launcher.LaunchUriAsync(New Uri(oFile.Path))
#Enable Warning BC42358
    End Sub

    ''' <summary>
    ''' Add/Replace this file in StorageApplicationPermissions.FutureAccessList
    ''' </summary>
    <Extension()>
    Public Sub FutureAccessListAddOrReplace(ByVal oFile As Windows.Storage.StorageFile, token As String)
        Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, oFile)
    End Sub

#End Region
#End If

#Region "Uri"

    ''' <summary>
    ''' Open default browser on given URI
    ''' </summary>
    <Extension()>
    Public Sub OpenBrowser(ByVal oUri As System.Uri)
#If PK_WPF Then
        Dim sdpsi As New System.Diagnostics.ProcessStartInfo(oUri.ToString)
        sdpsi.UseShellExecute = True
        System.Diagnostics.Process.Start(sdpsi)
#Else
        oUri.OpenBrowser(False)
#End If
    End Sub

#If Not PK_WPF Then
    ''' <summary>
    ''' Open browser on given URI
    ''' </summary>
    ''' <param name="bForceEdge">True if Edge browser should be forced, False if default browser should be used</param>
    <Extension()>
    Public Sub OpenBrowser(ByVal oUri As System.Uri, bForceEdge As Boolean)
        If bForceEdge Then
            ' tylko w FilteredRss
            Dim options = New Windows.System.LauncherOptions With
                {
                    .TargetApplicationPackageFamilyName = "Microsoft.MicrosoftEdge_8wekyb3d8bbwe"
                }
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
            Windows.System.Launcher.LaunchUriAsync(oUri, options)
#Enable Warning BC42358
        Else

#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
            Windows.System.Launcher.LaunchUriAsync(oUri)
#Enable Warning BC42358
        End If
    End Sub

#End If

#End Region

#Region "WebView"

#If NETFX_CORE Then

    <Extension()>
    Private Async Function GetDocumentHtmlAsyncTask(ByVal uiWebView As WebView) As Task(Of String)
        Try
            Return Await uiWebView.InvokeScriptAsync("eval", New String() {"document.documentElement.outerHTML;"})
        Catch ex As Exception
            Return "" ' jesli strona jest pusta, jest Exception
        End Try
    End Function

    ''' <summary>
    ''' returns WebView contens as a string
    ''' </summary>
    ''' <returns>content, or "" if something is wrong (e.g. page is empty)</returns>
    Public Function GetDocumentHtmlAsync(ByVal uiWebView As WebView) As IAsyncOperation(Of String)
        Return uiWebView.GetDocumentHtmlAsyncTask.AsAsyncOperation
    End Function


#ElseIf Not PK_WPF Then
    ''' <summary>
    ''' returns WebView contens as a string
    ''' </summary>
    ''' <returns>content, or "" if something is wrong (e.g. page is empty)</returns>
    <Extension()>
    Public Async Function GetDocumentHtmlAsyncTask(ByVal uiWebView As WebView2) As Task(Of String)
        Try
            Return Await uiWebView.ExecuteScriptAsync("document.documentElement.outerHTML;")
        Catch ex As Exception
            Return "" ' jesli strona jest pusta, jest Exception
        End Try
    End Function

#End If


#End Region


#Region "ShowAppVersion"

    ''' <summary>
    ''' set item.Text = app version
    ''' </summary>
    ''' <param name="bDebug">if True, adds "(debug" with build datetime</param>
    <Extension()>
    Public Sub ShowAppVers(ByVal oItem As TextBlock, bDebug As Boolean)
        Dim sTxt As String = GetAppVers()
        If bDebug Then sTxt &= " (debug " & GetBuildTimestamp(True) & ")"
        oItem.Text = sTxt
    End Sub

    ''' <summary>
    ''' Adds TextBox named uiPkAutoVersion in center of Page.Grid.row=1 with app version 
    ''' </summary>
    ''' <param name="withDebug">if True, adds "(debug" with build datetime</param>
    ''' <exception cref="ArgumentException">Thrown if Page is not based on Grid</exception>
    <Extension()>
    <DefaultOverload>
    Public Sub ShowAppVers(ByVal oPage As Page, withDebug As Boolean)

        Dim oGrid As Grid = TryCast(oPage.Content, Grid)
        If oGrid Is Nothing Then
            ' skoro to nie Grid, to nie ma jak umiescic koniecznych elementow
            Debug.WriteLine("GetAppVers(null) wymaga Grid jako podstawy Page")
            Throw New ArgumentException("Page.GetAppVers() requires page based on Grid")
        End If

        Dim iCols As Integer = 0
        If oGrid.ColumnDefinitions IsNot Nothing Then iCols = oGrid.ColumnDefinitions.Count ' może być 0
        Dim iRows As Integer = 0
        If oGrid.RowDefinitions IsNot Nothing Then iRows = oGrid.RowDefinitions.Count ' może być 0

        Dim oTB As New TextBlock With {
            .Name = "uiPkAutoVersion",
            .VerticalAlignment = VerticalAlignment.Center,
            .HorizontalAlignment = HorizontalAlignment.Center,
            .FontSize = 10
        }

        If iRows > 2 Then Grid.SetRow(oTB, 1)
        If iCols > 1 Then
            Grid.SetColumn(oTB, 0)
            Grid.SetColumnSpan(oTB, iCols)
        End If
        oGrid.Children.Add(oTB)

        oTB.ShowAppVers(withDebug)
    End Sub
#End Region

#Region "MAUI_ulatwiacz"

#Region "Page navigation"

    ''' <summary>
    ''' Same method for all targets
    ''' </summary>
    <Extension()>
    Public Sub GoBack(ByVal oPage As Page)
#If PK_WPF Then
        If oPage.NavigationService.CanGoBack Then oPage.NavigationService.GoBack()
#Else
        If oPage.Frame Is Nothing Then Return

        If oPage.Frame.CanGoBack Then oPage.Frame.GoBack()
#End If
    End Sub

    ''' <summary>
    ''' Same method for all targets
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oPage As Page, destinationPageType As Type)
#If PK_WPF Then
        Dim newPage As Page = Activator.CreateInstance(destinationPageType) '.GetConstructor(Type.EmptyTypes).Invoke()
        oPage.NavigationService.Navigate(newPage)
#Else
        If oPage.Frame Is Nothing Then Return
        oPage.Navigate(destinationPageType, Nothing)
#End If
    End Sub

    ''' <summary>
    ''' Same method for all targets
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oPage As Page, destinationPageType As Type, parameter As Object)
#If PK_WPF Then
        oPage.NavigationService.Navigate(destinationPageType, parameter)
#Else
        If oPage.Frame Is Nothing Then Return
        oPage.Frame.Navigate(destinationPageType, parameter)
#End If
    End Sub

#End Region

#Region "Window navigation"

#If Not NETFX_CORE And Not PK_WPF Then
    ' WinUI
    ''' <summary>
    ''' Mapping MAUI-styled Navigate to WinUI style
    ''' </summary>
    <Extension()>
    Public Sub GoBack(ByVal oWnd As Window)
        Dim oPage As Page = TryCast(oWnd.Content, Page)
        If oPage Is Nothing Then Return
        If oPage.Frame.CanGoBack Then oPage.Frame.GoBack()
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Navigate to WinUI style
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oWnd As Window, sourcePageType As Type)
        oWnd.Navigate(sourcePageType, Nothing)
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Navigate to WinUI style
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oWnd As Window, sourcePageType As Type, parameter As Object)

        Dim oFrame As Frame = TryCast(oWnd.Content, Frame)
        If oFrame Is Nothing Then
            Dim oPage As Page = TryCast(oWnd.Content, Page)
            If oPage Is Nothing Then Return ' exception

            oFrame = New Frame
            oFrame.Content = oPage
            oWnd.Content = oFrame
        End If

        oFrame.Navigate(sourcePageType, parameter)
    End Sub
#End If

#End Region

    ''' <summary>
    ''' Same method for all targets
    ''' </summary>
    <Extension()>
    Public Sub Show(ByVal oFE As FrameworkElement)
        oFE.Show(True)
    End Sub


    ''' <summary>
    ''' Same method for all targets
    ''' </summary>
    <Extension()>
    Public Sub Show(ByVal oFE As FrameworkElement, show As Boolean)
        If show Then
            oFE.Visibility = Visibility.Visible
        Else
            oFE.Visibility = Visibility.Collapsed
        End If
    End Sub

    ''' <summary>
    ''' Same method for all targets
    ''' </summary>
    <Extension()>
    Public Sub Hide(ByVal oFE As FrameworkElement)
        oFE.Show(False)
    End Sub

#End Region

#Region "FrameworkElement"

    ''' <summary>
    ''' Escapes FrameworkElement from Grid, i.e. set row/col to 0, and rowspan/colspan to count of rows/cols. Use this is you already have Grid.
    ''' </summary>
    ''' <param name="oGrid">grid to escape from</param>
    ''' <param name="fromRows">if TRUE, escape from Rows</param>
    ''' <param name="fromCols">if TRIE, escape from Columns</param>
    <Extension()>
    Public Sub EscapeGrid(ByVal element As FrameworkElement, oGrid As Grid, fromRows As Boolean, fromCols As Boolean)
        Dim iCols As Integer = 0
        If oGrid.ColumnDefinitions IsNot Nothing Then iCols = oGrid.ColumnDefinitions.Count ' może być 0
        Dim iRows As Integer = 0
        If oGrid.RowDefinitions IsNot Nothing Then iRows = oGrid.RowDefinitions.Count ' może być 0

        If fromRows AndAlso iRows > 1 Then
            Grid.SetRow(element, 0)
            Grid.SetRowSpan(element, iRows)
        End If

        If fromCols AndAlso iCols > 1 Then
            Grid.SetColumn(element, 0)
            Grid.SetColumnSpan(element, iCols)
        End If

    End Sub

    ''' <summary>
    ''' Escapes FrameworkElement from Grid, i.e. set row/col to 0, and rowspan/colspan to count of rows/cols. Use this is Grid is to be found.
    ''' </summary>
    ''' <param name="fromRows">if TRUE, escape from Rows</param>
    ''' <param name="fromCols">if TRIE, escape from Columns</param>
    <Extension()>
    Public Sub EscapeGrid(ByVal element As FrameworkElement, fromRows As Boolean, fromCols As Boolean)

        Dim oFE As FrameworkElement = element.Parent
        While oFE IsNot Nothing
            If oFE.GetType Is GetType(Grid) Then
                oFE.EscapeGrid(oFE, fromRows, fromCols)
                Return
            End If
            oFE = oFE.Parent
        End While
    End Sub


#End Region


#Region "ProgressBar/Ring"
    ' dodałem 25 X 2020 (ale do pkmodule, nie do Nuget)

    'Private _mProgRing As ProgressRing = Nothing
    'Private _mProgBar As ProgressBar = Nothing
    Private _mProgRingShowCnt As Integer = 0


    ''' <summary>
    ''' Initialization of ProgressRing (center of Page) and ProgressBar (top of last Page.Grid.Row)
    ''' </summary>
    ''' <param name="bRing">True if ProgressRing should be created</param>
    ''' <param name="bBar">True if ProgressBar should be created</param>
    ''' <exception cref="ArgumentException">Thrown if Page is not based on Grid</exception>
    <Extension()>
    Private Sub ProgRingInitPriv(ByVal oPage As Grid, bRing As Boolean, bBar As Boolean)

        ' tak dziwnie, bo wcześniej było oPage.ProgRingInit, i to wyciągało oGrid
        Dim oGrid As Grid = oPage

        ' 2020.11.24: dodaję force-off do ProgRing na Init
        _mProgRingShowCnt = 0   ' skoro inicjalizuje, to znaczy że na pewno trzeba wyłączyć

        If oPage.FindName("uiPkAutoProgRing") Is Nothing Then
            If bRing Then
                Dim _mProgRing As New ProgressRing With {
                    .Name = "uiPkAutoProgRing",
                    .VerticalAlignment = VerticalAlignment.Center,
                    .HorizontalAlignment = HorizontalAlignment.Center,
                    .Visibility = Visibility.Collapsed
                }
                Canvas.SetZIndex(_mProgRing, 10000)
                _mProgRing.EscapeGrid(oGrid, True, True)
                oGrid.Children.Add(_mProgRing)

#If PK_WPF Then
                oPage.RegisterName("uiPkAutoProgRing", _mProgRing)
#End If
            End If
        End If

        ' tekst jest zawsze, bo i dla Ring i dla Bar jest przydatny
        If oPage.FindName("uiPkAutoProgText") Is Nothing Then

#If PK_WPF Then
            Dim color As Color = Colors.Blue
#Else
            Dim color As Color = oPage.Resources("SystemAccentColor")
#End If
            Dim _mProgText As New TextBlock With {
                .Name = "uiPkAutoProgText",
                .VerticalAlignment = VerticalAlignment.Center,
                .HorizontalAlignment = HorizontalAlignment.Center,
                .Visibility = Visibility.Collapsed,
                .Foreground = New Media.SolidColorBrush(color)
            }
            Canvas.SetZIndex(_mProgText, 10000)
            _mProgText.EscapeGrid(oGrid, True, True)

            oGrid.Children.Add(_mProgText)

#If PK_WPF Then
            oPage.RegisterName("uiPkAutoProgText", _mProgText)
#End If
        End If


        If oPage.FindName("uiPkAutoProgBar") Is Nothing Then
            If bBar Then
                Dim _mProgBar As New ProgressBar With {
                    .Name = "uiPkAutoProgBar",
                    .VerticalAlignment = VerticalAlignment.Bottom,
                    .HorizontalAlignment = HorizontalAlignment.Stretch,
                    .Visibility = Visibility.Collapsed
                }
                Canvas.SetZIndex(_mProgBar, 10000)
                Dim iRows As Integer = 0
                If oGrid.RowDefinitions IsNot Nothing Then iRows = oGrid.RowDefinitions.Count ' może być 0
                If iRows > 1 Then Grid.SetRow(_mProgBar, iRows - 1)
                _mProgBar.EscapeGrid(oGrid, False, True)
                oGrid.Children.Add(_mProgBar)
#If PK_WPF Then
                oPage.RegisterName("uiPkAutoProgBar", _mProgBar)
#End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Initialization of ProgressRing (center of Page) and ProgressBar (top of last Page.Grid.Row); uses existing uiPkAutoProgRing and uiPkAutoProgBar or creates own
    ''' </summary>
    ''' <param name="bRing">True if ProgressRing should be created</param>
    ''' <param name="bBar">True if ProgressBar should be created</param>
    ''' <exception cref="ArgumentException">Thrown if Page is not based on Grid</exception>
    <Extension()>
    Public Sub ProgRingInit(ByVal oPage As Page, bRing As Boolean, bBar As Boolean)
        'Dim oFrame As Frame = Window.Current.Content
        'Dim oWin As Page = oFrame.Content
        Dim oGrid As Grid = TryCast(oPage.Content, Grid)
        If oGrid Is Nothing Then
            ' skoro to nie Grid, to nie ma jak umiescic koniecznych elementow
            Debug.WriteLine("ProgRingInit wymaga Grid jako podstawy Page")
            Throw New ArgumentException("Page.ProgRingInit requires Grid")
        End If

        oGrid.ProgRingInitPriv(bRing, bBar)
    End Sub

#If PK_WPF Then
    ''' <summary>
    ''' Initialization of ProgressRing (center of Page) and ProgressBar (top of last Page.Grid.Row)
    ''' </summary>
    ''' <param name="bRing">True if ProgressRing should be created</param>
    ''' <param name="bBar">True if ProgressBar should be created</param>
    ''' <exception cref="ArgumentException">Thrown if Page is not based on Grid</exception>
    <Extension()>
    Public Sub ProgRingInit(ByVal oWin As Window, bRing As Boolean, bBar As Boolean)
        'Dim oFrame As Frame = Window.Current.Content
        'Dim oWin As Page = oFrame.Content
        Dim oGrid As Grid = TryCast(oWin.Content, Grid)
        If oGrid Is Nothing Then
            ' skoro to nie Grid, to nie ma jak umiescic koniecznych elementow
            Debug.WriteLine("ProgRingInit wymaga Grid jako podstawy Page")
            Throw New ArgumentException("Page.ProgRingInit requires Grid")
        End If

        oGrid.ProgRingInitPriv(bRing, bBar)
    End Sub

#End If

    ''' <summary>
    ''' Show/Hide of ProgressRing/Bar, Bar has Min=0 and Max=100
    ''' </summary>
    <Extension()>
    Public Sub ProgRingShow(ByVal oPage As Page, bVisible As Boolean)
        oPage.ProgRingShow(bVisible, False, 0, 100)
    End Sub

    ''' <summary>
    ''' Forced Show/Hide of ProgressRing/Bar, Bar has Min=0 and Max=100
    ''' </summary>
    ''' <param name="bForce">True if Show/Hide should be done without using nested levels count</param>
    <Extension()>
    Public Sub ProgRingShow(ByVal oPage As Page, bVisible As Boolean, bForce As Boolean)
        oPage.ProgRingShow(bVisible, bForce, 0, 100)
    End Sub

    ''' <summary>
    ''' Show/Hide of ProgressRing/Bar
    ''' </summary>
    ''' <param name="bForce">True if Show/Hide should be done without using nested levels count</param>
    ''' <param name="dMax">Value to be used as ProgressBar.Max (default Min=0)</param>
    <Extension()>
    Public Sub ProgRingShow(ByVal oPage As Page, bVisible As Boolean, bForce As Boolean, dMax As Double)
        oPage.ProgRingShow(bVisible, bForce, 0, dMax)
    End Sub

#If Not PK_WPF Then
    ' UWP, WinUI
    Private _PoprzedniKursor As Windows.UI.Core.CoreCursor
#End If

    ''' <summary>
    ''' Show/Hide of ProgressRing/Bar
    ''' </summary>
    ''' <param name="bForce">True if Show/Hide should be done without using nested levels count</param>
    ''' <param name="dMax">Value to be used as ProgressBar.Max</param>
    ''' <param name="dMin">Value to be used as ProgressBar.Min</param>
    <Extension()>
    Private Sub ProgRingShowPriv(ByVal oPage As FrameworkElement, bVisible As Boolean, bForce As Boolean, dMin As Double, dMax As Double)

        '2021.10.02: tylko gdy jeszcze nie jest pokazywany
        '2021.10.13: gdy min<>max, oraz tylko gdy ma pokazać - inaczej nie zmieniaj zakresu!

        ' FrameworkElement.FindName(String) 

        Dim _mProgBar As ProgressBar = TryCast(oPage.FindName("uiPkAutoProgBar"), ProgressBar)
        If bVisible And _mProgBar IsNot Nothing And _mProgRingShowCnt < 1 Then
            If dMin <> dMax Then
                Try
                    _mProgBar.Minimum = dMin
                    _mProgBar.Value = dMin
                    _mProgBar.Maximum = dMax
                Catch ex As Exception
                End Try
            End If
        End If

        If bForce Then
            If bVisible Then
                _mProgRingShowCnt = 1
            Else
                _mProgRingShowCnt = 0
            End If
        Else
            If bVisible Then
                _mProgRingShowCnt += 1
            Else
                _mProgRingShowCnt -= 1
            End If
        End If
        Debug.WriteLine("ProgRingShow(" & bVisible & ", " & bForce & "...), current ShowCnt=" & _mProgRingShowCnt)

        If _mProgRingShowCnt > 0 Then
#If PK_WPF Then
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait
#Else
            If _PoprzedniKursor Is Nothing Then _PoprzedniKursor = Window.Current.CoreWindow.PointerCursor
            Window.Current.CoreWindow.PointerCursor = New Core.CoreCursor(Core.CoreCursorType.Wait, 1)
#End If
        Else
#If PK_WPF Then
            Mouse.OverrideCursor = Nothing
#ElseIf NETFX_CORE Then
            If _PoprzedniKursor IsNot Nothing Then
                Window.Current.CoreWindow.PointerCursor = _PoprzedniKursor
            End If
#End If
        End If



            Try
            Dim _mProgRing As ProgressRing = TryCast(oPage.FindName("uiPkAutoProgRing"), ProgressRing)
            Dim _mProgText As TextBlock = TryCast(oPage.FindName("uiPkAutoProgText"), TextBlock)

            If _mProgRingShowCnt > 0 Then
                'VBlib.DebugOut("ProgRingShow - mam pokazac")
                If _mProgRing IsNot Nothing Then
                    Dim dSize As Double
                    dSize = (Math.Min(TryCast(_mProgRing.Parent, Grid).ActualHeight, TryCast(_mProgRing.Parent, Grid).ActualWidth)) / 2
                    dSize = Math.Max(dSize, 50) ' głównie na później, dla Android
                    _mProgRing.Width = dSize
                    _mProgRing.Height = dSize

                    _mProgRing.Visibility = Visibility.Visible
                    _mProgRing.IsActive = True
                End If

                If _mProgBar IsNot Nothing Then _mProgBar.Visibility = Visibility.Visible
                If _mProgText IsNot Nothing Then _mProgText.Visibility = Visibility.Visible
            Else
                'VBlib.DebugOut("ProgRingShow - mam ukryc")
                If _mProgRing IsNot Nothing Then
                    _mProgRing.Visibility = Visibility.Collapsed
                    _mProgRing.IsActive = False
                End If
                If _mProgBar IsNot Nothing Then _mProgBar.Visibility = Visibility.Collapsed
                If _mProgText IsNot Nothing Then _mProgText.Visibility = Visibility.Collapsed
            End If

        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Show/Hide of ProgressRing/Bar
    ''' </summary>
    ''' <param name="bForce">True if Show/Hide should be done without using nested levels count</param>
    ''' <param name="dMax">Value to be used as ProgressBar.Max</param>
    ''' <param name="dMin">Value to be used as ProgressBar.Min</param>
    <Extension()>
    Public Sub ProgRingShow(ByVal oPage As Page, bVisible As Boolean, bForce As Boolean, dMin As Double, dMax As Double)
        oPage.ProgRingShowPriv(bVisible, bForce, dMin, dMax)
    End Sub

#If PK_WPF Then
    ''' <summary>
    ''' Show/Hide of ProgressRing/Bar
    ''' </summary>
    ''' <param name="bForce">True if Show/Hide should be done without using nested levels count</param>
    ''' <param name="dMax">Value to be used as ProgressBar.Max</param>
    ''' <param name="dMin">Value to be used as ProgressBar.Min</param>
    <Extension()>
    Public Sub ProgRingShow(ByVal oWin As Window, bVisible As Boolean, bForce As Boolean, dMin As Double, dMax As Double)
        oWin.ProgRingShowPriv(bVisible, bForce, dMin, dMax)
    End Sub

    ''' <summary>
    ''' Show/Hide of ProgressRing/Bar, Bar has Min=0 and Max=100
    ''' </summary>
    <Extension()>
    Public Sub ProgRingShow(ByVal oPage As Window, bVisible As Boolean)
        oPage.ProgRingShowPriv(bVisible, False, 0, 100)
    End Sub

    ''' <summary>
    ''' Forced Show/Hide of ProgressRing/Bar, Bar has Min=0 and Max=100
    ''' </summary>
    ''' <param name="bForce">True if Show/Hide should be done without using nested levels count</param>
    <Extension()>
    Public Sub ProgRingShow(ByVal oPage As Window, bVisible As Boolean, bForce As Boolean)
        oPage.ProgRingShowPriv(bVisible, bForce, 0, 100)
    End Sub

    ''' <summary>
    ''' Show/Hide of ProgressRing/Bar
    ''' </summary>
    ''' <param name="bForce">True if Show/Hide should be done without using nested levels count</param>
    ''' <param name="dMax">Value to be used as ProgressBar.Max (default Min=0)</param>
    <Extension()>
    Public Sub ProgRingShow(ByVal oPage As Window, bVisible As Boolean, bForce As Boolean, dMax As Double)
        oPage.ProgRingShowPriv(bVisible, bForce, 0, dMax)
    End Sub


#End If



    <Extension()>
    Private Sub ProgRingSetTextPriv(ByVal oPage As FrameworkElement, message As String)
        Dim _mProgText As TextBlock = TryCast(oPage.FindName("uiPkAutoProgText"), TextBlock)
        If _mProgText Is Nothing Then
            ' skoro to nie Grid, to nie ma jak umiescic koniecznych elementow
            Debug.WriteLine("ProgRingText called, ale nie ma uiPkAutoProgText")
            Throw New ArgumentException("Page.ProgRingSetText called, but Init was not called")
        End If

        _mProgText.Text = message

    End Sub

    ''' <summary>
    ''' Set message inside ProgressRing
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingSetText(ByVal oPage As Page, message As String)
        oPage.ProgRingSetTextPriv(message)
    End Sub

#If PK_WPF Then
    ''' <summary>
    ''' Set message inside ProgressRing
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingSetText(ByVal oWin As Window, message As String)
        oWin.ProgRingSetTextPriv(message)
    End Sub

#End If


    <Extension()>
    Private Sub ProgRingSetMaxPriv(ByVal oPage As FrameworkElement, dMaxValue As Double)
        Dim _mProgBar As ProgressBar = TryCast(oPage.FindName("uiPkAutoProgBar"), ProgressBar)
        If _mProgBar Is Nothing Then
            ' skoro to nie Grid, to nie ma jak umiescic koniecznych elementow
            Debug.WriteLine("ProgRing(double) wymaga wczesniej ProgRingInit")
            Throw New ArgumentException("Page.ProgRingSetMax called, but Init was not called")
        End If

        _mProgBar.Maximum = dMaxValue

    End Sub

    ''' <summary>
    ''' Set ProgressBar.Max
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingSetMax(ByVal oPage As Page, dMaxValue As Double)
        oPage.ProgRingSetMaxPriv(dMaxValue)
    End Sub

#If PK_WPF Then
    ''' <summary>
    ''' Set ProgressBar.Max
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingSetMax(ByVal oWin As Window, dMaxValue As Double)
        oWin.ProgRingSetMaxPriv(dMaxValue)
    End Sub

#End If

    <Extension()>
    Private Sub ProgRingSetValPriv(ByVal oPage As FrameworkElement, dValue As Double)
        Dim _mProgBar As ProgressBar = TryCast(oPage.FindName("uiPkAutoProgBar"), ProgressBar)
        If _mProgBar Is Nothing Then
            Debug.WriteLine("ProgRing(double) wymaga wczesniej ProgRingInit")
            Throw New ArgumentException("Page.ProgRingSetVal called, but Init was not called")
        End If

        _mProgBar.Value = dValue
        _mProgBar.ToolTip = _mProgBar.Value & "/" & _mProgBar.Maximum

    End Sub

#If PK_WPF Then
    ''' <summary>
    ''' Set ProgressBar.Value
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingSetVal(ByVal oWin As Window, dValue As Double)
        oWin.ProgRingSetValPriv(dValue)
    End Sub

#End If

    ''' <summary>
    ''' Set ProgressBar.Value
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingSetVal(ByVal oPage As Page, dValue As Double)
        oPage.ProgRingSetValPriv(dValue)
    End Sub

    <Extension()>
    Private Sub ProgRingIncPriv(ByVal oFE As FrameworkElement)
        Dim _mProgBar As ProgressBar = TryCast(oFE.FindName("uiPkAutoProgBar"), ProgressBar)
        If _mProgBar Is Nothing Then
            Debug.WriteLine("ProgRing(double) wymaga wczesniej ProgRingInit")
            Throw New ArgumentException("Page.ProgRingInc called, but Init was not called")
        End If

        Dim dVal As Double = _mProgBar.Value + 1
        If dVal > _mProgBar.Maximum Then
            Debug.WriteLine("ProgRingInc na wiecej niz Maximum?")
            _mProgBar.Value = _mProgBar.Maximum
        Else
            _mProgBar.Value = dVal
        End If

        _mProgBar.ToolTip = _mProgBar.Value & "/" & _mProgBar.Maximum
    End Sub


    ''' <summary>
    ''' Increment ProgressBar.Value
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
#If PK_WPF Then
    <Extension()>
    Public Async Function ProgRingInc(ByVal oWin As Window) As Task
        oWin.ProgRingIncPriv
        Await Task.Delay(1)
    End Function


    ''' <summary>
    ''' Increment ProgressBar.Value
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Async Function ProgRingInc(ByVal oPage As Page) As Task
#Else
    <Extension()>
    Public Sub ProgRingInc(ByVal oPage As Page)
#End If
        oPage.ProgRingIncPriv

#If PK_WPF Then
        Await Task.Delay(1)
    End Function
#Else
    End Sub
#End If


#End Region

#Region "dialog boxes"
    Private Function IsInteractive() As Boolean
#If NETFX_CORE Then
        If Window.Current?.Content Is Nothing Then Return False
#ElseIf PK_WPF Then
        If Application.Current.Dispatcher.Thread IsNot Thread.CurrentThread Then Return False
#Else
        ' WinUI
        If Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread Is Nothing Then Return False
#End If
        Return True
    End Function

#If NETFX_CORE Then
    <Extension()>
    Private Async Function MsgBoxAsyncTask(ByVal oPage As FrameworkElement, message As String) As Task
        If Not IsInteractive() Then Return

        Dim oMsg As New Windows.UI.Popups.MessageDialog(TryGetResManString(message))
        Await oMsg.ShowAsync()
    End Function

    ''' <summary>
    ''' Show message on screen, can be awaited
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    <Extension()>
    Public Function MsgBoxAsync(ByVal oPage As FrameworkElement, message As String) As IAsyncAction
        Return oPage.MsgBoxAsyncTask(message).AsAsyncAction
    End Function
#Else

    <Extension()>
    Private Async Function MsgBoxAsyncTask(ByVal oPage As FrameworkElement, message As String) As Task
        If Not IsInteractive() Then Return

#If PK_WPF Then
        Dim oWnd As Window = Window.GetWindow(oPage)
        If oWnd Is Nothing Then
            MessageBox.Show(TryGetResManString(message))
        Else
            MessageBox.Show(Window.GetWindow(oPage), TryGetResManString(message))
        End If
#Else
        Await oPage.DialogBoxYNAsync(message, Nothing, "OK")
#End If
    End Function

    ''' <summary>
    ''' Show message on screen, can be awaited
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    <Extension()>
    Public Async Function MsgBoxAsync(ByVal oPage As FrameworkElement, message As String) As Task
        Await oPage.MsgBoxAsyncTask(message)
    End Function

#End If

    ''' <summary>
    ''' Show message on screen
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    <Extension()>
    Public Sub MsgBox(ByVal oPage As FrameworkElement, message As String)
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
        oPage.MsgBoxAsyncTask(message)
#Enable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
    End Sub


#If NETFX_CORE Then

    ''' <summary>
    ''' Show message on screen, and waits for Yes/No buttons
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <returns>TRUE if YES button was pressed</returns>
    <Extension()>
    Public Function DialogBoxYNAsync(ByVal oPage As FrameworkElement, message As String) As IAsyncOperation(Of Boolean)
        Return oPage.DialogBoxYNAsyncTask(message, "Yes", "No").AsAsyncOperation
    End Function


    ''' <summary>
    ''' Show message on screen, and waits for Yes/No buttons
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <param name="buttonYes">text to be shown instead of "Yes"</param>
    ''' <param name="buttonNo">text to be shown instead of "No"</param>
    ''' <returns>TRUE if YES button was pressed</returns>
    <Extension()>
    Public Function DialogBoxYNAsync(ByVal oPage As FrameworkElement, message As String, buttonYes As String, buttonNo As String) As IAsyncOperation(Of Boolean)
        Return oPage.DialogBoxYNAsyncTask(message, buttonYes, buttonNo).AsAsyncOperation
    End Function

    <Extension()>
    Private Async Function DialogBoxYNAsyncTask(ByVal oPage As FrameworkElement, sMsg As String, sYes As String, sNo As String) As Task(Of Boolean)
        If Not IsInteractive() Then Return False

        Dim oMsg As New Windows.UI.Popups.MessageDialog(TryGetResManString(sMsg))
        Dim oYes As New Windows.UI.Popups.UICommand(TryGetResManString(sYes))
        Dim oNo As New Windows.UI.Popups.UICommand(TryGetResManString(sNo))
        oMsg.Commands.Add(oYes)
        oMsg.Commands.Add(oNo)
        oMsg.DefaultCommandIndex = 1    ' default: No
        oMsg.CancelCommandIndex = 1
        Dim oCmd As Windows.UI.Popups.IUICommand = Await oMsg.ShowAsync
        If oCmd Is Nothing Then Return False
        If oCmd.Label = sYes Then Return True
        Return False
    End Function
#Else
    ''' <summary>
    ''' Show message on screen, and waits for Yes/No buttons
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <param name="buttonYes">text to be shown instead of "Yes"</param>
    ''' <param name="buttonNo">text to be shown instead of "No"</param>
    ''' <returns>TRUE if YES button was pressed</returns>
    <Extension()>
    Public Async Function DialogBoxYNAsync(ByVal oPage As FrameworkElement, message As String, Optional buttonYes As String = "Yes", Optional buttonNo As String = "No") As Task(Of Boolean)
        If Not IsInteractive() Then Return False

#If PK_WPF Then
        Dim sAppName As String = Application.Current.MainWindow.GetType().Assembly.GetName.Name
        Dim iRet As MessageBoxResult = MessageBox.Show(
            TryGetResManString(message), sAppName, MessageBoxButton.YesNo)
        Return iRet = MessageBoxResult.Yes
#Else


        Dim oDlg As New ContentDialog With {
            .Content = TryGetResManString(message),
            .PrimaryButtonText = TryGetResManString(buttonNo),
            .SecondaryButtonText = TryGetResManString(buttonYes),
            .DefaultButton = ContentDialogButton.Primary
        }

        oDlg.XamlRoot = oPage.XamlRoot
        Dim oCmd = Await oDlg.ShowAsync
        Return oCmd = ContentDialogResult.Secondary
#End If
    End Function

#End If
#If PK_WPF Then
    ''' <summary>
    ''' Show message on screen, waits for Continue/Cancel buttons and return text entered by user
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <param name="defaultText">default value</param>
    ''' <param name="buttonContinue">text to be shown instead of "Continue"</param>
    ''' <param name="buttonCancel">text to be shown instead of "Cancel"</param>
    ''' <returns>text entered by user, or empty if Cancel was pressed</returns>
    <Extension()>
    Public Async Function InputBoxAsync(ByVal oPage As FrameworkElement, message As String, Optional defaultText As String = "", Optional buttonContinue As String = "Continue", Optional buttonCancel As String = "Cancel") As Task(Of String)
        If Not IsInteractive() Then Return ""

        Dim sAppName As String = Application.Current.MainWindow.GetType().Assembly.GetName.Name

        Dim oStack As New StackPanel With {.Margin = New Thickness(5, 5, 5, 5)}
        Dim oTxt As New TextBlock With {.Text = message}
        oStack.Children.Add(oTxt)

        Dim oBox As New TextBox With {.Text = defaultText, .Margin = New Thickness(5, 5, 5, 5)}
        oStack.Children.Add(oBox)

        Dim oButt As New Button With
            {
            .Content = " " & buttonContinue & " ",
            .HorizontalAlignment = HorizontalAlignment.Center,
            .Margin = New Thickness(5, 10, 5, 5),
            .FontSize = 14,
            .IsDefault = True
            }
        AddHandler oButt.Click, AddressOf pkmoduleInputBox_Click

        oStack.Children.Add(oButt)

        Dim oWnd As New Window With {.Height = 140, .Width = 400}
        oWnd.Content = oStack

        oBox.Focus()

        Dim bRet = oWnd.ShowDialog

        If Not bRet Then Return ""

        Return oBox.Text

    End Function

    Private Sub pkmoduleInputBox_Click(sender As Object, e As RoutedEventArgs)

        Dim oFE As FrameworkElement = sender

        Dim guard As Integer = 5

        Do
            Dim oWnd As Window = TryCast(oFE, Window)
            If oWnd IsNot Nothing Then
                oWnd.DialogResult = True
                oWnd.Close()
                Exit Do
            End If
            oFE = oFE.Parent
            guard -= 1
        Loop While guard > 0
    End Sub

#Else

#If NETFX_CORE Then

    ''' <summary>
    ''' Show message on screen, waits for Continue/Cancel buttons and return text entered by user
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <returns>text entered by user, or empty if Cancel was pressed</returns>
    <Extension()>
    Public Function InputBoxAsync(ByVal oPage As FrameworkElement, message As String) As IAsyncOperation(Of String)
        Return oPage.InputBoxAsync(message, "")
    End Function


    ''' <summary>
    ''' Show message on screen, waits for Continue/Cancel buttons and return text entered by user
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <param name="defaultText">default value</param>
    ''' <returns>text entered by user, or empty if Cancel was pressed</returns>
    <Extension()>
    Public Function InputBoxAsync(ByVal oPage As FrameworkElement, message As String, defaultText As String) As IAsyncOperation(Of String)
        Return oPage.InputBoxAsyncTask(message, defaultText, "Continue", "Cancel").AsAsyncOperation
    End Function

    ''' <summary>
    ''' Show message on screen, waits for Continue/Cancel buttons and return text entered by user
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <param name="defaultText">default value</param>
    ''' <param name="buttonContinue">text to be shown instead of "Continue"</param>
    ''' <param name="buttonCancel">text to be shown instead of "Cancel"</param>
    ''' <returns>text entered by user, or empty if Cancel was pressed</returns>
    <Extension()>
    Public Function InputBoxAsync(ByVal oPage As FrameworkElement, message As String, defaultText As String, buttonContinue As String, buttonCancel As String) As IAsyncOperation(Of String)
        Return oPage.InputBoxAsyncTask(message, defaultText, buttonContinue, buttonCancel).AsAsyncOperation
    End Function


    <Extension()>
    Private Async Function InputBoxAsyncTask(ByVal oPage As FrameworkElement, message As String, defaultText As String, buttonContinue As String, buttonCancel As String) As Task(Of String)
#Else
    ''' <summary>
    ''' Show message on screen, waits for Continue/Cancel buttons and return text entered by user
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <param name="defaultText">default value</param>
    ''' <param name="buttonContinue">text to be shown instead of "Continue"</param>
    ''' <param name="buttonCancel">text to be shown instead of "Cancel"</param>
    ''' <returns>text entered by user, or empty if Cancel was pressed</returns>

    <Extension()>
    Public Async Function InputBoxAsync(ByVal oPage As FrameworkElement, message As String, Optional defaultText As String = "", Optional buttonContinue As String = "Continue", Optional buttonCancel As String = "Cancel") As Task(Of String)

#End If
        If Not IsInteractive() Then Return ""

        Dim oInputTextBox = New TextBox With {
            .AcceptsReturn = False,
            .Text = TryGetResManString(defaultText),
            .IsSpellCheckEnabled = False
        }

        Dim oDlg As New ContentDialog With {
            .Content = oInputTextBox,
            .PrimaryButtonText = TryGetResManString(buttonContinue),
            .SecondaryButtonText = TryGetResManString(buttonCancel),
            .Title = TryGetResManString(message)
        }

#If Not NETFX_CORE Then
        oDlg.XamlRoot = oPage.XamlRoot
#End If
        Dim oCmd = Await oDlg.ShowAsync
        If oCmd <> ContentDialogResult.Primary Then Return ""

        Return oInputTextBox.Text

    End Function
#End If


#End Region


#Region "localizing strings"

    ''' <summary>
    ''' Try to localize properties (Localize.SetPropertiesUsingObjectName)
    ''' </summary>
    <Extension()>
    Public Sub LocalizePropertiesUsingObjectName(ByVal element As DependencyObject, recursive As Boolean)
        Localize.SetPropertiesUsingObjectName(element)
        If Not recursive Then Return
        For iLp = 0 To VisualTreeHelper.GetChildrenCount(element)
            VisualTreeHelper.GetChild(element, iLp).LocalizePropertiesUsingObjectName(True)
        Next
    End Sub

    ''' <summary>
    ''' Try to localize properties (Localize.SetPrefixedProperties)
    ''' </summary>
    <Extension()>
    Public Sub LocalizePrefixedProperties(ByVal element As DependencyObject, recursive As Boolean)
        Localize.SetPrefixedProperties(element)
        If Not recursive Then Return
        For iLp = 0 To VisualTreeHelper.GetChildrenCount(element)
            VisualTreeHelper.GetChild(element, iLp).LocalizePrefixedProperties(True)
        Next
    End Sub

    ''' <summary>
    ''' Try to localize properties (Localize.SetPropertiesUsingObjectName)
    ''' </summary>
    <Extension()>
    Public Sub LocalizePropertiesUsingObjectName(ByVal element As DependencyObject)
        element.LocalizePropertiesUsingObjectName(False)
    End Sub

    ''' <summary>
    ''' Try to localize properties (Localize.SetPrefixedProperties)
    ''' </summary>
    <Extension()>
    Public Sub LocalizePrefixedProperties(ByVal element As DependencyObject)
        element.LocalizePrefixedProperties(False)
    End Sub
#End Region


#Region "fonts"

#If PK_WPF Then
    <Extension>
    Public Function SupportsUnicodeChar(ByVal czcionka As GlyphTypeface, codepoint As Integer) As Boolean
        Dim dummy As UShort
        Return czcionka.CharacterToGlyphMap.TryGetValue(codepoint, dummy)
    End Function

    <Extension>
    Public Function SupportsUnicodeChar(ByVal czcionka As Typeface, codepoint As Integer) As Boolean
        ' typeface: TImes New Roman Bold ...
        Dim dummy As GlyphTypeface = Nothing
        If Not czcionka.TryGetGlyphTypeface(dummy) Then Return False
        Return dummy.SupportsUnicodeChar(codepoint)
    End Function

    <Extension>
    Public Function SupportsUnicodeChar(ByVal rodzina As FontFamily, codepoint As Integer) As Boolean
        ' family: Times New Roman
        For Each czcionka As Typeface In rodzina.GetTypefaces
            If czcionka.SupportsUnicodeChar(codepoint) Then Return True
        Next
        Return False

    End Function


#End If

#End Region


#End Region

#Region "not-UI related"

#Region "Dla kompatybilności ze starymi app"

#If Not PK_WPF Then

#Region "Read/Write text"



#Region "StorageFile"

    ''' <summary>
    ''' Writes string to file, using Utf8 encoding
    ''' </summary>
#If NETFX_CORE Then
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function WriteAllTextAsync(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As IAsyncAction
        Return oFile.WriteAllTextAsyncTask(sTxt).AsAsyncAction
    End Function

    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Private Async Function WriteAllTextAsyncTask(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task
#Else
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function WriteAllTextAsync(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task
#End If
        Dim oStream As Stream = Await oFile.OpenStreamForWriteAsync
        Using oWriter As New Windows.Storage.Streams.DataWriter(oStream.AsOutputStream) With {
            .UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8
        }
            oWriter.WriteString(sTxt)
            Await oWriter.FlushAsync()
            Await oWriter.StoreAsync()
        End Using

        Try
            oStream.Dispose()
        Catch ex As Exception

        End Try

    End Function

    ''' <summary>
    ''' Appends string and \r\n to file, using Utf8 encoding
    ''' </summary>
    ''' <returns>False if file cannot be opened</returns>
#If NETFX_CORE Then
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function AppendLineAsync(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As IAsyncAction
        Return oFile.AppendLineAsyncTask(sTxt).AsAsyncAction
    End Function

    <Extension()>
    Private Async Function AppendLineAsyncTask(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task
        Await oFile.AppendStringAsyncTask(sTxt & vbCrLf)
#Else
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function AppendLineAsync(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task
        Await oFile.AppendStringAsync(sTxt & vbCrLf)
#End If
    End Function


    ''' <summary>
    ''' Appends string to file, using Utf8 encoding
    ''' </summary>
    ''' <returns>False if file cannot be opened</returns>
#If NETFX_CORE Then

    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function AppendStringAsync(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As IAsyncOperation(Of Boolean)
        Return oFile.AppendStringAsyncTask(sTxt).AsAsyncOperation
    End Function

    <Extension()>
    Private Async Function AppendStringAsyncTask(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task(Of Boolean)
#Else
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function AppendStringAsync(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task(Of Boolean)
#End If

        Dim oStream As Stream

        Try
            oStream = Await oFile.OpenStreamForWriteAsync
        Catch ex As Exception
            Return False ' mamy błąd otwarcia pliku
        End Try

        oStream.Seek(0, SeekOrigin.End)
        Using oWriter As New Windows.Storage.Streams.DataWriter(oStream.AsOutputStream) With {
            .UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8
        }
            oWriter.WriteString(sTxt)
            Await oWriter.FlushAsync()
            Await oWriter.StoreAsync()
        End Using

        Try
            oStream.Dispose()
        Catch ex As Exception

        End Try

        Return True
    End Function

#End Region

#Region "StorageFolder + filename"

#If NETFX_CORE Then

    <Extension()>
    Private Async Function WriteAllTextToFileAsyncTask(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String, sTxt As String, oOption As Windows.Storage.CreationCollisionOption) As Task
        Dim oFile As Windows.Storage.StorageFile = Await oFold.CreateFileAsync(sFileName, oOption)
        If oFile Is Nothing Then Return

        Await oFile.WriteAllTextAsyncTask(sTxt)
    End Function

    ''' <summary>
    ''' Writes string to file, using Utf8 encoding
    ''' </summary>
    ''' <param name="sFileName">name of file</param>
    ''' <param name="sTxt">string to be written</param>
    ''' <param name="oOption">option for CreateFileAsync</param>
    ''' <returns></returns>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function WriteAllTextToFileAsync(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String, sTxt As String, oOption As Windows.Storage.CreationCollisionOption) As IAsyncAction
        Return oFold.WriteAllTextToFileAsyncTask(sFileName, sTxt, oOption).AsAsyncAction
    End Function


    ''' <summary>
    ''' Writes string to file, using Utf8 encoding; file is opened with FailIfExists
    ''' </summary>
    ''' <param name="sFileName">name of file</param>
    ''' <param name="sTxt">string to be written</param>
    ''' <returns></returns>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function WriteAllTextToFileAsync(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String, sTxt As String) As IAsyncAction
        Return oFold.WriteAllTextToFileAsyncTask(sFileName, sTxt, Windows.Storage.CreationCollisionOption.FailIfExists).AsAsyncAction
    End Function

#Else

    ''' <summary>
    ''' Writes string to file, using Utf8 encoding
    ''' </summary>
    ''' <param name="sFileName">name of file</param>
    ''' <param name="sTxt">string to be written</param>
    ''' <param name="oOption">option for CreateFileAsync</param>
    ''' <returns></returns>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function WriteAllTextToFileAsync(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String, sTxt As String, Optional oOption As Windows.Storage.CreationCollisionOption = Windows.Storage.CreationCollisionOption.FailIfExists) As Task
        Dim oFile As Windows.Storage.StorageFile = Await oFold.CreateFileAsync(sFileName, oOption)
        If oFile Is Nothing Then Return

        Await oFile.WriteAllTextAsync(sTxt)
    End Function

#End If


    ''' <summary>
    ''' Return contens of given file as a string
    ''' </summary>
#If NETFX_CORE Then
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function ReadAllTextAsync(ByVal oFile As Windows.Storage.StorageFile) As IAsyncOperation(Of String)
        Return oFile.ReadAllTextAsyncTask.AsAsyncOperation
    End Function

    <Extension()>
    Private Async Function ReadAllTextAsyncTask(ByVal oFile As Windows.Storage.StorageFile) As Task(Of String)
#Else
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function ReadAllTextAsync(ByVal oFile As Windows.Storage.StorageFile) As Task(Of String)
#End If
        ' zamiast File.ReadAllText(oFile.Path)
        Dim sTxt As String

        Using oStream As Stream = Await oFile.OpenStreamForReadAsync
            Using oReader As New Windows.Storage.Streams.DataReader(oStream.AsInputStream) With {
            .UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8
        }
                Dim iSize As Integer = oStream.Length
                Await oReader.LoadAsync(iSize)
                sTxt = oReader.ReadString(iSize)
                oReader.Dispose()
            End Using
        End Using
        Return sTxt
    End Function


    ''' <summary>
    ''' Return contens of given file as a string
    ''' </summary>
    ''' <returns>File contens, or NULL if file not exists</returns>
#If NETFX_CORE Then
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function ReadAllTextFromFileAsync(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As IAsyncOperation(Of String)
        Return oFold.ReadAllTextFromFileAsyncTask(sFileName).AsAsyncOperation
    End Function

    <Extension()>
    Private Async Function ReadAllTextFromFileAsyncTask(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As Task(Of String)
#Else
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function ReadAllTextFromFileAsync(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As Task(Of String)
#End If

        Dim oFile As Windows.Storage.StorageFile = Await oFold.TryGetItemAsync(sFileName)
        If oFile Is Nothing Then Return Nothing
        Return Await oFile.ReadAllTextAsync
    End Function

#End Region

#End Region
    ''' <summary>
    ''' Check if given file exists in folder
    ''' </summary>
#If NETFX_CORE Then

    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function FileExistsAsync(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As IAsyncOperation(Of Boolean)
        Return oFold.FileExistsAsyncTask(sFileName).AsAsyncOperation
    End Function


    <Extension()>
    Private Async Function FileExistsAsyncTask(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As Task(Of Boolean)
#Else
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function FileExistsAsync(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As Task(Of Boolean)
#End If
        Try
            Dim oTemp As Windows.Storage.StorageFile = Await oFold.TryGetItemAsync(sFileName)
            If oTemp Is Nothing Then Return False
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End If

#End Region

#Region "clipboard"

    <Extension()>
    Public Sub SendToClipboard(ByVal text As String)
#If PK_WPF Then
        Clipboard.SetText(text)
#Else
        Try

            Dim oClipCont As New DataPackage With {
                .RequestedOperation = DataPackageOperation.Copy
            }
            oClipCont.SetText(text)
            Clipboard.SetContent(oClipCont)
        Catch ex As Exception
            ' czasem daje "Not enough memory resources are available to process this command. (Exception from HRESULT: 0x80070008)"
        End Try
#End If
    End Sub

#End Region

#End Region


End Module
