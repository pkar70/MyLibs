
Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        Dim ala = "ma <kota> 'i juz"
        Dim ela = New XText(ala)
        Debug.WriteLine(ela.ToString)
    End Sub


End Class
