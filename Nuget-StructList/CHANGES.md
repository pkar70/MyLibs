
# version 1.1.0
Added BaseStruct.CopyTo(anyObject) and BaseStruct.CopyFrom(anyObject)


# version 1.0.1
 BaseList.Load: If file is just dump of data (as `Append(item)`), it would have no ']' closing list. So, if standard deserialization fails, we add this ']' and try deserialization again.
 (special case in one of my apps)