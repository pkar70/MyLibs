
This Nuget contains wrappers for UWP Triggers.


    Function IsTriggersRegistered() As Boolean
    Function IsTriggersRegistered(nameFragment As String) As Boolean
    Async Function CanRegisterTriggersAsync() As Task(Of Boolean)

    Sub UnregisterTriggers()
    Sub UnregisterTriggers(nameFragment As String)

    Function RegisterTimerTrigger(name As String, freshnessMinutes As Integer, Optional oneShot As Boolean = False, Optional condition As Background.SystemCondition = Nothing) As BackgroundTaskRegistration
    Function RegisterUserPresentTrigger(Optional Name As String = "", Optional oneShot As Boolean = False) As BackgroundTaskRegistration
    Function RegisterServicingCompletedTrigger(Optional Name As String = "") As BackgroundTaskRegistration
    Function RegisterToastTrigger(Optional Name As String = ") As BackgroundTaskRegistration

    Function RegisterMidnightTrigger() As BackgroundTaskRegistration
    Function IsThisMidnightTrigger(args As BackgroundActivatedEventArgs) As Boolean

    Function DumpTriggers() As String
    Function GetLastMidnightTriggerDate() As Date


Since 1.2.0, package contains one UserControl: AppBarClockTriggerToggleButton, that is AppBarToggleButton with:

        AppBarClockTriggerToggleButton.Icon=SymbolIcon(Symbol.Clock)
        AppBarClockTriggerToggleButton.Label="Autorun"
        AppBarClockTriggerToggleButton.RunEveryMin=30
        AppBarClockTriggerToggleButton.RunEveryHr=0.5
        AppBarClockTriggerToggleButton.RunPolnoc=false ' be warned! you should call IsThisMidnightTrigger from timer code
        AppBarClockTriggerToggleButton.TriggerName= Package.Current.DisplayName & "_Timer"
        AppBarClockTriggerToggleButton.DefaultState=false
        AppBarClockTriggerToggleButton.AutoUnregister=false' If TRUE, unchecking AppBarToggleButton would cause unregistering timer
        Event AppBarClockTriggerToggleButton.OnClick(sender As Object, RoutedEventArgs As Object)
        

Simply add this AppBarClockTriggerToggleButton to your XAML page. Its state (.IsChecked) would reflect IsTriggerRegistered - checking would register trigger, and unregistering can unregister (see AutoUnregister). Check/uncheck can also raise event OnClick (AFTER registering/unregistering)