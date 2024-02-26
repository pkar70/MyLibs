

Imports System.Windows.Controls


Public Class AppBarButton
    Inherits Button

    Private _symbol As New SymbolIcon
    Private _label As New TextBlock

    Public Sub New()

        Dim sp As New StackPanel
        sp.Orientation = Orientation.Vertical

        sp.Children.Add(_symbol)
        sp.Children.Add(_label)

        Me.Content = sp
    End Sub

    Public Property Icon As String
        Get
            Return _symbol.Symbol
        End Get
        Set(value As String)
            _symbol.Symbol = value
        End Set
    End Property


    Public Property Label As String
        Get
            Return _label.Text
        End Get
        Set(value As String)
            _label.Text = value
        End Set
    End Property

    Private _IsCompact As Boolean = False
    Public Property IsCompact As Boolean
        Get
            Return _IsCompact
        End Get
        Set(value As Boolean)
            _IsCompact = value
            _label.Visibility = If(_IsCompact, Windows.Visibility.Collapsed, Windows.Visibility.Visible)
        End Set
    End Property


    ''' <summary>
    ''' Ignored, ale używam tego czasem w UWP
    ''' </summary>
    Public Property AllowFocusOnInteraction As Boolean

End Class



