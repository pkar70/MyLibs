
 Helpers bases for items and lists.

 Almost every app writes/read data from file, and in every app we have to place same code... This Nuget should help with this.

# BaseStruct

 It is base for class/struct you use in your code. If your class inherits from BaseStruct, you will get:
two debugging helpers, to dump properties values:

    Function DumpAsText() As String     // dump of all properties' values (in something like table)
    Function DumpAsJSON(Optional bSkipDefaults As Boolean = False) As String    // similar, but gives full dump of tree of properties as JSON dump

methods you can utilize in code, and sometimes even you can love it:

    Function Clone() As Object  // deep Clone of item (dump to JSON and read it to new item)
    Sub CopyFrom(anyObject)     // copy all properties/fields from anyObject, if their names matches, since 1.1.0
    Sub CopyTo(anyObject)       // copy all properties/fields to anyObject, if their names matches, since 1.1.0

Also, BaseStruct implements INotifyPropertyChanged and INotifyPropertyChanging, and gives you helper method:

    Sub NotifyPropChange(propertyName As String) // raise Event notifying that given property is changed
    Sub NotifyPropChanging(propertyName As String) // raise Event notifying that given property is about to be changed (so you can get old value of property before change occurs)


# BaseList

 It is base for lists backed by JSON file; in many cases your app would be relieved from Nugetting JSON. It uses ObservableList as internal data storage.
 Using:
 
    VB: Class YourList Inherits BaseList(Of YourClass)
    C#: class YourList : BaseList<YourClass>

 Maybe you would like to override one method, called by Load if file is empty. In overrided method you can add default entries to list.
 
    Protected Overridable Sub InsertDefaultContent()


## constructor

 You have to give folder for file, and you can override default file name. Since 1.5, folder can be NULL (or empty) to create only memory-backed list (no serialization)

     New(sFolder As String, Optional sFileName As String = "items.json")

## operations on data file

 Most of these methods throws exception if called in memory-only List.

    Overridable Function Load() As Boolean
    Overridable Function Import(string) As Boolean // since 1.2.0, can be used in memory-only list
    Overridable Function Save(Optional bIgnoreNulls As Boolean = False) As Boolean
    Function GetFileDate() As Date
    Function IsObsolete(iDays As Integer)   // since 1.5.0: for memory-only lists, always returns false
    Function GetFileSize() As Integer       // since 1.8.1
    Sub MaintainCopy(folderForCopy As String)   // since 1.4.0
    UseBak As Boolean // since 1.6.0; if TRUE, .Save creates .bak file
    Function SaveTemp(Optional bIgnoreNulls As Boolean = False) As Boolean // since 1.6.1

 Sometimes list is backed by SSD storage - so we can group together many changes and save only when really needed. Behaviour can be controlled using two methods: first, save is done after defined number of calls to DelayedSave; second, save is done after defined TimeSpan after last change. Functionality added in 1.8 version.

    Sub SetDelay(count As UInt16, time As TimeSpan) // save current list (if IsDirty), and set both counter and timespan
    Sub DelayedSave(Optional force As Boolean = False) // mark as dirty, and wait for saving


## proxies for internal list

    Function GetList() As ObservableList(Of TYP)    // obsolete since 1.5.0 - simply skip .GetList from expressions

 Since 1.5.0, you can use BaseList in any LINQ expressions. In older version, you can use only these:

    Function Count() As Integer
    Sub Clear()
    Sub Add(oNew As TYP)
    Sub Remove(oDel As TYP)
    Function Find(match As Predicate(Of TYP)) As TYP
    Sub Remove(match As Predicate(Of TYP))
    Function Export(Optional bIgnoreNulls As Boolean = False) As String // since 1.6.1


## other functions

    Function LoadItem(sJSON As String) As TYP
    Public Function GetOnLoadMemSizeKB() As Integer // since 1.6.0; gets memory "consumed" while reading file (if your app doesn't consume memory in other threads, it would be size of List)

# BaseDict [since 1.2.0]

 It is base for dictionary backed by JSON file; in many cases your app would be relieved from Nugetting JSON.
 Using:
 
    VB: Class YourList Inherits BaseDict(Of TypeOfKey, TypeOfValue)
    C#: class YourList : BaseDict<TKey, TValue>

 Maybe you would like to override one method, called by Load if file is empty. In overrided method you can add default entries to dictionary.
 
    Protected Overridable Sub InsertDefaultContent()

## constructor

 You have to give folder for file, and you can override default file name.

     New(sFolder As String, Optional sFileName As String = "items.json")

## operations on data file

    Overridable Function Load() As Boolean
    Overridable Function Import(string) As Boolean
    Overridable Function Save(Optional bIgnoreNulls As Boolean = False) As Boolean
    Function GetFileDate() As Date
    Function IsObsolete(iDays As Integer)
    Function GetFileSize() As Integer       // since x.8.1
    Sub MaintainCopy(folderForCopy As String)   // since 1.4.0
    UseBak As Boolean // since 1.6.0; if TRUE, .Save creates .bak file
    Function SaveTemp(Optional bIgnoreNulls As Boolean = False) As Boolean // since 1.6.1

Sometimes list is backed by SSD storage - so we can group together many changes and save only when really needed. Behaviour can be controlled using two methods: first, save is done after defined number of calls to DelayedSave; second, save is done after defined TimeSpan after last change. Functionality added in 1.8 version.

    Sub SetDelay(count As UInt16, time As TimeSpan) // save current list (if IsDirty), and set both counter and timespan
    Sub DelayedSave(Optional force As Boolean = False) // mark as dirty, and wait for saving


## proxies for internal list

    Function GetDictionary() As Dictionary(Of TKEY, TVALUE)
    Function Count() As Integer
    Sub Clear()
    Function TryAdd(oNew As KeyValuePair(Of TKEY, TVALUE)) As Boolean
    Function ContainsKey(key As TKEY) As Boolean
    Function TryAdd(key As TKEY, value As TVALUE) As Boolean
    Sub Remove(oDel As KeyValuePair(Of TKEY, TVALUE))
    Sub Remove(key As TKEY)
    Function Item(key As TKEY) As TVALUE
    Function TryGetValue(key As TKEY, ByRef value As TVALUE) As Boolean

## other functions

    Function LoadItem(sJSON As String) As TYP
    Public Function GetOnLoadMemSizeKB() As Integer // since 1.6.0; gets memory "consumed" while reading file (if your app doesn't consume memory in other threads, it would be size of Dict)
    Function Export(Optional bIgnoreNulls As Boolean = False) As String // since 1.6.1


# ObservableList [since 1.3.0]

 This is only partial implementation of ObservableList - it sends only CollectionChanged (not PropertyChanged).
Also, not every method of List manipulation is monitored. 

    Class ObservableList(Of TYP) Inherits List(Of TYP) Implements INotifyCollectionChanged, INotifyPropertyChanged

    Public Event CollectionChanged As NotifyCollectionChangedEventHandler 
    Public Event PropertyChanged As PropertyChangedEventHandler 

 These operations on ObservableList sends apropriate CollectionChanged event:

    Clear()
    Add(item As TYP)
    AddRange(collection As IEnumerable(Of TYP))
    Remove(item As TYP)
    RemoveAll(match As Predicate(Of TYP))
    RemoveAt(index As Int32)
    RemoveRange(index As Int32, count As Int32)
    Reverse()
    Reverse(index As Int32, count As Int32)
    Sort()
    Sort(compar As Comparison(Of TYP))
    Sort(compar As IComparer(Of TYP))
    Sort(index As Int32, count As Int32, compar As IComparer(Of TYP))

