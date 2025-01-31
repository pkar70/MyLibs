' Nuget:
' "Microsoft.OneDriveSDK" 2.0.7
' "Microsoft.OneDriveSDK.Authentication" 1.0.10

' u¿ywanie:
' 1) rejestracja app z dostêpem do Azure: https://docs.microsoft.com/en-us/archive/msdn-magazine/2016/february/windows-10-implementing-a-uwp-app-with-the-official-onedrive-sdk
' albo tu: https://docs.microsoft.com/en-us/onedrive/developer/rest-api/getting-started/msa-oauth?view=odsp-graph-online
' ale chyba to dla Android jest potrzebne, bo dla Windows - nie. Dla Win: dodaj Permission dla Internet :)
' 2) z UI: ODfolder = ODclient.GetRootAsync(true/false) w zale¿noœci od tego czy w ramach \Apps\ czy w ogóle root
' 3) ODfolder.GetFolderAsync, ODfolder.GetFileAsync
' 4) ODfile.ReadContentAsync, WriteContentAsync

' "Accounts in any organizational directory (Any Azure AD directory - Multitenant) and personal Microsoft accounts (e.g. Skype, Xbox)"
' RedirectUri: public client, https://login.microsoftonline.com/common/oauth2/nativeclient
' Application (client) ID jest potem wykorzystywane w wersji dla Android (see: Andro2UWP, shared, app.xaml.cs)

Imports Windows.ApplicationModel.AppService
Imports Windows.Storage.Streams

