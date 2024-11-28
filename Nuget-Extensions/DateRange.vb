
#If False

' jeszcze operator < i > z DateTime (przed/po)


Public Class DateRange

    ''' <summary>
    ''' Lower bound of DateRange
    ''' </summary>
    Public Property Min As Date
    ''' <summary>
    ''' Upper bound of DateRange
    ''' </summary>
    Public Property Max As Date


#Region "Valid date range"

    ''' <summary>
    ''' Bounds of valid DateRange; default is DefaultMin (1700.01.01)
    ''' </summary>
    Private Shared Property ValidRange As New DateRange(New Date(1700, 1, 1), Date.Now.AddMonths(1))

    Public Shared Sub SetValidRange(minValid As Date, maxValid As Date)
        ValidRange = New DateRange(minValid, maxValid)
    End Sub

    Public Shared Function GetValidRange()
        Return ValidRange
    End Function

#End Region

    Public Sub New(minval As Date, maxval As Date)
        Min = minval
        Max = maxval
    End Sub


    ''' <summary>
    ''' check if given testDate is inside Valid range
    ''' </summary>
    Public Function IsDateValid(testDate As Date) As Boolean
        If testDate < ValidRange.Min Then Return False
        If testDate > ValidRange.Max Then Return False
        Return True
    End Function

    ''' <summary>
    ''' check if DateRange is Empty (i.e. Max is less than Min)
    ''' </summary>
    ''' <returns></returns>
    Public Function IsEmpty() As Boolean
        Return Min <= Max
    End Function


    ''' <summary>
    ''' if newmin is Valid, then adjust Min to min(Min, newmin)
    ''' </summary>
    Public Sub AdjustMin(newmin As Date)
        If Not IsDateValid(newmin) Then Return
        If newmin > Min Then Return
        Min = newmin
    End Sub

    ''' <summary>
    ''' if newmax is Valid, then adjust Max to max(Max, newmax)
    ''' </summary>
    Public Sub AdjustMax(newmax As Date)
        If Not IsDateValid(newmax) Then Return
        If newmax < Max Then Return
        Max = newmax
    End Sub

    ''' <summary>
    ''' if newmin is Valid, then adjust Min to min(Min, newmin);
    ''' if newmax is Valid, then adjust Max to max(Max, newmax);
    ''' same as AdjustMin(newmin); AdjustMax(newmax)
    ''' </summary>
    Public Sub AdjustMinMax(newmin As Date, newmax As Date)
        AdjustMax(newmax)
        AdjustMin(newmin)
    End Sub


    ''' <summary>
    ''' checks if testDate is inside DateRange
    ''' </summary>
    ''' <returns>True only if testDate is valid, and is between Min and Max</returns>
    Public Function Contains(testDate As Date) As Boolean
        If Not IsDateValid(testDate) Then Return False
        If IsDateValid(Min) AndAlso testDate < Min Then Return False
        If IsDateValid(Max) AndAlso testDate > Max Then Return False
        Return True
    End Function

    ''' <summary>
    ''' checks if current range is inside secondRange (secondRange overlaps current range)
    ''' </summary>
    Public Function IsInsideRange(testRange As DateRange) As Boolean
        If Not testRange.Contains(Min) Then Return False
        If Not testRange.Contains(Max) Then Return False
        Return True
    End Function

    ''' <summary>
    ''' return range that is intersection of current range and secondRange; using Valid range 
    ''' </summary>
    Public Function Intersection(secondRange As DateRange) As DateRange

        Dim newmin As Date
        If IsDateValid(Min) AndAlso IsDateValid(secondRange.Min) Then
            newmin = Min.Max(secondRange.Min)
        ElseIf IsDateValid(Min) Then
            newmin = Min
        ElseIf IsDateValid(secondRange.Min) Then
            newmin = secondRange.Min
        Else
            newmin = Date.MinValue
        End If

        Dim newmax As Date
        If IsDateValid(Max) AndAlso IsDateValid(secondRange.Max) Then
            newmax = Max.Min(secondRange.Max)
        ElseIf IsDateValid(Max) Then
            newmax = Max
        ElseIf IsDateValid(secondRange.Max) Then
            newmax = secondRange.Max
        Else
            newmax = Date.MaxValue
        End If

        If IsDateValid(newmin) AndAlso IsDateValid(newmax) Then
            If newmin > newmax Then Return New DateRange(Date.MaxValue, Date.MinValue)
        End If

        Return New DateRange(newmin, newmax)

    End Function

    ''' <summary>
    ''' calculate range that is intersection of both ranges
    ''' </summary>
    Public Shared Operator *(ByVal dr1 As DateRange, ByVal dr2 As DateRange) As DateRange
        Return dr1.Intersection(dr2)
    End Operator


    ''' <summary>
    ''' checks if current range and secondRange has intersection; all dates are treated as valid
    ''' </summary>
    Public Function HasIntersection(testRange As DateRange) As Boolean
        Dim rng As DateRange = Me * testRange
        If IsDateValid(rng.Min) AndAlso IsDateValid(rng.Max) Then
            Return rng.Min < rng.Max
        End If
        If testRange.Max < Min Then Return False
        If testRange.Min > Max Then Return False
        Return True
    End Function


    ''' <summary>
    ''' return range that is union of current range and secondRange (and everything between these two ranges); using Valid range 
    ''' </summary>
    Public Function Union(secondRange As DateRange) As DateRange

        Dim newmin As Date
        If IsDateValid(Min) AndAlso IsDateValid(secondRange.Min) Then
            newmin = Min.Min(secondRange.Min)
        ElseIf IsDateValid(Min) Then
            newmin = Min
        ElseIf IsDateValid(secondRange.Min) Then
            newmin = secondRange.Min
        Else
            newmin = Date.MinValue
        End If

        Dim newmax As Date
        If IsDateValid(Max) AndAlso IsDateValid(secondRange.Max) Then
            newmax = Max.Max(secondRange.Max)
        ElseIf IsDateValid(Max) Then
            newmax = Max
        ElseIf IsDateValid(secondRange.Max) Then
            newmax = secondRange.Max
        Else
            newmax = Date.MaxValue
        End If

        Return New DateRange(newmin, newmax)

    End Function

    ''' <summary>
    ''' calculate range that is union of both ranges (and everything between these two ranges)
    ''' </summary>
    Public Shared Operator +(ByVal dr1 As DateRange, ByVal dr2 As DateRange) As DateRange
        Return dr1.Union(dr2)
    End Operator



    ''' <summary>
    ''' return date that is half-between Min and Max, assumes that Min and Max dates are valid
    ''' </summary>
    Public Function MidDate() As Date
        Dim oDateDiff As TimeSpan = Max - Min
        Return Min.AddMinutes(oDateDiff.TotalMinutes)
    End Function

    ''' <summary>
    ''' return date in string as long as common characters from Min and Max
    ''' </summary>
    Public Function ToStringCommon(Optional format As String = "yyyy.MM.dd") As String
        Dim strMin As String = Min.ToString(format)
        Dim strMax As String = Max.ToString(format)

        Dim iLp As Integer
        For iLp = Math.Min(strMin.Length, strMax.Length) - 1 To 0 Step -1
            If strMin(iLp) = strMax(iLp) Then Exit For
        Next

        Return strMin.Substring(0, iLp)
    End Function

    ''' <summary>
    ''' return "Min - Max"
    ''' </summary>
    Public Function ToStringRange(Optional format As String = "yyyy.MM.dd", Optional separator As String = " - ") As String
        Dim strMin As String = Min.ToString(format)
        Dim strMax As String = Max.ToString(format)
        Return strMin & separator & strMax
    End Function

