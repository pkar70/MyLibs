
This Nuget contains extensions of UWP classes.

# UwpMethods
 Helper methods, used in this Nuget.

    Function GetAppVers() As String     ' x.y.z (major, minor, build)
    Function GetBuildTimestamp(bWithTime As Boolean) As String ' date in "yyyy.MM.dd HH:mm" format

# Extensions

 Many extensions for UWP classes.

## UI related

    StorageFolder.OpenExplorer()
    Uri.OpenBrowser()
    Uri.OpenBrowser(bForceEdge As Boolean)
    WebView.GetDocumentHtmlAsync() As String

    TextBlock.ShowAppVers(withDebug As Boolean) ' sets Text to x.y.z
    Page.ShowAppVers(withDebug As Boolean)  ' creates TextBox in row=1 with app version

    ' dialogboxes, since v1.1
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

## file system helpers

 All these methods are marked as obsolete; you should not use them in your new apps - instead, try to use System.IO methods (to make portable code). 

    StorageFile.WriteAllTextAsync(sTxt As String)   ' using Utf8 encoding
    StorageFile.AppendLineAsync(sTxt As String)     ' appends string and \r\n to file, using Utf8 encoding
    StorageFile.AppendStringAsync(sTxt As String)     ' appends string to file, using Utf8 encoding
    StorageFile.ReadAllTextAsync() As String

    StorageFolder.WriteAllTextToFileAsync(sFileName As String, sTxt As String, oOption As CreationCollisionOption = FailIfExists)
    StorageFolder.ReadAllTextFromFileAsync(sFileName As String) As String

    StorageFolder.FileExistsAsync(sFileName As String) As Boolean
