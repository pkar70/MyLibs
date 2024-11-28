

Public Class GridedTBox
    Inherits TextBox

    Private _Label As New TextBlock

    Public Property Label As String
        Get
            Return _Label.Text
        End Get
        Set(value As String)
            _Label.Text = value
        End Set
    End Property

    Public Property BoldLabel As Boolean
        Get
            Return _Label.FontWeight = FontWeights.Bold
        End Get
        Set(value As Boolean)
            _Label.FontWeight = If(value, FontWeights.Bold, FontWeights.Normal)
        End Set
    End Property

    Public Property AutoRow As Boolean
    Public Property ResetRowcount As Boolean
        Get
            Return False
        End Get
        Set(value As Boolean)
            If value Then _rowcount = 0
        End Set
    End Property

    Private Shared _rowcount As Integer = 0

    Private _applyingTemplate As Boolean

    Protected Overrides Sub OnInitialized(e As EventArgs)
        MyBase.OnInitialized(e)
        Debug.WriteLine("OnInitialized")
    End Sub

    Public Overrides Sub OnApplyTemplate()

        Debug.WriteLine("OnApplyTemplate")

        If _applyingTemplate Then
            Debug.WriteLine("aplyinguje template")
            Return
        End If

        Dim gridek As Grid = TryCast(Me.VisualParent, Grid)
        If gridek Is Nothing Then Return

        _applyingTemplate = True

        gridek.Children.Add(_Label)

        If Grid.GetColumn(Me) > 0 Then
            Grid.SetColumn(_Label, Grid.GetColumn(Me) - 1)
        Else
            'Grid.SetColumn(Me, 1)
            Grid.SetColumn(_Label, 0)
        End If

        If AutoRow Then
            Grid.SetRow(Me, _rowcount)
            Grid.SetRow(_Label, _rowcount)
            _rowcount += 1
        End If

        ' jest dodany, ale height=NaN, desiredsize=0,0, actualwidht=0,

        _Label.MinHeight = 10
        _Label.MinWidth = 10
        _Label.Measure(New Size(20, 20))

        _Label.ApplyTemplate()
        MyBase.OnApplyTemplate()

        'gridek.UpdateLayout()

        _applyingTemplate = False
    End Sub
End Class
