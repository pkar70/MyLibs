
# version 1.6.1, 2024.06.14
 a) UseBak property, for .Save
 b) GetOnLoadMemSizeKB (very coarse)
 c) SaveTemp, saving temporary file
 d) Export to JSON string

# version 1.5.0, 2023.10.04
 As BaseList now inherits ObservableList (previously it was only Protected field), you can use BaseList directly in all LINQ expressions.

# version 1.4.1, 2023.08.07
 Change Private to Protected of _list in BaseList

# version 1.4.0, 2023.04.21
 Added Base[List|Dict].MaintainCopy

# version 1.3.0, 2023.03.05
 Changed Nuget ID from StructList to pkar.StructList
 Added ObservableList class
 Updated BaseList to use ObservableList

# version 1.2.0, 2023.03.10
 Added BaseDict, similar do BaseList
 Added BaseList.Import method
 Split source file into two files.

# version 1.1.0, 2023.02.27
Added BaseStruct.CopyTo(anyObject) and BaseStruct.CopyFrom(anyObject)

# version 1.0.1, 2023.01.12
 BaseList.Load: If file is just dump of data (as `Append(item)`), it would have no ']' closing list. So, if standard deserialization fails, we add this ']' and try deserialization again.
 (special case in one of my apps)

# version 1.0.0, 2023.01.09