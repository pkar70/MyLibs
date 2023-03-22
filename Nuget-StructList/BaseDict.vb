
''' <summary>
''' simple "database class", using dictionary JSON file
''' </summary>
Public Class BaseDict(Of TKEY, TVALUE)
    Protected _lista As Dictionary(Of TKEY, TVALUE)
    Private _filename As String

    ''' <summary>
    ''' create new Dictionary, and use given file in given folder as data backing
    ''' </summary>
    Public Sub New(sFolder As String, Optional sFileName As String = "items.json")

        If String.IsNullOrWhiteSpace(sFolder) OrElse String.IsNullOrWhiteSpace(sFileName) Then
            Throw New ArgumentException("you have to provide both folder and filename")
        End If
        _lista = New Dictionary(Of TKEY, TVALUE)
        _filename = IO.Path.Combine(sFolder, sFileName)
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
    ''' load dictionary from string
    ''' </summary>
    ''' <returns>False if string cannot be deserialized</returns>
    Public Overridable Function Import(JsonContent As String) As Boolean

        Try
            _lista = Newtonsoft.Json.JsonConvert.DeserializeObject(JsonContent, GetType(Dictionary(Of TKEY, TVALUE)))
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
    '''  save data to file
    ''' </summary>
    ''' <param name="bIgnoreNulls">if NullValueHandling.Ignore and DefaultValueHandling.Ignore should be true (shorten file)</param>
    Public Overridable Function Save(Optional bIgnoreNulls As Boolean = False) As Boolean

        If _lista Is Nothing Then
            Return False
        End If
        If _lista.Count < 1 Then
            Return False
        End If

        Dim sTxt As String
        If bIgnoreNulls Then
            Dim oSerSet As New Newtonsoft.Json.JsonSerializerSettings With {.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, .DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore}
            sTxt = Newtonsoft.Json.JsonConvert.SerializeObject(_lista, Newtonsoft.Json.Formatting.Indented, oSerSet)
        Else
            sTxt = Newtonsoft.Json.JsonConvert.SerializeObject(_lista, Newtonsoft.Json.Formatting.Indented)
        End If

        IO.File.WriteAllText(_filename, sTxt)

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
#End Region

#Region "proxies for internal dictionary"

    ''' <summary>
    ''' get internal dictionary
    ''' </summary>
    ''' <returns></returns>
    Public Function GetDictionary() As Dictionary(Of TKEY, TVALUE)
        Return _lista
    End Function

    ''' <summary>
    ''' get the number of elements containted in dictionary
    ''' </summary>
    Public Function Count() As Integer
        Return _lista.Count
    End Function

    ''' <summary>
    '''  removes all elements from dictionary
    ''' </summary>
    Public Sub Clear()
        _lista.Clear()
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
        Return _lista.ContainsKey(key)
    End Function

    ''' <summary>
    ''' try to add new item to dictionary
    ''' </summary>
    ''' <returns>True if item was added, False if not (e.g. it already exists)</returns>
    Public Function TryAdd(key As TKEY, value As TVALUE) As Boolean
        If ContainsKey(key) Then Return False
        _lista.Add(key, value)
        Return True
    End Function

    ''' <summary>
    ''' Removes the value with the key spcified in oDel
    ''' </summary>
    Public Sub Remove(oDel As KeyValuePair(Of TKEY, TVALUE))
        _lista.Remove(oDel.Key)
    End Sub

    ''' <summary>
    ''' Removes the value with the specified key
    ''' </summary>
    Public Sub Remove(key As TKEY)
        _lista.Remove(key)
    End Sub

    ''' <summary>
    ''' Gets the value associated with the specified key, or KeyNotFoundException
    ''' </summary>
    Public Function Item(key As TKEY) As TVALUE
        Return _lista.Item(key)
    End Function

    ''' <summary>
    ''' Gets the value associated with the specified key
    ''' </summary>
    ''' <returns>True if value is returned, False if dictionary doesn't contain item with given key</returns>
    Public Function TryGetValue(key As TKEY, ByRef value As TVALUE) As Boolean
        Return _lista.TryGetValue(key, value)
    End Function
#End Region


End Class



