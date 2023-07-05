
Public Class PhysicalValue(Of TYPE)

    ' TYPE can be any arithmetic type, especially double or decimal, also BigInteger, Complex, Quaternion...
    Public Property value As TYPE
    Public Property unit As Units

    Public Sub New(_value As TYPE, _unit As Units)
        value = _value
        unit = _unit
    End Sub

    Public Shared Function FromLength(value As TYPE, Optional lengthUnit As LengthUnits = LengthUnits.metric) As PhysicalValue(Of TYPE)

        Select Case lengthUnit
            Case LengthUnits.metric
                Return New PhysicalValue(Of TYPE)(value, Units.Meter)
            Case LengthUnits.foots
                Return New PhysicalValue(Of TYPE)(value * 0.3048, Units.Meter)
            Case LengthUnits.inches
                Return New PhysicalValue(Of TYPE)(value * 0.0254, Units.Meter)
        End Select

        Throw New ArgumentException("Unrecognized lengthUnit - internal error")
    End Function

End Class


Public Enum LengthUnits
    metric = 0
    foots = 1
    inches = 2
End Enum