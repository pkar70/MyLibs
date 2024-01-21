
Imports System.IO
Imports System.Reflection
Imports System.Runtime
Imports Microsoft
Imports Microsoft.Extensions.Configuration

Imports pkar.DotNetExtensions
Imports pkar


'Imports Newtonsoft.Json
'Imports Newtonsoft.Json.Linq
' Imports pkar.NetConfigs.Extensions

' 2022.11.08
' mybasicgeopos EmptyGeopos i IsEmpty, StringLat i StringLon (z "." w zapisie)

' Partial Public Class App
' #Region "Back button" - not in .Net
' #Region "RemoteSystem/Background" - not in .Net

#Disable Warning IDE0079 ' Remove unnecessary suppression
#Disable Warning CA2007 'Consider calling ConfigureAwait On the awaited task

Partial Public Module pkarlibmodule14

    Private sLastError As String = ""

    Public Function LibLastError() As String
        Return sLastError
    End Function


#Region "Dump/Crash"
    'Private miLogLevel As Integer = 0
    'Private msLogfilePath As String = ""
    Private msCurrentLog As String = ""

    ''' <summary>
    ''' gdy sLogFilePath="", to nie ma zapisywania do pliku, szablon poniżej w remark
    ''' VB: VBlib.pkarlibmodule.InitDump(GetSettingsInt("debugLogLevel", 0), Windows.Storage.ApplicationData.Current.TemporaryFolder.Path)
    ''' </summary>
    'Public Sub LibInitDump(iLogLevel As Integer, Optional sLogfilePath As String = "")
    '    ' VB: VBlib.pkarlibmodule.InitDump(GetSettingsInt("debugLogLevel", 0), Windows.Storage.ApplicationData.Current.TemporaryFolder.Path)
    '    miLogLevel = iLogLevel
    '    If sLogfilePath <> "" Then sLogfilePath = IO.Path.Combine(sLogfilePath, "log-lib.txt")
    '    'msLogfilePath = sLogfilePath
    'End Sub


    Private Sub DumpMethodOrMsg(bAddMethod As Boolean, sMsg As String, iLevel As Integer)
        ' wyłączenie do SUB poniższych, żeby był wspólny kod do liczenia głębokości

        Dim sPrefix As String = ""
        Dim iDepth As Integer = 0
        Dim sCurrMethod As String = ""

        Dim sTrace As String = Environment.StackTrace
        If String.IsNullOrWhiteSpace(sTrace) Then
            sCurrMethod = "<stack is empty>"
        Else
            Dim subs As String() = sTrace.Split(vbCr, options:=StringSplitOptions.RemoveEmptyEntries)
            Dim iCurrMethod As Integer = -1

            For iLoop As Integer = 0 To subs.Length - 2
                If subs(iLoop).Contains(".DumpMethodOrMsg(") Then
                    If subs(iLoop + 1).Contains(".DumpCurrMethod(") OrElse
                                subs(iLoop + 1).Contains(".DumpMessage(") Then

                        iCurrMethod = iLoop + 2 ' bo ma pominąć: DumpMethodOrMsg oraz DumpCurrMethod, 
                    Else
                        iCurrMethod = iLoop + 1 ' bo ma pominąć: DumpMethodOrMsg (w Release nie ma DumpCurrMethod, kod jest optymalizowany?)
                    End If

                    Exit For
                End If
            Next


            If iCurrMethod < 1 Then
                sCurrMethod = "<bad stack?>"
            Else
                'Debug.WriteLine("subs(iCurrMethod)=" & subs(iCurrMethod).Trim)
                If bAddMethod Then sCurrMethod = subs(iCurrMethod).Trim.Substring(3)    ' z pominięciem "at "
                'Debug.WriteLine("iLoop from " & iCurrMethod + 1 & " to " & subs.Length - 1)
                For iLoop As Integer = iCurrMethod + 1 To subs.Length - 1
                    If subs(iLoop).Contains("System.Runtime.CompilerServices.") Then Continue For
                    If subs(iLoop).Contains("System.Threading.Tasks.") Then Continue For
                    If subs(iLoop).Contains("System.Windows.") Then Continue For
                    If subs(iLoop).Contains("at MS.Internal.") Then Continue For
                    If subs(iLoop).Contains("at MS.Win32.") Then Continue For

                    sPrefix &= "  "
                    iDepth += 1
                Next

            End If

            'Debug.WriteLine("iDepth=" & iDepth)

            '' skrócenie bardzo długiego typu:
            '' BtWatchDump.MainPage.VB$StateMachine_13_BTwatch_Received.MoveNext() 
            sCurrMethod = sCurrMethod.Replace(".VB$StateMachine_", ".VB$")

            If sCurrMethod.EndsWithOrdinal(".MoveNext()") Then sCurrMethod = sCurrMethod.Substring(0, sCurrMethod.Length - 11)
        End If

        DebugOut(iDepth + iLevel, sPrefix & sCurrMethod & " " & sMsg)

    End Sub

    ''' <summary>
    ''' DebugOut z nazwą aktualnej funkcji i sMsg, oraz odpowiednio głęboko cofnięte
    ''' </summary>
    Public Sub DumpCurrMethod(Optional sMsg As String = "")
        DumpMethodOrMsg(True, sMsg, 0)
    End Sub

    ''' <summary>
    ''' DebugOut z komunikatem, odpowiednio głęboko cofnięte wedle głębokości CallStack oraz iLevel
    ''' </summary>
    Public Sub DumpMessage(sMsg As String, Optional iLevel As Integer = 1)
        DumpMethodOrMsg(False, sMsg, iLevel)
    End Sub

    ''' <summary>
    ''' Wyślij DebugOut dodając prefix --PKAR-- (do łatwiejszego znajdywania w logu), także do pliku/zmiennej
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification:="<Pending>")>
    Public Sub DebugOut(logLevel As Integer, sMsg As String)
        Debug.WriteLine("--PKAR---:    " & sMsg)

        Dim iLogLevel As Integer = GetSettingsInt("debugLogLevel", 0)  ' nawet jak nie będzie wcześniej inicjalizacji, i tak sobie poradzi

        If iLogLevel < logLevel Then Return
        msCurrentLog = msCurrentLog & vbCrLf & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & " " & sMsg & vbCrLf
        If msCurrentLog.Length < 2048 Then Return
        DebugOutFlush()
        msCurrentLog = ""
    End Sub

    ''' <summary>
    ''' zapis zmiennej do pliku, gdy go nie ma - create wraz z nagłówkiem (datowanie rozpoczecia pliku)
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification:="<Pending>")>
    Public Sub DebugOutFlush()
        ' ISSUE: reference to a compiler-generated method

        Dim sLogfilePath As String = IO.Path.GetTempPath
        sLogfilePath = sLogfilePath.Replace("AC\Temp", "TempState")
        sLogfilePath = IO.Path.Combine(sLogfilePath, "log-lib.txt")

        If Not IO.File.Exists(sLogfilePath) Then IO.File.AppendAllLines(sLogfilePath,
                             {vbCrLf & "===========================================",
                             "Start @" & Date.Now.ToString("yyyy.MM.dd HH:mm:ss") & vbCrLf})
        IO.File.AppendAllText(sLogfilePath, msCurrentLog)
        msCurrentLog = ""
    End Sub

    ''' <summary>
    ''' Wyślij DebugOut dodając prefix --PKAR-- (do łatwiejszego znajdywania w logu), także do pliku/zmiennej, dla Level=1
    ''' </summary>
    Public Sub DebugOut(sMsg As String)
        DebugOut(1, sMsg)
    End Sub

    ''' <summary>
    ''' Zwykły Dump plus dodaj do logu w zmiennej, Toast jeśli wiadomo jak
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification:="<Pending>")>
    Public Sub CrashMessageAdd(sTxt As String)
        Dim sMsg = Date.Now.ToString("HH:mm:ss") & " " & sTxt & vbCrLf & sTxt & vbCrLf
        DebugOut(0, sMsg)
#If DEBUG Then
        MakeToast(sMsg)
#End If
        SetSettingsString("appFailData", GetSettingsString("appFailData") & sMsg)
    End Sub

    ''' <summary>
    ''' Zwykły Dump plus dodaj do logu w zmiennej 
    ''' </summary>
    Public Sub CrashMessageAdd(sTxt As String, exMsg As String)
        CrashMessageAdd(sTxt & vbCrLf & exMsg)
    End Sub

    ''' <summary>
    ''' Zwykły Dump plus dodaj do logu w zmiennej 
    ''' </summary>
    Public Sub CrashMessageAdd(sTxt As String, ex As Exception, Optional bWithStack As Boolean = False)
        Dim exMsg As String = ""
        If ex IsNot Nothing Then
            exMsg = ex.ToString() & ":" & ex.Message
            If bWithStack AndAlso Not Equals(ex.StackTrace, Nothing) Then exMsg = exMsg & vbCrLf & ex.StackTrace
        End If
        CrashMessageAdd(sTxt, exMsg)
    End Sub

#End Region

#Region "ClipBoard"
    Public Delegate Sub UIclipPut(sTxt As String)
    Public Delegate Sub UIclipPutHtml(sHtml As String)
    'Public Delegate Function UIclipGet() As Task(Of String)

    Private moUIclipPut As UIclipPut ' = Nothing przez samą deklarację
    Private moUIclipPutHtml As UIclipPutHtml
    'Private moUIclipGet As UIclipGet

    Public Sub LibInitClip(oUIclipPut As UIclipPut, oUIclipPutHtml As UIclipPutHtml)
        moUIclipPut = oUIclipPut
        moUIclipPutHtml = oUIclipPutHtml
    End Sub

    Public Sub ClipPut(sTxt As String)
        moUIclipPut(sTxt)
    End Sub

    Public Sub ClipPutHtml(sHtml As String)
        moUIclipPutHtml(sHtml)
    End Sub

    '''' <summary>
    '''' w razie Catch() zwraca ""
    '''' </summary>
    'Public Async Function ClipGetAsync() As Task(Of String)
    '    Return Await moUIclipGet
    'End Function


