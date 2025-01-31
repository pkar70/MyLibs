
Public NotInheritable Class OneDriveSync

    Private _listaPlikow As New Dictionary(Of String, Integer)
    Private _localFolder As Windows.Storage.StorageFolder ' 

    Public Sub New(listaPlikow As IList(Of String), localFolder As Windows.Storage.StorageFolder)
        For Each plik In listaPlikow
            _listaPlikow.Add(plik, 0)
        Next
        _localFolder = localFolder
    End Sub

    Private Shared _mODroot As ODfolder ' nie może być tu inicjalizowane, bo potrzebuje UI do logowania do OneDrive

    Private _LastSyncSummary As String

    Private Async Function ZalogujTask(bCanUseUI As Boolean) As Task(Of Boolean)
        If _mODroot Is Nothing Then _mODroot = Await ODclient.GetRootTask(True, bCanUseUI)
        Return _mODroot IsNot Nothing
    End Function

    Public Function ZalogujAsync(bCanUseUI As Boolean) As IAsyncOperation(Of Boolean)
        Return ZalogujTask(bCanUseUI).AsAsyncOperation
    End Function


    'Private Async Function GetLocalFolderAsync() As Task(Of Windows.Storage.StorageFolder)
    '    Try
    '        Return Await Windows.Storage.StorageFolder.GetFolderFromPathAsync(_localFolder)
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function


    Private Async Function SyncujTask() As Task(Of String)

        ' data w parametrach
        ' a jak nie ma daty, to bierze z settings - ale jak sprawdzic czy settings jest initializniete? dodac taką funkcje w Nuget.settings?

        ' opakowywać Syncuj w ProgRingShow

        ' synchronizacja z OneDrive
        If Not NetIsIPavailable() Then Return "FAIL: network is unavailable"
        If _mODroot Is Nothing Then _mODroot = Await ODclient.GetRootTask(True, False)
        If _mODroot Is Nothing Then Return "FAIL: cannot login to OneDrive - try use LogujAsync"

        _LastSyncSummary = "Summary of syncing @" & Date.Now.ToString("yyyy.MM.dd HH:mm") & vbCrLf & vbCrLf

        Dim dLastSync As DateTimeOffset = pkar.NetConfigs.GetSettingsDate("lastODSync", "2000.01.01 12:00:00")

        _LastSyncSummary &= "Last sync time: " & dLastSync.ToString("yyyy.MM.dd HH:mm") & vbCrLf

        _LastSyncSummary &= "From OneDrive to local" & vbCrLf
        Await CopyAllFilesFromOneDriveIfNewer(dLastSync)
        _LastSyncSummary &= $"{vbCrLf}From local to OneDrive{vbCrLf}"
        Await CopyAllFilesToOneDriveIfNewer()

        NetConfigs.SetSettingsCurrentDate("lastODSync")

        Return _LastSyncSummary
    End Function

    Public Function SyncujAsync() As IAsyncOperation(Of String)
        Return SyncujTask.AsAsyncOperation
    End Function

