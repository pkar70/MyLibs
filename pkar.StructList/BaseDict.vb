
''' <summary>
''' simple "database class", using dictionary JSON file
''' </summary>
Public Class BaseDict(Of TKEY, TVALUE)
    Protected _dict As Dictionary(Of TKEY, TVALUE)
    Private ReadOnly _filename As String
    Private _copyFilename As String

    ''' <summary>
    ''' If TRUE, .Save makes .bak file with previous state
    ''' </summary>
    Public UseBak As Boolean = False

    Private _loadMemSizeKB As Integer

    ''' <summary>
    ''' create new Dictionary, and use given file in given folder as data backing
    ''' </summary>
    Public Sub New(sFolder As String, Optional sFileName As String = "items.json")

        If String.IsNullOrWhiteSpace(sFolder) OrElse String.IsNullOrWhiteSpace(sFileName) Then
            Throw New ArgumentException("you have to provide both folder and filename")
        End If
        _dict = New Dictionary(Of TKEY, TVALUE)
        _filename = IO.Path.Combine(sFolder, sFileName)
    End Sub

    ''' <summary>
    ''' Create copy of data file in given folder. Since this call, saving would save file to both folders.
    ''' </summary>
    ''' <param name="folderForCopy">Folder for copy of data file, or String.Empty or NULL if no copy should be maintained</param>
    Public Sub MaintainCopy(folderForCopy As String)

        If String.IsNullOrEmpty(folderForCopy) Then
            _copyFilename = ""
            Return
        End If

        _copyFilename = _filename.Replace(IO.Path.GetDirectoryName(_filename), folderForCopy)
        IO.File.Copy(_filename, _copyFilename, True)

        TryMakeCopy()
    End Sub

    Private Sub TryMakeCopy()
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
    ''' load dictionary from file
    ''' </summary>
    ''' <returns>False: no or empty file, True: something was read</returns>
    Public Overridable Function Load() As Boolean

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
    ''' load dictionary from string
    ''' </summary>
    ''' <returns>False if string cannot be deserialized</returns>
    Public Overridable Function Import(JsonContent As String) As Boolean

        Try
            Dim initMem As Long = GetKibibytes()
            _dict = Newtonsoft.Json.JsonConvert.DeserializeObject(JsonContent, GetType(Dictionary(Of TKEY, TVALUE)))
            _loadMemSizeKB = GetKibibytes() - initMem
            Return True
        Catch ex As Exception
        End Try

        Return False
    End Function


    ''' <summary>
    ''' you can use it as a constructor of dictionary item from JSON string
    ''' </summary>
    ''' <param name="sJSON"></param>
    ''' <returns></returns>
    Public Function LoadItem(sJSON As String) As KeyValuePair(Of TKEY, TVALUE)
        Try
            Return Newtonsoft.Json.JsonConvert.DeserializeObject(sJSON, GetType(KeyValuePair(Of TKEY, TVALUE)))
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
        If _dict Is Nothing Then Return ""
        If _dict.Count < 1 Then Return ""

        If bIgnoreNulls Then
            Dim oSerSet As New Newtonsoft.Json.JsonSerializerSettings With {.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, .DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore}
            Return Newtonsoft.Json.JsonConvert.SerializeObject(_dict, Newtonsoft.Json.Formatting.Indented, oSerSet)
        Else
            Return Newtonsoft.Json.JsonConvert.SerializeObject(_dict, Newtonsoft.Json.Formatting.Indented)
        End If
    End Function


    ''' <summary>
    '''  save data to file
    ''' </summary>
    ''' <param name="bIgnoreNulls">if NullValueHandling.Ignore and DefaultValueHandling.Ignore should be true (shorten file)</param>
    Public Overridable Function Save(Optional bIgnoreNulls As Boolean = False) As Boolean

        If _dict Is Nothing Then Return False
        If _dict.Count < 1 Then Return False

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

        If _dict.Count < 1 Then Return False

        Dim sTxt As String = Export(bIgnoreNulls)

        IO.File.WriteAllText(_filename & ".tmp", sTxt)

        Return True
    End Function

    ''' <summary>
    ''' GetLastWriteTime of data file, or 1 I 1970 if file doesn't exist (so it would seem as very old) 
    ''' </summary>
    Public Function GetFileDate() As Date
        If IO.File.Exists(_filename) Then
            Return IO.File.GetLastWriteTime(_filename)
        Else
            Return New Date(1970, 1, 1)
        End If
    End Function

    ''' <summary>
    ''' check if file last write was more than iDays  ago
    ''' </summary>
    Public Function IsObsolete(iDays As Integer)
        If GetFileDate.AddDays(iDays) < Date.Now Then Return True
        Return False
    End Function


