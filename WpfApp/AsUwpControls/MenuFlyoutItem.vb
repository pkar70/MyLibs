

Public Class MenuFlyoutItem
    Inherits MenuItem

    Public Property Text
        Get
            Return Me.Header
        End Get
        Set(value)
            Me.Header = value
        End Set
    End Property

End Class