''' <summary>
''' To jest w³asciwie tylko do wykorzystywania jako new ODclient, await ODclient.GetRoot
''' </summary>
Public NotInheritable Class ODclient
    Friend Shared _oOneDriveClnt As Microsoft.OneDrive.Sdk.IOneDriveClient

    ''' <summary>
    ''' zwyk³e sprawdzenie czy _oOneDriveClnt jest nie NULL
    ''' </summary>
    Public Shared Function IsOneDriveOpened() As Boolean
        Return _oOneDriveClnt IsNot Nothing
    End Function

    ''' <summary>
    ''' Zwraca ROOT folder, albo w ramach OD:\apps\[app] albo OD:\
    ''' </summary>
    ''' <param name="bInApp">true, w ramach \apps\; false: generalny root</param>
    ''' <param name="bCanUseUI">true, mo¿na pokazaæ okienka; false: dla pracy w tle</param>
    Public Shared Function GetRootAsync(bInApp As Boolean, bCanUseUI As Boolean) As IAsyncOperation(Of ODfolder)
        Return GetRootTask(bInApp, bCanUseUI).AsAsyncOperation
    End Function

    ''' <summary>
    ''' Zwraca ROOT folder, albo w ramach OD:\apps\[app] albo OD:\; mo¿liwe u¿ycie UI
    ''' </summary>
    ''' <param name="bInApp">true, w ramach \apps\; false: generalny root</param>
    Public Shared Function GetRootAsync(bInApp As Boolean) As IAsyncOperation(Of ODfolder)
        Return GetRootTask(bInApp).AsAsyncOperation
    End Function

    ''' <summary>
    ''' Zwraca ROOT folder dla aplikacji (OD:\apps\[app]); mo¿liwe u¿ycie UI
    ''' </summary>
    ''' <returns>ODfolder</returns>
    Public Shared Function GetRootAsync() As IAsyncOperation(Of ODfolder)
        Return GetRootTask().AsAsyncOperation
    End Function


    Friend Shared Async Function GetRootTask(Optional bInApp As Boolean = True, Optional bCanUseUI As Boolean = True) As Task(Of ODfolder)
        If Not IsOneDriveOpened() Then
            'If Not NetIsIPavailable(False) Then Return Nothing
            If Not Await OpenOneDriveAsync(bCanUseUI) Then Return Nothing
        End If

        If bInApp Then
            'Return New ODfolder(_oOneDriveClnt.Drive.Special.AppRoot)
            ' to powy¿ej dzia³a jak jest include w app, ale jak jest w Nuget, b¹dŸ included project, niestety nie - st¹d "rêczne" przejœcie
            Dim oRt As New ODfolder(_oOneDriveClnt.Drive.Root)
            If oRt Is Nothing Then Return Nothing
            oRt = Await oRt.GetFolderAsync("Apps", True)
            If oRt Is Nothing Then Return Nothing

            Dim sAppName As String = Package.Current.DisplayName
            sAppName = sAppName.Replace(" ", "").Replace("'", "")

            oRt = Await oRt.GetFolderAsync(sAppName, True)
            Return oRt
        Else
            Return New ODfolder(_oOneDriveClnt.Drive.Root)
        End If

    End Function

    Private Shared Async Function OpenOneDriveAsync(bCanUseUI As Boolean) As Task(Of Boolean)
        ' https://github.com/OneDrive/onedrive-sample-photobrowser-uwp/blob/master/OneDrivePhotoBrowser/AccountSelection.xaml.cs
        ' dla PC tu bedzie error, wiec zwróci FALSE

        Dim sScopes As String() = {"onedrive.readwrite", "offline_access"}
        Const oneDriveConsumerBaseUrl As String = "https://api.onedrive.com/v1.0"

        Try
            Dim onlineIdAuthProvider As New Microsoft.OneDrive.Sdk.OnlineIdAuthenticationProvider(sScopes)
            Dim authTask As Task
            If bCanUseUI Then
                authTask = onlineIdAuthProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync()
            Else
                authTask = onlineIdAuthProvider.RestoreMostRecentFromCacheAsync()
            End If
            'Await authTask

            _oOneDriveClnt = New Microsoft.OneDrive.Sdk.OneDriveClient(oneDriveConsumerBaseUrl, onlineIdAuthProvider)
            Await authTask     ' tu jest w samplu - po moOneDriveClnt

            Return True
        Catch ex As Exception
            _oOneDriveClnt = Nothing
            If ex.InnerException.Message.Contains("The application requesting authentication tokens is either disabled or incorrectly configured") Then
                Throw New InvalidOperationException("¯eby zadzia³a³ OneDrive, musi app byæ podpiêta do Store - zarezerwuj nazwê, zrób associate; wysy³aæ app do Store nie trzeba")
            End If

            Return False
        End Try

    End Function

End Class

Public Class ODfolder
    Private ReadOnly _oBuilder As Microsoft.OneDrive.Sdk.ItemRequestBuilder
    Private ReadOnly _sPath As String

    Friend Sub New(oBuilder As Microsoft.OneDrive.Sdk.ItemRequestBuilder, Optional sPath As String = "")
        If oBuilder Is Nothing Then Throw New ArgumentNullException("cannot create ODfolder from NULL")
        _oBuilder = oBuilder

        _sPath = sPath
        If sPath <> "" AndAlso Not sPath.EndsWith("/") Then _sPath = sPath & "/"

    End Sub

    Private Function GetPathNoSlash() As String
        Dim path As String = _sPath
        If Not path.EndsWith("/") Then Return path

        Return path.Substring(0, path.Length - 1)
    End Function

    Private Function GetPathWithSlash() As String
        Dim path As String = _sPath
        If path.EndsWith("/") Then Return path

        Return path & "/"
    End Function

#Region "chodzenie po katalogach"

    ''' <summary>
    ''' zwróæ podkatalog w ramach aktualnego katalogu, z ewentualnym jego tworzeniem
    ''' </summary>
    ''' <param name="sName">nazwa podkatalogu</param>
    ''' <param name="bCreate">czy mo¿na utworzyæ katalog</param>
    ''' <returns>MULL gdy nie ma katalogu (lub go nie mo¿na stworzyæ)</returns>
    Public Function GetFolderAsync(sName As String, bCreate As Boolean) As IAsyncOperation(Of ODfolder)
        Return GetFolderTask(sName, bCreate).AsAsyncOperation
    End Function

    Private Async Function GetFolderTask(sName As String, bCreate As Boolean) As Task(Of ODfolder)
        If sName = "" Then Return Nothing

        'If Not NetIsIPavailable(False) Then Return Nothing

        Try
            Dim oReq As Microsoft.OneDrive.Sdk.ItemRequest = _oBuilder.ItemWithPath(GetPathWithSlash() & sName).Request
            Dim oItem = Await oReq.GetAsync()
            'Return New ODfolder(_oBuilder.ItemWithPath(sName))
            Return New ODfolder(_oBuilder, GetPathWithSlash() & sName)
        Catch ex As Exception
            Dim a = 1
        End Try

        ' jak tu jestem, znaczy ¿e nie ma folderu
        If Not bCreate Then Return Nothing

        ' proba utworzenia katalogu
        Dim oNew As New Microsoft.OneDrive.Sdk.Item
        oNew.Name = sName
        oNew.Folder = New Microsoft.OneDrive.Sdk.Folder()

        Dim oFolder As Microsoft.OneDrive.Sdk.Item
        oFolder = Await _oBuilder.ItemWithPath(_sPath).Children.Request().AddAsync(oNew)

        Return New ODfolder(_oBuilder, GetPathWithSlash() & sName)

    End Function

#End Region

#Region "dir listing"

    ''' <summary>
    ''' Zwróæ nazwy plików/folderów w katalogu
    ''' </summary>
    ''' <param name="bFolders">uwzglêdniaj foldery</param>
    ''' <param name="bFiles">uwzglêdniaj pliki</param>
    Public Function GetItemsAsStringsAsync(bFolders As Boolean, bFiles As Boolean) As IAsyncOperation(Of IList(Of String))
        Return GetItemsAsStringsTask(bFolders, bFiles).AsAsyncOperation
    End Function


    Private Async Function GetItemsAsStringsTask(bFolders As Boolean, bFiles As Boolean) As Task(Of IList(Of String))

        Dim lNames As New List(Of String)

        Dim oItems As List(Of ODfile) = Await GetItemsAsItemsAsync(bFolders, bFiles)
        If oItems Is Nothing Then Return lNames

        For Each oItem As ODfile In oItems
            lNames.Add(oItem.GetName)
        Next

        Return lNames

    End Function

    ''' <summary>
    ''' Zwróæ pliki/foldery w katalogu
    ''' </summary>
    ''' <param name="bFolders">uwzglêdniaj foldery</param>
    ''' <param name="bFiles">uwzglêdniaj pliki</param>
    Public Function GetItemsAsItemsAsync(bFolders As Boolean, bFiles As Boolean) As IAsyncOperation(Of IList(Of ODfile))
        Return GetItemsAsItemsTask(bFolders, bFiles).AsAsyncOperation
    End Function


    Private Async Function GetItemsAsItemsTask(bFolders As Boolean, bFiles As Boolean) As Task(Of IList(Of ODfile))

        Dim oItems As New List(Of ODfile)

        Try

            Dim oLista As Microsoft.OneDrive.Sdk.Item =
                Await _oBuilder.ItemWithPath(GetPathNoSlash).Request().Expand("children").GetAsync

            For Each oItem As Microsoft.OneDrive.Sdk.Item In oLista.Children.CurrentPage
                If bFolders AndAlso oItem.Folder IsNot Nothing Then oItems.Add(New ODfile(oItem))
                If bFiles AndAlso oItem.File IsNot Nothing Then oItems.Add(New ODfile(oItem))
            Next

            If oLista.Children.NextPageRequest Is Nothing Then Return oItems

            Dim oNew As Microsoft.OneDrive.Sdk.ItemChildrenCollectionPage =
            Await oLista.Children.NextPageRequest.GetAsync

            For iGuard As Integer = 1 To 12000 / 200   ' itemow moze byc, przez itemów na stronê
                For Each oItem As Microsoft.OneDrive.Sdk.Item In oNew.CurrentPage
                    If bFolders AndAlso oItem.Folder IsNot Nothing Then oItems.Add(New ODfile(oItem))
                    If bFiles AndAlso oItem.File IsNot Nothing Then oItems.Add(New ODfile(oItem))
                Next
                If oNew.NextPageRequest Is Nothing Then Return oItems
                oNew = Await oNew.NextPageRequest.GetAsync
            Next

            Return oItems

        Catch ex As Exception
            Debug.WriteLine("@OneDriveGetAllChildsSDK: " & ex.Message)
        End Try

        Return Nothing

    End Function

#End Region

    ''' <summary>
    ''' zwróæ plik o podanej nazwie w bie¿¹cym katalogu
    ''' </summary>
    ''' <param name="sFilename">nazwa pliku</param>
    ''' <param name="bCreate">jak nie ma pliku to go stwórz</param>
    ''' <returns>NULL gdy nie ma pliku</returns>
    Public Function GetFileAsync(sFilename As String, bCreate As Boolean) As IAsyncOperation(Of ODfile)
        Return GetFileTask(sFilename, bCreate).AsAsyncOperation
    End Function

    ''' <summary>
    ''' zwróæ plik o podanej nazwie w bie¿¹cym katalogu
    ''' </summary>
    ''' <param name="sFilename">nazwa pliku</param>
    ''' <returns>NULL gdy nie ma pliku</returns>
    Public Function GetFileAsync(sFilename As String) As IAsyncOperation(Of ODfile)
        Return GetFileTask(sFilename).AsAsyncOperation
    End Function


    Friend Async Function GetFileTask(sFilename As String, Optional bCreate As Boolean = False) As Task(Of ODfile)

        Dim oItem As Microsoft.OneDrive.Sdk.Item

        Try
            Dim req = _oBuilder.ItemWithPath(GetPathWithSlash() & sFilename).Request()
            oItem = Await req.GetAsync
            If oItem IsNot Nothing Then Return New ODfile(oItem)
        Catch
        End Try

        If Not bCreate Then Return Nothing

        ' stwórz plik

        Using oStream As New MemoryStream
            Using oWrtr = New StreamWriter(oStream)
                oWrtr.WriteLine("")
                oWrtr.Flush()
                oStream.Seek(0, SeekOrigin.Begin)

                Try
                    ' utworzy plik dwubajtowy
                    oItem = Await _oBuilder.ItemWithPath(GetPathWithSlash() & sFilename).Content.Request.PutAsync(Of Microsoft.OneDrive.Sdk.Item)(oStream)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Using
        End Using

        Return New ODfile(oItem)

    End Function

    Private mbInUsunPlikiOneDrive As Boolean = False

    ''' <summary>
    ''' kasowanie podanego pliku
    ''' </summary>
    ''' <param name="sFilename">nazwa pliku</param>
    Public Function RemoveFileAsync(sFilename As String) As IAsyncAction
        Return RemoveFileTask(sFilename).AsAsyncAction
    End Function

    Private Async Function RemoveFileTask(sFilename As String) As Task

        ' gdy nie ma sieci, przerwij - na wypadek jakby trwa³o Del, a zacz¹³ robiæ fotkê i by³ error powoduj¹cy reset WiFi
        'If Not NetIsIPavailable(False) Then Return

        Try

            Await _oBuilder.ItemWithPath(GetPathWithSlash() & sFilename).Request.DeleteAsync
        Catch ex As Exception
            ' pliku moze nie byc
        End Try

    End Function

    ''' <summary>
    ''' kasowanie listy plików (nazwy)
    ''' </summary>
    ''' <param name="lFilesToDel">lista nazw plików</param>
    Public Function RemoveFilesAsync(lFilesToDel As IList(Of String)) As IAsyncAction
        Return RemoveFilesTask(lFilesToDel).AsAsyncAction
    End Function

    Private Async Function RemoveFilesTask(lFilesToDel As IList(Of String)) As Task

        If mbInUsunPlikiOneDrive Then Exit Function
        mbInUsunPlikiOneDrive = True

        For Each sFileName As String In lFilesToDel
            Await RemoveFileAsync(sFileName)
        Next

        mbInUsunPlikiOneDrive = False
    End Function

    ''' <summary>
    ''' Kopiuje plik na OneDrive (z LastModifiedDateTime zgodnym z dat¹ z pliku), RET true = OK
    ''' </summary>
    ''' <param name="oFile">Plik do przekopiowania</param>
    ''' <returns>True na OK</returns>
    Public Function CopyFileToOneDriveAsync(oFile As Windows.Storage.StorageFile) As IAsyncOperation(Of Boolean)
        Return CopyFileToOneDriveBoolTask(oFile).AsAsyncOperation
    End Function


    Private Async Function CopyFileToOneDriveBoolTask(oFile As Windows.Storage.StorageFile) As Task(Of Boolean)
        Dim plik As ODfile = Await CopyFileToOneDriveTask(oFile)
        Return plik IsNot Nothing
    End Function

    Friend Async Function CopyFileToOneDriveTask(oFile As Windows.Storage.StorageFile) As Task(Of ODfile)

        Try

            Dim oStream = Await oFile.OpenStreamForReadAsync()
            Dim oItem As Microsoft.OneDrive.Sdk.Item = Nothing
            Dim bError As Boolean = False

            Try
                oItem = Await _oBuilder.ItemWithPath(GetPathWithSlash() & oFile.Name).
                    Content.Request.PutAsync(Of Microsoft.OneDrive.Sdk.Item)(oStream)   ' (oRdr.BaseStream)
                oItem.LastModifiedDateTime = (Await oFile.GetBasicPropertiesAsync).DateModified
            Catch ex As Exception
                Debug.WriteLine("@CopyFileToOneDrive while trying to copy file: " & ex.Message)
                Return Nothing
            End Try

            Return New ODfile(Await _oBuilder.ItemWithPath(GetPathWithSlash() & oFile.Name).Request.GetAsync)

        Catch ex As Exception
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' zwraca zawartoœæ pliku (jako string)
    ''' </summary>
    ''' <param name="sFilename">nazwa pliku</param>
    Public Function FileReadStringAsync(sFilename As String) As IAsyncOperation(Of String)
        Return FileReadStringTask(sFilename).AsAsyncOperation
    End Function

    Private Async Function FileReadStringTask(sFilename As String) As Task(Of String)
        Dim oFile As ODfile = Await GetFileAsync(sFilename, False)
        If oFile Is Nothing Then Return ""

        Return Await oFile.ReadContentAsync()
    End Function

    ''' <summary>
    ''' Zapisuje plik (overwrite), zwraca false gdy zapis nieudany
    ''' </summary>
    ''' <param name="sFilename">nazwa pliku</param>
    ''' <param name="sContent">zawartoœæ pliku</param>
    ''' <returns></returns>
    Public Function FileWriteStringAsync(sFilename As String, sContent As String) As IAsyncOperation(Of Boolean)
        Return FileWriteStringTask(sFilename, sContent).AsAsyncOperation
    End Function


    Private Async Function FileWriteStringTask(sFilename As String, sContent As String) As Task(Of Boolean)
        Dim oFile As ODfile = Await GetFileAsync(sFilename, True)
        If oFile Is Nothing Then Return False

        Return Await oFile.WriteContentAsync(sContent)
    End Function


End Class

Public Class ODfile
    Private ReadOnly _oSDKitem As Microsoft.OneDrive.Sdk.Item
    Private ReadOnly _sPath As String

    ''' <summary>
    ''' zwraca nazwê pliku (bez œcie¿ki)
    ''' </summary>
    Public Function GetName() As String
        Dim sRet As String = _oSDKitem.Name
        Dim iInd As Integer = sRet.LastIndexOf("/")
        If iInd < 0 Then Return sRet
        Return sRet.Substring(iInd + 1)
    End Function

    Friend Sub New(oItem As Microsoft.OneDrive.Sdk.Item)
        If oItem Is Nothing Then Throw New ArgumentNullException("cannot create ODfile with NULL item")
        _oSDKitem = oItem
    End Sub

    ''' <summary>
    ''' zwraca datê ostatniej aktualizacji pliku
    ''' </summary>
    ''' <returns>data, lub 1970.01.01 gdy b³¹d</returns>
    Public Function GetLastModDate() As DateTimeOffset
        Dim oData As DateTimeOffset? = _oSDKitem.LastModifiedDateTime
        If oData Is Nothing Then Return New DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromSeconds(0))
        Return oData
    End Function

    ''' <summary>
    ''' zwraca datê stworzenia pliku
    ''' </summary>
    ''' <returns>data, lub 1970.01.01 gdy b³¹d</returns>
    Public Function GetCreatedDate() As DateTimeOffset
        Dim oData As DateTimeOffset? = _oSDKitem.CreatedDateTime
        If oData Is Nothing Then Return New DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromSeconds(0))
        Return oData
    End Function



    ''' <summary>
    ''' zwraca RandomAccessString (bo nie mo¿na w WinRT zwróciæ Stream)
    ''' </summary>
    Public Function GetRandomAccessStreamAsync() As IAsyncOperation(Of RandomAccessStream)
        Return GetRandomAccessStreamTask.AsAsyncOperation
    End Function

    Private Async Function GetRandomAccessStreamTask() As Task(Of RandomAccessStream)
        Dim strumyk As Stream = Await GetStreamTask()
        Return strumyk.AsRandomAccessStream
    End Function


    Friend Async Function GetStreamTask() As Task(Of Stream)
        'https://msdn.microsoft.com/en-us/magazine/mt632271.aspx

        Dim oItemReq As Microsoft.OneDrive.Sdk.ItemRequestBuilder
        oItemReq = ODclient._oOneDriveClnt.Drive.Items(_oSDKitem.Id)

        Try
            Dim oStream As Stream = Await oItemReq.Content.Request.GetAsync

            Return oStream
        Catch ex As Exception
            Debug.WriteLine("@GetOneDriveFileStream: " & ex.Message)
            Return Nothing
        End Try

    End Function



    ''' <summary>
    ''' zapisanie sTresc do pliku (overwrite)
    ''' </summary>
    ''' <param name="sTresc">docelowa zawartoœæ pliku</param>
    ''' <returns></returns>
    Public Function WriteContentAsync(sTresc As String) As IAsyncOperation(Of Boolean)
        Return WriteContentTask(sTresc).AsAsyncOperation
    End Function


    Private Async Function WriteContentTask(sTresc As String) As Task(Of Boolean)

        Using oStream As New MemoryStream
            Using oWrtr = New StreamWriter(oStream)
                oWrtr.WriteLine(sTresc)
                oWrtr.Flush()

                oStream.Seek(0, SeekOrigin.Begin)

                Dim oItemReq As Microsoft.OneDrive.Sdk.ItemRequestBuilder
                oItemReq = ODclient._oOneDriveClnt.Drive.Items(_oSDKitem.Id)

                Try
                    Await oItemReq.Content.Request.PutAsync(Of Microsoft.OneDrive.Sdk.Item)(oStream)   ' (oRdr.BaseStream)
                Catch ex As Exception
                    Return False
                End Try
            End Using
        End Using

        Return True
    End Function


    ''' <summary>
    ''' wczytanie zawartoœci pliku
    ''' </summary>
    ''' <returns></returns>
    Public Function ReadContentAsync() As IAsyncOperation(Of String)
        Return ReadContentTask.AsAsyncOperation
    End Function


    Private Async Function ReadContentTask() As Task(Of String)
        Dim oStream As Stream = Await GetStreamTask()
        If oStream Is Nothing Then Return ""

        Dim oRdr = New StreamReader(oStream)
        Dim sTxt As String = oRdr.ReadToEnd()
        oRdr.Dispose()
        oStream.Dispose()

        Return sTxt

    End Function


    ''' <summary>
    ''' zwraca link (typu VIEW) do pliku
    ''' </summary>
    Public Function GetLinkAsync() As IAsyncOperation(Of String)
        Return GetLinkTask.AsAsyncOperation
    End Function

    Private Async Function GetLinkTask() As Task(Of String)
        Dim sLink = Await ODclient._oOneDriveClnt.Drive.Items(_oSDKitem.Id).CreateLink("view").Request().PostAsync()
        Return sLink.Link.WebUrl.ToString
    End Function

End Class

