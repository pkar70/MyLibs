
Imports System.Reflection
Imports pkar.DotNetExtensions

Public Class Localize

    Private Shared _ResManLangList As New List(Of ResManLang)
    Private Shared _resPrefix As String = "res:"
    Private Shared _fallbackReplace As String = ""
    Private Shared _currResMan As ResManLang
    Private Shared _defaultResMan As ResManLang
    Private Shared _resmanStack As ResManLang

    ''' <summary>
    ''' initializes library
    ''' </summary>
    ''' <param name="resPrefix">Prefix in input string which means that this string is to be looked-up from ResourceManager</param>
    ''' <param name="fallbackReplacePrefix">Prefix which would be replacement for resPrefix if no such string would be found</param>
    Public Shared Sub InitResMan(Optional resPrefix As String = "res:", Optional fallbackReplacePrefix As String = "")
        _ResManLangList = New List(Of ResManLang)
        _resPrefix = resPrefix
        _fallbackReplace = fallbackReplacePrefix
    End Sub

    ''' <summary>
    ''' Check how many resman we have
    ''' </summary>
    ''' <returns>Number of resmans in internal list</returns>
    Public Shared Function GetResManCount() As Integer
        Return _ResManLangList.Count
    End Function

    ''' <summary>
    ''' Check if we have at least one resman 
    ''' </summary>
    Public Shared Function IsInitialized() As Boolean
        Return _ResManLangList.Count > 0
    End Function

    ''' <summary>
    ''' Add ResourceManager to list of known ResourceManagers, possibly using is also as default
    ''' </summary>
    ''' <param name="lang">For which language this ResourceManager should be used</param>
    ''' <param name="IsDefault">If True, this ResourceManager would be used if no other matches selected language</param>
    Public Shared Sub AddResMan(lang As String, resman As Resources.ResourceManager, Optional IsDefault As Boolean = False)
        Dim oNew As New ResManLang(lang, resman)
        _ResManLangList.Add(oNew)
        If IsDefault Then _defaultResMan = oNew
    End Sub

    ''' <summary>
    ''' Force language (use ResourceManager for this language)
    ''' </summary>
    ''' <param name="forceLanguage">Language to be forced, or NULL to return to resman used before Override</param>
    ''' <returns>Language used previously</returns>
    Public Shared Function LangOverride(forceLanguage As String) As String

        If String.IsNullOrWhiteSpace(forceLanguage) Then
            Dim currLang As String = _currResMan?.lang
            _currResMan = _resmanStack
            _resmanStack = Nothing
            Return currLang
        End If

        If _resmanStack Is Nothing Then _resmanStack = _currResMan

        For Each oResMan As ResManLang In _ResManLangList
            If oResMan.lang.StartsWithCI(forceLanguage) Then
                _currResMan = oResMan
                Return _resmanStack?.lang
            End If
        Next

        Return Nothing

    End Function

    ''' <summary>
    ''' Force this ResourceManager, or return to resman used before Override
    ''' </summary>
    ''' <returns>Language used previously</returns>
    Public Shared Function ResManOverride(forceResMan As Resources.ResourceManager) As String

        If forceResMan Is Nothing Then
            Dim currLang As String = _currResMan?.lang
            _currResMan = _resmanStack
            _resmanStack = Nothing
            Return currLang
        End If

        If _resmanStack Is Nothing Then _resmanStack = _currResMan
        _currResMan = New ResManLang("__", forceResMan)

        Return _resmanStack?.lang

    End Function

    ''' <summary>
    ''' select resman language using CurrentCulture.TwoLetterISOLanguageName
    ''' </summary>
    ''' <returns>Language used previously</returns>
    Public Shared Function SelectCurrentLang() As String
        Return LangOverride(Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
    End Function

    ''' <summary>
    ''' check language of currently used resman (sometimes it would be resman forced!)
    ''' </summary>
    ''' <param name="lang"></param>
    ''' <returns></returns>
    Public Shared Function IsCurrentLang(Optional lang As String = "en") As Boolean
        Return _currResMan?.lang.StartsWithCI(lang)
    End Function

    Private Shared Sub EnsureInit()
        If _currResMan IsNot Nothing Then Return
        _currResMan = _defaultResMan
    End Sub


    ''' <summary>
    ''' return string from current ResMan
    ''' </summary>
    ''' <param name="sResID">resource ID to be looked-up</param>
    ''' <param name="sDefault">default value</param>
    Public Shared Function GetResManString(sResID As String, Optional sDefault As String = "") As String
        EnsureInit()
        Return If(_currResMan?.resman.GetString(sResID), sDefault)
    End Function

    ''' <summary>
    ''' Try to return localized string, if inputString is prefixed with resource prefix (set in InitResMan)
    ''' </summary>
    ''' <param name="inputString">String to be looked-up</param>
    ''' <param name="sDefault">Default value; if NULL or empty, it would be constructed from inputString and fallback set up in InitResMan</param>
    ''' <returns>If inputString is not prefixed, original string; else localized string (maybe using default or fallback)</returns>
    Public Shared Function TryGetResManString(inputString As String, Optional sDefault As String = "") As String
        EnsureInit()

        If Not inputString.StartsWith(_resPrefix) Then Return inputString
        Dim defVal As String = sDefault
        If String.IsNullOrWhiteSpace(defVal) Then
            defVal = inputString.Replace(_resPrefix, _fallbackReplace)
        End If

        Return GetResManString(inputString.Replace(_resPrefix, ""), defVal)
    End Function

    ''' <summary>
    ''' Tries to set all String properties of Object using resources, by object name and property name (e.g. uiPageTitle.Text). If object has no 'name' property, do not touch it; and don't touch property if it has no string in resources defined.
    ''' </summary>
    Public Shared Sub SetPropertiesUsingObjectName(anyObject As Object)
        EnsureInit()

        If anyObject Is Nothing Then Return
        If _currResMan Is Nothing Then Return

        ' search for NAME
        Dim name As String = ""
        For Each oProperty As PropertyInfo In anyObject.GetType.GetRuntimeProperties
            Debug.WriteLine("Property " & oProperty.Name)
            If oProperty.Name = "Name" Then
                name = oProperty.GetValue(anyObject)
                Exit For
            End If
        Next

        If name = "" Then Return

        For Each oProperty As PropertyInfo In anyObject.GetType.GetRuntimeProperties
            ' we are not checking if type is string, because e.g. Button.Content is not string but can be set to string
            'If oProperty.PropertyType Is GetType(String) Then
            Dim newValue As String = GetResManString(name & "." & oProperty.Name, Nothing)
            If newValue IsNot Nothing Then
                Try
                    oProperty.SetValue(anyObject, newValue)
                Catch ex As Exception
                    ' ... but as we don't check property type, maybe we are trying to set property that cannot be set to string
                End Try
            End If
            'End If
        Next

    End Sub

    ''' <summary>
    ''' Tries to set all String properties of Object using resources, if such property value begins with resPrefix.
    ''' </summary>
    Public Shared Sub SetPrefixedProperties(anyObject As Object)
        EnsureInit()

        If anyObject Is Nothing Then Return
        If _currResMan Is Nothing Then Return

        For Each oProperty As PropertyInfo In anyObject.GetType.GetRuntimeProperties
            If oProperty.PropertyType Is GetType(String) Then
                Dim oldVal As String = oProperty.GetValue(anyObject).ToString
                If oldVal.StartsWith(_resPrefix) Then
                    Dim newValue As String = TryGetResManString(oldVal)
                    If newValue IsNot Nothing Then
                        oProperty.SetValue(anyObject, newValue)
                    End If
                End If
            End If
        Next

    End Sub

    Protected Class ResManLang
        Public Property lang As String
        Public Property resman As Resources.ResourceManager
        Public Sub New(forLang As String, ResourcesMngr As Resources.ResourceManager)
            lang = forLang
            resman = ResourcesMngr
        End Sub
    End Class

End Class


