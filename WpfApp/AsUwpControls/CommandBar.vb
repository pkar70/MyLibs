Imports pkar.DotNetExtensions

Public Class CommandBar
    Inherits StackPanel


    Private _ClosedDisplayIcons As Boolean = True
    Private _NotClosed As Boolean = False

    Public Property ClosedDisplayMode As String
        Get
            Return If(_ClosedDisplayIcons, "Closed", "Minimal")
        End Get
        Set(value As String)
            _ClosedDisplayIcons = value.EqualsCI("Closed")
            UstawWidzialnosc()
        End Set
    End Property

    Public Sub New()
        MyBase.New

        Orientation = Orientation.Horizontal
        HorizontalAlignment = HorizontalAlignment.Right
    End Sub

    Public Overrides Sub EndInit()
        MyBase.EndInit()

        Dim rozwijacz As New AppBarButton With {.Icon = "⋯", .Label = ""}
        AddHandler rozwijacz.Click, AddressOf Rozwijanie_Click
        Me.Children.Add(rozwijacz)

        UstawWidzialnosc()
    End Sub

    Private Sub UstawWidzialnosc()
        If Me.Children Is Nothing Then Return

        For Each oChild In Me.Children
            Dim oABButt As AppBarButton = TryCast(oChild, AppBarButton)
            If oABButt Is Nothing Then Continue For

            If _NotClosed Then
                oABButt.Visibility = Visibility.Visible
                oABButt.IsCompact = False
            Else
                If _ClosedDisplayIcons Then
                    oABButt.Visibility = Visibility.Visible
                    oABButt.IsCompact = True
                Else
                    If oABButt.Icon <> "⋯" Then oABButt.Visibility = Visibility.Collapsed
                End If
            End If

        Next
    End Sub

    Private Sub Rozwijanie_Click(sender As Object, data As RoutedEventArgs)
        _NotClosed = Not _NotClosed
        UstawWidzialnosc()
    End Sub
End Class
