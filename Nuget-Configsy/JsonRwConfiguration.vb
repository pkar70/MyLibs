Imports System.Reflection
Imports MsExtConf = Microsoft.Extensions.Configuration


' Difference with Microsoft's implementation:
' a) Microsoft's implementation doesn't work in .Net Standard 1.4, so it cannot be used on phones
' b) Ms version is read-only
' c) limit of this implementation: no tree of values, only flat version
' d) we can have two files for values - local and roaming

Public Class JsonRwConfigurationProvider
    Inherits Microsoft.Extensions.Configuration.ConfigurationProvider

    Private ReadOnly _sPathnameLocal As String = Nothing
    Private ReadOnly _sPathnameRoam As String = Nothing
    Private ReadOnly _sPathnameTemp As String = Nothing

    Protected DataTemp As New Dictionary(Of String, String)
    Protected DataRoam As New Dictionary(Of String, String)

#If NETCOREAPP3_1_OR_GREATER Or NET48_OR_GREATER Then
    Private ReadOnly _sPathnameMachine As String = Nothing
    Private ReadOnly _sPathnameOneDrive As String = Nothing
    Protected DataMachine As New Dictionary(Of String, String)
    Protected DataOneDrive As New Dictionary(Of String, String)
#End If

    Private ReadOnly _bReadOnly As Boolean

    Private Const JSON_FILENAME As String = "AppSettings.json"

    Public Sub New(sPathnameLocal As String, sPathnameRoam As String, bReadOnly As Boolean)
        _sPathnameLocal = sPathnameLocal
        _sPathnameRoam = sPathnameRoam
        _bReadOnly = bReadOnly
    End Sub

    Public Sub New(bUseTemp As Boolean, sPathnameLocal As String, sPathnameRoam As String, bReadOnly As Boolean)
        _sPathnameLocal = sPathnameLocal
        _sPathnameRoam = sPathnameRoam
        _bReadOnly = bReadOnly

        If bUseTemp Then _sPathnameTemp = IO.Path.GetTempFileName()
    End Sub

#If NETSTANDARD2_0_OR_GREATER Then
    Public Sub New(bUseTemp As Boolean, bUseLocal As Boolean, bUseRoam As Boolean, bReadOnly As Boolean)

        Dim sAppName As String = GetAppName()

        _bReadOnly = bReadOnly
        If bUseTemp Then _sPathnameTemp = IO.Path.Combine(IO.Path.GetTempPath, sAppName & ".json")
        If bUseLocal Then _sPathnameLocal = GetLocalPathname(sAppName)
        If bUseRoam Then _sPathnameRoam = GetRoamingPathname(sAppName)

    End Sub

    Private Shared Function GetLocalPathname(sAppName As String) As String
        Dim sPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)

        If sPath.ToLowerInvariant.Contains("local" & IO.Path.DirectorySeparatorChar & "packages") Then
            ' UWP = C:\Users\xxx\AppData\Local\Packages\xxx\LocalState)
            Return IO.Path.Combine(sPath, JSON_FILENAME)
        End If

        ' WPF = C:\Users\xxx\AppData\Local
        sPath = IO.Path.Combine(sPath, sAppName)
        IO.Directory.CreateDirectory(sPath)
        Return IO.Path.Combine(sPath, JSON_FILENAME)
    End Function

    Private Shared Function GetRoamingPathname(sAppName As String) As String
        ' in UWP, we got C:\Users\pkar\AppData\Roaming as a result of Environment.SpecialFolder.ApplicationData!
        ' so we have to use work-around
        Dim sPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)

        If sPath.ToLowerInvariant.Contains("local" & IO.Path.DirectorySeparatorChar & "packages") Then
            ' UWP = C:\Users\xxx\AppData\Local\Packages\xxx\LocalState)
            sPath = sPath.Replace("LocalState", "RoamingState")
            Return IO.Path.Combine(sPath, JSON_FILENAME)
        End If

        ' WPF = C:\Users\xxx\AppData\Local 
        sPath = sPath.Replace("Local", "Roaming")
        sPath = IO.Path.Combine(sPath, sAppName)
        IO.Directory.CreateDirectory(sPath)
        Return IO.Path.Combine(sPath, JSON_FILENAME)

    End Function

    Protected Friend Shared Function GetAppName() As String
        Dim sAssemblyFullName = System.Reflection.Assembly.GetEntryAssembly().FullName
        Dim oAss As New AssemblyName(sAssemblyFullName)
        Return oAss.Name
    End Function