#Region "sync files"

    Private Async Function CopyAllFilesFromOneDriveIfNewer(dLastSync As DateTimeOffset) As Task
        'Dim locFold As Windows.Storage.StorageFolder = _localFolder 'Await GetLocalFolderAsync()
        'If locFold Is Nothing Then Return

        Dim newdict As New Dictionary(Of String, Integer)

        For Each plik In _listaPlikow
            If Await CopyOneFileFromOneDriveIfNewerAsync(_localFolder, plik.Key, dLastSync) Then
                newdict.Add(plik.Key, 1)
            Else
                newdict.Add(plik.Key, plik.Value)
            End If
        Next

        _listaPlikow = newdict

    End Function

    Private Async Function CopyAllFilesToOneDriveIfNewer() As Task
        'Dim locFold As Windows.Storage.StorageFolder = Await GetLocalFolderAsync()
        'If locFold Is Nothing Then Return


        Dim newdict As New Dictionary(Of String, Integer)

        For Each plik In _listaPlikow
            If Await CopyOneFileToOneDriveIfNewerAsync(_localFolder, plik.Key) Then
                newdict.Add(plik.Key, -1)
            Else
                newdict.Add(plik.Key, plik.Value)
            End If
        Next

        _listaPlikow = newdict

    End Function


    Private Async Function CopyOneFileFromOneDriveIfNewerAsync(oDstFolder As Windows.Storage.StorageFolder, sFilename As String, dLastSync As DateTimeOffset) As Task(Of Boolean)

        Dim oRoamFile As Windows.Storage.StorageFile
        Dim oODfile As ODfile = Nothing
        oODfile = Await _mODroot.GetFileTask(sFilename)
        If oODfile Is Nothing Then
            _LastSyncSummary &= $"Skipping {sFilename}: not exist on OD{vbCrLf}"
            Return False ' nie ma pliku w OneDrive, to go nie kopiujemy
        End If

        Dim oDTO As DateTimeOffset = oODfile.GetLastModDate

        If Not Await FileExistsInFolderAsync(oDstFolder, sFilename) Then
            _LastSyncSummary &= $"Downloading {sFilename}, as there is no such file locally{vbCrLf}"
        Else

            If dLastSync.AddSeconds(2) > oDTO Then
                'vb14.DumpMessage("nie kopiuję bo last sync " & dLastSync.ToString & ", onedrive: " & oDTO.ToString)
                _LastSyncSummary &= $"Skipping {sFilename}, OD date is older than last sync{vbCrLf}"
                Return False ' plik w OneDrive jest starszy niż last sync
            End If

            oRoamFile = Await oDstFolder.TryGetItemAsync(sFilename)
            Dim oRoamProp As Windows.Storage.FileProperties.BasicProperties = Await oRoamFile.GetBasicPropertiesAsync

            If oRoamProp.DateModified.AddSeconds(2) > oDTO Then
                _LastSyncSummary &= $"Skipping {sFilename}, OD is older than local{vbCrLf}"
                'vb14.DumpMessage("nie kopiuję bo lokalnie " & oRoamProp.DateModified.ToString & ", onedrive: " & oDTO.ToString)
                Return False ' plik w OneDrive jest starszy
            End If
            'vb14.DumpMessage("kopiuję: lokalnie " & oRoamProp.DateModified.ToString & " < onedrive: " & oDTO.ToString)
            _LastSyncSummary &= $"Downloading {sFilename}, OD is newer than local{vbCrLf}"
        End If
        oRoamFile = Await oDstFolder.CreateFileAsync(sFilename, Windows.Storage.CreationCollisionOption.ReplaceExisting)

        ' no to kopiujemy
        Using oStreamOneDrive = Await oODfile.GetStreamTask
            Using oStreamRoaming = Await oRoamFile.OpenStreamForWriteAsync()
                oStreamOneDrive.CopyTo(oStreamRoaming)
                oStreamRoaming.Flush()
            End Using
        End Using

        Dim sPath As String = oRoamFile.Path
        oRoamFile = Nothing ' tak, żeby plik był zamknięty, zasoby zwolnione, i w ogóle - żeby zadziałała zmiana daty

        ' Dim oFileProp As Windows.Storage.FileProperties.BasicProperties = Await oRoamFile.GetBasicPropertiesAsync
        ' oFileProp.DateModified = oODfile.GetLastModDate
        IO.File.SetLastWriteTime(sPath, New Date(oDTO.Ticks))

        Return True

    End Function


    Private Async Function CopyOneFileToOneDriveIfNewerAsync(oSrcFolder As Windows.Storage.StorageFolder, sFilename As String) As Task(Of Boolean)
        'vb14.DumpCurrMethod(sFilename & " z folderu " & oSrcFolder.Path)

        Dim oRoamFile As Windows.Storage.StorageFile = Await oSrcFolder.TryGetItemAsync(sFilename)

        If oRoamFile Is Nothing Then
            'vb14.DumpMessage("tego pliku nie ma lokalnie")
            Return False
        End If

        Dim oODfile As ODfile = Await _mODroot.GetFileTask(sFilename)

        If oODfile Is Nothing Then
            'vb14.DumpMessage("kopiuję - tego pliku nie ma w OneDrive")
            _LastSyncSummary &= $"Uploading {sFilename}: not exist on OD{vbCrLf}"
            Await _mODroot.CopyFileToOneDriveTask(oRoamFile)
            Return True
        Else
            Dim oRoamProp As Windows.Storage.FileProperties.BasicProperties = Await oRoamFile.GetBasicPropertiesAsync
            Dim oDTO As DateTimeOffset = oODfile.GetLastModDate
            If oRoamProp.DateModified < oODfile.GetLastModDate.AddSeconds(2) Then
                _LastSyncSummary &= $"Skipping {sFilename}, OD is newer or same as local{vbCrLf}"
                'vb14.DumpMessage("nie kopiuję bo lokalnie " & oRoamProp.DateModified.ToString & ", onedrive: " & oDTO.ToString)
                Return False ' plik w OneDrive jest nowszy
            End If

            'vb14.DumpMessage("kopiuję: lokalnie " & oRoamProp.DateModified.ToString & " > onedrive: " & oDTO.ToString)
            _LastSyncSummary &= $"Uploading {sFilename}, OD is older than local{vbCrLf}"
            Await _mODroot.CopyFileToOneDriveTask(oRoamFile)
            Return True
        End If

    End Function



#End Region

#Region "z pkarmodule"
    Private Function NetIsIPavailable() As Boolean
        Return Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()
    End Function

    Private Async Function FileExistsInFolderAsync(oFold As Windows.Storage.StorageFolder, sFileName As String) As Task(Of Boolean)
        Try
            Dim oTemp As Windows.Storage.StorageFile = Await oFold.TryGetItemAsync(sFileName)
            If oTemp Is Nothing Then Return False
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function



#End Region


End Class
