Imports System.IO
Imports System.Reflection

Partial Public Module DotNetExtensions

#Region "String"

    ''' <summary>
    ''' convert string MAC address (hex bytes separated by '-' or ':') to ULong
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function MacStringToULong(ByVal MACstring As String) As ULong
        If String.IsNullOrWhiteSpace(MACstring) Then Throw New ArgumentException(NameOf(MACstring), "MacStringToULong should have non-empty parameter")
        MACstring = MACstring.Replace("-", ":")
        If Not MACstring.Contains(":") Then Throw New ArgumentException("MacStringToULong - no ':' in string")

        MACstring = MACstring.Replace(":", "")
        Dim uLng As ULong = ULong.Parse(MACstring, System.Globalization.NumberStyles.HexNumber, Globalization.CultureInfo.InvariantCulture)

        Return uLng
    End Function


    ''' <summary>
    ''' convert input string from "PascalCase" to "normal case"
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function DePascal(ByVal input As String)
        If String.IsNullOrWhiteSpace(input) Then Return ""

        Dim result As String = ""
        Dim letter As String = ""
        'foreach(Char letter In input)
        '{ if(char.isupper(letter) result = result.trim() + " ";
        '  result += letter
        '}
        For i = 0 To input.Length - 1
            letter = input.Substring(0, 1)
            If letter.ToUpperInvariant = letter Then
                result = result.Trim() & " "
            End If
            result &= letter
        Next

        Return result.Trim
    End Function

    ''' <summary>
    ''' wrapper for StartsWith(startString, StringComparison.Ordinal)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function StartsWithOrdinal(ByVal baseString As String, value As String) As Boolean
        Return baseString.StartsWith(value, StringComparison.Ordinal)
    End Function

    ''' <summary>
    ''' wrapper for EndsWith(endString, StringComparison.Ordinal)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function EndsWithOrdinal(ByVal baseString As String, value As String) As Boolean
        Return baseString.EndsWith(value, StringComparison.Ordinal)
    End Function

    ''' <summary>
    ''' wrapper for IndexOf() z StringComparison.Ordinal
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function IndexOfOrdinal(ByVal baseString As String, value As String) As Integer
        Return baseString.IndexOf(value, StringComparison.Ordinal)
    End Function

    ''' <summary>
    ''' trim beginning of string (same as Substring(IndexOf(startString), no trimminig if startString is not found)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function TrimBefore(ByVal baseString As String, startString As String) As String
        If String.IsNullOrEmpty(startString) Then Return baseString

        Dim iInd As Integer = baseString.IndexOf(startString, StringComparison.Ordinal)
        If iInd < 0 Then Return baseString
        Return baseString.Substring(iInd)
    End Function

    ''' <summary>
    ''' trim ending of string (same as Substring(0,IndexOf(endString)+endString.Len, no trimminig if endString is not found)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function TrimAfter(ByVal baseString As String, endString As String) As String
        If String.IsNullOrEmpty(endString) Then Return baseString

        Dim iInd As Integer = baseString.IndexOf(endString, StringComparison.Ordinal)
        If iInd < 0 Then Return baseString
        Return baseString.Substring(0, iInd + endString.Length)
    End Function

    ''' <summary>
    ''' trim beginning of string (same as Substring(LastIndexOf(startString), no trimminig if startString is not found)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function TrimBeforeLast(ByVal baseString As String, startString As String) As String
        If String.IsNullOrEmpty(startString) Then Return baseString

        Dim iInd As Integer = baseString.LastIndexOf(startString, StringComparison.Ordinal)
        If iInd < 0 Then Return baseString
        Return baseString.Substring(iInd)
    End Function

    ''' <summary>
    ''' trim ending of string (same as Substring(0,LastIndexOf(endString)+endString.Len, no trimminig if endString is not found)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function TrimAfterLast(ByVal baseString As String, endString As String) As String
        If String.IsNullOrEmpty(endString) Then Return baseString

        Dim iInd As Integer = baseString.LastIndexOf(endString, StringComparison.Ordinal)
        If iInd < 0 Then Return baseString
        Return baseString.Substring(0, iInd + endString.Length)
    End Function

    ''' <summary>
    ''' get substring between startString and endString (including these strings)
    ''' </summary>
    ''' <returns>substring, or Empty if any substring is not present</returns>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function SubstringBetweenInclusive(ByVal baseString As String, startString As String, endString As String) As String
        Dim iInd As Integer = baseString.IndexOf(startString)
        If iInd < 0 Then Return String.Empty

        baseString = baseString.Substring(iInd)
        iInd = baseString.IndexOf(endString)
        If iInd < 0 Then Return String.Empty

        Return baseString.Substring(0, iInd + endString.Length)

    End Function

    ''' <summary>
    ''' get substring between startString and endString (excluding these substrings)
    ''' </summary>
    ''' <returns>substring, or Empty if any substring is not present</returns>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function SubstringBetweenExclusive(ByVal baseString As String, startString As String, endString As String) As String
        Dim iInd As Integer = baseString.IndexOf(startString)
        If iInd < 0 Then Return ""

        baseString = baseString.Substring(iInd + startString.Length)
        iInd = baseString.IndexOf(endString)
        If iInd < 0 Then Return ""

        Return baseString.Substring(0, iInd)

    End Function

    ''' <summary>
    ''' Remove, but with strings as start/end (not indexes, as in .Net version) - start/end substrings are not removed
    ''' </summary>
    ''' <returns>string with removed part, or original string if any substring is not present</returns>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function RemoveBetween(ByVal baseString As String, sStart As String, sEnd As String) As String
        If String.IsNullOrEmpty(sStart) Then Return baseString
        If String.IsNullOrEmpty(sEnd) Then Return baseString

        Dim iIndS As Integer = baseString.IndexOf(sStart, StringComparison.Ordinal)
        If iIndS < 0 Then Return baseString
        Dim iIndE As Integer = baseString.IndexOf(sEnd, StringComparison.Ordinal)
        If iIndE < 0 Then Return baseString
        Return baseString.Remove(iIndS + sStart.Length, iIndE - iIndS + 1 - sStart.Length)
    End Function

    ''' <summary>
    ''' change Polish letters to their latin base letters (drop accents)
    ''' </summary>
    ''' <param name="basestring"></param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    Public Function Depolit(ByVal basestring As String) As String
        Dim sRet As String = basestring
        sRet = sRet.Replace("ą", "a")
        sRet = sRet.Replace("ć", "c")
        sRet = sRet.Replace("ę", "e")
        sRet = sRet.Replace("ł", "l")
        sRet = sRet.Replace("ń", "n")
        sRet = sRet.Replace("ó", "o")
        sRet = sRet.Replace("ś", "s")
        sRet = sRet.Replace("ż", "z")
        sRet = sRet.Replace("ź", "z")
        sRet = sRet.Replace("Ą", "a")
        sRet = sRet.Replace("Ć", "c")
        sRet = sRet.Replace("Ę", "E")
        sRet = sRet.Replace("Ł", "L")
        sRet = sRet.Replace("Ń", "N")
        sRet = sRet.Replace("Ó", "O")
        sRet = sRet.Replace("Ś", "S")
        sRet = sRet.Replace("Ż", "Z")
        sRet = sRet.Replace("Ź", "Z")

        Return sRet
    End Function

    ''' <summary>
    ''' try to convert string to valid filename
    ''' </summary>
    ''' <param name="useDepolit">True if you want to drop accents from Polish letters</param>
    ''' <param name="invalidCharPlaceholder">replacement string for invalid characters</param>
    <Runtime.CompilerServices.Extension()>
    Public Function ToValidPath(ByVal basestring As String, Optional useDepolit As Boolean = True, Optional invalidCharPlaceholder As String = "") As String
        Dim sRet As String = basestring
        If useDepolit Then sRet = sRet.Depolit

        Dim aInvChars As Char() = IO.Path.GetInvalidFileNameChars
        For Each sInvChar As Char In aInvChars
            sRet = sRet.Replace(sInvChar, invalidCharPlaceholder)
        Next

        aInvChars = IO.Path.GetInvalidPathChars
        For Each sInvChar As Char In aInvChars
            sRet = sRet.Replace(sInvChar, invalidCharPlaceholder)
        Next

        Return sRet

    End Function


#If NETSTANDARD2_0_OR_GREATER Then

    ''' <summary>
    ''' try to convert string to valid filename (very strict: POSIX portable filename, see IEEE 1003.1, 3.282), dropping accents etc., and all other characters change to '_'
    ''' POSIX allows only latin letters, digits, dot, underscore and minus.
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToPOSIXportableFilename(ByVal basestring As String) As String
        Dim FKD As String = basestring.Normalize(Text.NormalizationForm.FormKD)
        Dim sRet As String = ""

        For Each cTmp As Char In FKD
            If "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789._-".Contains(cTmp) Then
                sRet &= cTmp
            ElseIf AscW(cTmp) >= &H300 AndAlso AscW(cTmp) < &H36F Then
                ' combining - skip
            Else
                ' nie wiadomo co, więc podmieniamy
                sRet &= "_"
            End If
        Next

        Return sRet
    End Function

    ''' <summary>
    ''' try to convert string to filename, dropping accents
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function DropAccents(ByVal basestring As String) As String
        Dim FKD As String = basestring.Normalize(Text.NormalizationForm.FormKD)
        Dim sRet As String = ""

        For Each cTmp As Char In FKD
            If AscW(cTmp) >= &H2B0 AndAlso AscW(cTmp) < &H36F Then
                ' 02B0 - 02FF	Spacing Modifier Letters
                ' 0300 - 036F	Combining Diacritical Marks
            ElseIf AscW(cTmp) >= &H1AB0 AndAlso AscW(cTmp) < &H1B00 Then
                ' 1AB0 - 1AFF	Combining Diacritical Marks Extended
            ElseIf AscW(cTmp) >= &H1DC0 AndAlso AscW(cTmp) < &H1E00 Then
                ' 1DC0 - 1DFF	Combining Diacritical Marks Supplement
            ElseIf AscW(cTmp) >= &H20D0 AndAlso AscW(cTmp) < &H2100 Then
                ' 20D0 - 20FF	Combining Diacritical Marks for Symbols
            ElseIf AscW(cTmp) >= &HFE20 AndAlso AscW(cTmp) < &HFE30 Then
                ' FE20 - FE2F	Combining Half Marks
            Else
                sRet &= cTmp
            End If
        Next

        Return sRet
    End Function

#End If


#End Region

#Region "Integer"

    ''' <summary>
    ''' as ToString, but uses space as thousands separator
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToStringWithSpaces(ByVal value As Integer) As String
        Dim temp As Long = value
        Return temp.ToStringWithSpaces
    End Function

    ''' <summary>
    ''' as ToString, but uses SI form (space as thousands separator, if more than 9999)
    ''' </summary>
    ''' <param name="iValue"></param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    Public Function ToSIstring(ByVal value As Integer) As String
        Dim temp As Long = value
        Return temp.ToSIstring
    End Function

    ''' <summary>
    ''' Convert seconds number to "XXd HH:MM:SS", integer.max = ~150 days
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToStringDHMS(ByVal iSecs As Integer) As String
        ' integer = 2,147,483,647, z sekund na 3600 godzin, 150 dni
        Dim temp As Long = iSecs
        Return temp.ToStringDHMS
    End Function


#End Region

#Region "Long"

    ''' <summary>
    ''' return file len converted to string, using full name (byte/bytes, with binary prefixes: kibi, mebi, gibi, tebi, pebi and exbi
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function FileLen2string(ByVal iBytes As Long) As String
        ' integer.max = 2 147 483 647, ≈ 2 GiB; long.max = 9,223,372,036,854,775,807 ≈ 9 EiB
        If iBytes = 1 Then Return "1 byte"
        If iBytes < 10000 Then Return iBytes & " bytes"
        iBytes \= 1024
        If iBytes = 1 Then Return "1 kibibyte"
        If iBytes < 2000 Then Return iBytes & " kibibytes"
        iBytes \= 1024
        If iBytes = 1 Then Return "1 mebibyte"
        If iBytes < 2000 Then Return iBytes & " mebibytes"
        iBytes \= 1024
        If iBytes = 1 Then Return "1 gibibyte"
        If iBytes < 2000 Then Return iBytes & " gibibytes"

        ' now, only LONG
        iBytes \= 1024
        If iBytes = 1 Then Return "1 tebibyte"
        If iBytes < 2000 Then Return iBytes & " tebibytes"
        iBytes \= 1024
        If iBytes = 1 Then Return "1 pebibyte"
        If iBytes < 2000 Then Return iBytes & " pebibytes"
        iBytes \= 1024
        If iBytes = 1 Then Return "1 exbibyte"
        Return iBytes & " exbibytes"

    End Function

    ''' <summary>
    ''' as ToString, but uses space as thousands separator
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToStringWithSpaces(ByVal iLong As Long) As String
        Dim nfi As System.Globalization.NumberFormatInfo
        nfi = System.Globalization.NumberFormatInfo.InvariantInfo.Clone
        nfi.NumberGroupSeparator = " "
        Return iLong.ToString(nfi)
    End Function

    ''' <summary>
    ''' as ToString, but uses SI form (space as thousands separator, if more than 9999)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToSIstring(ByVal value As Long) As String
        Dim nfi As System.Globalization.NumberFormatInfo
        nfi = System.Globalization.NumberFormatInfo.InvariantInfo.Clone
        nfi.NumberGroupSeparator = If(value < 10000, "", " ")
        Return value.ToString(nfi)
    End Function

    ''' <summary>
    ''' Convert seconds number to "XXd HH:MM:SS", integer.max = ~150 days
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToStringDHMS(ByVal seconds As Long) As String
        Dim sTmp As String = ""

        If seconds > 60 * 60 * 24 Then
            sTmp = sTmp & seconds \ (60 * 60 * 24) & "d "
            seconds = seconds Mod (60 * 60 * 24)
        End If

        If seconds > 60 * 60 Then
            sTmp = sTmp & seconds \ (60 * 60) & ":"
            seconds = seconds Mod (60 * 60)
        End If

        If seconds \ 60 < 10 And sTmp.Length > 1 Then sTmp &= "0"
        sTmp = sTmp & seconds \ 60 & ":"
        seconds = seconds Mod 60

        If seconds < 10 And sTmp.Length > 1 Then sTmp &= "0"
        sTmp &= seconds.ToString(Globalization.CultureInfo.InvariantCulture)

        Return sTmp
    End Function

    ''' <summary>
    ''' as ToString, but use SI prefixes (k, M, G, T, P, E), 'switching' point is 9999 (9999, 10 k)
    ''' </summary>
    ''' <param name="unitSymbol">symbol of unit, e.g. "g", or name ("gram"); if you use names please set fullPrefix, as 1 e.g. kgram or KiByte is incorrect</param>
    ''' <param name="binary">True to use 1024 divider, False (default) to use 1000 divider</param>
    <Runtime.CompilerServices.Extension()>
    Public Function ToSIstringWithPrefix(ByVal value As Long, unitSymbol As String, Optional fullPrefix As Boolean = False, Optional binary As Boolean = False) As String
        ' zakres integer: 2.1 Gi        (2 147 483 647)
        ' zakres long: 9.2  (9 223 372 036 854 775 808)

        Dim iDivider As Integer = If(binary, 1024, 1000)

        If value < 9999 Then Return value.ToSIstring & If(unitSymbol = "", "", " " & unitSymbol)

        Dim aPrefixes As String(,) = {{"k", "kilo", "Ki", "kibi"}, {"M", "mega", "Mi", "mebi"}, {"G", "giga", "Gi", "gibi"}, {"T", "tera", "Ti", "tebi"}, {"P", "peta", "Pi", "pebi"}, {"E", "exa", "Ei", "exbi"}}
        Dim iNameInd As Integer = 0
        If binary Then iNameInd += 2
        If fullPrefix Then iNameInd += 1

        For iLp As Integer = 0 To 5
            value = value / iDivider + 1
            If value < 9999 Then Return value.ToSIstring & $" {aPrefixes(iLp, iNameInd)}{unitSymbol}"
        Next

        'EXA jeśli nie było wcześniej
        Return value.ToSIstring & $" {aPrefixes(5, iNameInd)}{unitSymbol}"

    End Function


#End Region

#Region "Ulong"
    ''' <summary>
    ''' convert ULong to shortest hex bytes string (e.g. "15", "01:2A", etc.)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToHexBytesString(ByVal value As ULong) As String
        Dim sTmp As String = String.Format(Globalization.CultureInfo.InvariantCulture, "{0:X}", value)
        If sTmp.Length Mod 2 <> 0 Then sTmp = "0" & sTmp

        Dim sRet As String = ""
        Dim bDwukrop As Boolean = False

        While sTmp.Length > 0
            If bDwukrop Then sRet &= ":"
            bDwukrop = True
            sRet &= sTmp.Substring(0, 2)
            sTmp = sTmp.Substring(2)
        End While

        ' gniazdko BT18, daje 15:A6:00:E8:07 (bez 00:)
        ' 71:0A:22:CD:4F:20
        ' 12345678901234567
        If sRet.Length < 17 Then sRet = "00:" & sRet
        If sRet.Length < 17 Then sRet = "00:" & sRet


        Return sRet
    End Function

#End Region

    ''' <summary>
    ''' Same as Clamp in .Net 7
    ''' </summary>
    ''' <param name="value">current value</param>
    ''' <param name="minVal">The inclusive minimum to which value should clamp</param>
    ''' <param name="maxVal">The inclusive maximum to which value should clamp</param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension()>
    Public Function Between(Of T)(ByVal value As T, minVal As T, maxVal As T) As T
        If Comparer(Of T).Default.Compare(minVal, value) > 0 Then Return minVal
        If Comparer(Of T).Default.Compare(maxVal, value) < 0 Then Return maxVal
        Return value
    End Function


#Region "dates"

    ''' <summary>
    ''' return Modified Julian Day (days since 0:00 November 17, 1858)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToModifiedJulianDay(ByVal data As Date) As Double
        Dim epoch As New Date(1858, 11, 17)
        Dim datediff As TimeSpan = Date.Now - epoch
        Return datediff.TotalDays
    End Function


    ''' <summary>
    ''' return Julian Day (days since 12:00 January 1, 4713 BC)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToJulianDay(ByVal data As Date) As Integer
        ' integer = 2 Gdays, a MJD offset to 2 Mdays
        Return data.ToModifiedJulianDay + 2400000.5
    End Function

    <Runtime.CompilerServices.Extension()>
    Public Function ToStringDHMS(ByVal czas As TimeSpan) As Integer
        Dim temp As Integer = Math.Round(czas.TotalSeconds)
        Return temp.ToStringDHMS
    End Function

#End Region

    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    <CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification:="<Pending>")>
    Public Function ToDebugString(ByVal aArr As Byte(), iSpaces As Integer) As String

        Dim sPrefix As String = ""
        For i As Integer = 1 To iSpaces
            sPrefix &= " "
        Next

        Dim sBytes As String = ""
        Dim sAscii As String = sBytes

        For i As Integer = 0 To Math.Min(aArr.Length - 1, 32) ' bylo oVal

            Dim cBajt As Byte = aArr.ElementAt(i)

            ' hex: tylko 16 bajtow
            If i < 16 Then
                Try
                    sBytes = sBytes & " 0x" & String.Format(Globalization.CultureInfo.InvariantCulture, "{0:X}", cBajt)
                Catch ex As Exception
                    sBytes &= " ??"
                End Try
            End If

            ' ascii: do 32 bajtow
            If cBajt > 31 And cBajt < 160 Then
                sAscii &= ChrW(cBajt)
            Else
                sAscii &= "?"
            End If
        Next

        If aArr.Length - 1 > 16 Then sBytes &= " ..."
        If aArr.Length - 1 > 32 Then sAscii &= " ..."

        Dim sRet As String = ""
        If aArr.Length > 6 Then sRet = sPrefix & "length: " & aArr.Length
        sRet = sRet & sPrefix & "binary: " & sBytes & vbCrLf &
            sPrefix & "ascii:  " & sAscii

        Return sRet & vbCrLf

    End Function


    <Runtime.CompilerServices.Extension()>
    Public Async Function IsSameStreamContent(ByVal oStream1 As Stream, oStream2 As Stream) As Task(Of Boolean)
        ' This is not merely an optimization, as incrementing one stream's position
        ' should Not affect the position of the other.
        If oStream1.Equals(oStream2) Then Return True

        If oStream1.Length <> oStream2.Length Then Return False

        Dim oBuf1 As Byte() = New Byte(4100) {}
        Dim oBuf2 As Byte() = New Byte(4100) {}

        Do
            Dim iBytes1 As Integer = Await oStream1.ReadAsync(oBuf1, 0, 4096)
            Dim iBytes2 As Integer = Await oStream2.ReadAsync(oBuf2, 0, 4096)

            If iBytes1 = 0 Then Return True

            For iLp As Integer = 0 To iBytes1
                If oBuf1(iLp) <> oBuf2(iLp) Then Return False
            Next

        Loop

        Return True
    End Function


End Module
