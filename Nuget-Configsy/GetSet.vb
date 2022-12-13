Imports MsExtConf = Microsoft.Extensions.Configuration

Module GetSet

    Private _settingsGlobal As MsExtConf.IConfigurationRoot

    Public Sub LibInitSettings(settings As MsExtConf.IConfigurationRoot)
        _settingsGlobal = settings
    End Sub

    Private Sub SettingsCheckInit()
        If _settingsGlobal IsNot Nothing Then Return

        Debug.WriteLine("Settings calls without LibInitSettings, using only temporary values")
        Dim settings As MsExtConf.IConfigurationRoot =
                (New MsExtConf.ConfigurationBuilder).AddJsonRwSettings(IO.Path.GetTempFileName, Nothing).Build()
    End Sub

    Public Sub SetSettingsString(sName As String, value As String, Optional bRoam As Boolean = False)
        SettingsCheckInit()

        If bRoam Then value = "[ROAM]" & value

        _settingsGlobal(sName) = value

    End Sub

    Public Sub SetSettingsInt(sName As String, value As Integer, Optional bRoam As Boolean = False)
        SetSettingsString(sName, value.ToString(System.Globalization.CultureInfo.InvariantCulture), bRoam)
    End Sub

    Public Sub SetSettingsBool(sName As String, value As Boolean, Optional bRoam As Boolean = False)
        SetSettingsString(sName, If(value, "True", "False"), bRoam)
    End Sub

    Public Sub SetSettingsLong(sName As String, value As Long, Optional bRoam As Boolean = False)
        SetSettingsString(sName, value.ToString(System.Globalization.CultureInfo.InvariantCulture), bRoam)
    End Sub

    Public Sub SetSettingsDate(sName As String, value As DateTimeOffset, Optional bRoam As Boolean = False)
        SetSettingsString(sName, value.ToString("yyyy.MM.dd HH:mm:ss"), bRoam)
    End Sub

    Public Sub SetSettingsCurrentDate(sName As String, Optional bRoam As Boolean = False)
        SetSettingsDate(sName, DateTimeOffset.Now, bRoam)
    End Sub


    Private Function GetSettingsNet(sName As String, sDefault As String)
        SettingsCheckInit()
        Dim sRetVal As String = _settingsGlobal(sName)
        If sRetVal IsNot Nothing Then Return sRetVal

        Return sDefault
        ' https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration/src/ConfigurationRoot.cs
        ' widać że zwraca NULL gdy nie trafi na zmienną nigdzie
    End Function

    Public Function GetSettingsString(sName As String, Optional sDefault As String = "") As String
        Return GetSettingsNet(sName, sDefault)
    End Function

    Public Function GetSettingsInt(sName As String, Optional iDefault As Integer = 0) As Integer
        Dim sRetVal As String = GetSettingsNet(sName, iDefault.ToString(System.Globalization.CultureInfo.InvariantCulture))
        Dim iRetVal As Integer = 0
        If Integer.TryParse(sRetVal, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, iRetVal) Then
            Return iRetVal
        End If
        Return iDefault
    End Function

    Public Function GetSettingsBool(sName As String, Optional bDefault As Boolean = False) As Boolean
        Dim sRetVal As String = GetSettingsNet(sName, If(bDefault, "True", "False"))
        If sRetVal.ToLower = "true" Then Return True
        Return False
    End Function

    Public Function GetSettingsLong(sName As String, Optional iDefault As Long = 0) As Long
        Dim sRetVal As String = GetSettingsNet(sName, iDefault.ToString(System.Globalization.CultureInfo.InvariantCulture))
        Dim iRetVal As Long = 0
        If Long.TryParse(sRetVal, Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, iRetVal) Then
            Return iRetVal
        End If
        Return iDefault
    End Function

    Public Function GetSettingsDate(sName As String, Optional sDefault As String = "") As DateTimeOffset
        If sDefault = "" Then sDefault = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")
        Dim sRetVal As String = GetSettingsNet(sName, sDefault)
        Dim dRetVal As DateTimeOffset
        If DateTimeOffset.TryParseExact(sRetVal, {"yyyy.MM.dd HH:mm:ss"},
                             Globalization.CultureInfo.InvariantCulture.DateTimeFormat,
                             Globalization.DateTimeStyles.AllowWhiteSpaces, dRetVal) Then
            Return dRetVal
        End If

        Return DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")
    End Function

End Module
