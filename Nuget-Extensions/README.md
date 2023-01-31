
This Nuget contains several extensions to .Net types, that I use in many of my programs.

# for String

    String.MacStringToULong() As ULong
    String.DePascal() As String
    String.StartsWithOrdinal(value As String) As Boolean
    String.EndsWithOrdinal(value As String) As Boolean
    String.IndexOfOrdinal(value As String) As Integer
    String.TrimBefore(startString As String) As String
    String.TrimAfter(endString As String) As String
    String.TrimBeforeLast(startString As String) As String
    String.TrimAfterLast(endString As String) As String
    String.SubstringBetweenInclusive(startString As String, endString As String) As String
    String.SubstringBetweenExclusive(startString As String, endString As String) As String
    String.RemoveBetween(sStart As String, sEnd As String) As String
    String.Depolit() As String
    String.ToValidPath(Optional useDepolit As Boolean = True, Optional invalidCharPlaceholder As String = "") As String

    String.DropAccents // in v2.0.0
    String.ToPOSIXportableFilename(optional replacement as string = "_") // in v2.0.0

# for Integer

    Integer.ToStringWithSpaces() As String
    Integer.ToSIstring() As String
    Integer.ToStringDHMS() As String

# for Long

    Long.FileLen2string() As String
    Long.ToStringWithSpaces() As String
    Long.ToSIstring() As String
    Long.ToStringDHMS() As String
    Long.ToSIstringWithPrefix(unitSymbol As String, Optional fullPrefix As Boolean = False, Optional binary As Boolean = False) As String

# for Ulong

    ULong.ToHexBytesString() As String

# for dates

    Date.ToJulianDay() As Integer
    Date.ToModifiedJulianDay() As Double
    TimeSpan.ToStringDHMS() As Integer
    Date.TwoLetterWeekDayPL() As String
    Date.ToExifString() As String

# other
    Function Between(Of T)(ByVal value As T, minVal As T, maxVal As T) As T
    Byte().ToDebugString(iSpaces As Integer) As String
    Stream.IsSameStreamContent(oStream2 As Stream) As Task(Of Boolean)
