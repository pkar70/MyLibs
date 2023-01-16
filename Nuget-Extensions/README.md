
This Nuget contains several extensions to .Net types, that I use in many of my programs.

# for String

    Function MacStringToULong(ByVal MACstring As String) As ULong
    Function DePascal(ByVal input As String)
    Function StartsWithOrdinal(ByVal baseString As String, value As String) As Boolean
    Function EndsWithOrdinal(ByVal baseString As String, value As String) As Boolean
    Function IndexOfOrdinal(ByVal baseString As String, value As String) As Integer
    Function TrimBefore(ByVal baseString As String, startString As String) As String
    Function TrimAfter(ByVal baseString As String, endString As String) As String
    Function TrimBeforeLast(ByVal baseString As String, startString As String) As String
    Function TrimAfterLast(ByVal baseString As String, endString As String) As String
    Function SubstringBetweenInclusive(ByVal baseString As String, startString As String, endString As String) As String
    Function SubstringBetweenExclusive(ByVal baseString As String, startString As String, endString As String) As String
    Function RemoveBetween(ByVal baseString As String, sStart As String, sEnd As String) As String
    Function Depolit(ByVal basestring As String) As String
    Function ToValidPath(ByVal basestring As String, Optional useDepolit As Boolean = True, Optional invalidCharPlaceholder As String = "") As String

# for Integer

    Function ToStringWithSpaces(ByVal value As Integer) As String
    Function ToSIstring(ByVal value As Integer) As String
    Function ToStringDHMS(ByVal iSecs As Integer) As String

# for Long

    Function FileLen2string(ByVal iBytes As Long) As String
    Function ToStringWithSpaces(ByVal iLong As Long) As String
    Function ToSIstring(ByVal value As Long) As String
    Function ToStringDHMS(ByVal seconds As Long) As String
    Function ToSIstringWithPrefix(ByVal value As Long, unitSymbol As String, Optional fullPrefix As Boolean = False, Optional binary As Boolean = False) As String

# for Ulong

    Function ToHexBytesString(ByVal value As ULong) As String

# for dates

    Function ToModifiedJulianDay(ByVal data As Date) As Double
    Function ToJulianDay(ByVal data As Date) As Integer
    Function ToStringDHMS(ByVal czas As TimeSpan) As Integer


# other
    Function Between(Of T)(ByVal value As T, minVal As T, maxVal As T) As T
    Function ToDebugString(ByVal aArr As Byte(), iSpaces As Integer) As String
    Async Function IsSameStreamContent(ByVal oStream1 As Stream, oStream2 As Stream) As Task(Of Boolean)
