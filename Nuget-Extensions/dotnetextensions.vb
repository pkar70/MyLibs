Imports System.IO
Imports System.Reflection

Partial Public Module DotNetExtensions

#Region "String"

    ''' <summary>
    ''' Get len of prefix, common in two strings (CommonPrefixLen("PREFIX", "PREFACE") = 4, CommonPrefixLen("PREFIX", "PRE") = 3)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function CommonPrefixLen(ByVal string0 As String, string1 As String) As Integer
        Dim iLp As Integer
        For iLp = Math.Min(string0.Length, string1.Length) - 1 To 0 Step -1
            If string0(iLp) = string1(iLp) Then Exit For
        Next
        Return iLp
    End Function

    ''' <summary>
    ''' Get prefix common in two strings (CommonPrefix("PREFIX", "PREFACE") = "PREF", CommonPrefixLen("PREFIX", "PRE") = "PRE")
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function CommonPrefix(ByVal string0 As String, string1 As String) As String
        Return string0.Substring(0, CommonPrefixLen(string0, string1))
    End Function


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
    ''' convert input string from "PascalCase" to "Pascal Case"
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function DePascal(ByVal input As String) As String
        If String.IsNullOrWhiteSpace(input) Then Return ""

        Dim result As String = ""
        Dim letter As String

        For i = 1 To input.Length - 1
            letter = input.Substring(i, 1)
            If letter.ToUpperInvariant = letter Then
                result = result.Trim() & " "
            End If
            result &= letter
        Next

        Return result.Trim
    End Function

    ''' <summary>
    ''' wrapper for StartsWith(value, StringComparison.Ordinal)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function StartsWithOrdinal(ByVal baseString As String, value As String) As Boolean
        Return baseString.StartsWith(value, StringComparison.Ordinal)
    End Function

    ''' <summary>
    ''' wrapper for EndsWith(value, StringComparison.Ordinal)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification:="<Pending>")>
    Public Function EndsWithOrdinal(ByVal baseString As String, value As String) As Boolean
        Return baseString.EndsWith(value, StringComparison.Ordinal)
    End Function

    ''' <summary>
    ''' wrapper for IndexOf(value) z StringComparison.Ordinal
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

    ''' <summary>
    ''' same as Not string.Contains, making expressions look nicer
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function NotContains(ByVal basestring As String, value As String) As Boolean
        Return Not basestring.Contains(value)
    End Function



#If NETSTANDARD2_0_OR_GREATER Then

    ''' <summary>
    ''' try to convert string to valid filename (very strict: POSIX portable filename, see IEEE 1003.1, 3.282), dropping accents etc., and all other characters change to '_'
    ''' POSIX allows only latin letters, digits, dot, underscore and minus.
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToPOSIXportableFilename(ByVal basestring As String, Optional replacement As String = "_") As String
        Dim FKD As String = basestring.Normalize(Text.NormalizationForm.FormKD)
        Dim sRet As String = ""

        For Each cTmp As Char In FKD
            If "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789._-".Contains(cTmp) Then
                sRet &= cTmp
            ElseIf AscW(cTmp) >= &H300 AndAlso AscW(cTmp) < &H36F Then
                ' combining - skip
            Else
                ' nie wiadomo co, więc podmieniamy
                sRet &= replacement
            End If
        Next

        Return sRet
    End Function

    ''' <summary>
    ''' try to convert string to valid filename (very strict: POSIX portable filename, see IEEE 1003.1, 3.282), dropping accents etc., and all other characters change to '_'
    ''' POSIX allows only latin letters, digits, dot, underscore and minus.
    ''' <paramref name="useTransliteration">True, if first transliteration should be performed (string.Transliterate*ToLatin)</paramref>
    ''' <paramref name="replacement">String that would be used as a replacement for invalid characters</paramref>
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToPOSIXportableFilename(ByVal basestring As String, useTransliteration As Boolean, Optional replacement As String = "_") As String
        Dim FKD As String = basestring.Normalize(Text.NormalizationForm.FormKD)

        If useTransliteration Then
            FKD = basestring.TransliterateCyrilicToLatin
            FKD = basestring.TransliterateGreekToLatin
            FKD = basestring.Normalize(Text.NormalizationForm.FormKD)
            FKD = basestring.DropAccents
        End If

        Dim sRet As String = ""

        For Each cTmp As Char In FKD
            If "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789._-".Contains(cTmp) Then
                sRet &= cTmp
            ElseIf AscW(cTmp) >= &H300 AndAlso AscW(cTmp) < &H36F Then
                ' combining - skip
            Else
                ' nie wiadomo co, więc podmieniamy
                sRet &= replacement
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

    ''' <summary>
    ''' transliterate Cyrilic to Latin, according to ISO 9
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function TransliterateCyrilicToLatin(ByVal basestring As String) As String
        Dim sRet As String = basestring
        sRet = sRet.Replace("А", "A")
        sRet = sRet.Replace("Б", "B")
        sRet = sRet.Replace("В", "V")
        sRet = sRet.Replace("Г", "G")
        sRet = sRet.Replace("Д", "D")
        sRet = sRet.Replace("Е", "E")
        sRet = sRet.Replace("Ж", "Ž")
        sRet = sRet.Replace("З", "Z")
        sRet = sRet.Replace("И", "I")
        sRet = sRet.Replace("Й", "J")
        sRet = sRet.Replace("К", "K")
        sRet = sRet.Replace("Л", "L")
        sRet = sRet.Replace("М", "M")
        sRet = sRet.Replace("Н", "N")
        sRet = sRet.Replace("О", "O")
        sRet = sRet.Replace("П", "P")
        sRet = sRet.Replace("Р", "R")
        sRet = sRet.Replace("С", "S")
        sRet = sRet.Replace("Т", "T")
        sRet = sRet.Replace("У", "U")
        sRet = sRet.Replace("Ф", "F")
        sRet = sRet.Replace("Х", "H")
        sRet = sRet.Replace("Ц", "C")
        sRet = sRet.Replace("Ч", "Č")
        sRet = sRet.Replace("Ш", "Š")
        sRet = sRet.Replace("Щ", "Ŝ")
        sRet = sRet.Replace("Ъ", "''")
        sRet = sRet.Replace("Ы", "Y")
        sRet = sRet.Replace("Ь", "'")
        sRet = sRet.Replace("Э", "È")
        sRet = sRet.Replace("Ю", "Û")
        sRet = sRet.Replace("Я", "Â")

        sRet = sRet.Replace("а", "a")
        sRet = sRet.Replace("б", "b")
        sRet = sRet.Replace("в", "v")
        sRet = sRet.Replace("г", "g")
        sRet = sRet.Replace("д", "d")
        sRet = sRet.Replace("е", "e")
        sRet = sRet.Replace("ж", "ž")
        sRet = sRet.Replace("з", "z")
        sRet = sRet.Replace("и", "i")
        sRet = sRet.Replace("й", "j")
        sRet = sRet.Replace("к", "k")
        sRet = sRet.Replace("л", "l")
        sRet = sRet.Replace("м", "m")
        sRet = sRet.Replace("н", "n")
        sRet = sRet.Replace("о", "o")
        sRet = sRet.Replace("п", "p")
        sRet = sRet.Replace("р", "r")
        sRet = sRet.Replace("с", "s")
        sRet = sRet.Replace("т", "t")
        sRet = sRet.Replace("у", "u")
        sRet = sRet.Replace("ф", "f")
        sRet = sRet.Replace("х", "h")
        sRet = sRet.Replace("ц", "c")
        sRet = sRet.Replace("ш", "š")
        sRet = sRet.Replace("щ", "ŝ")
        sRet = sRet.Replace("ъ", "''")
        sRet = sRet.Replace("ы", "y")
        sRet = sRet.Replace("ь", "'")
        sRet = sRet.Replace("э", "è")
        sRet = sRet.Replace("ю", "û")
        sRet = sRet.Replace("я", "â")


        sRet = sRet.Replace("ђ", "đ")
        sRet = sRet.Replace("ѓ", "ǵ")
        sRet = sRet.Replace("ё", "ë")
        sRet = sRet.Replace("є", "ê")
        sRet = sRet.Replace("ѕ", "ẑ")
        sRet = sRet.Replace("і", "ì")
        sRet = sRet.Replace("ї", "ï")
        sRet = sRet.Replace("ј", "ǰ")
        sRet = sRet.Replace("љ", "l̂")
        sRet = sRet.Replace("њ", "n̂")
        sRet = sRet.Replace("ћ", "ć")
        sRet = sRet.Replace("ќ", "ḱ")
        sRet = sRet.Replace("ў", "ǔ")
        sRet = sRet.Replace("џ", "d̂")

        sRet = sRet.Replace("̈Ђ", "D̄")
        sRet = sRet.Replace("Ѓ", "Ǵ")
        sRet = sRet.Replace("Ё", "Ë")
        sRet = sRet.Replace("Є", "Ê")
        sRet = sRet.Replace("Ѕ", "Ẑ")
        sRet = sRet.Replace("І", "Ì")
        sRet = sRet.Replace("Ї", "Ï")
        sRet = sRet.Replace("J", "J̌")
        sRet = sRet.Replace("Љ", "L̂")
        sRet = sRet.Replace("Њ", "N̂")
        sRet = sRet.Replace("̈Ћ", "Ć")
        sRet = sRet.Replace("̈Ќ", "Ḱ")
        sRet = sRet.Replace("̈Ў", "Ǔ")
        sRet = sRet.Replace("̈Џ", "D̂")

        Return sRet
    End Function

    ''' <summary>
    ''' transliterate Greek to Latin, according to ISO 843
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function TransliterateGreekToLatin(ByVal basestring As String) As String
        Dim sRet As String = basestring

        sRet = sRet.Replace("Α", "A")
        sRet = sRet.Replace("Β", "V")
        sRet = sRet.Replace("Γ", "G")
        sRet = sRet.Replace("Δ", "D")
        sRet = sRet.Replace("Ε", "E")
        sRet = sRet.Replace("Ζ", "Z")
        sRet = sRet.Replace("Η", "ī")
        sRet = sRet.Replace("Θ", "TH")
        sRet = sRet.Replace("Ι", "I")
        sRet = sRet.Replace("Κ", "K")
        sRet = sRet.Replace("Λ", "L")
        sRet = sRet.Replace("Μ", "M")
        sRet = sRet.Replace("Ν", "N")
        sRet = sRet.Replace("Ξ", "X")
        sRet = sRet.Replace("Ο", "O")
        sRet = sRet.Replace("Π", "P")
        sRet = sRet.Replace("Ρ", "R")
        sRet = sRet.Replace("Σ", "S")
        sRet = sRet.Replace("Τ", "T")
        sRet = sRet.Replace("Υ", "U")
        sRet = sRet.Replace("Φ", "F")
        sRet = sRet.Replace("Χ", "CH")
        sRet = sRet.Replace("Ψ", "PS")
        sRet = sRet.Replace("Ω", "ō")


        sRet = sRet.Replace("α", "a")
        sRet = sRet.Replace("β", "v")
        sRet = sRet.Replace("γ", "g")
        sRet = sRet.Replace("δ", "d")
        sRet = sRet.Replace("ε", "e")
        sRet = sRet.Replace("ζ", "z")
        sRet = sRet.Replace("η", "ī")
        sRet = sRet.Replace("θ", "th")
        sRet = sRet.Replace("ι", "i")
        sRet = sRet.Replace("κ", "k")
        sRet = sRet.Replace("λ", "l")
        sRet = sRet.Replace("μ", "m")
        sRet = sRet.Replace("ν", "n")
        sRet = sRet.Replace("ξ", "x")
        sRet = sRet.Replace("ο", "o")
        sRet = sRet.Replace("π", "p")
        sRet = sRet.Replace("ρ", "r")
        sRet = sRet.Replace("ς", "s")
        sRet = sRet.Replace("σ", "s")
        sRet = sRet.Replace("τ", "t")
        sRet = sRet.Replace("υ", "u")
        sRet = sRet.Replace("φ", "f")
        sRet = sRet.Replace("ψ", "ch")
        sRet = sRet.Replace("χ", "ps")
        sRet = sRet.Replace("ω", "ō")

        Return sRet
    End Function


#End Region

#Region "Integer"

    ''' <summary>
    ''' as ToString, but uses space as thousands separator (only separator allowed in SI)
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToStringWithSpaces(ByVal value As Integer) As String
        Dim temp As Long = value
        Return temp.ToStringWithSpaces
    End Function

    ''' <summary>
    ''' as ToString, but uses SI form (space as thousands separator, if more than 9999)
    ''' </summary>
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

    ''' <summary>
    ''' Shortcut for taking max value (Math.Max(x,y))
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function Max(ByVal value As Integer, value1 As Integer) As Integer
        Return Math.Max(value, value1)
    End Function

    ''' <summary>
    ''' Shortcut for taking min value (Math.Min(x,y))
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function Min(ByVal value As Integer, value1 As Integer) As Integer
        Return Math.Min(value, value1)
    End Function

    ''' <summary>
    ''' checks if testVal is MinValue or MaxValue
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function IsMinOrMax(ByVal testVal As Integer) As Boolean
        If testVal = Integer.MinValue Then Return True
        If testVal = Integer.MaxValue Then Return True
        Return False
    End Function
