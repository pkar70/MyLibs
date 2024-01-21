
This Nuget contains extensions of WPF classes (similar to pkar.Uwp.Extensions)

# WpfMethods
 Helper methods, used in this Nuget.

    Function GetAppVers() As String     ' x.y.z (major, minor, build)
    Function GetBuildTimestamp(bWithTime As Boolean) As String ' date in "yyyy.MM.dd HH:mm" format

# Extensions

 Many extensions for WPF UI classes.

## UI related

    String.OpenExplorer()
    Uri.OpenBrowser()
    // WebView.GetDocumentHtmlAsync() As String

    TextBlock.ShowAppVers(withDebug As Boolean) ' sets Text to x.y.z
    Page.ShowAppVers(withDebug As Boolean)  ' creates TextBox in row=1 with app version

    ' dialogboxes
    FrameworkElement.MsgBox(message As String)
    FrameworkElement.MsgBoxAsync(message As String) As Task
    FrameworkElement.DialogBoxYNAsync(message As String, Optional sYes As String = "Yes", Optional sNo As String = "No") As Task(Of Boolean))
    FrameworkElement.InputBox(message As String, Optional sDefault As String = "", Optional sYes As String = "Continue", Optional sNo As String = "Cancel") As Task(Of String)




### MAUI style calls

    Page.GoBack()
    Page.Navigate(sourcePageType As Type)
    Page.Navigate(sourcePageType As Type, parameter As Object)
    FrameworkElement.Show()
    FrameworkElement.Show(show As Boolean)
    FrameworkElement.Hide()


### ProgressRIng and Bar
 These methods are for:
 a) ProgressRing, centered on Page, sized 50 % of Page, with TextBox for messages in center of Ring
 b) ProgressBar, on top of last Page.Grid.Row

 First, you should Init this, telling what should be created (Ring, and/or Bar):

    Page.ProgRingInit(bRing As Boolean, bBar As Boolean)


 Then, you can show/hide it. Calls can be nested, i.e. sequence: Show(true); Show(true); Show(false) would not hide Ring/Bar.

    Page.ProgRingShow(bVisible As Boolean, bForce As Boolean = False, dMin As Double = 0, dMax As Double = 100)

 Any text can be shown on page center:

    Page.ProgRingSetText(message As String)

 Other methods (for ProgressBar):

    Page.ProgRingSetMax(dMaxValue As Double)
    Page.ProgRingSetVal(dValue As Double)
    Page.ProgRingInc()

## others

    String.SendToClipboard


