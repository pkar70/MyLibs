﻿Imports System
Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.UI.Xaml
Imports Microsoft.UI.Xaml.Controls
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Foundation
Partial Public Module extensions

#Region "UI related"

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

#End Region

#Region "Uri"

    ''' <summary>
    ''' Open default browser on given URI
    ''' </summary>
    <Extension()>
    Public Sub OpenBrowser(ByVal oUri As System.Uri)
        oUri.OpenBrowser(False)
    End Sub

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

#End Region

#Region "WebView"

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
            .VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center,
            .HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
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

    ''' <summary>
    ''' Mapping MAUI-styled GoBack to WinUI style
    ''' </summary>
    <Extension()>
    Public Sub GoBack(ByVal oPage As Page)
        If oPage.Frame Is Nothing Then Return
        If oPage.Frame.CanGoBack Then oPage.Frame.GoBack()
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Navigate to WinUI style
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oPage As Page, sourcePageType As Type)
        oPage.Navigate(sourcePageType, Nothing)
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Navigate to WinUI style
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oPage As Page, sourcePageType As Type, parameter As Object)
        If oPage.Frame Is Nothing Then Return
        oPage.Frame.Navigate(sourcePageType, parameter)
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled GoBack to WinUI style
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


    ''' <summary>
    ''' Mapping MAUI-styled Show to WinUI style
    ''' </summary>
    <Extension()>
    Public Sub Show(ByVal oFE As FrameworkElement)
        oFE.Show(True)
    End Sub


    ''' <summary>
    ''' Mapping MAUI-styled Show to WinUI style
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
    ''' Mapping MAUI-styled Hide to WinUI style
    ''' </summary>
    <Extension()>
    Public Sub Hide(ByVal oFE As FrameworkElement)
        oFE.Show(False)
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
    Public Sub ProgRingInit(ByVal oPage As Page, bRing As Boolean, bBar As Boolean)

        ' 2020.11.24: dodaję force-off do ProgRing na Init
        _mProgRingShowCnt = 0   ' skoro inicjalizuje, to znaczy że na pewno trzeba wyłączyć

        'Dim oFrame As Frame = Window.Current.Content
        'Dim oPage As Page = oFrame.Content
        Dim oGrid As Grid = TryCast(oPage.Content, Grid)
        If oGrid Is Nothing Then
            ' skoro to nie Grid, to nie ma jak umiescic koniecznych elementow
            Debug.WriteLine("ProgRingInit wymaga Grid jako podstawy Page")
            Throw New ArgumentException("Page.ProgRingInit requires Grid")
        End If

        Dim iCols As Integer = 0
        If oGrid.ColumnDefinitions IsNot Nothing Then iCols = oGrid.ColumnDefinitions.Count ' może być 0
        Dim iRows As Integer = 0
        If oGrid.RowDefinitions IsNot Nothing Then iRows = oGrid.RowDefinitions.Count ' może być 0

        If oPage.FindName("uiPkAutoProgRing") Is Nothing Then
            If bRing Then
                Dim _mProgRing As New ProgressRing With {
                    .Name = "uiPkAutoProgRing",
                    .VerticalAlignment = VerticalAlignment.Center,
                    .HorizontalAlignment = HorizontalAlignment.Center,
                    .Visibility = Visibility.Collapsed
                }
                Canvas.SetZIndex(_mProgRing, 10000)
                If iRows > 1 Then
                    Grid.SetRow(_mProgRing, 0)
                    Grid.SetRowSpan(_mProgRing, iRows)
                End If
                If iCols > 1 Then
                    Grid.SetColumn(_mProgRing, 0)
                    Grid.SetColumnSpan(_mProgRing, iCols)
                End If
                oGrid.Children.Add(_mProgRing)
            End If
        End If

        ' tekst jest zawsze, bo i dla Ring i dla Bar jest przydatny
        If oPage.FindName("uiPkAutoProgText") Is Nothing Then

            Dim color As Windows.UI.Color = oPage.Resources("SystemAccentColor")

            Dim _mProgText As New TextBlock With {
                .Name = "uiPkAutoProgText",
                .VerticalAlignment = VerticalAlignment.Center,
                .HorizontalAlignment = HorizontalAlignment.Center,
                .Visibility = Visibility.Collapsed,
                .Foreground = New Media.SolidColorBrush(color)
            }
            Canvas.SetZIndex(_mProgText, 10000)
            If iRows > 1 Then
                Grid.SetRow(_mProgText, 0)
                Grid.SetRowSpan(_mProgText, iRows)
            End If
            If iCols > 1 Then
                Grid.SetColumn(_mProgText, 0)
                Grid.SetColumnSpan(_mProgText, iCols)
            End If
            oGrid.Children.Add(_mProgText)
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
                If iRows > 1 Then Grid.SetRow(_mProgBar, iRows - 1)
                If iCols > 1 Then
                    Grid.SetColumn(_mProgBar, 0)
                    Grid.SetColumnSpan(_mProgBar, iCols)
                End If
                oGrid.Children.Add(_mProgBar)
            End If
        End If

    End Sub

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

    ''' <summary>
    ''' Show/Hide of ProgressRing/Bar
    ''' </summary>
    ''' <param name="bForce">True if Show/Hide should be done without using nested levels count</param>
    ''' <param name="dMax">Value to be used as ProgressBar.Max</param>
    ''' <param name="dMin">Value to be used as ProgressBar.Min</param>
    <Extension()>
    Public Sub ProgRingShow(ByVal oPage As Page, bVisible As Boolean, bForce As Boolean, dMin As Double, dMax As Double)

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
    ''' Set message inside ProgressRing
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingSetText(ByVal oPage As Page, message As String)
        Dim _mProgText As TextBlock = TryCast(oPage.FindName("uiPkAutoProgText"), TextBlock)
        If _mProgText Is Nothing Then
            ' skoro to nie Grid, to nie ma jak umiescic koniecznych elementow
            Debug.WriteLine("ProgRingText called, ale nie ma uiPkAutoProgText")
            Throw New ArgumentException("Page.ProgRingSetText called, but Init was not called")
        End If

        _mProgText.Text = message

    End Sub

    ''' <summary>
    ''' Set ProgressBar.Max
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingSetMax(ByVal oPage As Page, dMaxValue As Double)
        Dim _mProgBar As ProgressBar = TryCast(oPage.FindName("uiPkAutoProgBar"), ProgressBar)
        If _mProgBar Is Nothing Then
            ' skoro to nie Grid, to nie ma jak umiescic koniecznych elementow
            Debug.WriteLine("ProgRing(double) wymaga wczesniej ProgRingInit")
            Throw New ArgumentException("Page.ProgRingSetMax called, but Init was not called")
        End If

        _mProgBar.Maximum = dMaxValue

    End Sub

    ''' <summary>
    ''' Set ProgressBar.Value
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingSetVal(ByVal oPage As Page, dValue As Double)
        Dim _mProgBar As ProgressBar = TryCast(oPage.FindName("uiPkAutoProgBar"), ProgressBar)
        If _mProgBar Is Nothing Then
            Debug.WriteLine("ProgRing(double) wymaga wczesniej ProgRingInit")
            Throw New ArgumentException("Page.ProgRingSetVal called, but Init was not called")
        End If

        _mProgBar.Value = dValue

    End Sub

    ''' <summary>
    ''' Increment ProgressBar.Value
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Sub ProgRingInc(ByVal oPage As Page)
        Dim _mProgBar As ProgressBar = TryCast(oPage.FindName("uiPkAutoProgBar"), ProgressBar)
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

    End Sub