#End Region


#Region "Settings"

    ' wersja z NUGET pełnym

    'Public Sub LibInitSettings(settings As Microsoft.Extensions.Configuration.IConfigurationRoot)
    '    pkar.NetConfigs.InitSettings(settings)
    'End Sub

    Public Sub InitSettings(
                        applicationName As String, dictionaryOfEnvVars As System.Collections.IDictionary,
                        configSource As Microsoft.Extensions.Configuration.IConfigurationSource,
                        localJSONdirName As String, roamJSONdirNname As String,
                        cmdLineArgs As List(Of String))


        Dim sINIcontent As String = IniLikeDefaults.sIniContent
#If DEBUG Then
        Dim bIniUseDebug As Boolean = True
#Else
            Dim bIniUseDebug As Boolean = false
#End If

        pkar.NetConfigs.InitSettings(sINIcontent, bIniUseDebug,
                        applicationName, dictionaryOfEnvVars,
                        configSource,
                        localJSONdirName, roamJSONdirNname, False,
                        cmdLineArgs)

    End Sub

    Public Sub SetSettingsString(sName As String, value As String, Optional bRoam As Boolean = False)
        pkar.NetConfigs.SetSettingsString(sName, value, bRoam)
    End Sub

    Public Sub SetSettingsInt(sName As String, value As Integer, Optional bRoam As Boolean = False)
        pkar.NetConfigs.SetSettingsInt(sName, value, bRoam)
    End Sub

    Public Sub SetSettingsBool(sName As String, value As Boolean, Optional bRoam As Boolean = False)
        pkar.NetConfigs.SetSettingsBool(sName, value, bRoam)
    End Sub

    Public Sub SetSettingsLong(sName As String, value As Long, Optional bRoam As Boolean = False)
        pkar.NetConfigs.SetSettingsLong(sName, value, bRoam)
    End Sub

    Public Sub SetSettingsDate(sName As String, value As DateTimeOffset, Optional bRoam As Boolean = False)
        pkar.NetConfigs.SetSettingsDate(sName, value, bRoam)
    End Sub

    Public Sub SetSettingsCurrentDate(sName As String, Optional bRoam As Boolean = False)
        pkar.NetConfigs.SetSettingsCurrentDate(sName, bRoam)
    End Sub


    Public Function GetSettingsString(sName As String, Optional sDefault As String = "") As String
        Return pkar.NetConfigs.GetSettingsString(sName, sDefault)
    End Function

    Public Function GetSettingsInt(sName As String, Optional iDefault As Integer = 0) As Integer
        Return pkar.NetConfigs.GetSettingsInt(sName, iDefault)
    End Function

    Public Function GetSettingsBool(sName As String, Optional bDefault As Boolean = False) As Boolean
        Return pkar.NetConfigs.GetSettingsBool(sName, bDefault)
    End Function

    Public Function GetSettingsLong(sName As String, Optional iDefault As Long = 0) As Long
        Return pkar.NetConfigs.GetSettingsLong(sName, iDefault)
    End Function

    Public Function GetSettingsDate(sName As String, Optional sDefault As String = "") As DateTimeOffset
        Return pkar.NetConfigs.GetSettingsDate(sName, sDefault)
    End Function

    Public Function GetSettingsDate(sName As String, dDefault As DateTimeOffset) As DateTimeOffset
        Return pkar.NetConfigs.GetSettingsDate(sName, dDefault)
    End Function

