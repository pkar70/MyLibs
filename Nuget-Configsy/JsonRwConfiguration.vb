Imports MsExtConf = Microsoft.Extensions.Configuration


' Difference with Microsoft's implementation:
' a) Microsoft's implementation doesn't work in .Net Standard 1.4, so it cannot be used on phones
' b) Ms version is read-only
' c) limit of this implementation: no tree of values, only flat version
' d) we can have two files for values - local and roaming

Public Class JsonRwConfigurationProvider
    Inherits Microsoft.Extensions.Configuration.ConfigurationProvider

    Private ReadOnly _sPathnameLocal As String
    Private ReadOnly _sPathnameRoam As String
    Private ReadOnly _bReadOnly As Boolean

    Protected DataRoam As New Dictionary(Of String, String)

    Friend Sub New(sPathnameLocal As String, sPathnameRoam As String, bReadOnly As Boolean)
        _sPathnameLocal = sPathnameLocal
        _sPathnameRoam = sPathnameRoam
        _bReadOnly = bReadOnly
    End Sub

    Public Overrides Sub Load()

        ' load settings

        If _sPathnameLocal <> "" AndAlso IO.File.Exists(_sPathnameLocal) Then
            Dim sFileContent As String = IO.File.ReadAllText(_sPathnameLocal)
            Data = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(sFileContent)
        End If

        If _sPathnameRoam <> "" AndAlso IO.File.Exists(_sPathnameRoam) Then
            Dim sFileContent As String = IO.File.ReadAllText(_sPathnameRoam)
            DataRoam = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(sFileContent)
        End If

    End Sub

    Public Overrides Sub [Set](key As String, value As String)

        If _bReadOnly Then
            Data.Remove(key)
            DataRoam.Remove(key)
            Return
        End If

        If _sPathnameRoam <> "" Then
            ' interpretujemy [roam]
            If value.ToLower.StartsWith("[roam]", StringComparison.Ordinal) Then
                ' wersja ROAM
                value = value.Substring("[roam]".Length)
                DataRoam(key) = value
                Dim sJson As String = Newtonsoft.Json.JsonConvert.SerializeObject(DataRoam, Newtonsoft.Json.Formatting.Indented)
                IO.File.WriteAllText(_sPathnameRoam, sJson)
            End If

        End If

        If _sPathnameLocal <> "" Then
            Data(key) = value
            Dim sJson As String = Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented)
            IO.File.WriteAllText(_sPathnameLocal, sJson)
        End If

    End Sub

    Public Overrides Function TryGet(key As String, ByRef value As String) As Boolean
        Dim bRoam As Boolean = DataRoam.TryGetValue(key, value)
        Dim bLocal As Boolean = Data.TryGetValue(key, value)

        Return (bLocal Or bRoam)

    End Function

End Class

Public Class JsonRwConfigurationSource
    Implements MsExtConf.IConfigurationSource

    Private ReadOnly _sPathnameLocal As String
    Private ReadOnly _sPathnameRoam As String
    Private _bReadOnly As Boolean

    Public Function Build(builder As MsExtConf.IConfigurationBuilder) As MsExtConf.IConfigurationProvider Implements MsExtConf.IConfigurationSource.Build
        Return New JsonRwConfigurationProvider(_sPathnameLocal, _sPathnameRoam, _bReadOnly)
    End Function

    Public Sub New(sPathnameLocal As String, sPathnameRoam As String, bReadOnly As Boolean)

        If String.IsNullOrWhiteSpace(sPathnameLocal) AndAlso String.IsNullOrWhiteSpace(sPathnameRoam) Then
            Throw New ArgumentException("You have to use at least one real path (to file, or to folder) for JsonRwConfigurationSource constructor")
        End If

        _bReadOnly = bReadOnly  ' przed poni¿szymi, bo poni¿sze w³¹cza r/o gdy jest to plik w appx
        _sPathnameLocal = TryFileOrPathExist(sPathnameLocal, "AppSettings.json")
        _sPathnameRoam = TryFileOrPathExist(sPathnameRoam, "AppRoamSettings.json")

    End Sub

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







