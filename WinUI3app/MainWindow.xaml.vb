Option Strict On

Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Microsoft.UI
Imports Microsoft.UI.Xaml
Imports Microsoft.UI.Xaml.Controls
Imports Microsoft.UI.Xaml.Input
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Foundation

Public Class MainWindow
    Inherits Window

    Private ReadOnly _backdrop As BackdropHelper

    Sub New()

        Title = "WinUI 3 VB Demo - mp4 converter"

        InitializeComponent()

        _backdrop = New BackdropHelper(Me)
        Dim useAcrylic = False
        _backdrop.SetBackdrop(BackdropType.Mica, useAcrylic)

        TryCustomizeTitleBar(useAcrylic)


    End Sub

    Private Sub TryCustomizeTitleBar(useAcrylic As Boolean)
        Dim appWnd = GetAppWindow
        Dim titleBar = appWnd.TitleBar
        If titleBar IsNot Nothing Then
            ' Windows 11 or Windows 10
            With appWnd.TitleBar
                .ExtendsContentIntoTitleBar = True
                .ButtonBackgroundColor = Colors.Transparent
            End With
            Dim titleBarHeight = appWnd.GetTitleBarHeight
            LayoutRoot.RowDefinitions(0).Height = New GridLength(titleBarHeight + 4, GridUnitType.Pixel)
            If Not useAcrylic Then
                LayoutRoot.Background = Nothing
                ConvertingFiles.Background = Nothing
            End If
        Else
            ' Windows 10
            TblTitleText.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private _loaded As Boolean
    Private Sub MainWindow_Activated(sender As Object, args As WindowActivatedEventArgs) Handles Me.Activated
        WinUIVbHost.Instance.CurrentWindow = Me

    End Sub

End Class