#End Region

    ' IsFamilyMobile  - not in .Net
    ' IsFamilyDesktop - not in .Net
    ' NetIsIPavailable (Std2.0) - not in .Net
    ' NetIsCellInet - not in .Net
    ' GetHostName (Std2.0) - not in .Net
    ' IsThisMoje (bo korzysta z GetHostName) - not in .Net
    ' IsFullVersion - not in .Net
    ' NetWiFiOffOnAsync - not in .Net
    ' NetIsBTavailableAsync - not in .Net
    ' NetTrySwitchBTOnAsync - not in .Net

#Region "DialogBoxes"

    Public Delegate Function UIdialogBox(sMsg As String) As Task
    Public Delegate Function UIdialogBoxYN(sMsg As String, sYes As String, sNo As String) As Task(Of Boolean)
    Public Delegate Function UIdialogBoxInput(sMsgResId As String, sDefault As String, sYes As String, sNo As String) As Task(Of String)

    Private moUIdialogBox As UIdialogBox ' = Nothing przez samą deklarację
    Private moUIdialogBoxYN As UIdialogBoxYN
    Private moUIdialogBoxInput As UIdialogBoxInput

    ''' <summary>
    ''' inicjalizacja, zob. pkarModuleWithLib.InitLib
    ''' </summary>
    Public Sub LibInitDialogBox(ByVal oUIdialogBox As UIdialogBox, ByVal oUIdialogBoxYN As UIdialogBoxYN, ByVal oUIdialogBoxInput As UIdialogBoxInput)
        ' VB: VBlib.pkarlibmodule.InitDialogBox(AddressOf pkar.FromLibDialogBoxAsync, AddressOf pkar.FromLibDialogBoxYNAsync, AddressOf pkar.FromLibDialogBoxInputAllDirectAsync)
        moUIdialogBox = oUIdialogBox
        moUIdialogBoxYN = oUIdialogBoxYN
        moUIdialogBoxInput = oUIdialogBoxInput
    End Sub

    ''' <summary>
    ''' Dialog, z czekaniem
    ''' </summary>
    Public Async Function DialogBoxAsync(sMsg As String) As Task
        If moUIdialogBox Is Nothing Then Throw New InvalidOperationException("DialogBoxAsync w VBLib wymaga wczesniejszego InitMsgBox")
        Await moUIdialogBox(sMsg)
    End Function

    Public Sub DialogBox(sMsg As String)
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
        DialogBoxAsync(sMsg)
