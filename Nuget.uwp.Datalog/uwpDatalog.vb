
' *TODO*
'WINMDEXP: Error WME1019: Windows Runtime Class 'Nuget.uwp.Datalog.UwpDatalog' has an invalid base type, 'pkar.Datalog.Datalog'.  Classes must derive either from System.Object or from a composable Windows Runtime class.  Implementation inheritance is not allowed.
'WINMDEXP: Error WME1038 : Method 'Nuget.uwp.Datalog.UwpDatalog.InitDatalogFolder(System.Boolean)' has a parameter of type 'System.Threading.Tasks.Task' in its signature.  Although this type is not a valid Windows Runtime type, it implements interfaces that are valid Windows Runtime types.  Consider changing the method signature to use one of the following types instead: ''.
'WINMDEXP: Error WME1019: Windows Runtime Class 'Nuget.uwp.Datalog.UwpDatalog' has an invalid base type, 'pkar.Datalog.Datalog'.  Classes must derive either from System.Object or from a composable Windows Runtime class.  Implementation inheritance is not allowed.
'WINMDEXP: Error WME1038 : Method 'Nuget.uwp.Datalog.UwpDatalog.InitDatalogFolder(System.Boolean)' has a parameter of type 'System.Threading.Tasks.Task' in its signature.  Although this type is not a valid Windows Runtime type, it implements interfaces that are valid Windows Runtime types.  Consider changing the method signature to use one of the following types instead: ''.
' *TODO*
' nie Inherits nugeta, tylko bezpośrednio Project?
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\Datalog.vb(218,21,218,36): Error WME1095 : Method 'pkar.Datalog.GetLogFileDaily(System.String, System.String)' has parameter 'sBaseName' which is optional. Windows Runtime methods cannot have optional parameters.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\Datalog.vb(218,21,218,36): Error WME1095 : Method 'pkar.Datalog.GetLogFileDaily(System.String, System.String)' has parameter 'sExtension' which is optional. Windows Runtime methods cannot have optional parameters.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\Datalog.vb(229,21,229,44): Error WME1095 : Method 'pkar.Datalog.GetLogFileDailyWithTime(System.String, System.String)' has parameter 'sBaseName' which is optional. Windows Runtime methods cannot have optional parameters.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\Datalog.vb(229,21,229,44): Error WME1095 : Method 'pkar.Datalog.GetLogFileDailyWithTime(System.String, System.String)' has parameter 'sExtension' which is optional. Windows Runtime methods cannot have optional parameters.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\Datalog.vb(240,21,240,38): Error WME1095 : Method 'pkar.Datalog.GetLogFileMonthly(System.String, System.String)' has parameter 'sBaseName' which is optional. Windows Runtime methods cannot have optional parameters.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\Datalog.vb(240,21,240,38): Error WME1095 : Method 'pkar.Datalog.GetLogFileMonthly(System.String, System.String)' has parameter 'sExtension' which is optional. Windows Runtime methods cannot have optional parameters.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\Datalog.vb(251,21,251,37): Error WME1095 : Method 'pkar.Datalog.GetLogFileYearly(System.String, System.String)' has parameter 'sBaseName' which is optional. Windows Runtime methods cannot have optional parameters.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\Datalog.vb(251,21,251,37): Error WME1095 : Method 'pkar.Datalog.GetLogFileYearly(System.String, System.String)' has parameter 'sExtension' which is optional. Windows Runtime methods cannot have optional parameters.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\uwpDatalog.vb(17,27,17,44): Error WME1038 : Method 'pkar.UwpDatalog.InitDatalogFolder(System.Boolean)' has a parameter of type 'System.Threading.Tasks.Task' in its signature.  Although this type is not a valid Windows Runtime type, it implements interfaces that are valid Windows Runtime types.  Consider changing the method signature to use one of the following types instead: ''.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\uwpDatalog.vb(17,27,17,44): Error WME1038 : Method 'pkar.UwpDatalog.InitDatalogFolder(System.Boolean)' has a parameter of type 'System.Threading.Tasks.Task' in its signature.  Although this type is not a valid Windows Runtime type, it implements interfaces that are valid Windows Runtime types.  Consider changing the method signature to use one of the following types instead: ''.
'H:\Home\PIOTR\VStudio\_Vs2017\MyLibs\Nuget.uwp.Datalog\Datalog.vb(3,14,3,21): Error WME1086: Exporting inheritable types Is Not supported. Please mark type 'pkar.Datalog' as NotInheritable.



Imports pkar.Datalog
Imports Windows.UI.Xaml.Shapes

Public NotInheritable Class UwpDatalog

    ''' <summary>
    ''' Create new Datalog instance, with folder "Datalog" located (device: phone) on memory card (Datalogs\appname), else in ApplicationData.Current.LocalFolder\Datalogs
    ''' Requires Capability "removableStorage" defined in Manifest
    ''' </summary>
    ''' <param name="bUseOwnFolderIfNotSD">True if it can use LocalFolder also on phones</param>
    ''' <returns></returns>
    Public Shared Function GetDatalogAsync(bUseOwnFolderIfNotSD As Boolean) As IAsyncOperation(Of pkar.Datalog.Datalog)
        Return GetDatalogAsyncTask(bUseOwnFolderIfNotSD).AsAsyncOperation
    End Function


    Private Shared Async Function GetDatalogAsyncTask(bUseOwnFolderIfNotSD As Boolean) As Task(Of pkar.Datalog.Datalog)
        Dim oFold As Windows.Storage.StorageFolder = Await GetLogFolderRootAsync(bUseOwnFolderIfNotSD)
        If oFold Is Nothing Then Return Nothing

        If Not IO.Directory.Exists(oFold.Path) Then IO.Directory.CreateDirectory(oFold.Path)

        Return New pkar.Datalog.Datalog(oFold.Path, "")

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

