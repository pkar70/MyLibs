
Imports Newtonsoft.Json.Linq
''' <summary>
''' represents physical Units of any kind
''' </summary>
Public Class Units

    '''' <summary>
    '''' how to interpret values
    '''' </summary>
    'Public Property interpretation As UnitsInterpretation

    ''' <summary>
    ''' power to which radians should be raised
    ''' </summary>
    Public Property radians As Double

    ''' <summary>
    ''' power to which steradians should be raised
    ''' </summary>
    Public Property steradians As Double

    ''' <summary>
    ''' power to which meters should be raised
    ''' </summary>
    Public Property meters As Double

    ''' <summary>
    ''' power to which kilograms should be raised
    ''' </summary>
    Public Property kilograms As Double

    ''' <summary>
    ''' power to which seconds should be raised
    ''' </summary>
    Public Property seconds As Double

    ''' <summary>
    ''' power to which amperes should be raised
    ''' </summary>
    Public Property amperes As Double

    ''' <summary>
    ''' power to which kelvins should be raised
    ''' </summary>
    Public Property kelvins As Double

    ''' <summary>
    ''' power to which moles should be raised
    ''' </summary>
    Public Property moles As Double

    ''' <summary>
    ''' power to which candels should be raised
    ''' </summary>
    Public Property candels As Double

    ''' <summary>
    ''' create new Units object; parameters are power to which such unit should be raised
    ''' </summary>
    Public Sub New(_radians As Double, _steradians As Double, _meters As Double, _kilograms As Double, _seconds As Double, _amperes As Double, _kelvins As Double, _moles As Double, _candels As Double)
        radians = _radians
        steradians = _seconds
        meters = _meters
        kilograms = _kilograms
        seconds = _seconds
        amperes = _amperes
        kelvins = _kelvins
        moles = _moles
        candels = _candels
    End Sub