#Enable Warning BC42358
    End Sub

    Public Async Function DialogBoxResAsync(sResId As String) As Task
        sResId = GetLangString(sResId)
        Await DialogBoxAsync(sResId)
    End Function

    Public Sub DialogBoxRes(sResId As String)
#Disable Warning BC42358 ' Because this call is not awaited, execution of the current method continues before the call is completed
        DialogBoxResAsync(sResId)
#Enable Warning BC42358
    End Sub

    Public Async Function DialogBoxYNAsync(sMsg As String, Optional sYes As String = "Tak", Optional sNo As String = "Nie") As Task(Of Boolean)
        If moUIdialogBoxYN Is Nothing Then Throw New InvalidOperationException("DialogBoxYNAsync w VBLib wymaga wczesniejszego InitMsgBox")
        Return Await moUIdialogBoxYN(sMsg, sYes, sNo)
    End Function

    Public Async Function DialogBoxResYNAsync(sMsgResId As String, Optional sYesResId As String = "resDlgYes", Optional sNoResId As String = "resDlgNo") As Task(Of Boolean)
        Dim sMsg = GetLangString(sMsgResId)
        Dim sYes = GetLangString(sYesResId)
        Dim sNo = GetLangString(sNoResId)
        Return Await DialogBoxYNAsync(sMsg, sYes, sNo)
    End Function

    ''' <summary>
    ''' Dla Cancel zwraca ""
    ''' </summary>
    Public Async Function DialogBoxInputAllDirectAsync(sMsg As String, Optional sDefault As String = "", Optional sYes As String = "Yes", Optional sNo As String = "No") As Task(Of String)
        If moUIdialogBoxInput Is Nothing Then Throw New InvalidOperationException("DialogBoxInputAllDirectAsync w VBLib wymaga wczesniejszego InitMsgBox")
        Return Await moUIdialogBoxInput(sMsg, sDefault, sYes, sNo)
    End Function

    ''' <summary>
    ''' Dla Cancel zwraca ""
    ''' </summary>
    Public Async Function DialogBoxInputDirectAsync(sMsg As String, Optional sDefault As String = "", Optional sYesResId As String = "resDlgContinue", Optional sNoResId As String = "resDlgCancel") As Task(Of String)
        Dim sYes = GetLangString(sYesResId)
        Dim sNo = GetLangString(sNoResId)
        Return Await DialogBoxInputAllDirectAsync(sMsg, sDefault, sYes, sNo)
    End Function

    ''' <summary>
    ''' Dla Cancel zwraca ""
    ''' </summary>
    Public Async Function DialogBoxInputResAsync(sMsgResId As String, Optional sDefaultResId As String = "", Optional sYesResId As String = "resDlgContinue", Optional sNoResId As String = "resDlgCancel") As Task(Of String)
        Dim sDefault = ""
        Dim sMsg = GetLangString(sMsgResId)
        If sDefaultResId <> "" Then sDefault = GetLangString(sDefaultResId)
        Return Await DialogBoxInputDirectAsync(sMsg, sDefault, sYesResId, sNoResId)
    End Function

#End Region

#Region "Globalization"
    Private moResMan As Resources.ResourceManager ' = Nothing
    Private moResManForced As String

    ''' <summary>
    ''' Wymuszenie języka (na razie "pl*" versus cokolwiek innego trafiające na en)
    ''' </summary>
    ''' <param name="forceLang"></param>
    Public Sub LangForce(Optional forceLang As String = "")
        moResManForced = forceLang
        moResMan = Nothing
    End Sub

    Public Function LangGetCurrent() As String
        If Not String.IsNullOrWhiteSpace(moResManForced) Then Return moResManForced

        Return Globalization.CultureInfo.CurrentCulture.Name
    End Function

    Public Function LangIsCurrent(Optional lang As String = "pl") As Boolean
        Return LangGetCurrent.StartsWithCI(lang)
    End Function


    ''' <summary>
    ''' uprawnia się że moResMan jest ustawiony i można z niego korzystać
    ''' </summary>
    Private Sub LangEnsureInit()
        If moResMan IsNot Nothing Then Return

        If LangGetCurrent.StartsWithCI("PL") Then
            moResMan = My.Resources.Resource_PL.ResourceManager
        Else
            moResMan = My.Resources.Resource_EN.ResourceManager
        End If
    End Sub


    ''' <summary>
    ''' zwraca tekst o identyfikatorze sResId dla aktualnego języka, lub default
    ''' </summary>
    ''' <param name="sResID">identyfikator żądanego tekstu</param>
    ''' <param name="sDefault">default, może być NULL</param>
    ''' <returns>tekst z resources gdy istnieje, a gdy nie to default o ile jest; gdy default = "" to sResId, lub NULL gdy nie ma stringu a default is null </returns>
    Public Function GetLangString(sResID As String, Optional sDefault As String = "") As String
        If sResID = "" Then Return ""
        LangEnsureInit()

        Dim sRet As String
