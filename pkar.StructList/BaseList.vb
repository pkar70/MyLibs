


''' <summary>
''' simple "database class", using JSON file
''' </summary>
''' <typeparam name="TYP">Type of elements</typeparam>
Public Class BaseList(Of TYP)
    Inherits ObservableList(Of TYP)

    Private ReadOnly _filename As String
    Private _copyFilename As String

    ''' <summary>
    ''' If TRUE, .Save makes .bak file with previous state
    ''' </summary>
    Public UseBak As Boolean = False

    Private _loadMemSizeKB As Integer

    ''' <summary>
    ''' create new list, and use given file in given folder as data backing
    ''' </summary>
    ''' <param name="folder">If NULL, then list would be not file backed</param>
    Public Sub New(folder As String, Optional fileName As String = "items.json")
        MyBase.New

        If String.IsNullOrWhiteSpace(folder) Then
            _filename = ""
            _copyFilename = ""
        Else
            If String.IsNullOrWhiteSpace(fileName) Then
                Throw New ArgumentException("if you provided folder, filename cannot be null")
            End If
            _filename = IO.Path.Combine(folder, fileName)
        End If

    End Sub

    ''' <summary>
    ''' Create copy of data file in given folder. Since this call, saving would save file to both folders.
    ''' </summary>
    ''' <param name="folderForCopy">Folder for copy of data file, or String.Empty or NULL if no copy should be maintained</param>
    Public Sub MaintainCopy(folderForCopy As String)
        If _filename = "" Then
            Throw New ArgumentException("List is not file backed - cannot maintan copy")
        End If

        If String.IsNullOrEmpty(folderForCopy) Then
            _copyFilename = ""
            Return
        End If

        _copyFilename = _filename.Replace(IO.Path.GetDirectoryName(_filename), folderForCopy)
        IO.File.Copy(_filename, _copyFilename, True)

        TryMakeCopy()
    End Sub

    Private Sub TryMakeCopy()
        If _filename = "" Then Return

        If String.IsNullOrEmpty(_copyFilename) Then Return
        IO.File.Copy(_filename, _copyFilename, True)
    End Sub

    ''' <summary>
    ''' This method is called when Load ends with empty list - override it to fill default entries
    ''' </summary>
    Protected Overridable Sub InsertDefaultContent()

    End Sub