#Region "delayed save"


    Private _SaveMetaDataAfterCount As UInt16 = 1
    Private _SaveMetaDataAfterTime As New TimeSpan(0, 0, 60)

    Private _CurrSaveMetaDataCounter As UInt16 = 1
    Private _SaveMetaDataTimer As System.Threading.Timer

    Private _isDirty As Boolean

    ''' <summary>
    ''' Configure DelayedSave (and saves dictionary if it is dirty now). 
    ''' </summary>
    ''' <param name="count">Save after defined number of DelayedSave call</param>
    ''' <param name="time">Save after defined TimeSpan after last DelayedSave call</param>
    Public Sub SetDelay(count As UInt16, time As TimeSpan)
        _SaveMetaDataTimer.Dispose()
        _SaveMetaDataTimer = Nothing

        If _isDirty Then Save()
        _isDirty = False

        _SaveMetaDataAfterTime = time
        _SaveMetaDataAfterCount = Math.Max(count, 1)
        _CurrSaveMetaDataCounter = _SaveMetaDataAfterCount
    End Sub

    ''' <summary>
    ''' mark dictionary as dirty for DelayedSave.
    ''' </summary>
    ''' <param name="force">TRUE if it should be saved without delay</param>
    Public Sub DelayedSave(Optional force As Boolean = False)
        _isDirty = True

        _SaveMetaDataTimer.Dispose()
        _SaveMetaDataTimer = Nothing

        _CurrSaveMetaDataCounter -= 1
        If force Then _CurrSaveMetaDataCounter = 0

        If _CurrSaveMetaDataCounter > 0 Then
            _SaveMetaDataTimer = New System.Threading.Timer(Sub() DelayedSave(True), Nothing, _SaveMetaDataAfterTime, Threading.Timeout.InfiniteTimeSpan)
            Return
        End If

        Save()
        _isDirty = False
        _CurrSaveMetaDataCounter = 0
    End Sub

#End Region
#End Region

#Region "proxies for internal dictionary"

    ''' <summary>
    ''' get internal dictionary
    ''' </summary>
    ''' <returns></returns>
    Public Function GetDictionary() As Dictionary(Of TKEY, TVALUE)
        Return _dict
    End Function

    ''' <summary>
    ''' get the number of elements containted in dictionary
    ''' </summary>
    Public Function Count() As Integer
        Return _dict.Count
    End Function

    ''' <summary>
    '''  removes all elements from dictionary
    ''' </summary>
    Public Sub Clear()
        _dict.Clear()
    End Sub

    ''' <summary>
    ''' try to add new item to dictionary
    ''' </summary>
    ''' <returns>True if item was added, False if not (e.g. it already exists)</returns>
    Public Function TryAdd(oNew As KeyValuePair(Of TKEY, TVALUE)) As Boolean
        Return TryAdd(oNew.Key, oNew.Value)
    End Function

    ''' <summary>
    ''' checks if dictionary contains element with given key
    ''' </summary>
    Public Function ContainsKey(key As TKEY) As Boolean
        Return _dict.ContainsKey(key)
    End Function

    ''' <summary>
    ''' try to add new item to dictionary
    ''' </summary>
    ''' <returns>True if item was added, False if not (e.g. it already exists)</returns>
    Public Function TryAdd(key As TKEY, value As TVALUE) As Boolean
        If ContainsKey(key) Then Return False
        _dict.Add(key, value)
        Return True
    End Function

    ''' <summary>
    ''' Removes the value with the key spcified in oDel
    ''' </summary>
    Public Sub Remove(oDel As KeyValuePair(Of TKEY, TVALUE))
        _dict.Remove(oDel.Key)
    End Sub

    ''' <summary>
    ''' Removes the value with the specified key
    ''' </summary>
    Public Sub Remove(key As TKEY)
        _dict.Remove(key)
    End Sub

    ''' <summary>
    ''' Gets the value associated with the specified key, or KeyNotFoundException
    ''' </summary>
    Public Function Item(key As TKEY) As TVALUE
        Return _dict.Item(key)
    End Function

    ''' <summary>
    ''' Gets the value associated with the specified key
    ''' </summary>
    ''' <returns>True if value is returned, False if dictionary doesn't contain item with given key</returns>
    Public Function TryGetValue(key As TKEY, ByRef value As TVALUE) As Boolean
        Return _dict.TryGetValue(key, value)
    End Function
#End Region


End Class




