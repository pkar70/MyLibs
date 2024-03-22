
' https://stackoverflow.com/questions/61850696/how-to-implement-circle-progress-bar-wpf

Imports System.Windows.Threading
Imports pkar.DotNetExtensions

Public Class ProgressRing
    Private ReadOnly _spinnerTimer As DispatcherTimer

    '// the nominal size of the spinner - the actual size Is determined by the Width / Height as the spinner Is contained within a ViewBox
    Public Diameter As Double = 100.0
    Public ItemDiameter As Double = 10 ' Diameter / 6.0
    Public ItemPositionRadius As Double = (Diameter - ItemDiameter) / 2.0

    Public Sub New()

        InitializeComponent()

        _spinnerTimer = New DispatcherTimer(DispatcherPriority.Normal, Dispatcher)

        AddHandler _spinnerTimer.Tick, Sub() SpinnerRotateTransform.Angle = (SpinnerRotateTransform.Angle + 30) Mod 360
        AddHandler Loaded, AddressOf OnLoaded
        AddHandler Unloaded, AddressOf MeStop
        AddHandler IsVisibleChanged, Sub(sender As Object, e As DependencyPropertyChangedEventArgs) OnIsVisibleChanged(e.NewValue)

    End Sub

    '/// <summary>
    '/// IsVisibleChanged also covers the case where the spinner Is placed inside another control which itself Is collapsed Or hidden
    '/// </summary>
    '/// <param name="isVisible">
    '/// </param>
    Private Sub OnIsVisibleChanged(IsVisible As Boolean)
        ' // disable spinning in the Visual Studio designer
        'If perViewModelHelper.IsInDesignMode Then Return

        If IsVisible Then
            MeStart()
        Else
            MeStop(Nothing, Nothing)
        End If
    End Sub

    '/// <summary>
    '/// Rotations per minute
    '/// </summary>
    Public Property Speed As Integer
        Get
            Return GetValue(SpeedProperty)
        End Get
        Set(value As Integer)
            SetValue(SpeedProperty, value)
        End Set
    End Property

    Public Shared ReadOnly SpeedProperty As DependencyProperty =
        DependencyProperty.Register("Speed", GetType(Integer), GetType(ProgressRing), New PropertyMetadata(24))

    Private Sub OnLoaded(sender As Object, e As RoutedEventArgs)
        SetItemPosition(Item1, 0)
        SetItemPosition(Item2, 1)
        SetItemPosition(Item3, 2)
        SetItemPosition(Item4, 3)
        SetItemPosition(Item5, 4)
        SetItemPosition(Item6, 5)
        SetItemPosition(Item7, 6)
        SetItemPosition(Item8, 7)
        SetItemPosition(Item9, 8)
        SetItemPosition(Item10, 9)
        SetItemPosition(Item11, 10)
        SetItemPosition(Item12, 11)

        SetItemPosition(Itema1, 12)
        SetItemPosition(Itema2, 13)
        SetItemPosition(Itema3, 14)
        SetItemPosition(Itema4, 15)
        SetItemPosition(Itema5, 16)
        SetItemPosition(Itema6, 17)
        SetItemPosition(Itema7, 18)
        SetItemPosition(Itema8, 19)
        SetItemPosition(Itema9, 20)
        SetItemPosition(Itema10, 21)
        SetItemPosition(Itema11, 22)
        SetItemPosition(Itema12, 23)


    End Sub

    Private Sub SetItemPosition(item As DependencyObject, index As Integer)

        Dim srodekX As Double = ActualWidth / 2
        Dim srodekY As Double = ActualHeight / 2
        Dim kat As Double = Math.PI * (index / 12.0) ' stopnie na radiany

        Dim xOffset As Double = Math.Sin(kat) * ItemPositionRadius
        Dim yOffset As Double = Math.Cos(kat) * ItemPositionRadius

        Debug.WriteLine($"SetItemPosition(index={index}, srodek={srodekX}, kat={kat}, xOffset={xOffset}, yOffset = {yOffset}")

        item.SetValue(Canvas.LeftProperty, srodekX + xOffset + Math.Sign(xOffset) * ItemDiameter / 2)
        item.SetValue(Canvas.TopProperty, srodekY + yOffset + Math.Sign(yOffset) * ItemDiameter / 2)
    End Sub

    Private Sub MeStop(sender As Object, e As RoutedEventArgs)
        _spinnerTimer.Stop()
    End Sub

    Private Sub MeStart()
        '// each tick of the timer Is 1 step of revolution
        _spinnerTimer.Interval = TimeSpan.FromMilliseconds(60000 / (12.0 * Speed))
        _spinnerTimer.Start()
    End Sub

    Protected Overrides Sub OnRenderSizeChanged(sizeInfo As SizeChangedInfo)
        MyBase.OnRenderSizeChanged(sizeInfo)

        Diameter = Math.Min(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height)
        ' kropka ma być w połowie wielkości pola/controlki
        ItemPositionRadius = (Diameter - ItemDiameter) / 4.0
        ' OnRenderSizeChanged: Diameter=646.0000000000001, positionradius=318.00000000000006
        Debug.WriteLine($"OnRenderSizeChanged: Diameter={Diameter}, positionradius={ItemPositionRadius}")

        OnLoaded(Nothing, Nothing)
    End Sub


    ' moje podmiany tak żeby działało w library - sam ustawiam różne rzeczy
    ' <Grid Background="{Binding Path=Background, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type ProgressRing}}}">
    ' Width="{Binding Path=Diameter, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type ProgressRing}}}"
    ' Height="{Binding Path=Diameter, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type ProgressRing}}}"
    '<Setter Property= "Fill" Value="{Binding Path=Foreground, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type ProgressRing}}}" />
    '<Setter Property= "Height" Value="{Binding Path=ItemDiameter, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type ProgressRing}}}" />
    ' <Setter Property="Width" Value="{Binding Path=ItemDiameter, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type ProgressRing}}}" />
    'OnRenderSizeChanged: Diameter=646.0000000000001, positionradius=318.00000000000006
    'SetItemPosition(index=0, srodek=323.00000000000006, kat=0, xOffset=0, yOffset = 318.00000000000006
    'SetItemPosition(index=1, srodek=323.00000000000006, kat=0.5235987755982988, xOffset=159, yOffset = 275.39607840345155
    'SetItemPosition(index=2, srodek=323.00000000000006, kat=1.0471975511965976, xOffset=275.3960784034515, yOffset = 159.00000000000006
    'SetItemPosition(index=3, srodek=323.00000000000006, kat=1.5707963267948966, xOffset=318.00000000000006, yOffset = 1.947188410644292E-14
    'SetItemPosition(index=4, srodek=323.00000000000006, kat=2.0943951023931953, xOffset=275.39607840345155, yOffset = -158.99999999999997
    'SetItemPosition(index=5, srodek=323.00000000000006, kat=2.6179938779914944, xOffset=159, yOffset = -275.39607840345155
    'SetItemPosition(index=6, srodek=323.00000000000006, kat=3.141592653589793, xOffset=3.894376821288584E-14, yOffset = -318.00000000000006
    'SetItemPosition(index=7, srodek=323.00000000000006, kat=3.6651914291880923, xOffset=-159.00000000000006, yOffset = -275.3960784034515
    'SetItemPosition(index=8, srodek=323.00000000000006, kat=4.1887902047863905, xOffset=-275.3960784034515, yOffset = -159.00000000000017
    'SetItemPosition(index=9, srodek=323.00000000000006, kat=4.71238898038469, xOffset=-318.00000000000006, yOffset = -5.841565231932875E-14
    'SetItemPosition(index=10, srodek=323.00000000000006, kat=5.235987755982989, xOffset=-275.3960784034515, yOffset = 159.00000000000006
    'SetItemPosition(index=11, srodek=323.00000000000006, kat=5.759586531581287, xOffset=-159.00000000000017, yOffset = 275.39607840345144

End Class
