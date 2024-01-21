
Imports pkar.DotNetExtensions

Imports Windows.ApplicationModel

Public Module Triggers

#Region "simple checks"
    ''' <summary>
    ''' check if any triggers with name that contains Package.Current.DisplayName is registered
    ''' </summary>
    Public Function IsTriggersRegistered() As Boolean
        Return IsTriggersRegistered(Package.Current.DisplayName)
    End Function

    ''' <summary>
    ''' check if any triggers with name that contains nameFragment is registered (case insensitive)
    ''' </summary>
    Public Function IsTriggersRegistered(nameFragment As String) As Boolean
        nameFragment = nameFragment.Replace(" ", "").Replace("'", "")

        Try
            For Each oTask As KeyValuePair(Of Guid, Background.IBackgroundTaskRegistration) In Background.BackgroundTaskRegistration.AllTasks
                If oTask.Value.Name.ContainsCI(nameFragment) Then Return True
            Next
        Catch ex As Exception
            ' np. gdy nie ma permissions, to może być FAIL
        End Try

        Return False
    End Function

    ''' <summary>
    ''' Check if app can register triggers (via BackgroundAccessStatus)
    ''' </summary>
    Public Async Function CanRegisterTriggersAsync() As Task(Of Boolean)

        Dim oBAS As Background.BackgroundAccessStatus
        oBAS = Await Background.BackgroundExecutionManager.RequestAccessAsync()

        If oBAS = Background.BackgroundAccessStatus.AlwaysAllowed Then Return True
        If oBAS = Background.BackgroundAccessStatus.AllowedSubjectToSystemPolicy Then Return True

        Return False

    End Function

#End Region

#Region "unregistering"
    ''' <summary>
    ''' remove all triggers with name that contains Package.Current.DisplayName 
    ''' </summary>
    Public Sub UnregisterTriggers()
        UnregisterTriggers(Package.Current.DisplayName)
    End Sub

    ''' <summary>
    ''' remove all triggers with name that contains nameFragment 
    ''' </summary>
    Public Sub UnregisterTriggers(nameFragment As String)
        nameFragment = nameFragment.Replace(" ", "").Replace("'", "")

        Dim doUsuniecia As New List(Of Background.IBackgroundTaskRegistration)

        Try
            For Each oTask As KeyValuePair(Of Guid, Background.IBackgroundTaskRegistration) In Background.BackgroundTaskRegistration.AllTasks
                If String.IsNullOrEmpty(nameFragment) OrElse oTask.Value.Name.ContainsCI(nameFragment) Then doUsuniecia.Add(oTask.Value)
            Next
        Catch ex As Exception
            ' np. gdy nie ma permissions, to może być FAIL
        End Try

        For Each oTask In doUsuniecia
            oTask.Unregister(True)
        Next

        ' z innego wyszlo, ze RemoveAccess z wnetrza daje Exception
        ' If bAll Then BackgroundExecutionManager.RemoveAccess()

    End Sub


#End Region

#Region "internal - creating names for triggers"
    Private Function GetTriggerNamePrefix() As String
        Dim sName As String = Package.Current.DisplayName
        sName = sName.Replace(" ", "").Replace("'", "")
        Return sName
    End Function

    Private Function GetTriggerName(sufix As String) As String
        Return GetTriggerNamePrefix() & "_" & sufix
    End Function

    Private Function GetTriggerPolnocnyName() As String
        Return GetTriggerName("polnocny")
    End Function
#End Region

#Region "debug support"

    ''' <summary>
    ''' Get names of all registered Triggers
    ''' </summary>
    Public Function DumpTriggers() As String
        Dim sRet As String = "Dumping Triggers" & vbCrLf & vbCrLf
        Try
            For Each oTask In Background.BackgroundTaskRegistration.AllTasks
                sRet &= oTask.Value.Name & vbCrLf ' //GetType niestety nie daje rzeczywistego typu
            Next
        Catch
        End Try

        Return sRet
    End Function

    Private gLastPolnoc As DateTimeOffset

    ''' <summary>
    ''' Get last time when midnight trigger was handled - but only when app is still running (in foreground or in background)
    ''' </summary>
    ''' <returns></returns>
    Public Function GetLastMidnightTriggerDate() As DateTimeOffset
        Return gLastPolnoc
    End Function

#End Region

