
This Nuget contains extensions of WPF classes (similar to pkar.Uwp.Extensions)

# WpfMethods
 Helper methods, used in this Nuget.

    Function GetAppVers() As String     ' x.y.z (major, minor, build)
    Function GetBuildTimestamp(bWithTime As Boolean) As String ' date in "yyyy.MM.dd HH:mm" format

# classes
    ValueConverterOneWay    ' since 1.2
    ValueConverterOneWaySimple ' since 1.2

# Extensions

 Many extensions for WPF UI classes.

## UI related

    String.OpenExplorer()
    Uri.OpenBrowser()
    // WebView.GetDocumentHtmlAsync() As String

    TextBlock.ShowAppVers(withDebug As Boolean) ' sets Text to x.y.z
    Page.ShowAppVers(withDebug As Boolean)  ' creates TextBox in row=1 with app version

## dialogboxes - all uses pkar.Localize, so strings can be localized
    FrameworkElement.MsgBox(message As String)
    FrameworkElement.MsgBoxAsync(message As String) As Task
    FrameworkElement.DialogBoxYNAsync(message As String, Optional sYes As String = "Yes", Optional sNo As String = "No") As Task(Of Boolean))
    FrameworkElement.InputBox(message As String, Optional sDefault As String = "", Optional sYes As String = "Continue", Optional sNo As String = "Cancel") As Task(Of String)

## localizations (using pkar.Localize)
    
### using object name

    DependencyObject.LocalizePropertiesUsingObjectName(recursive)
    XAML: <TextBox Name="uiTBox" ...>
    res:  uiTBox.Text="some localized text"
    .Net: uiTBox.LocalizePropertiesUsingObjectName
    .Net: yourPage.LocalizePropertiesUsingObjectName(True)    ' calls LocalizePropertiesUsingObjectName in whole visual tree

### using text values

    DependencyObject.LocalizePrefixedProperties(recursive)
    XAML: <TextBox Text="res:pagetitle" ...>
    res:  pagetitle="some localized text"
    .Net: uiTBox.LocalizePrefixedProperties()
    .Net: yourPage.LocalizePrefixedProperties(True)    ' calls LocalizePropertiesUsingObjectName in whole visual tree


### MAUI style calls

    Page.GoBack()
    Page.Navigate(sourcePage As Page)
    Page.Navigate(sourcePage As Page, parameter As Object)
    Page.Navigate(sourcePageType As Type)   ' since 1.2.1
    Page.Navigate(sourcePageType As Type, parameter As Object)   ' since 1.2.1
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