#End Region

#Region "Long"

    ''' <summary>
    ''' return file len converted to string, using full name (byte/bytes, with binary prefixes: kibi, mebi, gibi, tebi, pebi and exbi
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function FileLen2string(ByVal bytes As Long) As String
        ' integer.max = 2 147 483 647, ≈ 2 GiB; long.max = 9,223,372,036,854,775,807 ≈ 9 EiB
        If bytes = 1 Then Return "1 byte"
        If bytes < 10000 Then Return bytes & " bytes"
        bytes \= 1024
        If bytes = 1 Then Return "1 kibibyte"
        If bytes < 2000 Then Return bytes & " kibibytes"
        bytes \= 1024
        If bytes = 1 Then Return "1 mebibyte"
        If bytes < 2000 Then Return bytes & " mebibytes"
        bytes \= 1024
        If bytes = 1 Then Return "1 gibibyte"
        If bytes < 2000 Then Return bytes & " gibibytes"

        ' now, only LONG
        bytes \= 1024
        If bytes = 1 Then Return "1 tebibyte"
        If bytes < 2000 Then Return bytes & " tebibytes"
        bytes \= 1024
        If bytes = 1 Then Return "1 pebibyte"
        If bytes < 2000 Then Return bytes & " pebibytes"
        bytes \= 1024
        If bytes = 1 Then Return "1 exbibyte"
        Return bytes & " exbibytes"

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

#Region "double"

    ''' <summary>
    ''' checks if testVal is MinValue or MaxValue
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function IsMinOrMax(ByVal testVal As Double) As Boolean
        If testVal = Double.MinValue Then Return True
        If testVal = Double.MaxValue Then Return True
        Return False
    End Function

    ''' <summary>
    ''' Shortcut for taking max value (Math.Max(x,y))
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function Max(ByVal value As Double, value1 As Double) As Double
        Return Math.Max(value, value1)
    End Function

    ''' <summary>
    ''' Shortcut for taking min value (Math.Min(x,y))
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function Min(ByVal value As Double, value1 As Double) As Double
        Return Math.Min(value, value1)
    End Function
#End Region

#Region "Ulong"
    ''' <summary>
    ''' convert ULong to shortest hex bytes string (e.g. "15", "01:2A", etc.), e.g. for Bluetooth MAC
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

    ''' <summary>
    ''' Convert seconds number to "XXd HH:MM:SS", integer.max = ~150 days
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToStringDHMS(ByVal czas As TimeSpan) As Integer
        Dim temp As Integer = Math.Round(czas.TotalSeconds)
        Return temp.ToStringDHMS
    End Function

    ''' <summary>
    ''' returns two letter abbreviation of weekday name (in Polish language): pn,wt,śr,cz,pt,sb,nd
    ''' <param name="bezOgonkow">True if should return "sr" and not "śr"</param>
    ''' </summary>
    <Runtime.CompilerServices.Extension>
    Public Function TwoLetterWeekDayPL(ByVal oDate As Date, Optional bezOgonkow As Boolean = False) As String
        Select Case oDate.DayOfWeek
            Case DayOfWeek.Monday
                Return "pn"
            Case DayOfWeek.Tuesday
                Return "wt"
            Case DayOfWeek.Wednesday
                Return If(bezOgonkow, "sr", "śr")
            Case DayOfWeek.Thursday
                Return "cz"
            Case DayOfWeek.Friday
                Return "pt"
            Case DayOfWeek.Saturday
                Return "sb"
            Case DayOfWeek.Sunday
                Return "nd"
        End Select

        Return ""
    End Function

    ''' <summary>
    ''' return date as string in Exif 2.3 format ("yyyy.MM.dd HH:mm:ss")
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToExifString(ByVal oDate As Date) As String
        Return oDate.ToString("yyyy.MM.dd HH:mm:ss")
    End Function

    ''' <summary>
    ''' returns 'bigger' date of two dates
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function Min(ByVal date1 As Date, date2 As Date) As Date
        If date1 < date2 Then Return date1
        Return date2
    End Function

    ''' <summary>
    ''' returns 'smaller' date of two dates
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function Max(ByVal date1 As Date, date2 As Date) As Date
        If date1 > date2 Then Return date1
        Return date2
    End Function

    ''' <summary>
    ''' checks if date is MinValue or MaxValue
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function IsMinOrMax(ByVal testDate As Date) As Boolean
        If testDate = Date.MinValue Then Return True
        If testDate = Date.MaxValue Then Return True
        Return False
    End Function

#End Region

#Region "Byte()"

    ''' <summary>
    ''' dump byte array as hex string ("0x12 0x45 ..."), max 16 bytes
    ''' </summary>
    ''' <param name="aArr"></param>
    ''' <param name="iSpaces"></param>
    ''' <returns></returns>
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
#End Region

#Region "stream"

    ''' <summary>
    ''' compare two streams. Warning: stream position would be changed!
    ''' </summary>
    ''' <returns>True if streams' content is the same</returns>
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

#End Region

#Region "GUID"
    ''' <summary>
    ''' For Bluetooth dumps: returns name of GATT descriptor name from its GUID, or ""
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function AsGattReservedDescriptorName(ByVal oGUID As Guid) As String
        Dim sGuid As String = oGUID.ToString
        Select Case sGuid
            Case "00002900-0000-1000-8000-00805f9b34fb"
                Return "Characteristic Extended Properties"
            Case "00002901-0000-1000-8000-00805f9b34fb"
                Return "Characteristic User Description"
            Case "00002902-0000-1000-8000-00805f9b34fb"
                Return "Client Characteristic Configuration"
            Case "00002903-0000-1000-8000-00805f9b34fb"
                Return "Server Characteristic Configuration"
            Case "00002904-0000-1000-8000-00805f9b34fb"
                Return "Characteristic Presentation Format"
            Case "00002905-0000-1000-8000-00805f9b34fb"
                Return "Characteristic Aggregate Format"
            Case "00002906-0000-1000-8000-00805f9b34fb"
                Return "Valid Range"
            Case "00002907-0000-1000-8000-00805f9b34fb"
                Return "External Report Reference"
            Case "00002908-0000-1000-8000-00805f9b34fb"
                Return "Report Reference"
            Case "00002909-0000-1000-8000-00805f9b34fb"
                Return "Number of Digitals"
            Case "0000290a-0000-1000-8000-00805f9b34fb"
                Return "Value Trigger Setting"
            Case "0000290b-0000-1000-8000-00805f9b34fb"
                Return "Environmental Sensing Configuration"
            Case "0000290c-0000-1000-8000-00805f9b34fb"
                Return "Environmental Sensing Measurement"
            Case "0000290d-0000-1000-8000-00805f9b34fb"
                Return "Environmental Sensing Trigger Setting"
            Case "0000290e-0000-1000-8000-00805f9b34fb"
                Return "Time Trigger Setting"
        End Select
        Return ""
    End Function

    ''' <summary>
    ''' For Bluetooth dumps: returns name of GATT service name from its GUID, or ""
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function AsGattReservedServiceName(ByVal oGUID As Guid) As String
        Dim sServ As String = oGUID.ToString

        Select Case sServ
            Case "00001800-0000-1000-8000-00805f9b34fb"
                Return "Generic Access"
            Case "00001801-0000-1000-8000-00805f9b34fb"
                Return "Generic Attribute"
            Case "00001802-0000-1000-8000-00805f9b34fb"
                Return "Immediate Alert"
            Case "00001803-0000-1000-8000-00805f9b34fb"
                Return "Link Loss"
            Case "00001804-0000-1000-8000-00805f9b34fb"
                Return "Tx Power"
            Case "00001805-0000-1000-8000-00805f9b34fb"
                Return "Current Time Service"
            Case "00001806-0000-1000-8000-00805f9b34fb"
                Return "Reference Time Update Service"
            Case "00001807-0000-1000-8000-00805f9b34fb"
                Return "Next DST Change Service"
            Case "00001808-0000-1000-8000-00805f9b34fb"
                Return "Glucose"
            Case "00001809-0000-1000-8000-00805f9b34fb"
                Return "Health Thermometer"
            Case "0000180a-0000-1000-8000-00805f9b34fb"
                Return "Device Information"
            Case "0000180d-0000-1000-8000-00805f9b34fb"
                Return "Heart Rate"
            Case "0000180e-0000-1000-8000-00805f9b34fb"
                Return "Phone Alert Status Service"
            Case "0000180f-0000-1000-8000-00805f9b34fb"
                Return "Battery Service"
            Case "00001810-0000-1000-8000-00805f9b34fb"
                Return "Blood Pressure"
            Case "00001811-0000-1000-8000-00805f9b34fb"
                Return "Alert Notification Service"
            Case "00001812-0000-1000-8000-00805f9b34fb"
                Return "Human Interface Device"
            Case "00001813-0000-1000-8000-00805f9b34fb"
                Return "Scan Parameters"
            Case "00001814-0000-1000-8000-00805f9b34fb"
                Return "Running Speed and Cadence"
            Case "00001815-0000-1000-8000-00805f9b34fb"
                Return "Automation IO"
            Case "00001816-0000-1000-8000-00805f9b34fb"
                Return "Cycling Speed and Cadence"
            Case "00001818-0000-1000-8000-00805f9b34fb"
                Return "Cycling Power"
            Case "00001819-0000-1000-8000-00805f9b34fb"
                Return "Location and Navigation"
            Case "0000181a-0000-1000-8000-00805f9b34fb"
                Return "Environmental Sensing"
            Case "0000181b-0000-1000-8000-00805f9b34fb"
                Return "Body Composition"
            Case "0000181c-0000-1000-8000-00805f9b34fb"
                Return "User Data"
            Case "0000181d-0000-1000-8000-00805f9b34fb"
                Return "Weight Scale"
            Case "0000181e-0000-1000-8000-00805f9b34fb"
                Return "Bond Management Service"
            Case "0000181f-0000-1000-8000-00805f9b34fb"
                Return "Continuous Glucose Monitoring"
            Case "00001820-0000-1000-8000-00805f9b34fb"
                Return "Internet Protocol Support Service"
            Case "00001821-0000-1000-8000-00805f9b34fb"
                Return "Indoor Positioning"
            Case "00001822-0000-1000-8000-00805f9b34fb"
                Return "Pulse Oximeter Service"
            Case "00001823-0000-1000-8000-00805f9b34fb"
                Return "HTTP Proxy"
            Case "00001824-0000-1000-8000-00805f9b34fb"
                Return "Transport Discovery"
            Case "00001825-0000-1000-8000-00805f9b34fb"
                Return "Object Transfer Service"
            Case "00001826-0000-1000-8000-00805f9b34fb"
                Return "Fitness Machine"
            Case "00001827-0000-1000-8000-00805f9b34fb"
                Return "Mesh Provisioning Service"
            Case "00001828-0000-1000-8000-00805f9b34fb"
                Return "Mesh Proxy Service"
            Case "00001829-0000-1000-8000-00805f9b34fb"
                Return "Reconnection Configuration"
        End Select

        Return ""
    End Function

    ''' <summary>
    ''' For Bluetooth dumps: returns name of GATT characteristic name from its GUID, or ""
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function AsGattReservedCharacteristicName(ByVal oGUID As Guid) As String
        Dim sChar As String = oGUID.ToString

        Select Case sChar
            Case "00002a00-0000-1000-8000-00805f9b34fb"
                Return "Device SourceName"
            Case "00002a01-0000-1000-8000-00805f9b34fb"
                Return "Appearance"
            Case "00002a02-0000-1000-8000-00805f9b34fb"
                Return "Peripheral Privacy Flag"
            Case "00002a03-0000-1000-8000-00805f9b34fb"
                Return "Reconnection Address"
            Case "00002a04-0000-1000-8000-00805f9b34fb"
                Return "Peripheral Preferred Connection Parameters"
            Case "00002a05-0000-1000-8000-00805f9b34fb"
                Return "Service Changed"
            Case "00002a06-0000-1000-8000-00805f9b34fb"
                Return "Alert Level"
            Case "00002a07-0000-1000-8000-00805f9b34fb"
                Return "Tx Power Level"
            Case "00002a08-0000-1000-8000-00805f9b34fb"
                Return "Date Time"
            Case "00002a09-0000-1000-8000-00805f9b34fb"
                Return "AutoWeatherDay of Week"
            Case "00002a0a-0000-1000-8000-00805f9b34fb"
                Return "AutoWeatherDay Date Time"
            Case "00002a0b-0000-1000-8000-00805f9b34fb"
                Return "Exact Time 100"
            Case "00002a0c-0000-1000-8000-00805f9b34fb"
                Return "Exact Time 256"
            Case "00002a0d-0000-1000-8000-00805f9b34fb"
                Return "DST Offset"
            Case "00002a0e-0000-1000-8000-00805f9b34fb"
                Return "Time Zone"
            Case "00002a0f-0000-1000-8000-00805f9b34fb"
                Return "Local Time Information"
            Case "00002a10-0000-1000-8000-00805f9b34fb"
                Return "Secondary Time Zone"
            Case "00002a11-0000-1000-8000-00805f9b34fb"
                Return "Time with DST"
            Case "00002a12-0000-1000-8000-00805f9b34fb"
                Return "Time Accuracy"
            Case "00002a13-0000-1000-8000-00805f9b34fb"
                Return "Time Source"
            Case "00002a14-0000-1000-8000-00805f9b34fb"
                Return "Reference Time Information"
            Case "00002a15-0000-1000-8000-00805f9b34fb"
                Return "Time Broadcast"
            Case "00002a16-0000-1000-8000-00805f9b34fb"
                Return "Time Update Control Point"
            Case "00002a17-0000-1000-8000-00805f9b34fb"
                Return "Time Update State"
            Case "00002a18-0000-1000-8000-00805f9b34fb"
                Return "Glucose Measurement"
            Case "00002a19-0000-1000-8000-00805f9b34fb"
                Return "Battery Level"
            Case "00002a1a-0000-1000-8000-00805f9b34fb"
                Return "Battery Power State"
            Case "00002a1b-0000-1000-8000-00805f9b34fb"
                Return "Battery Level State"
            Case "00002a1c-0000-1000-8000-00805f9b34fb"
                Return "Temperature Measurement"
            Case "00002a1d-0000-1000-8000-00805f9b34fb"
                Return "Temperature Type"
            Case "00002a1e-0000-1000-8000-00805f9b34fb"
                Return "Intermediate Temperature"
            Case "00002a1f-0000-1000-8000-00805f9b34fb"
                Return "Temperature Celsius"
            Case "00002a20-0000-1000-8000-00805f9b34fb"
                Return "Temperature Fahrenheit"
            Case "00002a21-0000-1000-8000-00805f9b34fb"
                Return "Measurement Interval"
            Case "00002a22-0000-1000-8000-00805f9b34fb"
                Return "Boot Keyboard Input Report"
            Case "00002a23-0000-1000-8000-00805f9b34fb"
                Return "System ID"
            Case "00002a24-0000-1000-8000-00805f9b34fb"
                Return "Model Number String"
            Case "00002a25-0000-1000-8000-00805f9b34fb"
                Return "Serial Number String"
            Case "00002a26-0000-1000-8000-00805f9b34fb"
                Return "Firmware Revision String"
            Case "00002a27-0000-1000-8000-00805f9b34fb"
                Return "Hardware Revision String"
            Case "00002a28-0000-1000-8000-00805f9b34fb"
                Return "Software Revision String"
            Case "00002a29-0000-1000-8000-00805f9b34fb"
                Return "Manufacturer SourceName String"
            Case "00002a2a-0000-1000-8000-00805f9b34fb"
                Return "IEEE 11073-20601 Regulatory Certification Data List"
            Case "00002a2b-0000-1000-8000-00805f9b34fb"
                Return "Current Time"
            Case "00002a2c-0000-1000-8000-00805f9b34fb"
                Return "Magnetic Declination"
            Case "00002a2f-0000-1000-8000-00805f9b34fb"
                Return "Position 2D"
            Case "00002a30-0000-1000-8000-00805f9b34fb"
                Return "Position 3D"
            Case "00002a31-0000-1000-8000-00805f9b34fb"
                Return "Scan Refresh"
            Case "00002a32-0000-1000-8000-00805f9b34fb"
                Return "Boot Keyboard Output Report"
            Case "00002a33-0000-1000-8000-00805f9b34fb"
                Return "Boot Mouse Input Report"
            Case "00002a34-0000-1000-8000-00805f9b34fb"
                Return "Glucose Measurement Context"
            Case "00002a35-0000-1000-8000-00805f9b34fb"
                Return "Blood Pressure Measurement"
            Case "00002a36-0000-1000-8000-00805f9b34fb"
                Return "Intermediate Cuff Pressure"
            Case "00002a37-0000-1000-8000-00805f9b34fb"
                Return "Heart Rate Measurement"
            Case "00002a38-0000-1000-8000-00805f9b34fb"
                Return "Body Sensor Location"
            Case "00002a39-0000-1000-8000-00805f9b34fb"
                Return "Heart Rate Control Point"
            Case "00002a3a-0000-1000-8000-00805f9b34fb"
                Return "Removable"
            Case "00002a3b-0000-1000-8000-00805f9b34fb"
                Return "Service Required"
            Case "00002a3c-0000-1000-8000-00805f9b34fb"
                Return "Scientific Temperature Celsius"
            Case "00002a3d-0000-1000-8000-00805f9b34fb"
                Return "String"
            Case "00002a3e-0000-1000-8000-00805f9b34fb"
                Return "Network Availability"
            Case "00002a3f-0000-1000-8000-00805f9b34fb"
                Return "Alert Status"
            Case "00002a40-0000-1000-8000-00805f9b34fb"
                Return "Ringer Control point"
            Case "00002a41-0000-1000-8000-00805f9b34fb"
                Return "Ringer Setting"
            Case "00002a42-0000-1000-8000-00805f9b34fb"
                Return "Alert Category ID Bit Mask"
            Case "00002a43-0000-1000-8000-00805f9b34fb"
                Return "Alert Category ID"
            Case "00002a44-0000-1000-8000-00805f9b34fb"
                Return "Alert Notification Control Point"
            Case "00002a45-0000-1000-8000-00805f9b34fb"
                Return "Unread Alert Status"
            Case "00002a46-0000-1000-8000-00805f9b34fb"
                Return "New Alert"
            Case "00002a47-0000-1000-8000-00805f9b34fb"
                Return "Supported New Alert Category"
            Case "00002a48-0000-1000-8000-00805f9b34fb"
                Return "Supported Unread Alert Category"
            Case "00002a49-0000-1000-8000-00805f9b34fb"
                Return "Blood Pressure Feature"
            Case "00002a4a-0000-1000-8000-00805f9b34fb"
                Return "HID Information"
            Case "00002a4b-0000-1000-8000-00805f9b34fb"
                Return "Report Map"
            Case "00002a4c-0000-1000-8000-00805f9b34fb"
                Return "HID Control Point"
            Case "00002a4d-0000-1000-8000-00805f9b34fb"
                Return "Report"
            Case "00002a4e-0000-1000-8000-00805f9b34fb"
                Return "Protocol Mode"
            Case "00002a4f-0000-1000-8000-00805f9b34fb"
                Return "Scan Interval Window"
            Case "00002a50-0000-1000-8000-00805f9b34fb"
                Return "PnP ID"
            Case "00002a51-0000-1000-8000-00805f9b34fb"
                Return "Glucose Feature"
            Case "00002a52-0000-1000-8000-00805f9b34fb"
                Return "Record Access Control Point"
            Case "00002a53-0000-1000-8000-00805f9b34fb"
                Return "RSC Measurement"
            Case "00002a54-0000-1000-8000-00805f9b34fb"
                Return "RSC Feature"
            Case "00002a55-0000-1000-8000-00805f9b34fb"
                Return "SC Control Point"
            Case "00002a56-0000-1000-8000-00805f9b34fb"
                Return "Digital"
            Case "00002a57-0000-1000-8000-00805f9b34fb"
                Return "Digital Output"
            Case "00002a58-0000-1000-8000-00805f9b34fb"
                Return "Analog"
            Case "00002a59-0000-1000-8000-00805f9b34fb"
                Return "Analog Output"
            Case "00002a5a-0000-1000-8000-00805f9b34fb"
                Return "Aggregate"
            Case "00002a5b-0000-1000-8000-00805f9b34fb"
                Return "CSC Measurement"
            Case "00002a5c-0000-1000-8000-00805f9b34fb"
                Return "CSC Feature"
            Case "00002a5d-0000-1000-8000-00805f9b34fb"
                Return "Sensor Location"
            Case "00002a5e-0000-1000-8000-00805f9b34fb"
                Return "PLX Spot-Check Measurement"
            Case "00002a5f-0000-1000-8000-00805f9b34fb"
                Return "PLX Continuous Measurement Characteristic"
            Case "00002a60-0000-1000-8000-00805f9b34fb"
                Return "PLX Features"
            Case "00002a62-0000-1000-8000-00805f9b34fb"
                Return "Pulse Oximetry Control Point"
            Case "00002a63-0000-1000-8000-00805f9b34fb"
                Return "Cycling Power Measurement"
            Case "00002a64-0000-1000-8000-00805f9b34fb"
                Return "Cycling Power Vector"
            Case "00002a65-0000-1000-8000-00805f9b34fb"
                Return "Cycling Power Feature"
            Case "00002a66-0000-1000-8000-00805f9b34fb"
                Return "Cycling Power Control Point"
            Case "00002a67-0000-1000-8000-00805f9b34fb"
                Return "Location and Speed Characteristic"
            Case "00002a68-0000-1000-8000-00805f9b34fb"
                Return "Navigation"
            Case "00002a69-0000-1000-8000-00805f9b34fb"
                Return "Position Quality"
            Case "00002a6a-0000-1000-8000-00805f9b34fb"
                Return "LN Feature"
            Case "00002a6b-0000-1000-8000-00805f9b34fb"
                Return "LN Control Point"
            Case "00002a6c-0000-1000-8000-00805f9b34fb"
                Return "Elevation"
            Case "00002a6d-0000-1000-8000-00805f9b34fb"
                Return "Pressure"
            Case "00002a6e-0000-1000-8000-00805f9b34fb"
                Return "Temperature"
            Case "00002a6f-0000-1000-8000-00805f9b34fb"
                Return "Humidity"
            Case "00002a70-0000-1000-8000-00805f9b34fb"
                Return "True Wind Speed"
            Case "00002a71-0000-1000-8000-00805f9b34fb"
                Return "True Wind Direction"
            Case "00002a72-0000-1000-8000-00805f9b34fb"
                Return "Apparent Wind Speed"
            Case "00002a73-0000-1000-8000-00805f9b34fb"
                Return "Apparent Wind Direction"
            Case "00002a74-0000-1000-8000-00805f9b34fb"
                Return "Gust Factor"
            Case "00002a75-0000-1000-8000-00805f9b34fb"
                Return "Pollen Concentration"
            Case "00002a76-0000-1000-8000-00805f9b34fb"
                Return "UV Index"
            Case "00002a77-0000-1000-8000-00805f9b34fb"
                Return "Irradiance"
            Case "00002a78-0000-1000-8000-00805f9b34fb"
                Return "Rainfall"
            Case "00002a79-0000-1000-8000-00805f9b34fb"
                Return "Wind Chill"
            Case "00002a7a-0000-1000-8000-00805f9b34fb"
                Return "Heat Index"
            Case "00002a7b-0000-1000-8000-00805f9b34fb"
                Return "Dew Point"
            Case "00002a7d-0000-1000-8000-00805f9b34fb"
                Return "Descriptor Value Changed"
            Case "00002a7e-0000-1000-8000-00805f9b34fb"
                Return "Aerobic Heart Rate Lower Limit"
            Case "00002a7f-0000-1000-8000-00805f9b34fb"
                Return "Aerobic Threshold"
            Case "00002a80-0000-1000-8000-00805f9b34fb"
                Return "Age"
            Case "00002a81-0000-1000-8000-00805f9b34fb"
                Return "Anaerobic Heart Rate Lower Limit"
            Case "00002a82-0000-1000-8000-00805f9b34fb"
                Return "Anaerobic Heart Rate Upper Limit"
            Case "00002a83-0000-1000-8000-00805f9b34fb"
                Return "Anaerobic Threshold"
            Case "00002a84-0000-1000-8000-00805f9b34fb"
                Return "Aerobic Heart Rate Upper Limit"
            Case "00002a85-0000-1000-8000-00805f9b34fb"
                Return "Date of Birth"
            Case "00002a86-0000-1000-8000-00805f9b34fb"
                Return "Date of Threshold Assessment"
            Case "00002a87-0000-1000-8000-00805f9b34fb"
                Return "Email Address"
            Case "00002a88-0000-1000-8000-00805f9b34fb"
                Return "Fat Burn Heart Rate Lower Limit"
            Case "00002a89-0000-1000-8000-00805f9b34fb"
                Return "Fat Burn Heart Rate Upper Limit"
            Case "00002a8a-0000-1000-8000-00805f9b34fb"
                Return "First SourceName"
            Case "00002a8b-0000-1000-8000-00805f9b34fb"
                Return "Five Zone Heart Rate Limits"
            Case "00002a8c-0000-1000-8000-00805f9b34fb"
                Return "Gender"
            Case "00002a8d-0000-1000-8000-00805f9b34fb"
                Return "Heart Rate Max"
            Case "00002a8e-0000-1000-8000-00805f9b34fb"
                Return "Height"
            Case "00002a8f-0000-1000-8000-00805f9b34fb"
                Return "Hip Circumference"
            Case "00002a90-0000-1000-8000-00805f9b34fb"
                Return "Last SourceName"
            Case "00002a91-0000-1000-8000-00805f9b34fb"
                Return "Maximum Recommended Heart Rate"
            Case "00002a92-0000-1000-8000-00805f9b34fb"
                Return "Resting Heart Rate"
            Case "00002a93-0000-1000-8000-00805f9b34fb"
                Return "Sport Type for Aerobic and Anaerobic Thresholds"
            Case "00002a94-0000-1000-8000-00805f9b34fb"
                Return "Three Zone Heart Rate Limits"
            Case "00002a95-0000-1000-8000-00805f9b34fb"
                Return "Two Zone Heart Rate Limit"
            Case "00002a96-0000-1000-8000-00805f9b34fb"
                Return "VO2 Max"
            Case "00002a97-0000-1000-8000-00805f9b34fb"
                Return "Waist Circumference"
            Case "00002a98-0000-1000-8000-00805f9b34fb"
                Return "Weight"
            Case "00002a99-0000-1000-8000-00805f9b34fb"
                Return "Database Change Increment"
            Case "00002a9a-0000-1000-8000-00805f9b34fb"
                Return "User Index"
            Case "00002a9b-0000-1000-8000-00805f9b34fb"
                Return "Body Composition Feature"
            Case "00002a9c-0000-1000-8000-00805f9b34fb"
                Return "Body Composition Measurement"
            Case "00002a9d-0000-1000-8000-00805f9b34fb"
                Return "Weight Measurement"
            Case "00002a9e-0000-1000-8000-00805f9b34fb"
                Return "Weight Scale Feature"
            Case "00002a9f-0000-1000-8000-00805f9b34fb"
                Return "User Control Point"
            Case "00002aa0-0000-1000-8000-00805f9b34fb"
                Return "Magnetic Flux Density - 2D"
            Case "00002aa1-0000-1000-8000-00805f9b34fb"
                Return "Magnetic Flux Density - 3D"
            Case "00002aa2-0000-1000-8000-00805f9b34fb"
                Return "Language"
            Case "00002aa3-0000-1000-8000-00805f9b34fb"
                Return "Barometric Pressure Trend"
            Case "00002aa4-0000-1000-8000-00805f9b34fb"
                Return "Bond Management Control Point"
            Case "00002aa5-0000-1000-8000-00805f9b34fb"
                Return "Bond Management Features"
            Case "00002aa6-0000-1000-8000-00805f9b34fb"
                Return "Central Address Resolution"
            Case "00002aa7-0000-1000-8000-00805f9b34fb"
                Return "CGM Measurement"
            Case "00002aa8-0000-1000-8000-00805f9b34fb"
                Return "CGM Feature"
            Case "00002aa9-0000-1000-8000-00805f9b34fb"
                Return "CGM Status"
            Case "00002aaa-0000-1000-8000-00805f9b34fb"
                Return "CGM Session Start Time"
            Case "00002aab-0000-1000-8000-00805f9b34fb"
                Return "CGM Session Run Time"
            Case "00002aac-0000-1000-8000-00805f9b34fb"
                Return "CGM Specific Ops Control Point"
            Case "00002aad-0000-1000-8000-00805f9b34fb"
                Return "Indoor Positioning Configuration"
            Case "00002aae-0000-1000-8000-00805f9b34fb"
                Return "Latitude"
            Case "00002aaf-0000-1000-8000-00805f9b34fb"
                Return "Longitude"
            Case "00002ab0-0000-1000-8000-00805f9b34fb"
                Return "Local North Coordinate"
            Case "00002ab1-0000-1000-8000-00805f9b34fb"
                Return "Local East Coordinate"
            Case "00002ab2-0000-1000-8000-00805f9b34fb"
                Return "Floor Number"
            Case "00002ab3-0000-1000-8000-00805f9b34fb"
                Return "Altitude"
            Case "00002ab4-0000-1000-8000-00805f9b34fb"
                Return "Uncertainty"
            Case "00002ab5-0000-1000-8000-00805f9b34fb"
                Return "Location SourceName"
            Case "00002ab6-0000-1000-8000-00805f9b34fb"
                Return "URI"
            Case "00002ab7-0000-1000-8000-00805f9b34fb"
                Return "HTTP Headers"
            Case "00002ab8-0000-1000-8000-00805f9b34fb"
                Return "HTTP Status Code"
            Case "00002ab9-0000-1000-8000-00805f9b34fb"
                Return "HTTP Entity Body"
            Case "00002aba-0000-1000-8000-00805f9b34fb"
                Return "HTTP Control Point"
            Case "00002abb-0000-1000-8000-00805f9b34fb"
                Return "HTTPS Security"
            Case "00002abc-0000-1000-8000-00805f9b34fb"
                Return "TDS Control Point"
            Case "00002abd-0000-1000-8000-00805f9b34fb"
                Return "OTS Feature"
            Case "00002abe-0000-1000-8000-00805f9b34fb"
                Return "Object SourceName"
            Case "00002abf-0000-1000-8000-00805f9b34fb"
                Return "Object Type"
            Case "00002ac0-0000-1000-8000-00805f9b34fb"
                Return "Object Size"
            Case "00002ac1-0000-1000-8000-00805f9b34fb"
                Return "Object First-Created"
            Case "00002ac2-0000-1000-8000-00805f9b34fb"
                Return "Object Last-Modified"
            Case "00002ac3-0000-1000-8000-00805f9b34fb"
                Return "Object ID"
            Case "00002ac4-0000-1000-8000-00805f9b34fb"
                Return "Object Properties"
            Case "00002ac5-0000-1000-8000-00805f9b34fb"
                Return "Object Action Control Point"
            Case "00002ac6-0000-1000-8000-00805f9b34fb"
                Return "Object List Control Point"
            Case "00002ac7-0000-1000-8000-00805f9b34fb"
                Return "Object List Filter"
            Case "00002ac8-0000-1000-8000-00805f9b34fb"
                Return "Object Changed"
            Case "00002ac9-0000-1000-8000-00805f9b34fb"
                Return "Resolvable Private Address Only"
            Case "00002acc-0000-1000-8000-00805f9b34fb"
                Return "Fitness Machine Feature"
            Case "00002acd-0000-1000-8000-00805f9b34fb"
                Return "Treadmill Data"
            Case "00002ace-0000-1000-8000-00805f9b34fb"
                Return "Cross Trainer Data"
            Case "00002acf-0000-1000-8000-00805f9b34fb"
                Return "Step Climber Data"
            Case "00002ad0-0000-1000-8000-00805f9b34fb"
                Return "Stair Climber Data"
            Case "00002ad1-0000-1000-8000-00805f9b34fb"
                Return "Rower Data"
            Case "00002ad2-0000-1000-8000-00805f9b34fb"
                Return "Indoor Bike Data"
            Case "00002ad3-0000-1000-8000-00805f9b34fb"
                Return "Training Status"
            Case "00002ad4-0000-1000-8000-00805f9b34fb"
                Return "Supported Speed Range"
            Case "00002ad5-0000-1000-8000-00805f9b34fb"
                Return "Supported Inclination Range"
            Case "00002ad6-0000-1000-8000-00805f9b34fb"
                Return "Supported Resistance Level Range"
            Case "00002ad7-0000-1000-8000-00805f9b34fb"
                Return "Supported Heart Rate Range"
            Case "00002ad8-0000-1000-8000-00805f9b34fb"
                Return "Supported Power Range"
            Case "00002ad9-0000-1000-8000-00805f9b34fb"
                Return "Fitness Machine Control Point"
            Case "00002ada-0000-1000-8000-00805f9b34fb"
                Return "Fitness Machine Status"
            Case "00002aed-0000-1000-8000-00805f9b34fb"
                Return "Date UTC"
            Case "00002b1d-0000-1000-8000-00805f9b34fb"
                Return "RC Feature"
            Case "00002b1e-0000-1000-8000-00805f9b34fb"
                Return "RC Settings"
            Case "00002b1f-0000-1000-8000-00805f9b34fb"
                Return "Reconnection Configuration Control Point"
        End Select

        Return ""
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

End Module
