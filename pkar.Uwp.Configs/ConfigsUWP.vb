

Imports Microsoft.Extensions.Configuration
Imports MsExtConfig = Microsoft.Extensions.Configuration
Imports MsExtPrim = Microsoft.Extensions.Primitives
Imports WinAppData = Windows.Storage.ApplicationData

Public Module UwpConfig

    ''' <summary>
    ''' initialization of library - as a wrapper to pkar.NetConfigs. Used sources: (1) INI source; (2) Environment variables, prefixed with appname; (3) UWP ApplicationData.Current roaming and local; (4) JSON source, also roaming and local; (5) command line arguments
    ''' </summary>
    ''' <param name="sINIcontent">INI source; Content of INI file, NULL or "" if you don't want to use it</param>
    ''' <param name="bIniUseDebug">True if [debug] section is to be used ([main] is used always)</param>
    ''' <param name="cmdLineArgs">CmdLine source, Array of command line arguments (output from Environment.GetCommandLineArgs.ToList, unavailable in .Net Standard 1.4)</param>
    Public Sub InitSettings(
                        sIniContent As String, bIniUseDebug As Boolean,
                        cmdLineArgs As IList(Of String))


        Dim sAppName As String = Windows.ApplicationModel.Package.Current.DisplayName
        ' ale i tak jest Empty
        Dim oDict As IDictionary = Environment.GetEnvironmentVariables()    ' że, w 1.4, zwraca HashTable?

        pkar.NetConfigs.InitSettings(sIniContent, bIniUseDebug, sAppName, oDict, New UwpConfigurationSource,
                           Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                            Windows.Storage.ApplicationData.Current.RoamingFolder.Path, False, cmdLineArgs)
    End Sub

#Region "config from WinAppData.Current.*Settings"

    Private Class UwpConfigurationProvider
        Implements MsExtConfig.IConfigurationProvider

        Private ReadOnly _roamPrefix1 As String = Nothing
        Private ReadOnly _roamPrefix2 As String = Nothing

        ''' <summary>
        ''' Create Configuration Provider, for LocalSettings and RoamSettings
        ''' </summary>
        ''' <param name="sRoamPrefix1">prefix for RoamSettings, use NULL if want only LocalSettings</param>
        ''' <param name="sRoamPrefix2">prefix for RoamSettings, use NULL if want only LocalSettings</param>
        Public Sub New(Optional sRoamPrefix1 As String = "[ROAM]", Optional sRoamPrefix2 As String = Nothing)
            _roamPrefix1 = sRoamPrefix1
            _roamPrefix2 = sRoamPrefix2
        End Sub

        Private Sub LoadData(settSource As IPropertySet)
            For Each oItem In settSource
                Data(oItem.Key) = oItem.Value
            Next
        End Sub

        ''' <summary>
        ''' read current state of settings (all values); although it is not used in TryGet, but we should have Data property set for other reasons (e.g. for listing all variables)...
        ''' </summary>
        Public Sub Load() Implements MsExtConfig.IConfigurationProvider.Load
            LoadData(WinAppData.Current.RoamingSettings.Values)
            LoadData(WinAppData.Current.LocalSettings.Values)
        End Sub


        ''' <summary>
        ''' always set LocalSettings, and if value is prefixed with Roam prefix, also RoamSettings (prefix is stripped)
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="value"></param>
        Public Sub [Set](key As String, value As String) Implements MsExtConfig.IConfigurationProvider.Set
            If value Is Nothing Then value = ""

            If _roamPrefix1 IsNot Nothing AndAlso value.ToUpperInvariant().StartsWith(_roamPrefix1, StringComparison.Ordinal) Then
                value = value.Substring(_roamPrefix1.Length)
                Try
                    WinAppData.Current.RoamingSettings.Values(key) = value
                Catch
                    ' probably length is too big
                End Try
            End If

            If _roamPrefix2 IsNot Nothing AndAlso value.ToUpperInvariant().StartsWith(_roamPrefix2, StringComparison.Ordinal) Then
                value = value.Substring(_roamPrefix2.Length)
                Try
                    WinAppData.Current.RoamingSettings.Values(key) = value
                Catch
                    ' probably length is too big
                End Try
            End If

            Data(key) = value
            Try
                WinAppData.Current.LocalSettings.Values(key) = value
            Catch
                ' probably length is too big
            End Try

        End Sub

        ''' <summary>
        ''' this is used only for iterating keys, not for Get/Set
        ''' </summary>
        ''' <returns></returns>
        Protected Property Data As New Dictionary(Of String, String)

        ''' <summary>
        ''' gets current Value of Key; local value overrides roaming value
        ''' </summary>
        ''' <returns>True if Key is found (and Value is set)</returns>
        Public Function TryGet(key As String, ByRef value As String) As Boolean Implements MsExtConfig.IConfigurationProvider.TryGet

            Dim bFound As Boolean = False

            If WinAppData.Current.RoamingSettings.Values.ContainsKey(key) Then
                value = WinAppData.Current.RoamingSettings.Values(key).ToString
                bFound = True
            End If

            If WinAppData.Current.LocalSettings.Values.ContainsKey(key) Then
                value = WinAppData.Current.LocalSettings.Values(key).ToString
                bFound = True
            End If

            Return bFound

        End Function

        Public Function GetReloadToken() As MsExtPrim.IChangeToken Implements MsExtConfig.IConfigurationProvider.GetReloadToken
            Return New ConfigurationReloadToken
        End Function

        Public Function GetChildKeys(earlierKeys As IEnumerable(Of String), parentPath As String) As IEnumerable(Of String) Implements MsExtConfig.IConfigurationProvider.GetChildKeys
            ' in this configuration, we don't have structure - so just return list

            Dim results As New List(Of String)
            For Each kv As KeyValuePair(Of String, String) In Data
                results.Add(kv.Key)
            Next

            results.Sort()

            Return results
        End Function

    End Class

    Private Class UwpConfigurationSource
        Implements MsExtConfig.IConfigurationSource

        Public Function Build(builder As MsExtConfig.IConfigurationBuilder) As MsExtConfig.IConfigurationProvider Implements MsExtConfig.IConfigurationSource.Build
            Return New UwpConfigurationProvider()
        End Function

    End Class

    ' nie może być public w WinRT, więc nie ma sensu w ogóle; ja i tak tego nie używam
    '<Runtime.CompilerServices.Extension()>
    'Public Function AddUwpSettings(ByVal configurationBuilder As MsExtConfig.IConfigurationBuilder) As MsExtConfig.IConfigurationBuilder
    '    configurationBuilder.Add(New UwpConfigurationSource())
    '    Return configurationBuilder
    'End Function

#End Region
End Module

