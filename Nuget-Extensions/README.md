﻿
This Nuget contains several extensions to .Net types, that I use in many of my programs.
There are two branches:
* v1, for .Net Standard 1.4 (working also in UWP apps for phones)
* v2, for .Net Standard 2.0 (if you can afford dropping support for phones, or if you use trick from https://gist.github.com/WamWooWam/e72e5137606f7c59ed657db6587cd5e8 - checked with 15063, Lumia 532)

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
    String.NotContains(value As String) As Boolean

    String.DropAccents // in v2.x.x
    String.ToPOSIXportableFilename(optional replacement as string = "_") // in v2.x.x
    String.TransliterateCyrilicToLatin(ByVal basestring As String) As String	// since vx.2.1
    String.TransliterateGreekToLatin(ByVal basestring As String) As String	// since vx.2.1
    String.ToPOSIXportableFilename(useTransliteration As Boolean, optional replacement as string = "_") // since v2.2.1

    String.CommonPrefix(string1 As String) As String    // since x.2.2
    String.CommonPrefixLen(string1 As String) As Integer // since x.2.2

    String.IsLowerInvariant As Boolean  // since x.2.3
    String.IsUpperInvariant As Boolean  // since x.2.3

    // same as SQL: case sensitive, i.e. standard methods (as same without CS suffix)
    String.ContainsCS     // since x.2.3
    String.StartsWithCS   // since x.2.3
    String.EndsWithCS     // since x.2.3
    String.EqualsCS       // since x.2.3

    // case insensitive methods; uses CurrentCultureIgnoreCase in 1.x.x, and InvariantCultureIgnoreCase in 2.x.x
    String.ContainsCI     // since x.2.3
    String.StartsWithCI   // since x.2.3
    String.EndsWithCI     // since x.2.3
    String.EqualsCI       // since x.2.3

    // same as SQL: case sensitive, accent sensitive; uses Normalization KFD form
    String.ContainsCSAS     // since 2.2.3
    String.StartsWithCSAS   // since 2.2.3
    String.EndsWithCSAS     // since 2.2.3
    String.EqualsCSAS       // since 2.2.3

    // case insensitive methods; uses OrdinalIgnoreCase on Normalization KFD form 
    String.ContainsCIAS     // since 2.2.3
    String.StartsWithCIAS   // since 2.2.3
    String.EndsWithCIAS     // since 2.2.3
    String.EqualsCIAS       // since 2.2.3

    // case insensitive, accent insensitive methods; uses InvariantCulture
    String.ContainsCSAI     // since 2.2.3
    String.StartsWithCSAI   // since 2.2.3
    String.EndsWithCSAI     // since 2.2.3
    String.EqualsCSAI       // since 2.2.3

    // case insensitive, accent insensitive methods; uses InvariantCultureIgnoreCase
    String.ContainsCIAI     // since 2.2.3
    String.StartsWithCIAI   // since 2.2.3
    String.EndsWithCIAI     // since 2.2.3
    String.EqualsCIAI       // since 2.2.3

    // "macros" to make some expressions more readable, since x.2.7
    String.NotContainsCI
    String.NotStartSwithCI
    String.NotStartsWith
    String.NotEndsWithCI
    String.NotEndsWith
    String.NotEqualsCI
    String.NotEquals

    // parse "macros" to make some expressions more readable, since x.2.7

    String.ParseInt
    String.ParseDouble
    String.ParseExifDate

    String.CountChar    // since x.2.7


# for Integer

    Integer.ToStringWithSpaces() As String
    Integer.ToSIstring() As String
    Integer.ToStringDHMS() As String

    Integer.Max(int1 As Integer) As Integer    // since x.2.2, same as Math.Max(int0, int1)
    Integer.Min(int1 As Integer) As Integer    // since x.2.2, same as Math.Min(int0, int1)
    Integer.IsMinOrMax As Boolean    // since x.2.2

    Integer.Abs // since x.2.5, same as Math.Abs
    Integer.Sign // since x.2.5, same as Math.Sign

# for Long

    Long.FileLen2string() As String
    Long.ToStringWithSpaces() As String
    Long.ToSIstring() As String
    Long.ToStringDHMS() As String
    Long.ToSIstringWithPrefix(unitSymbol As String, Optional fullPrefix As Boolean = False, Optional binary As Boolean = False) As String
    Long.Abs // since x.2.5, same as Math.Abs
    Long.Sign // since x.2.5, same as Math.Sign

# for Ulong

    ULong.ToHexBytesString() As String

# for Double

    Double.Max(double1 As Double) As Double    // since x.2.2, same as Math.Max(dbl0, dbl1)
    Double.Min(double1 As Double) As Double    // since x.2.2, same as Math.Min(dbl0, dbl1)
    Double.IsMinOrMax As Boolean    // since x.2.2
    Double.Abs // since x.2.5, same as Math.Abs
    Double.Sign // since x.2.5, same as Math.Sign
    Double.Floor // since x.2.5, same as Math.Floor
    Double.Ceiling // since x.2.5, same as Math.Ceiling
    Double.Round // since x.2.5, same as Math.Round
    Double.Round(digits) // since x.2.5, same as Math.Round(digits)
    Double.Equals(value, epsilon) As Boolean // since x.2.6

# for dates

    Date.ToJulianDay() As Integer
    Date.ToModifiedJulianDay() As Double
    TimeSpan.ToStringDHMS() As String
    Date.TwoLetterWeekDayPL(Optional bezOgonkow As Boolean = False) As String // ['bezOgonkow' since x.1.3]
    Date.ToExifString() As String

    Date.Min(date1 As Date) As Date    // since x.2.2, analog of Math.Min(int0, int1)
    Date.Min(date1 As Date) As Date    // since x.2.2, analog of Math.Min(int0, int1)
    Date.IsMinOrMax As Boolean    // since x.2.2

# for GUIDs
    These are for Bluetooth debugging

    GUID.AsGattReservedDescriptorName As String // [since x.1.3]
    GUID.AsGattReservedServiceName As String // [since x.1.3]
    GUID.AsGattReservedCharacteristicName As String // [since x.1.3]

# other

    Function Between(Of T)(ByVal value As T, minVal As T, maxVal As T) As T
    Byte().ToDebugString(iSpaces As Integer) As String
    Stream.IsSameStreamContent(oStream2 As Stream) As Task(Of Boolean)
