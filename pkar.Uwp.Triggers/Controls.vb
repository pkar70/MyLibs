

Namespace Controls


    Public NotInheritable Class AppBarClockTriggerToggleButton
        Inherits AppBarToggleButton 'Microsoft.UI.Xaml.Controls byłoby nowsze UI, a jest Windows.UI...

        ' uiClockTrigger Icon= Label= RunEveryMin= 5 RunEveryHr=5 RunPolnoc=True TriggerName= DefaultState=True [z settings] OnClick= AutoUnregister=TRUE


        Private _RunEveryMins As Integer = 30 ' minutes

        ''' <summary>
        ''' Default is 30 minutes - timer freshness. This is same as RunEveryHrs (only unit is different). Return 0 for midnight timer.
        ''' </summary>
        Public Property RunEveryMins As Integer
            Get
                If _IsPolnoc Then Return 0
                Return _RunEveryMins
            End Get
            Set(value As Integer)
                _RunEveryMins = Math.Max(15, value)
            End Set
        End Property

        ''' <summary>
        ''' Default is 30 minutes - timer freshness. This is same as RunEveryMins (only unit is different). Return 0 for midnight timer.
        ''' </summary>
        Public Property RunEveryHrs As Double
            Get
                If _IsPolnoc Then Return 0
                Return _RunEveryMins * 60
            End Get
            Set(value As Double)
                _RunEveryMins = Math.Max(0.25, value / 60)
            End Set
        End Property

        Private _IsPolnoc As Boolean
        ''' <summary>
        ''' If TRUE, we will use MidnightTimer (fired about midnight) and not standard timer 
        ''' </summary>
        Public Property RunMidnight As Boolean
            Get
                Return _IsPolnoc
            End Get
            Set(value As Boolean)
                _IsPolnoc = value
            End Set
        End Property

        Private _TrigName As String


        ''' <summary>
        ''' TimerTrigger name (not used for midnight timer)
        ''' </summary>
        ''' <returns></returns>
        Public Property TriggerName As String
            Get
                Return _TrigName
            End Get
            Set(value As String)
                _TrigName = value
                IsChecked = IsTriggerRegistered()
            End Set
        End Property

        ''' <summary>
        ''' If TRUE, unchecking AppBarToggleButton would cause unregistering timer
        ''' </summary>
        Public Property AutoUnregister As Boolean


        Public Sub New()
            MyBase.New

            Icon = New SymbolIcon(Symbol.Clock)
            Label = "Autorun"

            TriggerName = Package.Current.DisplayName & "_Timer"

            AddHandler Me.Checked, AddressOf CheckHandler
            AddHandler Me.Unchecked, AddressOf CheckHandler
        End Sub

        'Public Delegate Sub AppBarClockTriggerArgs(sender As Object, state As Boolean)

        ''' <summary>
        ''' This event is raised AFTER registering or unregistering trigger
        ''' </summary>
        Public Event OnClick As RoutedEventHandler

        ''' <summary>
        ''' current state of 
        ''' </summary>
        ''' <returns></returns>
        Public Property DefaultState As Boolean


        Private Sub CheckHandler(sender As Object, e As RoutedEventArgs)
            DefaultState = IsChecked
            If IsChecked Then
                If Not IsTriggerRegistered() Then RegisterTrigger()
            Else
                If IsTriggerRegistered() AndAlso AutoUnregister Then UnregisterTrigger()
            End If

            RaiseEvent OnClick(Me, Nothing)
        End Sub

        Private Function IsTriggerRegistered() As Boolean
            Return IsTriggersRegistered(TriggerName)
        End Function

        Private Function RegisterTrigger() As Background.BackgroundTaskRegistration
            If RunMidnight Then
                Return RegisterMidnightTrigger()
            Else
                Return RegisterTimerTrigger(TriggerName, RunEveryMins, False)
            End If

        End Function

        Private Sub UnregisterTrigger()
            If RunMidnight Then
                UnregisterMidnightTrigger()
            Else
                UnregisterTriggers(TriggerName)
            End If
        End Sub

    End Class



End Namespace





