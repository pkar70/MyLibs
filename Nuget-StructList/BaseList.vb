
Imports Newtonsoft.Json.Linq
''' <summary>
''' simple "database class", using JSON file
''' </summary>
''' <typeparam name="TYP">Type of elements</typeparam>
Public Class BaseList(Of TYP)
    Protected _lista As List(Of TYP)
    Private _filename As String

    ''' <summary>
    ''' create new list, and use given file in given folder as data backing
    ''' </summary>
    Public Sub New(sFolder As String, Optional sFileName As String = "items.json")

        If String.IsNullOrWhiteSpace(sFolder) OrElse String.IsNullOrWhiteSpace(sFileName) Then
            Throw New ArgumentException("you have to provide both folder and filename")
        End If
        _lista = New List(Of TYP)
        _filename = IO.Path.Combine(sFolder, sFileName)
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
    ''' load list from string
    ''' </summary>
    ''' <returns>False if string cannot be deserialized</returns>
    Public Overridable Function Import(JsonContent As String) As Boolean
        Try
            _lista = Newtonsoft.Json.JsonConvert.DeserializeObject(JsonContent, GetType(List(Of TYP)))
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
    '''  save data to file
    ''' </summary>
    ''' <param name="bIgnoreNulls">if NullValueHandling.Ignore and DefaultValueHandling.Ignore should be true (shorten file)</param>
    ''' <returns></returns>
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

#Region "proxies for internal list"
    ''' <summary>
    ''' get internal list of items
    ''' </summary>
    ''' <returns></returns>
    Public Function GetList() As List(Of TYP)
        Return _lista
    End Function

    ''' <summary>
    ''' get the number of elements containted in list
    ''' </summary>
    Public Function Count() As Integer
        Return _lista.Count
    End Function

    ''' <summary>
    '''  removes all elements from list
    ''' </summary>
    Public Sub Clear()
        _lista.Clear()
    End Sub

    ''' <summary>
    ''' add new item to the end of list
    ''' </summary>
    ''' <param name="oNew"></param>
    Public Sub Add(oNew As TYP)
        _lista.Add(oNew)
    End Sub

    ''' <summary>
    ''' remove first occurence of given item
    ''' </summary>
    ''' <param name="oDel"></param>
    Public Sub Remove(oDel As TYP)
        _lista.Remove(oDel)
    End Sub

    ''' <summary>
    ''' Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire list
    ''' e.g., in VB: Find(Function(x) x.PartName.Contains("seat"))
    ''' </summary>
    Public Function Find(match As Predicate(Of TYP)) As TYP
        Return _lista.Find(match)
    End Function

    ''' <summary>
    '''  Removes the first occurrence of a specific object from the list. No exception if 'no such item'
    '''  e.g., in VB: Remove(Function(x) x.PartName.Contains("seat"))
    ''' </summary>
    Public Sub Remove(match As Predicate(Of TYP))
        Dim oItem As TYP = Find(match)
        If oItem Is Nothing Then Return
        _lista.Remove(oItem)
    End Sub
#End Region


#If False Then
    Public Function Find(iID As Integer) As TYP
        Dim t As Type = TYP.GetType
        For Each oItem In _lista
            For Each oItem.GetType.
        Next
    End Function

    Public Function Find(sID As String) As TYP
        For Each oItem In _lista

        Next
    End Function
#End If

End Class



