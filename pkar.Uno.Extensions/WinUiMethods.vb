

''' <summary>
''' helper methods, used by UwpExtensions
''' </summary>
Partial Public Module WinUiMethods

    ''' <summary>
    ''' Same as System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString() in .Net Standard 2.0, which is unavailable for phone applications
    ''' </summary>
    ''' <returns>x.y.z (major, minor, build)</returns>
    Public Function GetAppVers() As String

        Return Windows.ApplicationModel.Package.Current.Id.Version.Major & "." &
        Windows.ApplicationModel.Package.Current.Id.Version.Minor & "." &
        Windows.ApplicationModel.Package.Current.Id.Version.Build

    End Function

    ''' <summary>
    ''' Get app build datetime (using date from AppxManifest.xml)
    ''' </summary>
    ''' <returns>date in "yyyy.MM.dd HH:mm" format, or "" if AppxManifest cannot be found</returns>
    Public Function GetBuildTimestamp(bWithTime As Boolean) As String

        Dim install_folder As String = Windows.ApplicationModel.Package.Current.InstalledLocation.Path
        Dim sManifestPath As String = IO.Path.Combine(install_folder, "AppxManifest.xml")

        Dim sFormat As String = If(bWithTime, "yyyy.MM.dd HH:mm", "yyyy.MM.dd")

        If IO.File.Exists(sManifestPath) Then
            Return IO.File.GetLastWriteTime(sManifestPath).ToString(sFormat)
        End If

        Return ""
    End Function





End Module