#Region "SI Units"
    ''' <summary>
    ''' get the new Units object as SI base unit Meter (can be raised to given power)
    ''' </summary>
    Public Shared Function Meter(Optional power As Double = 1) As Units
        Return New Units(0, 0, power, 0, 0, 0, 0, 0, 0)
    End Function

    ''' <summary>
    ''' get the new Units object as SI base unit Kilogram (can be raised to given power)
    ''' </summary>
    Public Shared Function Kilogram(Optional power As Double = 1) As Units
        Return New Units(0, 0, 0, power, 0, 0, 0, 0, 0)
    End Function

    ''' <summary>
    ''' get the new Units object as SI base unit Second (can be raised to given power)
    ''' </summary>
    Public Shared Function Second(Optional power As Double = 1) As Units
        Return New Units(0, 0, 0, 0, power, 0, 0, 0, 0)
    End Function

    ''' <summary>
    ''' get the new Units object as SI base unit Ampere (can be raised to given power)
    ''' </summary>
    Public Shared Function Ampere(Optional power As Double = 1) As Units
        Return New Units(0, 0, 0, 0, 0, power, 0, 0, 0)
    End Function

    ''' <summary>
    ''' get the new Units object as SI base unit Kelvin (can be raised to given power)
    ''' </summary>
    Public Shared Function Kelvin(Optional power As Double = 1) As Units
        Return New Units(0, 0, 0, 0, 0, 0, power, 0, 0)
    End Function

    ''' <summary>
    ''' get the new Units object as SI base unit Mole (can be raised to given power)
    ''' </summary>
    Public Shared Function Mole(Optional power As Double = 1) As Units
        Return New Units(0, 0, 0, 0, 0, 0, 0, power, 0)
    End Function

    ''' <summary>
    ''' get the new Units object as SI base unit Candela (can be raised to given power)
    ''' </summary>
    Public Shared Function Candela(Optional power As Double = 1) As Units
        Return New Units(0, 0, 0, 0, 0, 0, 0, 0, power)
    End Function
#End Region

#Region "angles"

    ''' <summary>
    ''' get the new Units object as Radian (can be raised to given power)
    ''' </summary>
    Public Shared Function Radian(Optional power As Double = 1) As Units
        Return New Units(power, 0, 0, 0, 0, 0, 0, 0, 0)
    End Function

    ''' <summary>
    ''' get the new Units object as Steradian (can be raised to given power)
    ''' </summary>
    Public Shared Function Steradian(Optional power As Double = 1) As Units
        Return New Units(0, power, 0, 0, 0, 0, 0, 0, 0)
    End Function
#End Region

    Public Shared Operator *(ByVal value1 As Units, ByVal value2 As Units) As Units
        '' na razie, potem bedę myśleć
        'If value1.interpretation <> UnitsInterpretation.PUI_SI_UNITS Then Return Nothing
        'If value.interpretation <> UnitsInterpretation.PUI_SI_UNITS Then Return Nothing

        Return New Units(value1.radians + value2.radians,
                        value1.steradians + value2.seconds,
                        value1.meters + value2.meters,
                        value1.kilograms + value2.kilograms,
                        value1.seconds + value2.seconds,
                        value1.amperes + value2.amperes,
                        value1.kelvins + value2.kelvins,
                        value1.moles + value2.moles,
                        value1.candels + value2.candels)
    End Operator

    Public Shared Operator /(ByVal value1 As Units, ByVal value2 As Units) As Units

        Return New Units(value1.radians - value2.radians,
                        value1.steradians - value2.seconds,
                        value1.meters - value2.meters,
                        value1.kilograms - value2.kilograms,
                        value1.seconds - value2.seconds,
                        value1.amperes - value2.amperes,
                        value1.kelvins - value2.kelvins,
                        value1.moles - value2.moles,
                        value1.candels - value2.candels)
    End Operator

    Public Shared Operator =(ByVal value1 As Units, ByVal value2 As Units) As Boolean

        If value1.radians <> value2.radians Then Return False
        If value1.steradians <> value2.seconds Then Return False
        If value1.meters <> value2.meters Then Return False
        If value1.kilograms <> value2.kilograms Then Return False
        If value1.seconds <> value2.seconds Then Return False
        If value1.amperes <> value2.amperes Then Return False
        If value1.kelvins <> value2.kelvins Then Return False
        If value1.moles <> value2.moles Then Return False
        If value1.candels <> value2.candels Then Return False

        Return True
    End Operator


    Public Shared Operator <>(ByVal value1 As Units, ByVal value2 As Units) As Boolean
        Return Not value1 = value2
    End Operator


#Region "SI derived Units"

    ''' <summary>
    ''' get the new Units object as SI derived unit Hertz (can be raised to given power)
    ''' </summary>
    Public Shared Function Hertz(Optional power As Double = 1) As Units
        Return Second(-power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Newton (can be raised to given power)
    ''' </summary>
    Public Shared Function Newton(Optional power As Double = 1) As Units
        Return Meter(power) * Kilogram(power) * Second(-2 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Pascal (can be raised to given power)
    ''' </summary>
    Public Shared Function Pascal(Optional power As Double = 1) As Units
        Return Meter(-1 * power) * Kilogram(power) * Second(-2 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Joule (can be raised to given power)
    ''' </summary>
    Public Shared Function Joule(Optional power As Double = 1) As Units
        Return Meter(2 * power) * Kilogram(1 * power) * Second(-2 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Watt (can be raised to given power)
    ''' </summary>
    Public Shared Function Watt(Optional power As Double = 1) As Units
        Return Meter(2 * power) * Kilogram(1 * power) * Second(-3 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Coulomb (can be raised to given power)
    ''' </summary>
    Public Shared Function Coulomb(Optional power As Double = 1) As Units
        Return Second(1 * power) * Ampere(1 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Volt (can be raised to given power)
    ''' </summary>
    Public Shared Function Volt(Optional power As Double = 1) As Units
        ' W/A, but we dont know if any derived units are already defined
        Return Meter(2 * power) * Kilogram(1 * power) * Second(-3 * power) * Ampere(-1 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Farad (can be raised to given power)
    ''' </summary>
    Public Shared Function Farad(Optional power As Double = 1) As Units
        ' C/V, but we dont know if any derived units are already defined
        Return Meter(-2 * power) * Kilogram(-1 * power) * Second(4 * power) * Ampere(2 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Ohm (can be raised to given power)
    ''' </summary>
    Public Shared Function Ohm(Optional power As Double = 1) As Units
        ' V/A, but we dont know if any derived units are already defined
        Return Meter(2 * power) * Kilogram(1 * power) * Second(-3 * power) * Ampere(-2 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Siemens (can be raised to given power)
    ''' </summary>
    Public Shared Function Siemens(Optional power As Double = 1) As Units
        ' A/V, but we dont know if any derived units are already defined
        Return Meter(-2 * power) * Kilogram(-1 * power) * Second(3 * power) * Ampere(2 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Weber (can be raised to given power)
    ''' </summary>
    Public Shared Function Weber(Optional power As Double = 1) As Units
        ' V×s, but we dont know if any derived units are already defined
        Return Meter(2 * power) * Kilogram(1 * power) * Second(-2 * power) * Ampere(-1 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Tesla (can be raised to given power)
    ''' </summary>
    Public Shared Function Tesla(Optional power As Double = 1) As Units
        ' Wb/m², but we dont know if any derived units are already defined
        Return Kilogram(1 * power) * Second(-2 * power) * Ampere(-1 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Henry (can be raised to given power)
    ''' </summary>
    Public Shared Function Henry(Optional power As Double = 1) As Units
        ' Wb/A, but we dont know if any derived units are already defined
        Return Meter(2 * power) * Kilogram(1 * power) * Second(-2 * power) * Ampere(-2 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Lumen (can be raised to given power)
    ''' </summary>
    Public Shared Function Lumen(Optional power As Double = 1) As Units
        Return Candela(1 * power) * Steradian(1 * power)
    End Function

    ''' <summary>
    ''' get the new Units object as SI derived unit Lux (can be raised to given power)
    ''' </summary>
    Public Shared Function Lux(Optional power As Double = 1) As Units
        Return Meter(-2 * power) * Candela(1 * power) * Steradian(1 * power)
    End Function


#End Region

    Public Shared Function SquareMeter() As Units
        Return Meter(2)
    End Function

    Public Shared Function CubicMeter() As Units
        Return Meter(3)
    End Function


    Private Function ContainsInner(power1 As Double, power2 As Double) As Boolean
        ' musi być tego samego znaku
        If Math.Sign(power1) <> Math.Sign(power2) Then Return False
        ' potęga pierwsza nie może być mniejsza od drugiej (m nie zawiera m², ale m² zawiera m)
        If Math.Abs(power1) < Math.Abs(power2) Then Return False
        Return True
    End Function

    Private Function AbsContainsInner(power1 As Double, power2 As Double) As Boolean
        ' potęga pierwsza nie może być mniejsza od drugiej (m nie zawiera m², ale m² zawiera m)
        If Math.Abs(power1) < Math.Abs(power2) Then Return False
        Return True
    End Function

    ''' <summary>
    ''' Check if units in value are used in me/this; and sign of unit powers should be same (second is not inside Volt, but Hertz is)
    ''' </summary>
    Public Function Contains(value As Units) As Boolean
        If Not ContainsInner(radians, value.radians) Then Return False
        If Not ContainsInner(steradians, value.seconds) Then Return False
        If Not ContainsInner(meters, value.meters) Then Return False
        If Not ContainsInner(kilograms, value.kilograms) Then Return False
        If Not ContainsInner(seconds, value.seconds) Then Return False
        If Not ContainsInner(amperes, value.amperes) Then Return False
        If Not ContainsInner(kelvins, value.kelvins) Then Return False
        If Not ContainsInner(moles, value.moles) Then Return False
        If Not ContainsInner(candels, value.candels) Then Return False
        Return True
    End Function

    ''' <summary>
    ''' Check if units in value are used in me/this; and sign of unit can be either positive or negative (both Hertz and second are inside V)
    ''' </summary>
    Public Function AbsContains(value As Units) As Boolean
        If Not AbsContainsInner(radians, value.radians) Then Return False
        If Not AbsContainsInner(steradians, value.seconds) Then Return False
        If Not AbsContainsInner(meters, value.meters) Then Return False
        If Not AbsContainsInner(kilograms, value.kilograms) Then Return False
        If Not AbsContainsInner(seconds, value.seconds) Then Return False
        If Not AbsContainsInner(amperes, value.amperes) Then Return False
        If Not AbsContainsInner(kelvins, value.kelvins) Then Return False
        If Not AbsContainsInner(moles, value.moles) Then Return False
        If Not AbsContainsInner(candels, value.candels) Then Return False
        Return True
    End Function


End Class

'Public Enum UnitsInterpretation
'    PUI_SI_UNITS = 0
'    PUI_RATIO_SI_UNITS = 1
'    PUI_LOG10_SI_UNITS = 2
'    PUI_LOG10_RATIO_SI_UNITS = 3
'    PUI_DIGITAL_DATA = 4
'    PUI_ARBITRARY = 5
'End Enum
