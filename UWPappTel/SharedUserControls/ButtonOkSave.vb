
Public Class ButtonOkSave
    Inherits pButton

    '<local:pButton Content="OK" HorizontalAlignment="Center" />

    Public Sub New()
        Content = "OK"
        HorizontalAlignment = HorizontalAlignment.Center
        ToolTip = "Zapisz dane i zamknij stronę"
    End Sub
End Class
