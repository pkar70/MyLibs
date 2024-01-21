
Imports System.Reflection.Metadata.Ecma335
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Interop
Imports System.Windows.Threading

Partial Public Module extensions

#Region "UI related"

#Region "StorageFolder"

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

#End Region

#Region "Uri"

    ''' <summary>
    ''' Open default browser on given URI
    ''' </summary>
    <Extension()>
    Public Sub OpenBrowser(ByVal oUri As System.Uri)
        Dim sdpsi As New System.Diagnostics.ProcessStartInfo(oUri.ToString)
        sdpsi.UseShellExecute = True
        System.Diagnostics.Process.Start(sdpsi)
    End Sub

#End Region

#Region "WebView"

    '<Extension()>
    'Public Function GetDocumentHtml(ByVal uiWebView As WebBrowser) As String
    '    Try
    '        Dim page As htmldocument = uiWebView.Document
    '        Return page.ToString
    '    Catch ex As Exception
    '        Return "" ' jesli strona jest pusta, jest Exception
    '    End Try
    'End Function

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
            .VerticalAlignment = Windows.VerticalAlignment.Center,
            .HorizontalAlignment = Windows.HorizontalAlignment.Center,
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
        If oPage.NavigationService.CanGoBack Then oPage.NavigationService.GoBack()
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Navigate to UWP style
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oPage As Page, destinationPageType As Page)
        oPage.NavigationService.Navigate(destinationPageType)
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Navigate to UWP style
    ''' </summary>
    <Extension()>
    Public Sub Navigate(ByVal oPage As Page, destinationPageType As Page, parameter As Object)
        oPage.NavigationService.Navigate(destinationPageType, parameter)
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Show to UWP style
    ''' </summary>
    <Extension()>
    Public Sub Show(ByVal oFE As Windows.FrameworkElement)
        oFE.Show(True)
    End Sub


    ''' <summary>
    ''' Mapping MAUI-styled Show to UWP style
    ''' </summary>
    <Extension()>
    Public Sub Show(ByVal oFE As Windows.FrameworkElement, show As Boolean)
        If show Then
            oFE.Visibility = Windows.Visibility.Visible
        Else
            oFE.Visibility = Windows.Visibility.Collapsed
        End If
    End Sub

    ''' <summary>
    ''' Mapping MAUI-styled Hide to UWP style
    ''' </summary>
    <Extension()>
    Public Sub Hide(ByVal oFE As Windows.FrameworkElement)
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

        ' WPF nie ma ProgressRingu!
        'If oPage.FindName("uiPkAutoProgRing") Is Nothing Then
        '    If bRing Then
        '        Dim _mProgRing As New ProgressRing With {
        '            .Name = "uiPkAutoProgRing",
        '            .VerticalAlignment = Windows.VerticalAlignment.Center,
        '            .HorizontalAlignment = Windows.HorizontalAlignment.Center,
        '            .Visibility = Windows.Visibility.Collapsed
        '        }
        '        Canvas.SetZIndex(_mProgRing, 10000)
        '        If iRows > 1 Then
        '            Grid.SetRow(_mProgRing, 0)
        '            Grid.SetRowSpan(_mProgRing, iRows)
        '        End If
        '        If iCols > 1 Then
        '            Grid.SetColumn(_mProgRing, 0)
        '            Grid.SetColumnSpan(_mProgRing, iCols)
        '        End If
        '        oGrid.Children.Add(_mProgRing)
        '    End If
        'End If

        ' tekst jest zawsze, bo i dla Ring i dla Bar jest przydatny
        If oPage.FindName("uiPkAutoProgText") Is Nothing Then

            Dim color As Windows.Media.Color = oPage.Resources("SystemAccentColor")

            Dim _mProgText As New TextBlock With {
                .Name = "uiPkAutoProgText",
                .VerticalAlignment = Windows.VerticalAlignment.Center,
                .HorizontalAlignment = Windows.HorizontalAlignment.Center,
                .Visibility = Windows.Visibility.Collapsed,
                .Foreground = New Windows.Media.SolidColorBrush(color)
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
                    .VerticalAlignment = Windows.VerticalAlignment.Bottom,
                    .HorizontalAlignment = Windows.HorizontalAlignment.Stretch,
                    .Visibility = Windows.Visibility.Collapsed
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
            'Dim _mProgRing As ProgressRing = TryCast(oPage.FindName("uiPkAutoProgRing"), ProgressRing)
            Dim _mProgText As TextBlock = TryCast(oPage.FindName("uiPkAutoProgText"), TextBlock)

            If _mProgRingShowCnt > 0 Then
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait
                ' Application.ShowWait(True)
                'VBlib.DebugOut("ProgRingShow - mam pokazac")
                'If _mProgRing IsNot Nothing Then
                '    Dim dSize As Double
                '    dSize = (Math.Min(TryCast(_mProgRing.Parent, Grid).ActualHeight, TryCast(_mProgRing.Parent, Grid).ActualWidth)) / 2
                '    dSize = Math.Max(dSize, 50) ' głównie na później, dla Android
                '    _mProgRing.Width = dSize
                '    _mProgRing.Height = dSize

                '    _mProgRing.Visibility = Windows.Visibility.Visible
                '    _mProgRing.IsActive = True
                'End If

                If _mProgBar IsNot Nothing Then _mProgBar.Visibility = Windows.Visibility.Visible
                If _mProgText IsNot Nothing Then _mProgText.Visibility = Windows.Visibility.Visible
            Else
                Mouse.OverrideCursor = Nothing
                'VBlib.DebugOut("ProgRingShow - mam ukryc")
                'If _mProgRing IsNot Nothing Then
                '    _mProgRing.Visibility = Windows.Visibility.Collapsed
                '    _mProgRing.IsActive = False
                'End If
                If _mProgBar IsNot Nothing Then _mProgBar.Visibility = Windows.Visibility.Collapsed
                If _mProgText IsNot Nothing Then _mProgText.Visibility = Windows.Visibility.Collapsed
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
    ''' Increment ProgressBar.Value; you should also use Await Task.Delay(1) in your code to give time to refresh screen
    ''' </summary>
    ''' <exception cref="ArgumentException">Thrown if Init was not called</exception>
    <Extension()>
    Public Async Function ProgRingInc(ByVal oPage As Page) As Task
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

        Await Task.Delay(1)

    End Function