#Region "time related triggers"

    ''' <summary>
    ''' Register TimeTrigger
    ''' </summary>
    ''' <param name="name">(full) name of trigger</param>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>
    Public Function RegisterTimerTrigger(name As String, freshnessMinutes As Integer) As Background.BackgroundTaskRegistration
        Return RegisterTimerTrigger(name, freshnessMinutes, False)
    End Function

    ''' <summary>
    ''' Register TimeTrigger
    ''' </summary>
    ''' <param name="name">(full) name of trigger</param>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>
    Public Function RegisterTimerTrigger(name As String, freshnessMinutes As Integer, oneShot As Boolean) As Background.BackgroundTaskRegistration
        Return RegisterTimerTrigger(name, freshnessMinutes, oneShot, Nothing)
    End Function

    ''' <summary>
    ''' Register TimeTrigger
    ''' </summary>
    ''' <param name="name">(full) name of trigger</param>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>
    Public Function RegisterTimerTrigger(name As String, freshnessMinutes As Integer, oneShot As Boolean, condition As Background.SystemCondition) As Background.BackgroundTaskRegistration

        Try
            Dim builder As New Background.BackgroundTaskBuilder
            Dim oRet As Background.BackgroundTaskRegistration

            builder.SetTrigger(New Background.TimeTrigger(freshnessMinutes, oneShot))
            builder.Name = name
            If condition IsNot Nothing Then builder.AddCondition(condition)
            oRet = builder.Register()
            Return oRet
        Catch ex As Exception
            ' brak możliwości rejestracji (na przykład)
        End Try

        Return Nothing
    End Function


    ''' <summary>
    ''' Register trigger to be run about midnight, but you should call IsThisMidnightTrigger in OnBackgroundActivated to adjust time of next triggers
    ''' </summary>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>

    Public Function RegisterMidnightTrigger() As Background.BackgroundTaskRegistration
        ' w Nuget tego nie będzie - zwróci po prostu NULLa
        'If Not Await CanRegisterTriggersAsync() Then Return

        Dim oDateNew As New DateTime(Date.Now.Year, Date.Now.Month, Date.Now.Day, 23, 40, 0)
            If Date.Now.Hour > 21 Then oDateNew = oDateNew.AddDays(1)

            Dim iMin As Integer = (oDateNew - DateTime.Now).TotalMinutes

            Return RegisterTimerTrigger(GetTriggerPolnocnyName(), iMin, False)
    End Function

    ''' <summary>
    ''' If this is midnight trigger, than save current datetime (see GetLastMidnightTriggerDate) and adjust trigger run time to be more midnight
    ''' </summary>
    Public Function IsThisMidnightTrigger(args As Activation.BackgroundActivatedEventArgs) As Boolean

        Dim sName As String = GetTriggerPolnocnyName()
        If args.TaskInstance.Task.Name <> sName Then Return False

        gLastPolnoc = Date.Now

        Dim bRet As Boolean '= False
        Dim oDateNew As New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 40, 0)

        If (DateTime.Now.Hour = 23 AndAlso DateTime.Now.Minute > 20) Then
            ' tak, to jest północny o północy
            bRet = True
            oDateNew = oDateNew.AddDays(1)
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



#End Region

    ''' <summary>
    ''' Register UserPresentTrigger
    ''' </summary>
    ''' <param name="Name">if not given, use default name, "_userpresent", prefixed with app name</param>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>

    Public Function RegisterUserPresentTrigger() As Background.BackgroundTaskRegistration
        Return RegisterUserPresentTrigger("")
    End Function


    ''' <summary>
    ''' Register UserPresentTrigger
    ''' </summary>
    ''' <param name="Name">if not given, use default name, "_userpresent", prefixed with app name</param>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>

    Public Function RegisterUserPresentTrigger(Name As String) As Background.BackgroundTaskRegistration
        Return RegisterUserPresentTrigger(Name, False)
    End Function


    ''' <summary>
    ''' Register UserPresentTrigger
    ''' </summary>
    ''' <param name="Name">if not given, use default name, "_userpresent", prefixed with app name</param>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>

    Public Function RegisterUserPresentTrigger(Name As String, oneShot As Boolean) As Background.BackgroundTaskRegistration
        If String.IsNullOrEmpty(Name) Then Name = GetTriggerNamePrefix() & "_userpresent"

        Try
            Dim builder As New Background.BackgroundTaskBuilder
            Dim oRet As Background.BackgroundTaskRegistration

            Dim oTrigger As Background.SystemTrigger
            oTrigger = New Background.SystemTrigger(Background.SystemTriggerType.UserPresent, oneShot)

            builder.SetTrigger(oTrigger)
            builder.Name = Name

            oRet = builder.Register()
            Return oRet
        Catch ex As Exception
            ' brak możliwości rejestracji (na przykład)
        End Try

        Return Nothing
    End Function


    ''' <summary>
    ''' Register UserPresentTrigger using default name, "_servcompleted", prefixed with app name
    ''' </summary>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>
    Public Function RegisterServicingCompletedTrigger() As Background.BackgroundTaskRegistration
        Return RegisterServicingCompletedTrigger("")
    End Function


    ''' <summary>
    ''' Register UserPresentTrigger
    ''' </summary>
    ''' <param name="Name">if not given, use default name, "_servcompleted", prefixed with app name</param>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>
    Public Function RegisterServicingCompletedTrigger(Name As String) As Background.BackgroundTaskRegistration
        If String.IsNullOrEmpty(Name) Then Name = GetTriggerNamePrefix() & "_servcompleted"

        Try
            Dim builder As New Background.BackgroundTaskBuilder

            builder.SetTrigger(New Background.SystemTrigger(Background.SystemTriggerType.ServicingComplete, True))
            builder.Name = Name

            Dim oRet As Background.BackgroundTaskRegistration
            oRet = builder.Register()
            Return oRet
        Catch ex As Exception
            ' brak możliwości rejestracji (na przykład)
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Register ToastNotificationActionTrigger using default name, "_toasttrigger", prefixed with app name
    ''' </summary>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>
    Public Function RegisterToastTrigger() As Background.BackgroundTaskRegistration
        Return RegisterToastTrigger("")
    End Function


    ''' <summary>
    ''' Register ToastNotificationActionTrigger
    ''' </summary>
    ''' <param name="Name">if not given, use default name, "_toasttrigger", prefixed with app name</param>
    ''' <returns>BackgroundTaskRegistration of created trigger, or NULL if fails</returns>
    Public Function RegisterToastTrigger(Name As String) As Background.BackgroundTaskRegistration
        If String.IsNullOrEmpty(Name) Then Name = GetTriggerNamePrefix() & "_toasttrigger"

        Try
            Dim builder As New Background.BackgroundTaskBuilder
            Dim oRet As Background.BackgroundTaskRegistration

            builder.SetTrigger(New Background.ToastNotificationActionTrigger)
            builder.Name = Name
            oRet = builder.Register()
            Return oRet
        Catch ex As Exception
            ' brak możliwości rejestracji (na przykład)
        End Try

        Return Nothing
    End Function


End Module