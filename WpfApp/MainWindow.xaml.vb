Option Strict On

Imports Microsoft.UI
Imports Microsoft.UI.Xaml

Public Class MainWindow
    Inherits Window


    Sub New()

        Title = "Ballots"

        'InitializeComponent()
        Me.Content = New MainPage
        ' Me.ContentRendered ?

        ' *TODO* AddHandler rootFrame.Navigated, AddressOf OnNavigatedAddBackButton

    End Sub


End Class
