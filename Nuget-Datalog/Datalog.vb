
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