#End Region
#Region "dialog boxes"
    Private Function IsInteractive() As Boolean
        If Application.Current.Dispatcher.Thread Is Thread.CurrentThread Then
            Return True
        End If

        Return False
    End Function

    ''' <summary>
    ''' Show message on screen, can be awaited
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    <Extension()>
    Public Async Function MsgBoxAsync(ByVal oPage As FrameworkElement, message As String) As Task
        oPage.MsgBox(message)
    End Function


    ''' <summary>
    ''' Show message on screen
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    <Extension()>
    Public Sub MsgBox(ByVal oPage As FrameworkElement, message As String)
        If Not IsInteractive() Then Return

        MessageBox.Show(Window.GetWindow(oPage), message)
    End Sub

    ''' <summary>
    ''' Show message on screen, and waits for Yes/No buttons
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <returns>TRUE if YES button was pressed</returns>
    <Extension()>
    Public Async Function DialogBoxYNAsync(ByVal oPage As FrameworkElement, message As String) As Task(Of Boolean)
        Return Await oPage.DialogBoxYNAsync(message, "Yes", "No")
    End Function


    ''' <summary>
    ''' Show message on screen, and waits for Yes/No buttons
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <param name="buttonYes">text to be shown instead of "Yes"</param>
    ''' <param name="buttonNo">text to be shown instead of "No"</param>
    ''' <returns>TRUE if YES button was pressed</returns>
    <Extension()>
    Public Async Function DialogBoxYNAsync(ByVal oPage As FrameworkElement, message As String, buttonYes As String, buttonNo As String) As Task(Of Boolean)
        If Not IsInteractive() Then Return False

        Dim sAppName As String = Application.Current.MainWindow.GetType().Assembly.GetName.Name
        Dim iRet As MessageBoxResult = MessageBox.Show(message, sAppName, MessageBoxButton.YesNo)
        If iRet = MessageBoxResult.Yes Then Return True
        Return False
    End Function

    ''' <summary>
    ''' Show message on screen, waits for Continue/Cancel buttons and return text entered by user
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <returns>text entered by user, or empty if Cancel was pressed</returns>
    <Extension()>
    Public Function InputBoxAsync(ByVal oPage As FrameworkElement, message As String) As Task(Of String)
        Return oPage.InputBoxAsync(message, "")
    End Function


    '''' <summary>
    '''' Show message on screen, waits for Continue/Cancel buttons and return text entered by user
    '''' </summary>
    '''' <param name="message">message to be shown</param>
    '''' <param name="defaultText">default value</param>
    '''' <returns>text entered by user, or empty if Cancel was pressed</returns>
    '<Extension()>
    'Public Async Function InputBoxAsync(ByVal oPage As FrameworkElement, message As String, defaultText As String) As Task(Of String)
    '    Return Await oPage.InputBoxAsync(message, defaultText, "Continue", "Cancel")
    'End Function

    ''' <summary>
    ''' Show message on screen, waits for Continue/Cancel buttons and return text entered by user
    ''' </summary>
    ''' <param name="message">message to be shown</param>
    ''' <param name="defaultText">default value</param>
    ''' <param name="buttonContinue">text to be shown instead of "Continue"</param>
    ''' <param name="buttonCancel">text to be shown instead of "Cancel"</param>
    ''' <returns>text entered by user, or empty if Cancel was pressed</returns>
    <Extension()>
    Public Async Function InputBoxAsync(ByVal oPage As FrameworkElement, message As String, defaultText As String, Optional buttonContinue As String = "Continue", Optional buttonCancel As String = "Cancel") As Task(Of String)
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
            .FontSize = 14
            }
        AddHandler oButt.Click, AddressOf pkmoduleInputBox_Click

        oStack.Children.Add(oButt)

        Dim oWnd As New Window
        oWnd.Content = oStack
        oWnd.Height = 140

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


#End Region


#Region "clipboard"

    ''' <summary>
    ''' send String to clipboard
    ''' </summary>
    <Extension()>
    Public Sub SendToClipboard(ByVal text As String)
        Clipboard.SetText(text)
    End Sub
#End Region

#End Region




End Module