#Disable Warning CA1304 ' Specify CultureInfo
        Try
            sRet = moResMan.GetString(sResID)
        Catch ex As Exception
            ' jakby co
            ' bez dodania dla VBLib defLang - zdarza się zawsze w RELEASE (i dla PL i dla EN)
            sRet = Nothing
        End Try

#Enable Warning CA1304 ' Specify CultureInfo
        If Not String.IsNullOrEmpty(sRet) Then Return sRet
        If sDefault Is Nothing Then Return Nothing
        If sDefault <> "" Then Return sDefault
        Return sResID
    End Function


    Public Sub SetUiPropertiesFromLang(anyObject As Object)
        If anyObject Is Nothing Then Return
        LangEnsureInit()

        ' search for NAME
        Dim name As String = ""
        For Each oProp As PropertyInfo In anyObject.GetType.GetRuntimeProperties
            Debug.WriteLine("Property " & oProp.Name)
            If oProp.Name = "Name" Then
                name = oProp.GetValue(anyObject)
                Exit For
            End If
        Next

        If name = "" Then Return

        For Each oPropFrom As PropertyInfo In anyObject.GetType.GetRuntimeProperties
            Dim newValue As String = GetLangString(name & "." & oPropFrom.Name, Nothing)
            If newValue Is Nothing Then Continue For

            oPropFrom.SetValue(anyObject, newValue)
        Next

    End Sub

#End Region