#End Region

#Region "dialog boxes"
    Private Function IsInteractive() As Boolean
        If Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread Is Nothing Then Return False
        ' UWP: If Window.Current?.Content Is Nothing Then Return False ; w nie UWP zawsze NULL dla desktop
        Return True
    End Function


    ''' <summary>
    ''' Show message on screen, can be awaited
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    <Extension()>
    Public Async Function MsgBoxAsync(ByVal oPage As FrameworkElement, message As String) As Task
        If Not IsInteractive() Then Return

        Await oPage.DialogBoxYNAsync(message, Nothing, "OK")
    End Function


    ''' <summary>
    ''' Show message on screen
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    <Extension()>
    Public Sub MsgBox(ByVal oPage As FrameworkElement, message As String)
        oPage.MsgBoxAsync(message)
    End Sub

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

        Dim oDlg As New ContentDialog With {
            .Content = message,
            .PrimaryButtonText = buttonNo,
            .SecondaryButtonText = buttonYes,
            .DefaultButton = ContentDialogButton.Primary
        }

        oDlg.XamlRoot = oPage.XamlRoot
        Dim oCmd = Await oDlg.ShowAsync
        Return oCmd = ContentDialogResult.Secondary

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
    Public Async Function InputBoxAsync(ByVal oPage As FrameworkElement, message As String, Optional defaultText As String = "", Optional buttonContinue As String = "Continue", Optional buttonCancel As String = "Cancel") As Task(Of String)
        If Not IsInteractive() Then Return ""

        Dim oInputTextBox = New TextBox With {
            .AcceptsReturn = False,
            .Text = defaultText,
            .IsSpellCheckEnabled = False
        }

        Dim oDlg As New ContentDialog With {
            .Content = oInputTextBox,
            .PrimaryButtonText = buttonContinue,
            .SecondaryButtonText = buttonCancel,
            .Title = message
        }

        oDlg.XamlRoot = oPage.XamlRoot
        Dim oCmd = Await oDlg.ShowAsync
        If oCmd <> ContentDialogResult.Primary Then Return ""

        Return oInputTextBox.Text

    End Function



