

Public NotInheritable Class MainPage
    Inherits Page

    Private Async Sub uiClicked_Click(sender As Object, e As RoutedEventArgs)


        'Dim odek As New pkar.UWP.ODclient

#Region "bezposrednio onedrive"

        Dim korzen = Await pkar.UWP.ODclient.GetRootAsync

        Await korzen.RemoveFileAsync("blabla.txt")
        Dim listka = Await korzen.GetItemsAsStringsAsync(False, True)

        Dim fname = Date.Now.ToString("yyyy.MM.dd.HH.mm") & ".txt"
        Await korzen.FileWriteStringAsync(fname, "a to plik w łandrajw")

        Dim plk = Await korzen.GetFileAsync(fname)
        Dim link = Await plk.GetLinkAsync

#End Region
#Region "via sync"

        Dim locfold = Windows.Storage.ApplicationData.Current.LocalFolder

        Dim korzen1 As New pkar.UWP.OneDriveSync({"2025.01.30.15.23.txt"}, locfold)
        Await korzen1.ZalogujAsync(True)

        pkar.NetConfigs.InitSettings("", False, "ala", Nothing, Nothing, "local", "remo", True, Nothing)

        Dim result As String = Await korzen1.SyncujAsync

#End Region


    End Sub
End Class
