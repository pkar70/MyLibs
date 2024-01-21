
This Nuget contains wrappers for WinUI3 Triggers (same methods as in my pkar.Uwp.Triggers)


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