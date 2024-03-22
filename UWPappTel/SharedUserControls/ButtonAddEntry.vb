
Public Class ButtonAddEntry
    Inherits Button

    '<Button HorizontalAlignment = "Right" Margin="0,15,5,0" Grid.Row="1" Content="+" Click="uiAddNew_Click" ToolTip="Dodaj"/>

    Public Sub New()
        HorizontalAlignment = HorizontalAlignment.Right
        FontWeight = FontWeights.Bold
        Content = "+"
        ToolTip = "Dodaj"
    End Sub

End Class
