
Imports System.Reflection

Public Class Datalog

    Private msDataLogRootFolder As String = ""

#Region "inits"

    ''' <summary>
    ''' Initialization of library: set root folder for datalog files
    ''' </summary>
    ''' <param name="sRootFolder">root folder for datalog files</param>
    Public Sub SetRootFolder(sRootFolder As String)
        If String.IsNullOrWhiteSpace(sRootFolder) Then
            Throw New InvalidOperationException("Datalog.SetRootFolder requires path to be used as root folder")
        End If
        If Not IO.Directory.Exists(sRootFolder) Then
            Throw New InvalidOperationException($"Datalog.SetRootFolder: folder '{sRootFolder}' doesn't exist")
        End If

        msDataLogRootFolder = sRootFolder
    End Sub

    ''' <summary>
    ''' Initialization of library: set root folder for datalog files
    ''' </summary>
    ''' <param name="sRootFolder">root folder for datalog files</param>
    Public Sub New(sRootFolder As String, sSubfolder As String)
        SetRootFolder(sRootFolder)
        If Not String.IsNullOrWhiteSpace(sSubfolder) Then msDataLogRootFolder = IO.Path.Combine(sRootFolder, sSubfolder)

    End Sub

#If NETSTANDARD2_0_OR_GREATER Then

    ''' <summary>
    ''' Initialization of library: set root folder for datalog files. Inside folder folder for App would be created.
    ''' </summary>
    ''' <param name="specialFolder">folder ID, in this folder subfolder with application name would be created</param>
    ''' <param name="sSubfolder">optional subfolder within specialFolder\APPNAME</param>
    Public Sub New(specialFolder As Environment.SpecialFolder, Optional sSubfolder As String = "Datalog")
        Dim folder As String = Environment.GetFolderPath(specialFolder)

        If Not IO.Directory.Exists(folder) Then
            Throw New InvalidOperationException($"Datalog.New: folder '{folder}' doesn't exist")
        End If

        folder = IO.Path.Combine(folder, GetAppName)
        IO.Directory.CreateDirectory(folder)

        SetRootFolder(folder)
        If Not String.IsNullOrWhiteSpace(sSubfolder) Then msDataLogRootFolder = IO.Path.Combine(folder, sSubfolder)
    End Sub

    ''' <summary>
    ''' Initialization of library: set root folder for datalog files. Inside folder folder for App would be created.
    ''' </summary>
    ''' <param name="folder">folder type (from ENUM)</param>
    ''' <param name="sSubfolder">optional subfolder within specialFolder\APPNAME</param>
    Public Sub New(folderType As DatalogFolder, Optional sSubfolder As String = "Datalog")
        Dim appName As String = GetAppName()
        Dim folder As String
        Select Case folderType
            Case DatalogFolder.Local
                folder = GetLocalPathname(appName)
            Case DatalogFolder.Roam
                folder = GetRoamingPathname(appName)
            Case Else
                Throw New ArgumentException("Datalog.New: bad DatalogFolder parameter")
        End Select

        SetRootFolder(folder)
        If Not String.IsNullOrWhiteSpace(sSubfolder) Then msDataLogRootFolder = IO.Path.Combine(folder, sSubfolder)
    End Sub

    ''' <summary>
    ''' Initialization of library, with Local datalog (works also on UWP). Inside folder folder for App would be created.
    ''' </summary>
    ''' <param name="sSubfolder">optional subfolder within specialFolder\APPNAME</param>
    Public Sub New(Optional sSubfolder As String = "Datalog")
        FromNew(DatalogFolder.Local, sSubfolder)
    End Sub

#Region "internal init subs"

    Public Shared Function GetAppName() As String
        Dim sAssemblyFullName = System.Reflection.Assembly.GetEntryAssembly().FullName
        Dim oAss As New AssemblyName(sAssemblyFullName)
        Return oAss.Name
    End Function

    Private Sub FromNew(folderType As DatalogFolder, sSubfolder As String)
        Dim appName As String = GetAppName()
        Dim folder As String
        Select Case folderType
            Case DatalogFolder.Local
                folder = GetLocalPathname(appName)
            Case DatalogFolder.Roam
                folder = GetRoamingPathname(appName)
            Case Else
                Throw New ArgumentException("Datalog.New: bad DatalogFolder parameter")
        End Select

        SetRootFolder(folder)
        If Not String.IsNullOrWhiteSpace(sSubfolder) Then msDataLogRootFolder = IO.Path.Combine(folder, sSubfolder)
    End Sub

    Private Shared Function GetLocalPathname(sAppName As String) As String
        Dim sPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)

        ' UWP
        If sPath.ToLowerInvariant.Contains("local" & IO.Path.DirectorySeparatorChar & "packages") Then
            Return sPath
        End If

        ' WPF = C:\Users\pkar\AppData\Local
        sPath = IO.Path.Combine(sPath, sAppName)
        IO.Directory.CreateDirectory(sPath)
        Return sPath
    End Function

    Private Shared Function GetRoamingPathname(sAppName As String) As String
        ' in UWP, we got C:\Users\pkar\AppData\Roaming as a result of Environment.SpecialFolder.ApplicationData!
        ' so we have to use work-around
        Dim sPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)

        If sPath.ToLowerInvariant.Contains("local" & IO.Path.DirectorySeparatorChar & "packages") Then
            ' UWP = C:\Users\xxx\AppData\Local\Packages\xxx\LocalState)
            Return sPath.Replace("LocalState", "RoamingState")
        End If

        ' WPF = C:\Users\pkar\AppData\Local 
        sPath = sPath.Replace("Local", "Roaming")
        sPath = IO.Path.Combine(sPath, sAppName)
        IO.Directory.CreateDirectory(sPath)
        Return sPath

    End Function