#Region "operations on data file"

    ''' <summary>
    ''' load list from file
    ''' </summary>
    ''' <returns>False: no or empty file, True: something was read</returns>
    Public Overridable Function Load() As Boolean
        If _filename = "" Then
            Throw New ArgumentException("List is not file backed - cannot load it")
        End If

        Dim sTxt As String = ""
        If IO.File.Exists(_filename) Then
            sTxt = IO.File.ReadAllText(_filename)
        End If

        If sTxt Is Nothing OrElse sTxt.Length < 5 Then
            Clear()
            InsertDefaultContent()
            Return False
        End If

        If Import(sTxt) Then Return True
        ' if we simply Append to file, it can have last "]" missing, so we try adding it
        Return Import(sTxt & "]")

    End Function

    ''' <summary>
    ''' return difference in managed memory before and after loading data from file (using GC.GetTotalMemory)
    ''' </summary>
    ''' <returns>KiB of consumed memory, or -1 if something goes bad</returns>
    Public Function GetOnLoadMemSizeKB() As Integer
        Return Math.Max(-1, _loadMemSizeKB)
    End Function

    Private Shared Function GetKibibytes() As Long
        Return GC.GetTotalMemory(False) / 1024
        'Return Process.GetCurrentProcess.WorkingSet64 / 1024
        ' Windows.System.MemoryManager.AppMemoryUsage
    End Function

    ''' <summary>
    ''' load list from string
    ''' </summary>
    ''' <returns>False if string cannot be deserialized</returns>
    Public Overridable Function Import(jsonContent As String) As Boolean

        Dim initMem As Long = GetKibibytes()

        Try
            Dim lista As ObservableList(Of TYP) = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonContent, GetType(ObservableList(Of TYP)))
            MyBase.AddRange(lista)
            _loadMemSizeKB = GetKibibytes() - initMem
            Return True
        Catch ex As Exception
        End Try

        Return False
    End Function
    ''' <summary>
    ''' you can use it as a constructor of list item from JSON string
    ''' </summary>
    ''' <param name="sJSON"></param>
    ''' <returns></returns>
    Public Function LoadItem(sJSON As String) As TYP
        Try
            Return Newtonsoft.Json.JsonConvert.DeserializeObject(sJSON, GetType(TYP))
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Export data to string
    ''' </summary>
    ''' <param name="bIgnoreNulls">if NullValueHandling.Ignore and DefaultValueHandling.Ignore should be true (shorten file)</param>
    ''' <returns>Empty if no items, else formatted JSON with data dump</returns>
    Public Function Export(Optional bIgnoreNulls As Boolean = False) As String
        If MyBase.Count < 1 Then Return ""

        If bIgnoreNulls Then
            Dim oSerSet As New Newtonsoft.Json.JsonSerializerSettings With {.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, .DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore}
            Return Newtonsoft.Json.JsonConvert.SerializeObject(MyBase.AsEnumerable, Newtonsoft.Json.Formatting.Indented, oSerSet)
        Else
            Return Newtonsoft.Json.JsonConvert.SerializeObject(MyBase.AsEnumerable, Newtonsoft.Json.Formatting.Indented)
        End If

    End Function


    ''' <summary>
    '''  save data to file
    ''' </summary>
    ''' <param name="bIgnoreNulls">if NullValueHandling.Ignore and DefaultValueHandling.Ignore should be true (shorten file)</param>
    ''' <returns></returns>
    Public Overridable Function Save(Optional bIgnoreNulls As Boolean = False) As Boolean
        If _filename = "" Then
            Throw New ArgumentException("List is not file backed - cannot save it")
        End If

        If MyBase.Count < 1 Then Return False

        Dim sTxt As String = Export(bIgnoreNulls)

        If UseBak AndAlso IO.File.Exists(_filename) Then
            IO.File.Delete(_filename & ".bak")
            IO.File.Move(_filename, _filename & ".bak")
        End If

        IO.File.WriteAllText(_filename, sTxt)
        TryMakeCopy()

        Return True
    End Function

    ''' <summary>
    '''  save data to file with filename suffixed with '.tmp'
    ''' </summary>
    ''' <param name="bIgnoreNulls">if NullValueHandling.Ignore and DefaultValueHandling.Ignore should be true (shorten file)</param>
    ''' <returns></returns>
    Public Overridable Function SaveTemp(Optional bIgnoreNulls As Boolean = False) As Boolean
        If _filename = "" Then
            Throw New ArgumentException("List is not file backed - cannot save it")
        End If

        If MyBase.Count < 1 Then Return False

        Dim sTxt As String = Export(bIgnoreNulls)

        IO.File.WriteAllText(_filename & ".tmp", sTxt)

        Return True
    End Function


    ''' <summary>
    ''' GetLastWriteTime of data file, or 1 I 1970 if file doesn't exist (so it would seem as very old) 
    ''' </summary>
    Public Function GetFileDate() As Date
        If _filename = "" Then
            Throw New ArgumentException("List is not file backed - cannot check file date")
        End If

        If IO.File.Exists(_filename) Then
            Return IO.File.GetLastWriteTime(_filename)
        Else
            Return New Date(1970, 1, 1)
        End If
    End Function

    ''' <summary>
    ''' check if file last write was more than days  ago; if list is not file backed returns false
    ''' </summary>
    Public Function IsObsolete(days As Integer)
        If _filename = "" Then Return False

        If GetFileDate.AddDays(days) < Date.Now Then Return True
        Return False
    End Function
#End Region

#Region "proxies for internal list"
    ''' <summary>
    ''' get internal list of items
    ''' </summary>
    <Obsolete("you can use internal list directly, please remove '.GetList' from call (e.g. foreach(x in list) instead of foreach(x in list.GetList)")>
    Public Function GetList() As ObservableList(Of TYP)
        Return MyBase.AsEnumerable
    End Function

    ''' <summary>
    ''' get internal list of items
    ''' </summary>
    <Obsolete("you can use internal list directly, please change '_list' to 'me' (C#: this) in calls")>
    Protected ReadOnly Property _list As ObservableList(Of TYP)
        Get
            Return MyBase.AsEnumerable
        End Get
    End Property



#End Region


#If False Then
    ' error BC37234: Late binding is not supported in current project type.
    ' wymaga³oby: Imports System.Reflection

    ''' <summary>
    ''' shortcut for Find(delegate), vb: Find(function(x) x.fieldName = fieldValue), c#: Find(x => x.fieldName = fieldValue)
    ''' </summary>
    ''' <param name="fieldName">Name of property (case sensitive)</param>
    ''' <returns>The first element that matches the condition, if found; otherwise, the default value for type T</returns>
    Public Function FindItem(fieldName As String, fieldValue As String) As TYP
        Return MyBase.Find(Function(x) x.GetType.GetRuntimeProperty(fieldName)?.GetValue(x) = fieldValue)
    End Function
#End If

End Class




