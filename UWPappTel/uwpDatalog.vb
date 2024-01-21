
Imports pkar
Imports Windows.UI.Xaml.Shapes

Public NotInheritable Class UwpDatalog

    ''' <summary>
    ''' Create new Datalog instance, with folder "Datalog" located (device: phone) on memory card (Datalogs\appname), else in ApplicationData.Current.LocalFolder\Datalogs
    ''' Requires Capability "removableStorage" defined in Manifest
    ''' </summary>
    ''' <param name="bUseOwnFolderIfNotSD">True if it can use LocalFolder also on phones</param>
    ''' <returns></returns>
    Public Shared Function GetDatalogAsync(bUseOwnFolderIfNotSD As Boolean) As IAsyncOperation(Of Datalog.Datalog)
        Return GetDatalogAsyncTask(bUseOwnFolderIfNotSD).AsAsyncOperation
    End Function


    Private Shared Async Function GetDatalogAsyncTask(bUseOwnFolderIfNotSD As Boolean) As Task(Of Datalog.Datalog)
        Dim oFold As Windows.Storage.StorageFolder = Await GetLogFolderRootAsync(bUseOwnFolderIfNotSD)
        If oFold Is Nothing Then Return Nothing

        If Not IO.Directory.Exists(oFold.Path) Then IO.Directory.CreateDirectory(oFold.Path)

        Return New Datalog.Datalog(oFold.Path, "")

        ' VBlib.LibInitDataLog(oFold.Path)
    End Function

    ''' <summary>
    ''' Checks if DeviceFamily is "Windows.Mobile"
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function IsFamilyMobile() As Boolean
        Return (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily = "Windows.Mobile")
    End Function

    Private Shared Async Function GetSDcardFolderAsync() As Task(Of Windows.Storage.StorageFolder)
        ' uwaga: musi być w Manifest RemoteStorage oraz fileext!

        Dim oRootDir As Windows.Storage.StorageFolder

        Try
            oRootDir = Windows.Storage.KnownFolders.RemovableDevices
        Catch ex As Exception
            Return Nothing ' brak uprawnień, może być także THROW
        End Try

        Try
            Dim oCards As IReadOnlyList(Of Windows.Storage.StorageFolder) = Await oRootDir.GetFoldersAsync()
            If oCards.Count < 1 Then Return Nothing
            Return oCards(0)
        Catch ex As Exception
            ' nie udało się folderu SD
        End Try

        Return Nothing


    End Function

    Private Shared Async Function GetLogFolderRootAsync(Optional bUseOwnFolderIfNotSD As Boolean = True) As Task(Of Windows.Storage.StorageFolder)
#Disable Warning IDE0059 ' Unnecessary assignment of a value
        Dim oSdCard As Windows.Storage.StorageFolder = Nothing
#Enable Warning IDE0059 ' Unnecessary assignment of a value
        Dim oFold As Windows.Storage.StorageFolder

        If IsFamilyMobile() Then
            oSdCard = Await GetSDcardFolderAsync()

            If oSdCard IsNot Nothing Then
                oFold = Await oSdCard.CreateFolderAsync("DataLogs", Windows.Storage.CreationCollisionOption.OpenIfExists)
                If oFold Is Nothing Then Return Nothing

                Dim sAppName As String = Package.Current.DisplayName
                sAppName = sAppName.Replace(" ", "").Replace("'", "")

                oFold = Await oFold.CreateFolderAsync(sAppName, Windows.Storage.CreationCollisionOption.OpenIfExists)
                If oFold Is Nothing Then Return Nothing
            Else
                If Not bUseOwnFolderIfNotSD Then Return Nothing
                oSdCard = Windows.Storage.ApplicationData.Current.LocalFolder
                oFold = Await oSdCard.CreateFolderAsync("DataLogs", Windows.Storage.CreationCollisionOption.OpenIfExists)
                If oFold Is Nothing Then Return Nothing
            End If
        Else
            oSdCard = Windows.Storage.ApplicationData.Current.LocalFolder
            oFold = Await oSdCard.CreateFolderAsync("DataLogs", Windows.Storage.CreationCollisionOption.OpenIfExists)
            If oFold Is Nothing Then Return Nothing
        End If

        Return oFold
    End Function


End Class