#If NETCOREAPP3_1_OR_GREATER Or NET48_OR_GREATER Then
        Public Sub New(bUseTemp As Boolean, bUseLocal As Boolean, bUseRoam As Boolean, bUseMachine as boolean, bUseOneDrive as boolean, bReadOnly As Boolean)

        Dim sAppName As String = GetAppName()

        _bReadOnly = bReadOnly
        If bUseTemp Then _sPathnameTemp = IO.Path.Combine(IO.Path.GetTempPath, sAppName & ".json")
        If bUseLocal Then _sPathnameLocal = GetLocalPathname(sAppName)
        If bUseRoam Then _sPathnameRoam = GetRoamingPathname(sAppName)
if bUseMachine then
_sPathnameMachine = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            _sPathnameMachine = IO.Path.Combine(_sPathnameMachine, sAppName)
            IO.Directory.CreateDirectory(_sPathnameMachine)
        _sPathnameMachine = IO.Path.Combine(_sPathnameMachine, JSON_FILENAME)
end if

if bUseOneDrive then
_sPathnameOneDrive = Environment.GetEnvironmentVariable("OneDriveConsumer")
if not string.isnullorwhitespace(_sPathnameOneDrive) then
            _sPathnameOneDrive = IO.Path.Combine(_sPathnameOneDrive, "Apps", sAppName)
            IO.Directory.CreateDirectory(_sPathnameOneDrive)
        _sPathnameOneDrive = IO.Path.Combine(_sPathnameOneDrive, JSON_FILENAME)
        end if
end if



end sub



#End If


#End If




    Public Overrides Sub Load()

        ' temp: nie odczytujemy

        ' load settings
        If Not String.IsNullOrWhiteSpace(_sPathnameLocal) AndAlso IO.File.Exists(_sPathnameLocal) Then
            Dim sFileContent As String = IO.File.ReadAllText(_sPathnameLocal)
            Data = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(sFileContent)
        End If

        If Not String.IsNullOrWhiteSpace(_sPathnameRoam) AndAlso IO.File.Exists(_sPathnameRoam) Then
            Dim sFileContent As String = IO.File.ReadAllText(_sPathnameRoam)
            DataRoam = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(sFileContent)
        End If


#If NETCOREAPP3_1_OR_GREATER Or NET48_OR_GREATER Then
        If Not String.IsNullOrWhiteSpace(_sPathnameMachine) AndAlso IO.File.Exists(_sPathnameMachine) Then
            Dim sFileContent As String = IO.File.ReadAllText(_sPathnameMachine)
            DataMachine = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(sFileContent)
        End If

                If Not String.IsNullOrWhiteSpace(_sPathnameOneDrive) AndAlso IO.File.Exists(_sPathnameOneDrive) Then
            Dim sFileContent As String = IO.File.ReadAllText(_sPathnameOneDrive)
            DataOneDrive = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(sFileContent)
        End If

#End If


    End Sub

    Public Overrides Sub [Set](key As String, value As String)

        If _bReadOnly Then
            Data.Remove(key)
            DataRoam.Remove(key)
#If NETCOREAPP3_1_OR_GREATER Or NET48_OR_GREATER Then
            DataMachine.Remove(key)
            DataOneDrive.Remove(key)
#End If
            Return
        End If

        If Not String.IsNullOrWhiteSpace(_sPathnameRoam) Then
            ' interpretujemy [roam]
            If value.ToLower.StartsWith("[roam]", StringComparison.Ordinal) Then
                ' wersja ROAM
                value = value.Substring("[roam]".Length)
                DataRoam(key) = value
                Dim sJson As String = Newtonsoft.Json.JsonConvert.SerializeObject(DataRoam, Newtonsoft.Json.Formatting.Indented)
                IO.File.WriteAllText(_sPathnameRoam, sJson)
            End If

        End If

        If Not String.IsNullOrWhiteSpace(_sPathnameLocal) Then
            Data(key) = value
            Dim sJson As String = Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented)
            IO.File.WriteAllText(_sPathnameLocal, sJson)
        End If

    End Sub

    Public Overrides Function TryGet(key As String, ByRef value As String) As Boolean

        If Data.TryGetValue(key, value) Then Return True
        If DataRoam.TryGetValue(key, value) Then Return True