#End Region

#End If


#End Region


#Region "appending data to files"

    Public Sub AppendLogYearly(dataToAppend As String)
        Dim sFile As String = GetLogFileYearly()
        IO.File.AppendAllText(sFile, dataToAppend)
    End Sub

    Public Sub AppendLogMonthly(dataToAppend As String)
        Dim sFile As String = GetLogFileMonthly()
        IO.File.AppendAllText(sFile, dataToAppend)
    End Sub

    Public Sub AppendLogDaily(dataToAppend As String)
        Dim sFile As String = GetLogFileDaily()
        IO.File.AppendAllText(sFile, dataToAppend)
    End Sub

    Public Sub AppendLogDailyWithTime(dataToAppend As String)
        Dim sFile As String = GetLogFileDailyWithTime()
        IO.File.AppendAllText(sFile, dataToAppend)
    End Sub


#End Region

    Private Sub EnsureDirExist(sPathname As String)
        If Not IO.Directory.Exists(sPathname) Then IO.Directory.CreateDirectory(sPathname)
    End Sub

#Region "get folder's path"

    ''' <summary>
    ''' return path to folder for year logs (root\year)
    ''' </summary>
    ''' <returns></returns>
    Public Function GetLogFolderYear() As String
        Dim sFold As String = IO.Path.Combine(msDataLogRootFolder, Date.Now.Year.ToString)
        EnsureDirExist(sFold)

        If Not IO.Directory.Exists(sFold) Then Return ""    ' error creating directory

        Return sFold
    End Function

    ''' <summary>
    ''' return path to folder for year logs (root\year\month)
    ''' </summary>
    ''' <returns></returns>
    <CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification:="<Pending>")>
    Public Function GetLogFolderMonth() As String
        Dim sFold As String = GetLogFolderYear()
        If sFold = "" Then Return ""

        sFold = IO.Path.Combine(sFold, Date.Now.ToString("MM"))
        EnsureDirExist(sFold)
        If Not IO.Directory.Exists(sFold) Then Return ""    ' error creating directory
        Return sFold

    End Function

#End Region


#Region "get file's pathnames"

    ''' <summary>
    ''' get filename for daily log (root\YYYY\MM\sBaseName YYYY.MM.dd.sExtension)
    ''' </summary>
    ''' <param name="sBaseName">prefix of filename</param>
    ''' <param name="sExtension">file extension</param>
    ''' <returns></returns>
    Public Function GetLogFileDaily(Optional sBaseName As String = "", Optional sExtension As String = ".txt") As String
        Return CreateFilename(GetLogFolderMonth(), sBaseName, "yyyy.MM.dd", sExtension)
    End Function


    ''' <summary>
    ''' get filename for daily log (root\YYYY\MM\sBaseName YYYY.MM.dd.HH.mm.sExtension)
    ''' </summary>
    ''' <param name="sBaseName">prefix of filename</param>
    ''' <param name="sExtension">file extension</param>
    ''' <returns></returns>
    Public Function GetLogFileDailyWithTime(Optional sBaseName As String = "", Optional sExtension As String = ".txt") As String
        Return CreateFilename(GetLogFolderMonth(), sBaseName, "yyyy.MM.dd.HH.mm", sExtension)
    End Function


    ''' <summary>
    ''' get filename for monthly log (root\YYYY\sBaseName YYYY.MM.sExtension)
    ''' </summary>
    ''' <param name="sBaseName">prefix of filename</param>
    ''' <param name="sExtension">file extension</param>
    ''' <returns></returns>
    Public Function GetLogFileMonthly(Optional sBaseName As String = "", Optional sExtension As String = ".txt") As String
        Return CreateFilename(GetLogFolderYear(), sBaseName, "yyyy.MM", sExtension)
    End Function


    ''' <summary>
    ''' get filename for yearly log (root\sBaseName YYYY.sExtension)
    ''' </summary>
    ''' <param name="sBaseName">prefix of filename</param>
    ''' <param name="sExtension">file extension</param>
    ''' <returns></returns>
    Public Function GetLogFileYearly(Optional sBaseName As String = "", Optional sExtension As String = ".txt") As String
        EnsureDirExist(msDataLogRootFolder)

        Return CreateFilename(msDataLogRootFolder, sBaseName, "yyyy", sExtension)
    End Function


#End Region

    <CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification:="<Pending>")>
    Private Function CreateFilename(sFolder As String, sBaseFilenamePart As String, sDateFormat As String, sExtension As String)
        If String.IsNullOrWhiteSpace(sExtension) Then sExtension = ".txt"
        If Not sExtension.ElementAt(0) = "." Then sExtension = "." & sExtension

        Dim sFile As String
        If String.IsNullOrWhiteSpace(sBaseFilenamePart) Then
            sFile = Date.Now.ToString(sDateFormat)
        Else
            sFile = sBaseFilenamePart & " " & Date.Now.ToString(sDateFormat)
        End If


        Return IO.Path.Combine(sFolder, sFile & sExtension)

    End Function


End Class


#If NETSTANDARD2_0_OR_GREATER Then
Public Enum DatalogFolder
    ''' <summary>
    ''' UWP: C:\Users\XX\AppData\Local\Packages\XXX\LocalState;
    ''' WPF etc: C:\Users\XX\AppData\Local\APPNAME
    ''' </summary>
    Local
    ''' <summary>
    ''' UWP: C:\Users\XX\AppData\Local\Packages\XXX\RoamingState;
    ''' WPF etc: C:\Users\XX\AppData\Roaming\APPNAME
    ''' </summary>
    Roam
    ' Machine
    ' OneDrive
End Enum
#End If
