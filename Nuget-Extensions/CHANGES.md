
# version x.2.7
* string.Not*
* string.Parse*
* string.CountChar

# version x.2.6
* double.Equals(double, epsilon)

# version x.2.5
* (int|long).(Abs|Sign)
* Double.(Abs|Sign|Floor|Round|Ceiling)

# version x.2.4
* corrected TimeSpan.ToStringDHMS

# version x.2.3
* corrected tooltips/intellisense for date.Min and date.Max
* added: string.IsLowerInvariant, IsUpperInvariant
* added: string.Contains/EndsWith/StartsWith/Equals CSAS, CIAS, CSAI, CIAI

# version x.2.2
* added: double.Max, double.Min, double.IsMinOrMax
* added: integer.Max, integer.Min, integer.IsMinOrMax
* added: date.Min, date.Max, date.IsMinOrMax
* added: string.CommonPrefixLen, string.CommonPrefix

# version x.2.1
* added:
* String.TransliterateCyrilicToLatin, TransliterateGreekToLatin
* String.ToPOSIXportableFilename(useTransliterate, optional replacement)
* String.NotContains

# version 2.0.0
* version for .Net Standard 2.0
* String.DropAccents
* String.ToPOSIXportableFilename(optional replacement as string = "_")

# version 1.1.3
* correction in DePascal
* TwoLetterWeekDayPL(True), if you want "sr" and not "śr"
* Bluetooth GUID debugs: AsGattReservedDescriptorName, AsGattReservedServiceName, AsGattReservedCharacteristicName

# version 1.1.0
* added two functions:
 * TwoLetterWeekDayPL(Date)
 * ToExifString(Date)
* added XMLdoc for TimeSpan.ToStringDHMS(), Byte().ToDebugString(), Stream.IsSameStreamContent(Stream)


# version 1.0.0
 initial version