#If NETCOREAPP3_1_OR_GREATER Or NET48_OR_GREATER Then
        If DataOneDrive.TryGetValue(key, value) Then Return True
        If DataMachine.TryGetValue(key, value) Then Return True
#End If

        Return False

    End Function

End Class

Public Class JsonRwConfigurationSource
    Implements MsExtConf.IConfigurationSource

    Private ReadOnly _sPathnameLocal As String
    Private ReadOnly _sPathnameRoam As String
    Private _bReadOnly As Boolean
    Private ReadOnly _buildMode As Integer
    Private ReadOnly _useTemp As Boolean
    Private ReadOnly _useLocal As Boolean
    Private ReadOnly _useRoam As Boolean

    Public Function Build(builder As MsExtConf.IConfigurationBuilder) As MsExtConf.IConfigurationProvider Implements MsExtConf.IConfigurationSource.Build

        Select Case _buildMode
            Case 1
                Return New JsonRwConfigurationProvider(_sPathnameLocal, _sPathnameRoam, _bReadOnly)
#If NETSTANDARD2_0_OR_GREATER Then
            Case 2
                Return New JsonRwConfigurationProvider(_useTemp, _useLocal, _useRoam, _bReadOnly)
#End If
        End Select

        Throw New NotImplementedException("Bad _buildMode in Build (internal error)")

    End Function

    Public Sub New(sPathnameLocal As String, sPathnameRoam As String, bReadOnly As Boolean)

        If String.IsNullOrWhiteSpace(sPathnameLocal) AndAlso String.IsNullOrWhiteSpace(sPathnameRoam) Then
            Throw New ArgumentException("You have to use at least one real path (to file, or to folder) for JsonRwConfigurationSource constructor")
        End If

        _buildMode = 1

        _bReadOnly = bReadOnly  ' przed poni¿szymi, bo poni¿sze w³¹cza r/o gdy jest to plik w appx
        _sPathnameLocal = TryFileOrPathExist(sPathnameLocal, "AppSettings.json")
        _sPathnameRoam = TryFileOrPathExist(sPathnameRoam, "AppRoamSettings.json")

    End Sub

#If NETSTANDARD2_0_OR_GREATER Then
    Public Sub New(bUseTemp As Boolean, bUseLocal As Boolean, bUseRoam As Boolean, bReadOnly As Boolean)

        If Not (bUseTemp Or bUseLocal Or bUseRoam) Then
            Throw New ArgumentException("You have to enable at least one datafile for JsonRwConfigurationSource constructor")
        End If

        _buildMode = 2

        _bReadOnly = bReadOnly  ' przed poni¿szymi, bo poni¿sze w³¹cza r/o gdy jest to plik w appx
        _useTemp = bUseTemp
        _useLocal = bUseLocal
        _useRoam = bUseRoam
    End Sub
#End If



    Private Function TryFileOrPathExist(sPath As String, sDefaultFileName As String) As String

        If String.IsNullOrWhiteSpace(sPath) Then Return ""

        ' mo¿e byæ œcie¿ka w ramach appx - ale wtedy readonly
        If Not IO.Path.IsPathRooted(sPath) Then
            sPath = IO.Path.Combine(System.AppContext.BaseDirectory, sPath)
            _bReadOnly = True
        End If

        ' gdy plik istnieje, to jest OK
        If IO.File.Exists(sPath) Then Return sPath

        ' gdy to œcie¿ka (katalog), to ma robimy tam plik o domyœlnej nazwie
        If IO.Directory.Exists(sPath) Then
            sPath = IO.Path.Combine(sPath, sDefaultFileName)
            Return sPath
        End If

        ' gdy jest to œcie¿ka do pliku, który nie istnieje - te¿ jest OK
        If IO.Directory.Exists(IO.Path.GetDirectoryName(sPath)) Then Return sPath

        Throw New ArgumentException("Pathname doesn't point to file")

    End Function


End Class