#End Region

#End Region

#Region "not-UI related"

#Region "Dla kompatybilności ze starymi app"
#Region "Read/Write text"

    ''' <summary>
    ''' Writes string to file, using Utf8 encoding
    ''' </summary>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function WriteAllTextAsyncTask(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task
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
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function AppendLineAsyncTask(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task
        Await oFile.AppendStringAsyncTask(sTxt & vbCrLf)
    End Function

    ''' <summary>
    ''' Appends string to file, using Utf8 encoding
    ''' </summary>
    ''' <returns>False if file cannot be opened</returns>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function AppendStringAsyncTask(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task(Of Boolean)

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


    ''' <summary>
    ''' Writes string to file, using Utf8 encoding
    ''' </summary>
    ''' <param name="sFileName">name of file</param>
    ''' <param name="sTxt">string to be written</param>
    ''' <param name="oOption">option for CreateFileAsync</param>
    ''' <returns></returns>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function WriteAllTextToFileAsyncTask(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String, sTxt As String, Optional oOption As Windows.Storage.CreationCollisionOption = Windows.Storage.CreationCollisionOption.FailIfExists) As Task
        Dim oFile As Windows.Storage.StorageFile = Await oFold.CreateFileAsync(sFileName, oOption)
        If oFile Is Nothing Then Return

        Await oFile.WriteAllTextAsyncTask(sTxt)
    End Function


    ''' <summary>
    ''' Return contens of given file as a string
    ''' </summary>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function ReadAllTextAsyncTask(ByVal oFile As Windows.Storage.StorageFile) As Task(Of String)
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
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function ReadAllTextFromFileAsyncTask(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As Task(Of String)
        Dim oFile As Windows.Storage.StorageFile = Await oFold.TryGetItemAsync(sFileName)
        If oFile Is Nothing Then Return Nothing
        Return Await oFile.ReadAllTextAsyncTask
    End Function

#End Region

    ''' <summary>
    ''' Check if given file exists in folder
    ''' </summary>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Async Function FileExistsAsyncTask(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As Task(Of Boolean)
        Try
            Dim oTemp As Windows.Storage.StorageFile = Await oFold.TryGetItemAsync(sFileName)
            If oTemp Is Nothing Then Return False
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "clipboard"

    ''' <summary>
    ''' send String to clipboard
    ''' </summary>
    <Extension()>
    Public Sub SendToClipboard(ByVal text As String)
        Try

            Dim oClipCont As New DataPackage With {
                .RequestedOperation = DataPackageOperation.Copy
            }
            oClipCont.SetText(text)
            Clipboard.SetContent(oClipCont)
        Catch ex As Exception
            ' czasem daje "Not enough memory resources are available to process this command. (Exception from HRESULT: 0x80070008)"
        End Try
    End Sub
#End Region


#End Region


End Module
