
Imports Microsoft.VisualBasic.FileIO
Imports pkar.UI.Configs

Public Class BrowseFileFolder
    Inherits Grid

    Public Property BoxReadOnly As Boolean
        Get
            Return _filenameTbox.IsReadOnly
        End Get
        Set(value As Boolean)
            _filenameTbox.IsReadOnly = value
        End Set
    End Property

    Public Property DefaultStartDir As String

    Public Property UseFileOpen As Boolean
    Public Property UseFileSave As Boolean
    Public Property UseFolder As Boolean

    ''' <summary>
    ''' items: typy w formacie: opis (maska)
    ''' </summary>
    Public Property FileTypesList As New List(Of String)
    Public Property FileTypesString As String
        Get
            Return GetFilterWPF()
        End Get
        Set(value As String)
            Dim arr As String() = value.Split("|")
            FileTypesList.Clear()
            For Each maska In arr
                FileTypesList.Add(maska)
            Next
        End Set
    End Property

    Private _filenameTbox As New TextBox

    Public Event ValueChanged As RoutedEventHandler


    Public Sub New()

        Me.ColumnDefinitions.Clear()

        Me.ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(1, GridUnitType.Star)})
        Me.ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(1, GridUnitType.Auto)})

        Dim buttBrowse As New Button With
            {
            .Content = " ... ",
            .ToolTip = "otwieranie browsera",
            .Margin = New Thickness(5, 0, 5, 0)
            }
        AddHandler buttBrowse.Click, AddressOf Browse_Click

        Me.Children.Add(_filenameTbox)
        Me.Children.Add(buttBrowse)
        Grid.SetColumn(_filenameTbox, 0)
        Grid.SetColumn(buttBrowse, 1)
    End Sub

    Public Sub SetSettingsString()
        _filenameTbox.SetSettingsString(Name)
    End Sub

    Public Sub GetSettingsString()
        _filenameTbox.GetSettingsString(Name)
    End Sub


    Private Sub NotifyChange()
        RaiseEvent ValueChanged(Me, Nothing)
    End Sub

    Private Sub Browse_Click(sender As Object, e As RoutedEventArgs)

        If UseFileOpen Then
            BrowseFileOpen()
        ElseIf UseFileSave Then
            BrowseFileSave()
        ElseIf UseFolder Then
            BrowseFolder()
        End If
    End Sub

#Region "fileopen"

    Private Function GetFilterWPF() As String

        If FileTypesList.Count < 1 Then Return "All files (*.*)|*.*"

        Dim ret As String = ""
        For Each item In FileTypesList
            ret = ret & "|" & item & "|"
            Dim iInd As Integer = item.IndexOf("(")
            If iInd > 1 Then
                ret &= item.Substring(iInd + 1).Replace(")", "")
            End If
        Next

        Return ret.Substring(1)
    End Function
#If PK_WPF Then



    Private Sub BrowseFileOpen()

        Dim browser = New Microsoft.Win32.OpenFileDialog()
        If String.IsNullOrWhiteSpace(_filenameTbox.Text) Then
            browser.InitialDirectory = DefaultStartDir
        Else
            browser.InitialDirectory = IO.Path.GetDirectoryName(_filenameTbox.Text)
        End If

        browser.Filter = GetFilterWPF()

        If Not browser.ShowDialog() Then Return

        _filenameTbox.Text = browser.FileName
        NotifyChange()
    End Sub

#ElseIf NETFX_CORE Then

    Private sub GetFilterUwp(picker As Pickers.FileOpenPicker) 

        If FileTypeFilter.Count < 1 Then
        picker.FileTypeFilter.Add(".*")
        Return 
        end if

        Dim ret As String = ""
        For Each item In FileTypeFilter
            Dim iInd As Integer = item.IndexOf("(")
            If iInd > 1 Then
                ' picker.FileTypeFilter.Add(".mp3")
                picker.FileTypeFilter.Add(item.Substring(iInd + 1).Replace(")", "").replace("*.",".")
            End If
        Next

        Return ret.Substring(1)
    End sub

    Private Sub BrowseFileOpen()

    Dim picker As New Pickers.FileOpenPicker
    picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail
    GetFilterUwp

    Dim selectedFile as StorageFile= Await picker.PickSingleFileAsync
    uiFileName.Text = selectedFile.Name

    NotifyChange
#End If
#End Region

#Region "filesave"

    Private Async Sub BrowseFileSave()

#If PK_WPF Then
#ElseIf NETFX_CORE Then
#End If

    End Sub
#End Region

#Region "openfolder"

    Private Sub BrowseFolder()
#If NET8_0_OR_GREATER Then
        Dim browser = New Microsoft.Win32.OpenFolderDialog
        If String.IsNullOrWhiteSpace(_filenameTbox.Text) Then
            browser.InitialDirectory = DefaultStartDir
        Else
            browser.InitialDirectory = IO.Path.GetDirectoryName(_filenameTbox.Text)
        End If

        If Not browser.ShowDialog() Then Return

        _filenameTbox.Text = browser.FolderName
        NotifyChange()
#Else
        Throw New InvalidOperationException("Browse foderu działa tylko przy .Net 8+")
#End If
    End Sub

#End Region


End Class
