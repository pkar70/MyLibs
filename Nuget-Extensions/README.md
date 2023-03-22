
This Nuget contains several extensions to .Net types, that I use in many of my programs.
There are two branches:
* v1, for .Net Standard 1.4 (working also in UWP apps for phones)
* v2, for .Net Standard 2.0 (if you can afford dropping support for phones)

If some extension is added in v1.x.y, then it would be present also in v2.x.y (not in e.g. 2.0.0)

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

    String.DropAccents // in v2.x.x
    String.ToPOSIXportableFilename(optional replacement as string = "_") // in v2.x.x
    String.TransliterateCyrilicToLatin(ByVal basestring As String) As String	// in v2.x.x
    String.Function TransliterateGreekToLatin(ByVal basestring As String) As String	// in v2.x.x

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
    Date.TwoLetterWeekDayPL(Optional bezOgonkow As Boolean = False) As String // ['bezOgonkow' since x.1.3]
    Date.ToExifString() As String

# for GUIDs
    These are for Bluetooth debugging

    GUID.AsGattReservedDescriptorName As String // [since x.1.3]
    GUID.AsGattReservedServiceName As String // [since x.1.3]
    GUID.AsGattReservedCharacteristicName As String // [since x.1.3]

# other

    Function Between(Of T)(ByVal value As T, minVal As T, maxVal As T) As T
    Byte().ToDebugString(iSpaces As Integer) As String
    Stream.IsSameStreamContent(oStream2 As Stream) As Task(Of Boolean)
