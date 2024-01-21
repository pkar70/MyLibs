
Imports System.ServiceModel.Channels

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
    <Metadata.DefaultOverload>
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

    ''' <summary>
    ''' Mapping MAUI-styled GoBack to UWP style
    ''' </summary>
    <Extension()>
    Public Sub GoBack(ByVal oPage As Page)
        If oPage.Frame.CanGoBack Then oPage.Frame.GoBack()
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Navigate to UWP style
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oPage As Page, sourcePageType As Type)
        oPage.Frame.Navigate(sourcePageType)
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Navigate to UWP style
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oPage As Page, sourcePageType As Type, parameter As Object)
        oPage.Frame.Navigate(sourcePageType, parameter)
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Show to UWP style
    ''' </summary>
    <Extension()>
    Public Sub Show(ByVal oFE As FrameworkElement)
        oFE.Show(True)
    End Sub


    ''' <summary>
    ''' Mapping MAUI-styled Show to UWP style
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
    ''' Mapping MAUI-styled Hide to UWP style
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
                .Foreground = New Windows.UI.Xaml.Media.SolidColorBrush(color)
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
        If Window.Current?.Content Is Nothing Then Return False
        Return True
    End Function

    <Extension()>
    Private Async Function MsgBoxAsyncTask(ByVal oPage As FrameworkElement, message As String) As Task
        If Not IsInteractive() Then Return

        Dim oMsg As New Windows.UI.Popups.MessageDialog(message)
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


    ''' <summary>
    ''' Show message on screen
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    <Extension()>
    Public Sub MsgBox(ByVal oPage As FrameworkElement, message As String)
        oPage.MsgBoxAsyncTask(message)
    End Sub

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
    Private Async Function InputBoxAsyncTask(ByVal oPage As FrameworkElement, sMsg As String, sDefault As String, sYes As String, sNo As String) As Task(Of String)
        If Not IsInteractive() Then Return ""


        Dim oInputTextBox = New TextBox With {
            .AcceptsReturn = False,
            .Text = sDefault,
            .IsSpellCheckEnabled = False
        }

        Dim oDlg As New ContentDialog With {
            .Content = oInputTextBox,
            .PrimaryButtonText = sYes,
            .SecondaryButtonText = sNo,
            .Title = sMsg
        }

        Dim oCmd = Await oDlg.ShowAsync
        If oCmd <> ContentDialogResult.Primary Then Return ""

        Return oInputTextBox.Text

    End Function


#End Region

#End Region

#Region "not-UI related"

#Region "Dla kompatybilności ze starymi app"
#Region "Read/Write text"

    <Extension()>
    Private Async Function WriteAllTextAsyncTask(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task
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
    ''' Writes string to file, using Utf8 encoding
    ''' </summary>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function WriteAllTextAsync(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As IAsyncAction
        Return oFile.WriteAllTextAsyncTask(sTxt).AsAsyncAction
    End Function


    <Extension()>
    Private Async Function AppendLineAsyncTask(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task
        Await oFile.AppendStringAsyncTask(sTxt & vbCrLf)
    End Function

    ''' <summary>
    ''' Appends string and \r\n to file, using Utf8 encoding
    ''' </summary>
    ''' <returns>False if file cannot be opened</returns>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function AppendLineAsync(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As IAsyncAction
        Return oFile.AppendLineAsyncTask(sTxt).AsAsyncAction
    End Function


    <Extension()>
    Private Async Function AppendStringAsyncTask(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As Task(Of Boolean)

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
    ''' Appends string to file, using Utf8 encoding
    ''' </summary>
    ''' <returns>False if file cannot be opened</returns>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function AppendStringAsync(ByVal oFile As Windows.Storage.StorageFile, sTxt As String) As IAsyncOperation(Of Boolean)
        Return oFile.AppendStringAsyncTask(sTxt).AsAsyncOperation
    End Function


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

    <Extension()>
    Private Async Function ReadAllTextAsyncTask(ByVal oFile As Windows.Storage.StorageFile) As Task(Of String)
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
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function ReadAllTextAsync(ByVal oFile As Windows.Storage.StorageFile) As IAsyncOperation(Of String)
        Return oFile.ReadAllTextAsyncTask.AsAsyncOperation
    End Function


    <Extension()>
    Private Async Function ReadAllTextFromFileAsyncTask(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As Task(Of String)
        Dim oFile As Windows.Storage.StorageFile = Await oFold.TryGetItemAsync(sFileName)
        If oFile Is Nothing Then Return Nothing
        Return Await oFile.ReadAllTextAsyncTask
    End Function

    ''' <summary>
    ''' Return contens of given file as a string
    ''' </summary>
    ''' <returns>File contens, or NULL if file not exists</returns>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function ReadAllTextFromFileAsync(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As IAsyncOperation(Of String)
        Return oFold.ReadAllTextFromFileAsyncTask(sFileName).AsAsyncOperation
    End Function


#End Region

    <Extension()>
    Private Async Function FileExistsAsyncTask(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As Task(Of Boolean)
        Try
            Dim oTemp As Windows.Storage.StorageFile = Await oFold.TryGetItemAsync(sFileName)
            If oTemp Is Nothing Then Return False
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Check if given file exists in folder
    ''' </summary>
    <Extension()>
    <Obsolete("Probably migration to System.IO would be beneficient (portable code)")>
    Public Function FileExistsAsync(ByVal oFold As Windows.Storage.StorageFolder, sFileName As String) As IAsyncOperation(Of Boolean)
        Return oFold.FileExistsAsyncTask(sFileName).AsAsyncOperation
    End Function


#End Region

#Region "clipboard"

    ''' <summary>
    ''' send String to clipboard
    ''' </summary>
    <Extension()>
    Public Sub SendToClipboard(ByVal text As String)
        Try

            Dim oClipCont As New DataTransfer.DataPackage With {
                .RequestedOperation = DataTransfer.DataPackageOperation.Copy
            }
            oClipCont.SetText(text)
            DataTransfer.Clipboard.SetContent(oClipCont)
        Catch ex As Exception
            ' czasem daje "Not enough memory resources are available to process this command. (Exception from HRESULT: 0x80070008)"
        End Try
    End Sub
#End Region

#End Region


End Module