#Region "Toasty itp"

    Public Delegate Sub UiMakeToast(sMsg As String, sMsg1 As String)
    Private moMakeToast As UiMakeToast ' = Nothing

    ''' <summary>
    ''' Umożliwienie Toast z VBlib
    ''' VB: VBlib.pkarlibmodule.LibInitToast(AddressOf pkar.MakeToast)
    ''' </summary>
    Public Sub LibInitToast(ByVal oMakeToast As UiMakeToast)
        ' VB: VBlib.pkarlibmodule.LibInitToast(AddressOf pkar.MakeToast)
        moMakeToast = oMakeToast
    End Sub


    ' SetBadgeNo - not in .Net

    Public Function XmlSafeString(sInput As String) As String
        Return New XText(sInput).ToString()
    End Function

    Public Function XmlSafeStringQt(sInput As String) As String
        Dim sTmp As String
        sTmp = XmlSafeString(sInput)
        sTmp = sTmp.Replace("""", "&quote;")
        Return sTmp
    End Function

    ''' <summary>
    ''' Tylko przerzucenie do App - wiec wykorzystywac tylko w VBlib!
    ''' </summary>
    Public Sub MakeToast(sMsg As String, Optional sMsg1 As String = "")
        If moMakeToast Is Nothing Then Throw New InvalidOperationException("MakeToast w VBLib wymaga wczesniejszego LibInitToast")
        moMakeToast(sMsg, sMsg1)
    End Sub

    ' ToastAction - not in .Net
    ' MakeToast(oDate As DateTime, sMsg As String, Optional sMsg1 As String = "") - not in .Net
    ' RemoveScheduledToasts - not in .Net
#End Region

    ' #Region "WinVer, AppVer"
    ' WinVer - not in .Net
    ' GetAppVers - not in .Net (System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString , ale od .Net 7)

#Region "GetWebPage + pomocnicze"
    Private moHttp As New Net.Http.HttpClient
    Private Const msDefaultHttpAgent As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4321.0 Safari/537.36 Edg/88.0.702.0"
    Private msAgent As String = msDefaultHttpAgent

    Public Sub HttpPageSetAgent(Optional sAgent As String = msDefaultHttpAgent)
        msAgent = sAgent
        moHttp?.DefaultRequestHeaders.UserAgent.TryParseAdd(msAgent)
    End Sub

    Public Sub HttpPageReset(Optional bAllowRedirects As Boolean = True)
#Disable Warning CA2000 ' Dispose objects before losing scope
        ' będzie Dispose razem z moHttp dispose
        Dim oHandler As New Net.Http.HttpClientHandler With {
            .AllowAutoRedirect = bAllowRedirects
        }
#Enable Warning CA2000 ' Dispose objects before losing scope
        If moHttp IsNot Nothing Then
            moHttp.Dispose()
        End If

        moHttp = New Net.Http.HttpClient(oHandler)
        moHttp.DefaultRequestHeaders.UserAgent.TryParseAdd(msAgent)
    End Sub

    Public Async Function HttpPageAsync(sLink As String, Optional sData As String = "", Optional bReset As Boolean = False) As Task(Of String)
        Return Await HttpPageAsync(New Uri(sLink), sData, bReset)
    End Function


    Public Async Function HttpPageAsync(oUri As Uri, Optional sData As String = "", Optional bReset As Boolean = False) As Task(Of String)
        DumpCurrMethod("uri=" & oUri.AbsoluteUri)
        If oUri Is Nothing OrElse oUri.ToString = "" Then
            sLastError = "HttpPageAsync but sUrl is empty"
            Return ""
        End If

        If moHttp Is Nothing OrElse bReset Then HttpPageReset()
        sLastError = ""
        Dim oResp As Net.Http.HttpResponseMessage

        ' przygotuj pContent, będzie przy redirect używany ponownie
        Dim pContent As Net.Http.StringContent = Nothing    ' żeby nie krzyczał że używam nieinicjalizowanego
        If sData <> "" Then pContent = New Net.Http.StringContent(sData, Text.Encoding.UTF8, "application/x-www-form-urlencoded")

        Try
            ' ISSUE: reference to a compiler-generated method
            If sData <> "" Then
                oResp = Await moHttp.PostAsync(oUri, pContent)
            Else
                oResp = Await moHttp.GetAsync(oUri)
            End If

#Disable Warning CA1031 ' Do not catch general exception types
        Catch ex As Exception
#Enable Warning CA1031 ' Do not catch general exception types
            sLastError = "ERROR @HttpPageAsync get/post " & oUri.ToString & " : " & ex.Message
            DumpMessage(sLastError)
            pContent?.Dispose()
            Return ""
        End Try

        If Not oResp.IsSuccessStatusCode Then
            DumpMessage($"Error code: {oResp.StatusCode}. {oResp.ReasonPhrase}")
        End If

        If oResp.StatusCode = 303 Or oResp.StatusCode = 302 Or oResp.StatusCode = 301 Then
            ' redirect
            oUri = oResp.Headers.Location
            'If sUrl.ToLower.Substring(0, 4) <> "http" Then
            '    sUrl = "https://sympatia.onet.pl/" & sUrl   ' potrzebne przy szukaniu
            'End If

            If sData <> "" Then
                oResp = Await moHttp.PostAsync(oUri, pContent)
            Else
                oResp = Await moHttp.GetAsync(oUri)
            End If
        End If
        pContent?.Dispose()

        Dim sPage As String

        ' override dla Facebook
        If oResp.Content.Headers.Contains("oContent-Type") Then
            If oResp.Content.Headers.ContentType.CharSet = """utf-8""" Then
                oResp.Content.Headers.ContentType.CharSet = "utf-8"
            End If
        End If

        'override dla Auchan
        If oResp.Content?.Headers?.ContentType?.CharSet = "utf8" Then
            oResp.Content.Headers.ContentType.CharSet = "utf-8"
        End If

        Try
            sPage = Await oResp.Content.ReadAsStringAsync()
#Disable Warning CA1031 ' Do not catch general exception types
        Catch ex As Exception
#Enable Warning CA1031 ' Do not catch general exception types
            sLastError = "ERROR @HttpPageAsync ReadAsync: " & ex.Message
            DumpMessage(sLastError)
            Return ""
        End Try

        Return sPage
    End Function


    Public Function RemoveHtmlTags(sHtml As String) As String
        If String.IsNullOrWhiteSpace(sHtml) Then Return ""

        Dim iInd0, iInd1 As Integer

        iInd0 = sHtml.IndexOfOrdinal("<script")
        If iInd0 > 0 Then
            iInd1 = sHtml.IndexOf("</script>", iInd0, StringComparison.Ordinal)
            If iInd1 > 0 Then
                sHtml = sHtml.Remove(iInd0, iInd1 - iInd0 + 9)
            End If
        End If

        iInd0 = sHtml.IndexOfOrdinal("<")
        iInd1 = sHtml.IndexOfOrdinal(">")
        While iInd0 > -1
            If iInd1 > -1 Then
                sHtml = sHtml.Remove(iInd0, iInd1 - iInd0 + 1)
            Else
                sHtml = sHtml.Substring(0, iInd0)
            End If
            sHtml = sHtml.Trim

            iInd0 = sHtml.IndexOfOrdinal("<")
            iInd1 = sHtml.IndexOfOrdinal(">")
        End While

        sHtml = sHtml.Replace("&nbsp;", " ")
        sHtml = sHtml.Replace(vbLf, vbCrLf)
        sHtml = sHtml.Replace(vbCrLf & vbCrLf, vbCrLf)
        sHtml = sHtml.Replace(vbCrLf & vbCrLf, vbCrLf)
        sHtml = sHtml.Replace(vbCrLf & vbCrLf, vbCrLf)

        Return sHtml.Trim

    End Function

    ' OpenBrowser - not in .Net

#End Region

#Region "triggers"
    '    #Region "zwykłe" - not in .Net
#Region "RemoteSystem"

    ''' <summary>
    ''' jeśli na wejściu jest jakaś standardowa komenda (obsługiwalna w .Net Standard),
    ''' to na wyjściu będzie jej rezultat. Else = ""
    ''' </summary>
    Public Function LibAppServiceStdCmd(sCommand As String, sLocalCmds As String) As String
        If String.IsNullOrWhiteSpace(sCommand) Then Return ""

        Dim sTmp As String

        If sCommand.StartsWithOrdinal("debug loglevel") Then
            Dim sRetVal As String = "Previous loglevel: " & GetSettingsInt("debugLogLevel") & vbCrLf
            sCommand = sCommand.Replace("debug loglevel", "").Trim
            Dim iTemp As Integer = 0
            If Not Integer.TryParse(sCommand, iTemp) Then
                Return sRetVal & "Not changed - bad loglevel value"
            End If

            SetSettingsInt("debugLogLevel", iTemp)
            Return sRetVal & "Current loglevel: " & iTemp
        End If

        Select Case sCommand.ToLower()
            Case "ping"
                Return "pong"
            ' Case "ver"
            ' Case "localdir"
            Case "appdir"
                Return System.AppContext.BaseDirectory
            ' Case "installeddate"
            Case "help"
                Return "App specific commands:" & vbCrLf & sLocalCmds

            Case "debug vars"
                Return DumpSettings()
            ' Case "debug triggers"
            ' Case "debug toasts"
            ' Case "debug memsize"
            ' Case "debug rungc"
            Case "debug crashmsg"
                sTmp = GetSettingsString("appFailData", "")
                If sTmp = "" Then sTmp = "No saved crash info"
                Return sTmp
            Case "debug crashmsg clear"
                sTmp = GetSettingsString("appFailData", "")
                If sTmp = "" Then sTmp = "No saved crash info"
                SetSettingsString("appFailData", "")
                Return sTmp

            ' Case "lib unregistertriggers"
            ' Case "lib isfamilymobile"
            ' Case "lib isfamilydesktop"
            ' Case "lib netisipavailable"
            ' Case "lib netiscellinet"
            ' Case "lib gethostname"
            ' Case "lib isthismoje"
            ' Case "lib istriggersregistered"
            Case "lib pkarmode 1"
                SetSettingsBool("pkarMode", True)
                Return "DONE"
            Case "lib pkarmode 0"
                SetSettingsBool("pkarMode", False)
                Return "DONE"
            Case "lib pkarmode"
                Return GetSettingsBool("pkarMode").ToString()
        End Select

        Return ""  ' oznacza: to nie jest standardowa komenda
    End Function

    <Obsolete("jak bedzie nowszy Nuget ktory to ma")>
    Private Function DumpSettings() As String
        ' GetDebugView(IConfigurationRoot) - ale to od późniejszych .Net, od platform extension 3

        Dim sRet As String = "Dump settings (VBlib version, v.1.4)" & vbCrLf

        'For Each oSett In _settingsGlobal.AsEnumerable
        '    sRet = sRet & oSett.Key & vbTab & oSett.Value & vbCrLf
        'Next


        Return sRet
    End Function

#End Region


#End Region

#If PKAR_USEDATALOG Then
    Public msDataLog As pkar.Datalog.Datalog
    Public Sub LibInitDataLog(sPath As String)
        If Not IO.Directory.Exists(sPath) Then IO.Directory.CreateDirectory(sPath)
        msDataLog = New pkar.Datalog.Datalog(sPath, "")
    End Sub
#End If



    ''' <summary>
    ''' ale to wymaga sprawdzenia jak się zachowuje na Android w Uno na przykład
    ''' </summary>
    Public Function GetPlatform() As String
        If Runtime.InteropServices.RuntimeInformation.IsOSPlatform(Runtime.InteropServices.OSPlatform.Windows) Then Return "uwp"
        If Runtime.InteropServices.RuntimeInformation.IsOSPlatform(Runtime.InteropServices.OSPlatform.OSX) Then Return "ios"
        If Runtime.InteropServices.RuntimeInformation.IsOSPlatform(Runtime.InteropServices.OSPlatform.Linux) Then Return "android"
        ' jest jeszcze FreeBSD, moze później będzie jeszcze więcej różnych
        Return "other"
    End Function


    ''' <summary>
    ''' Wybierze co ma być użyte - czy obiekt1 (OneDrive, ret 1), czy obiekt2 (local, ret 2), czy też są takie same (0)
    ''' await, bo dialogbox wyboru się pojawia
    ''' </summary>
    ''' <param name="oDate1">data danych 1 (zwykle OneDrive)</param>
    ''' <param name="oDate2">data danych 2 (zwykle local)</param>
    ''' <param name="iTolerance">tolerancja w sekundach</param>
    ''' <param name="bLastWas2">gdy oDate1 > oDate2, i bLastWas2, to kolizja</param>
    ''' <returns></returns>
    Public Async Function SelectOneContentChoose(oDate1 As DateTimeOffset, oDate2 As DateTimeOffset, bLastWas2 As Boolean,
                                                 Optional iTolerance As Integer = 10,
                                                 Optional resIdCollisionMsg As String = "msgConflictModifiedODandLocal",
                                                 Optional resIdCollisionUse1 As String = "msgConflictUseOD",
                                                 Optional resIdCollisionUse2 As String = "msgConflictUseLocal") As Task(Of Integer)

        If Math.Abs((oDate1 - oDate2).TotalSeconds) < iTolerance Then Return 0

        If oDate1.AddSeconds(iTolerance) > oDate2 Then
            If bLastWas2 Then
                ' ale lokalnie też zmienione i nie zapisane do OD, więc kolizja
                If Await DialogBoxResYNAsync(resIdCollisionMsg, resIdCollisionUse1, resIdCollisionUse2) Then Return 1
                Return 2
            Else
                ' nowsze OD, lokalnie nie było zapisu, więc wczytuj OD
                Return 1
            End If
        End If

        ' b) nowszy Roam
        If oDate2.AddSeconds(iTolerance) > oDate1 Then
            If bLastWas2 Then
                ' lokalnie było zmienione i nie zapisane do OD, więc OK, używaj lokalnego
                Return 2
            Else
                ' nie powinno się zdarzyć: nowsze lokalnie, ale ostatni zapis był do OneDrive
                Return 2
            End If
        End If

        Return 0

    End Function


End Module


Partial Module Extensions
    <Obsolete("to może nie działać!")>
    <Runtime.CompilerServices.Extension()>
    Public Function SelectSingleNode(ByVal oNode As Xml.XmlNode, sNodeName As String) As Xml.XmlNode
        Dim oElement As Xml.XmlElement = TryCast(oNode, Xml.XmlElement)
        If oElement Is Nothing Then Return Nothing

        Dim oListEls As Xml.XmlNodeList = oElement.GetElementsByTagName(sNodeName)
        If oListEls.Count < 1 Then Return Nothing
        Return oListEls(0)
    End Function

End Module



#Enable Warning CA2007 'Consider calling ConfigureAwait On the awaited task
#Enable Warning IDE0079 ' Remove unnecessary suppression


