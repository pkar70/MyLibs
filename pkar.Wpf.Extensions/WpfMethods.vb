''' <summary>
''' helper methods, used by UwpExtensions
''' </summary>
Partial Public Module UwpMethods

    ''' <summary>
    ''' Same as System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString() in .Net Standard 2.0, which is unavailable for phone applications
    ''' </summary>
    ''' <returns>x.y.z (major, minor, build)</returns>
    Public Function GetAppVers() As String
        Return System.Reflection.Assembly.GetEntryAssembly.GetName.Version.ToString(3)
    End Function

    ''' <summary>
    ''' Get app build datetime (using date from main app file)
    ''' </summary>
    ''' <returns>date in "yyyy.MM.dd HH:mm" format, or "" if AppxManifest cannot be found</returns>
    Public Function GetBuildTimestamp(bWithTime As Boolean) As String

        Dim appFile As String = System.Reflection.Assembly.GetEntryAssembly.Location

        Dim sFormat As String = If(bWithTime, "yyyy.MM.dd HH:mm", "yyyy.MM.dd")

        If IO.File.Exists(appFile) Then
            Return IO.File.GetLastWriteTime(appFile).ToString(sFormat)
        End If

        Return ""
    End Function

End Module