End Class

#End If

#If False Then

 na razie zablokowane, bo 
 * właściwie powinny być dwa New(), a to oznacza że JSON powinien mieć tu swoje atrybuty, czyli powinno używać Nugetu JSON
 * dobrze by było użyć mojego BaseStruct
 * najpierw trzeba przetestować :)

Public Class DateRange

    ''' <summary>
    ''' Lower bound of DateRange; default: Date.MinValue
    ''' </summary>
    Public Property Min As Date = DefaultMin
    ''' <summary>
    ''' Upper bound of DateRange; default: Date.MaxValue
    ''' </summary>
    Public Property Max As Date = DefaultMax

    ''' <summary>
    ''' Lower bound of valid DateRange; default is DefaultMin (1700.01.01)
    ''' </summary>
    Public Property MinValid As Date = DefaultMinValid
    ''' <summary>
    ''' Upper bound of valid DateRange; default is DefaultMax (current date + 1 day)
    ''' </summary>
    Public Property MaxValid As Date = DefaultMaxValid

    ''' <summary>
    ''' Default to be used for lower bound of DateRange; default is  DefaultMinValid (Date.MinValue)
    ''' </summary>
    Public Shared Property DefaultMin As Date = Date.MinValue
    ''' <summary>
    ''' Default to be used for upper bound of DateRange; default is DefaultMaxValid (Date.MaxValue)
    ''' </summary>
    Public Shared Property DefaultMax As Date = Date.MaxValue

    ''' <summary>
    ''' Default to be used for lower bound of valid DateRange; default: 1700.01.01
    ''' </summary>
    Public Shared Property DefaultMinValid As Date = New Date(1700, 1, 1)
    ''' <summary>
    ''' Default to be used for upper bound of valid DateRange; default: current date + 1 day
    ''' </summary>
    Public Shared Property DefaultMaxValid As Date = Date.Now.AddDays(1)

    'Public Sub New(minval As Date, maxval As Date)
    '    Min = minval
    '    Max = maxval
    '    MinValid = DefaultMinValid
    '    MaxValid = DefaultMaxValid
    'End Sub

    Public Sub New()
        Min = DefaultMin
        Max = DefaultMax
        MinValid = DefaultMinValid
        MaxValid = DefaultMaxValid
    End Sub


    ''' <summary>
    ''' check if given testDate is inside Valid range
    ''' </summary>
    Public Function IsDateValid(testDate As Date) As Boolean
        If testDate < MinValid Then Return False
        If testDate > MaxValid Then Return False
        Return True
    End Function

    ''' <summary>
    ''' if newmin is Valid, then adjust Min to min(Min, newmin)
    ''' </summary>
    Public Sub AdjustMin(newmin As Date)
        If Not IsDateValid(newmin) Then Return
        If newmin > Min Then Return
        Min = newmin
    End Sub

    ''' <summary>
    ''' if newmax is Valid, then adjust Max to max(Max, newmax)
    ''' </summary>
    Public Sub AdjustMax(newmax As Date)
        If Not IsDateValid(newmax) Then Return
        If newmax < Max Then Return
        Max = newmax
    End Sub

    ''' <summary>
    ''' if newmin is Valid, then adjust Min to min(Min, newmin);
    ''' if newmax is Valid, then adjust Max to max(Max, newmax);
    ''' same as AdjustMin(newmin); AdjustMax(newmax)
    ''' </summary>
    Public Sub AdjustMinMax(newmin As Date, newmax As Date)
        AdjustMax(newmax)
        AdjustMin(newmin)
    End Sub


    ''' <summary>
    ''' checks if testDate is inside DateRange
    ''' </summary>
    ''' <returns>True only if testDate is invalid, and is between Min and Max</returns>
    Public Function Matches(testDate As Date) As Boolean
        If Not IsDateValid(testDate) Then Return False
        If testDate < Min Then Return False
        If testDate > Max Then Return False
        Return True
    End Function

    ''' <summary>
    ''' checks if current range is inside testRange (testRange overlaps current range)
    ''' </summary>
    Public Function IsInsideRange(testRange As DateRange) As Boolean
        If Min < testRange.Min Then Return False
        If Max > testRange.Max Then Return False
        Return True
    End Function

    ''' <summary>
    ''' checks if current range overlaps testRange (testRange is inside current range)
    ''' </summary>
    Public Function OverlapsRange(testRange As DateRange) As Boolean
        If Min > testRange.Min Then Return False
        If Max < testRange.Max Then Return False
        Return True
    End Function

    ''' <summary>
    ''' return range that is intersection of current range and testRange; using Valid range from current range
    ''' </summary>
    Public Function Intersection(testRange As DateRange) As DateRange
        Dim newmin As Date = Min
        If testRange.Min > newmin Then newmin = testRange.Min
        Dim newmax As Date = Max
        If testRange.Max < newmax Then newmax = testRange.Max
        Dim oRng As New DateRange(newmin, newmax)

        oRng.MaxValid = MaxValid
        oRng.MinValid = MinValid

        Return oRng
    End Function

    ''' <summary>
    ''' checks if current range and testRange has intersection; all dates are treated as valid
    ''' </summary>
    Public Function HasIntersection(testRange As DateRange) As Boolean
        If testRange.Max < Min Then Return False
        If testRange.Min > Max Then Return False
        Return True
    End Function

    ''' <summary>
    ''' return date that is half-between Min and Max
    ''' </summary>
    Public Function MidDate() As Date
        Dim oDateDiff As TimeSpan = Max - Min
        Return Min.AddMinutes(oDateDiff.TotalMinutes)
    End Function

    ''' <summary>
    ''' return date in string as long as common characters from Min and Max
    ''' </summary>
    Public Function ToStringCommon(Optional format As String = "yyyy.MM.dd") As String
        Dim strMin As String = Min.ToString(format)
        Dim strMax As String = Max.ToString(format)

        Dim iLp As Integer
        For iLp = Math.Min(strMin.Length, strMax.Length) - 1 To 0 Step -1
            If strMin(iLp) = strMax(iLp) Then Exit For
        Next

        Return strMin.Substring(0, iLp)
    End Function

    ''' <summary>
    ''' return "Min - Max"
    ''' </summary>
    Public Function ToStringRange(Optional format As String = "yyyy.MM.dd", Optional separator As String = " - ") As String
        Dim strMin As String = Min.ToString(format)
        Dim strMax As String = Max.ToString(format)
        Return strMin & separator & strMax
    End Function

End Class

#End If