
 Helpers bases for items and lists.

 Almost every app writes/read data from file, and in every app we have to place same code... This Nuget should help with this.

# BaseStruct

 It is base for class/struct you use in your code. If your class inherits from BaseStruct, you will get:
 * two debugging helpers, to dump properties values:

    Function DumpAsText() As String     // dump of all properties' values (in something like table)
    Function DumpAsJSON(Optional bSkipDefaults As Boolean = False) As String    // similar, but gives full dump of tree of properties as JSON dump

* methods you can utilize in code, and sometimes even you can love it:

    Function Clone() As Object  // deep Clone of item (dump to JSON and read it to new item)
    Sub CopyFrom(anyObject)     // copy all properties/fields from anyObject, if their names matches
    Sub CopyTo(anyObject)       // copy all properties/fields to anyObject, if their names matches

# BaseList

 It is base for lists backed by JSON file; in many cases your app would be relieved from Nugetting JSON.
 Using:
 
    VB: Class YourList Inherits BaseList(Of YourClass)
    C#: class YourList : BaseList<YourClass>

 Maybe you would like to override one method, called by Load if file is empty. In overrided method you can add default entries to list.
 
    Protected Overridable Sub InsertDefaultContent()


## constructor

 You have to give folder for file, and you can override default file name.

     New(sFolder As String, Optional sFileName As String = "items.json")

## operations on data file

    Overridable Function Load() As Boolean
    Overridable Function Save(Optional bIgnoreNulls As Boolean = False) As Boolean
    Function GetFileDate() As Date
    Function IsObsolete(iDays As Integer)


## proxies for internal list

    Function GetList() As List(Of TYP)
    Function Count() As Integer
    Sub Clear()
    Sub Add(oNew As TYP)
    Sub Remove(oDel As TYP)
    Function Find(match As Predicate(Of TYP)) As TYP
    Sub Remove(match As Predicate(Of TYP))

## other functions

    Function LoadItem(sJSON As String) As TYP


