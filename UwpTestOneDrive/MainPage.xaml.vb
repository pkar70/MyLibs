
' sprawdzenie jakich permissionów wymaga zapis do JSON %onedriveconsumer%

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        Dim spath = Environment.GetEnvironmentVariable("OneDriveConsumer")

        If IO.Directory.Exists(spath) Then
            spath = ""
        Else
            spath = ""
        End If
    End Sub
End Class
