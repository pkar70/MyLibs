
Imports pkar.UI.Configs

Public Class pTextBox
    Inherits UserControl

    Public Property Header As String
        Get
            Return uiHeader.Text
        End Get
        Set(value As String)
            uiHeader.Text = value
        End Set
    End Property
    Public Property Text As String
        Get
            Return uiInput.Text
        End Get
        Set(value As String)
            uiInput.Text = value
        End Set
    End Property

    Public Property IsSpellCheckEnabled As Boolean
        Get
            Return uiInput.SpellCheck.IsEnabled
        End Get
        Set(value As Boolean)
            uiInput.SpellCheck.IsEnabled = value
        End Set
    End Property

    ' Ignored - bo w UWP inny niż w WPF
    Public Overloads Property InputScope As String


    Public Sub SetSettingsString()
        uiInput.SetSettingsString(Me.Name)
    End Sub

    Public Sub GetSettingsString()
        uiInput.GetSettingsString(Me.Name)
    End Sub

End Class
