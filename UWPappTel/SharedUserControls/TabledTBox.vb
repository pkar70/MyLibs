

Public Class TabledTBox
    Inherits TextBox

    Private _Label As New TextBlock

    Public Property Label As String
        Get
            Return _Label.Text
        End Get
        Set(value As String)
            If Grid.GetColumn(Me) > 0 Then
                Grid.SetColumn(_Label, Grid.GetColumn(Me) - 1)
            End If
            Grid.SetRow(_Label, Grid.GetRow(Me))
            _Label.Text = value
        End Set
    End Property

    Public Property BoldLabel
        Get
            Return _Label.FontWeight = FontWeights.Bold
        End Get
        Set(value)
            _Label.FontWeight = FontWeights.Bold
        End Set
    End Property

    Public Sub New()
        MyBase.New
    End Sub

End Class
