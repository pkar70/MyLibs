
' defaulty z UWP

Public Class pButton
    Inherits Button

    Public Sub New()
        MyBase.New

        FontSize = 14
        VerticalAlignment = VerticalAlignment.Center
        HorizontalAlignment = HorizontalAlignment.Center
        Padding = New Thickness(8, 4, 8, 5)
    End Sub

End Class
