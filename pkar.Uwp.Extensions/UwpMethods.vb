
#If Not NETFX_CORE Then
imports system.io
#End If



''' <summary>
''' helper methods, used by UwpExtensions
''' </summary>
#If NETFX_CORE Then
Partial Public Module UwpMethods
#ElseIf PK_WPF Then
Partial Public Module WpfMethods
#Else
' WinUI
Partial Public Module WinUiMethods
#End If

    ''' <summary>
    ''' Same as System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString() in .Net Standard 2.0, which is unavailable for phone applications
    ''' </summary>
    ''' <returns>x.y.z (major, minor, build)</returns>
    Public Function GetAppVers() As String

#If Not PK_WPF Then
        Return Windows.ApplicationModel.Package.Current.Id.Version.Major & "." &
        Windows.ApplicationModel.Package.Current.Id.Version.Minor & "." &
        Windows.ApplicationModel.Package.Current.Id.Version.Build
#Else
        Return System.Reflection.Assembly.GetEntryAssembly.GetName.Version.ToString(3)
#End If

    End Function

    ''' <summary>
    ''' Get app build datetime (using date from AppxManifest.xml)
    ''' </summary>
    ''' <returns>date in "yyyy.MM.dd HH:mm" format, or "" if AppxManifest cannot be found</returns>
    Public Function GetBuildTimestamp(bWithTime As Boolean) As String

#If Not PK_WPF Then
        Dim install_folder As String = Windows.ApplicationModel.Package.Current.InstalledLocation.Path
        Dim entryFile As String = Path.Combine(install_folder, "AppxManifest.xml")
#Else
        Dim entryFile As String = System.Reflection.Assembly.GetEntryAssembly.Location
#End If

        Dim sFormat As String = If(bWithTime, "yyyy.MM.dd HH:mm", "yyyy.MM.dd")

        If File.Exists(entryFile) Then
            Return File.GetLastWriteTime(entryFile).ToString(sFormat)
        End If

        Return ""
    End Function


End Module

