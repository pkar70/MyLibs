

Imports System.Collections.Specialized
Imports System.Globalization
Imports System.Reflection
Imports System.Text.RegularExpressions

Public Class BasicGeopos
    Public Property Altitude As Double
    Public Property Latitude As Double
    Public Property Longitude As Double

    ''' <summary>
    ''' create new object, with data validation (ArgumentOutOfRangeException would be thrown)
    ''' </summary>
    ''' <param name="latitude"></param>
    ''' <param name="longitude"></param>
    ''' <param name="altitude"></param>
    Public Sub New(latitude As Double, longitude As Double, Optional altitude As Double = 0)
        Me.Altitude = altitude
        Me.Longitude = longitude
        Me.Latitude = latitude

        If altitude < -6378000 Then Throw New ArgumentOutOfRangeException("Altitude", "Altitude below center of Earth")
        If altitude > 100000 Then Throw New ArgumentOutOfRangeException("Altitude", "Altitude above Kármán line")

        If latitude < -90 OrElse latitude > 90 Then Throw New ArgumentOutOfRangeException("Latitude", "Latitude should be between -90 and 90 (degrees)")
        If longitude < -180 OrElse longitude > 360 Then Throw New ArgumentOutOfRangeException("Longitude", "Longitude should be between -180 and 360 (degrees)")

    End Sub

    ''' <summary>
    ''' returns clone of current item
    ''' </summary>
    ''' <returns></returns>
    Public Function Clone()
        Return New BasicGeopos(Latitude, Longitude, Altitude)
    End Function

#Region "various distance metering"

    ''' <summary>
    ''' Measure distance to given coordinates (in meters)
    ''' </summary>
    ''' <returns>distance in meteres</returns>
    Public Function DistanceTo(dLatitude As Double, dLongitude As Double) As Double

        Try
            Dim iRadix = 6371000
            Dim tLat = (dLatitude - Latitude) * Math.PI / 180.0
            Dim tLon = (dLongitude - Longitude) * Math.PI / 180.0
            Dim a = 2.0 * Math.Asin(Math.Min(1.0, Math.Sqrt(Math.Sin(tLat / 2.0) *
                Math.Sin(tLat / 2.0) + Math.Cos(Math.PI / 180.0 * Latitude) * Math.Cos(Math.PI / 180.0 * dLatitude) *
                Math.Sin(tLon / 2.0) * Math.Sin(tLon / 2.0))))
            Return Math.Round(iRadix * a, 2)
        Catch ex As Exception
            Return 0
        End Try

    End Function

    ''' <summary>
    ''' Measure distance to given coordinates (in meters)
    ''' </summary>
    ''' <returns>distance in meteres</returns>
    Public Function DistanceTo(oGeoPos As BasicGeopos) As Double
        Return DistanceTo(oGeoPos.Latitude, oGeoPos.Longitude)
    End Function

    ''' <summary>
    ''' Measure distance to given coordinates (in kilometers)
    ''' </summary>
    ''' <returns>distance in kilometeres</returns>
    Public Function DistanceKmTo(oGeoPos As BasicGeopos) As Double
        Return DistanceTo(oGeoPos) / 1000
    End Function

    ''' <summary>
    ''' Measure distance to given coordinates (in meters)
    ''' </summary>
    ''' <returns>distance in kilometeres</returns>
    Public Function DistanceKmTo(dLatitude As Double, dLongitude As Double) As Double
        Return DistanceTo(dLatitude, dLongitude) / 1000
    End Function


    ''' <summary>
    ''' check if we are near given coordinates, same as DistanceTo &lt; distanceMeters
    ''' </summary>
    ''' <param name="oGeoPos"></param>
    ''' <param name="distanceMeters"></param>
    ''' <returns></returns>
    Public Function IsNear(oGeoPos As BasicGeopos, distanceMeters As Double) As Boolean
        Return DistanceTo(oGeoPos) < distanceMeters
    End Function

    ''' <summary>
    ''' Measure distance between two geopositions
    ''' </summary>
    ''' <returns>distance in meteres</returns>
    Public Shared Operator -(ByVal oGeoPos0 As BasicGeopos, ByVal oGeoPos1 As BasicGeopos) As Double
        Return oGeoPos0.DistanceTo(oGeoPos1)
    End Operator

#End Region

#Region "testing area"

    Private Shared Function IsValueBetween(valueCurr As Double, value0 As Double, value1 As Double) As Boolean
        Dim valueMin As Double = Math.Min(value0, value1)
        Dim valueMax As Double = Math.Max(value0, value1)
        Return valueCurr >= valueMin AndAlso valueCurr <= valueMax
    End Function

    ''' <summary>
    ''' check if Latitude is between two points (inside range)
    ''' </summary>
    Public Function IsLatitudeBetween(value0 As Double, value1 As Double) As Boolean
        Return IsValueBetween(Latitude, value0, value1)
    End Function

    ''' <summary>
    ''' check if Longitude is between two points (inside range)
    ''' </summary>
    Public Function IsLongitudeBetween(value0 As Double, value1 As Double) As Boolean
        Return IsValueBetween(Longitude, value0, value1)
    End Function

    ''' <summary>
    ''' check if current position is inside given rectangle
    ''' </summary>
    Public Function IsInsideRectangle(latMin As Double, latMax As Double, lonMin As Double, lonMax As Double) As Boolean
        If Not IsLatitudeBetween(latMin, latMax) Then Return False
        Return IsLongitudeBetween(lonMin, lonMax)
    End Function

    ''' <summary>
    ''' check if current position is inside rectangle defined by parameters
    ''' </summary>
    Public Function IsInsideRectangle(oGeoPos0 As BasicGeopos, oGeoPos1 As BasicGeopos) As Boolean
        Return IsInsideRectangle(oGeoPos0.Latitude, oGeoPos1.Latitude, oGeoPos0.Longitude, oGeoPos1.Longitude)
    End Function

    ''' <summary>
    ''' check if current position is inside given circle
    ''' </summary>
    Public Function IsInsideCircle(center As BasicGeopos, radius As Double) As Boolean
        Return DistanceTo(center) <= radius
    End Function

#End Region

#Region "some centers"

    ''' <summary>
    ''' geographical center of Europe (continent)
    ''' </summary>
    Public Shared Function GetEuropeCenter() As BasicGeopos
        Return New BasicGeopos(53.5, 29.0)
    End Function

    ''' <summary>
    ''' geographical center of European Union (since Brexit)
    ''' </summary>
    Public Shared Function GetEUCenter() As BasicGeopos
        ' https://en.wikipedia.org/wiki/Geographical_midpoint_of_Europe
        Return New BasicGeopos(49.843, 9.902056)
    End Function

    ''' <summary>
    ''' geographical center of Poland
    ''' </summary>
    Public Shared Function GetPolandCenter() As BasicGeopos
        ' https//pl.wikipedia.org/wiki/Geometryczny_%C5%9Brodek_Polski
        Return New BasicGeopos(52.2159333, 19.1344222)
    End Function

    ''' <summary>
    ''' geographical center of Kraków
    ''' </summary>
    Public Shared Function GetKrakowCenter() As BasicGeopos
        Return New BasicGeopos(50.06138, 19.93833)
    End Function

    ''' <summary>
    ''' check if we are inside "Mainland Europe" (using IsInsideRectangle)
    ''' </summary>
    Public Function IsInsideEurope() As Boolean
        ' https://en.wikipedia.org/wiki/Extreme_points_of_Europe "Mainland Europe"
        Return IsInsideRectangle(36.004167, 71.133889, -9.500556, 66.618056)
    End Function

    ''' <summary>
    ''' check if we are inside EU, not including overseas territories (using IsInsideRectangle)
    ''' </summary>
    Public Function IsInsideEU() As Boolean
        ' https://en.wikipedia.org/wiki/Extreme_points_of_the_European_Union "In Europe, not including overseas territories"
        Return IsInsideRectangle(27.634722, 70.091667, -31.268056, 34.601389)
    End Function

    ''' <summary>
    ''' check if we are inside Poland (using IsInsideRectangle)
    ''' </summary>
    Public Function IsInsidePoland() As Boolean
        ' https://pl.wikipedia.org/wiki/Polska
        Return IsInsideRectangle(49.0, 54.833333, 14.11805, 24.15138)
    End Function

    ''' <summary>
    ''' check if we are inside Kraków (using IsInsideRectangle)
    ''' </summary>
    Public Function IsInsideKrakow() As Boolean
        Return IsInsideRectangle(49.96799, 50.116, 19.79166, 20.2043)
    End Function

    ''' <summary>
    ''' probably you don't want to use this location in your code - it is for testing purposes
    ''' </summary>
    Public Shared Function GetMyTestGeopos(Optional iDecimalDigits As UInteger = 0) As BasicGeopos
        Dim iDigits As Integer = iDecimalDigits
        If iDigits > 5 Then iDigits = 0

        Return New BasicGeopos(Math.Round(50.01985, iDigits), Math.Round(19.97872, iDigits))
    End Function

#End Region

#Region "empty geoposition"

    ''' <summary>
    ''' get Empty position (middle of the ocean)
    ''' </summary>
    Public Shared Function Empty() As BasicGeopos
        Return New BasicGeopos(0, -150)
    End Function

    ''' <summary>
    ''' check if it is Empty
    ''' </summary>
    Public Function IsEmpty() As Boolean
        Dim oEmpty As BasicGeopos = Empty()
        If Latitude <> oEmpty.Latitude Then Return False
        If Longitude <> oEmpty.Longitude Then Return False
        Return True
    End Function

#End Region


#Region "ToStrings"

    ' ° latitude ≈ 111 km
    ' ° longitude ≈ 111 km at the equator, and 0 m on poles

    Private Shared Function Double2String(sVal As Double, iDigits As Integer) As String
        ' 5 digits means ≈ 10 m, should be enough
        Return Math.Round(sVal, Math.Min(5, iDigits)).ToString(System.Globalization.CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    ''' return Latitude as string with iDigits decimal digits
    ''' </summary>
    ''' <param name="iDigits">decimal digits (max 5, means ≈ 10 m)</param>
    Public Function StringLat(Optional iDigits As Integer = 5) As String
        Return Double2String(Latitude, iDigits)
    End Function

    ''' <summary>
    ''' return Longitude as string with iDigits decimal digits
    ''' </summary>
    ''' <param name="iDigits">decimal digits (max 5, means ≈ 10 m)</param>
    Public Function StringLon(Optional iDigits As Integer = 5) As String
        Return Double2String(Longitude, iDigits)
    End Function

    ''' <summary>
    ''' insert Latitude, Longitude and Altitude into string (%lat, %lon, %alt)
    ''' </summary>
    ''' <param name="sBaseLink">base link, use %lat and %lon as placeholders</param>
    Public Function FormatLink(sBaseLink As String) As String
        sBaseLink = sBaseLink.Replace("%lat", StringLat)
        sBaseLink = sBaseLink.Replace("%lon", StringLon)
        sBaseLink = sBaseLink.Replace("%alt", Altitude)
        Return sBaseLink
    End Function

    ''' <summary>
    ''' insert Latitude, Longitude, Altitude and given zoomLevel into string (%lat, %lon, %alt, %zoom)
    ''' </summary>
    ''' <param name="sBaseLink">base link, use %lat and %lon as placeholders</param>
    Public Function FormatLink(sBaseLink As String, zoomLevel As Integer) As String
        sBaseLink = sBaseLink.Replace("%lat", StringLat)
        sBaseLink = sBaseLink.Replace("%lon", StringLon)
        sBaseLink = sBaseLink.Replace("%alt", StringLon)
        sBaseLink = sBaseLink.Replace("%zoom", zoomLevel)
        Return sBaseLink
    End Function

    ''' <summary>
    ''' tries to create new BasicGeopos from link
    ''' </summary>
    ''' <param name="baselink">base link (with %lat, %lon, %zoom)</param>
    ''' <param name="link">real link (with data)</param>
    ''' <returns>NULL, or BasicGeopos with Latitude and Longitude set from link</returns>
    Public Shared Function FromLink(baselink As String, link As String) As BasicGeopos
        If link.Length < 15 Then Return Nothing

        Dim iLat As Integer = baselink.IndexOf("%lat")
        Dim iLon As Integer = baselink.IndexOf("%lon")

        Dim sRegMask As String = baselink.Replace("%lon", "([\.0-9]*)").
            Replace("%lat", "([\.0-9]*)").
            Replace("%zoom", "[0-9]*")

        Dim result As Match = Regex.Match(link, sRegMask, RegexOptions.IgnoreCase)

        If Not result.Success Then Return Nothing

        Try
            If iLat < iLon Then
                Return New BasicGeopos(result.Groups(1).Value, result.Groups(2).Value)
            Else
                Return New BasicGeopos(result.Groups(2).Value, result.Groups(1).Value)
            End If

        Catch ex As Exception
            Return Nothing
        End Try

    End Function


    Private Const _OSMlink As String = "https://www.openstreetmap.org/#map=%zoom/%lat/%long"

    ''' <summary>
    ''' returns link to OpenStreetMap from current geoposition
    ''' </summary>
    ''' <param name="zoom">zoom level to be used</param>
    Public Function ToOSMLink(Optional zoom As Integer = 16) As String
        zoom = Math.Min(zoom, 19)
        Return FormatLink(_OSMlink, zoom)
    End Function

    ''' <summary>
    ''' returns link to OpenStreetMap from current geoposition
    ''' </summary>
    ''' <param name="zoom">zoom level to be used</param>
    Public Function ToOSMUri(Optional zoom As Integer = 16) As Uri
        Return New Uri(ToOSMLink(zoom))
    End Function


    ''' <summary>
    ''' tries to create new BasicGeopos from OSM link
    ''' </summary>
    ''' <param name="link"></param>
    ''' <returns></returns>
    Public Shared Function FromOSMLink(link As String) As BasicGeopos
        Return FromLink(_OSMlink, link)
    End Function


#If False Then
    ''' <summary>
    ''' returns link to map service from current geoposition
    ''' </summary>
    ''' <param name="mapService">which map service should be used</param>
    ''' <param name="zoom">zoom level to be used</param>
    Public Function FormatLink(mapBaseLinks As String(), mapService As String, Optional zoom As Integer = 16)
        zoom = Math.Min(zoom, 19)
        Select Case mapService
            Case 
        End Select

        Return FormatLink($"https://www.openstreetmap.org/#map=%zoom/%lat/%long", zoom)
    End Function

    Public Shared Function GetFromLink()

    End Function

    ' raczej jako Dictionary
    Public Shared ReadOnly MapServices As String() =
        {"https://www.openstreetmap.org/#map=%zoom/%lat/%lon",
        "https://bing.com/maps/default.aspx?lvl=%zoom&cp=%lat~%lon",
        "https://www.google.pl/maps/@%lat,%lon,%zoomz",
        "https://mapa.wirtualneszlaki.pl/#%zoom/%lat/%lon",
        "https://www.arcgis.com/home/webmap/viewer.html?center=%lon,%lat&level=%zoom"}
#End If

    ''' <summary>
    ''' dumps content as one-line JSON token
    ''' </summary>
    ''' <returns></returns>
    Public Function DumpAsJson() As String
        Return "{""Latitude"": " & Latitude & ", ""Longitude"": " & Longitude & ", ""Altitude"": " & Altitude & "}"
    End Function
#End Region

#Region "DMS format"
    Private Shared Function Double2StringDMS(dVal As Double, sFormat As String, iDigits As Integer) As String

        Dim iDegrees As Integer = If(dVal < 0, Math.Ceiling(dVal), Math.Floor(dVal))
        dVal -= iDegrees

        Dim sRet As String = sFormat.Replace("%d", dVal.ToString(System.Globalization.CultureInfo.InvariantCulture))

        If sFormat.Contains("%s") Then
            Dim iMins As Integer = Math.Floor(dVal * 10.0 / 6.0)
            sRet = sRet.Replace("%m", iMins.ToString(System.Globalization.CultureInfo.InvariantCulture))
            dVal -= iMins
            sRet = sRet.Replace("%s", Math.Round(dVal, Math.Min(5, iDigits)).ToString(System.Globalization.CultureInfo.InvariantCulture))
        Else
            sRet = sRet.Replace("%m", Math.Round(dVal, Math.Min(5, iDigits)).ToString(System.Globalization.CultureInfo.InvariantCulture))
        End If

        Return sRet
    End Function

    ''' <summary>
    ''' return Latitude as DMS string with iDigits decimal digits
    ''' </summary>
    ''' <param name="sFormat">format of value, use %d %m %s as placeholders</param>
    ''' <param name="iDigits">decimal digits (max 5)</param>
    ''' <returns></returns>
    Public Function StringLatDM(Optional sFormat As String = "%d°%m′%s″", Optional iDigits As Integer = 5) As String
        Return Double2StringDMS(Latitude, sFormat, iDigits)
    End Function

    ''' <summary>
    ''' return Longitude as DMS string with iDigits decimal digits
    ''' </summary>
    ''' <param name="sFormat">format of value, use %d %m %s as placeholders</param>
    ''' <param name="iDigits">decimal digits (max 5)</param>
    ''' <returns></returns>
    Public Function StringLonDM(Optional sFormat As String = "%d°%m′%s″", Optional iDigits As Integer = 5) As String
        Return Double2StringDMS(Longitude, sFormat, iDigits)
    End Function

    ''' <summary>
    ''' create new BasicGeopos, from DMS-formatted values
    ''' </summary>
    ''' <param name="latD">latitude degrees</param>
    ''' <param name="latM">latitude minutes</param>
    ''' <param name="latS">latitude seconds</param>
    ''' <param name="latSN">latitude "S" or "N" </param>
    ''' <param name="lonD">longitude degrees</param>
    ''' <param name="lonM">longitude minutes</param>
    ''' <param name="lonS">longitude seconds</param>
    ''' <param name="lonEW">longitude "E" or "W"</param>
    Public Shared Function FromDMS(latD As Integer, latM As Double, latS As Double, latSN As String, lonD As Integer, lonM As Double, lonS As Double, lonEW As String) As BasicGeopos
        If latSN = "S" Then latD = -latD
        If lonEW = "W" Then lonD = -lonD

        Return New BasicGeopos(latD + 1 / 60 * latM + 1 / 3600 * latS, lonD + 1 / 60 * lonM + 1 / 3600 * lonS)
    End Function

    ''' <summary>
    ''' create new BasicGeopos, from DMS-formatted values
    ''' </summary>
    ''' <param name="latD">latitude degrees</param>
    ''' <param name="latM">latitude minutes</param>
    ''' <param name="latSN">latitude "S" or "N" </param>
    ''' <param name="lonD">longitude degrees</param>
    ''' <param name="lonM">longitude minutes</param>
    ''' <param name="lonEW">longitude "E" or "W"</param>
    Public Shared Function FromDM(latD As Integer, latM As Double, latSN As String, lonD As Integer, lonM As Double, lonEW As String) As BasicGeopos
        Return FromDMS(latD, latM, 0, latSN, lonD, lonM, 0, lonEW)
    End Function

    ''' <summary>
    ''' create new BasicGeopos, from DMS-formatted values
    ''' </summary>
    ''' <param name="latD">latitude degrees, decimal part: minutes (so 1.59 + 0.01 = 2.00)</param>
    ''' <param name="latSN">latitude "S" or "N" </param>
    ''' <param name="lonD">longitude degrees, decimal part: minutes (so 1.59 + 0.01 = 2.00)</param>
    ''' <param name="lonEW">longitude "E" or "W"</param>
    Public Shared Function FromDM(latD As Double, latSN As String, lonD As Double, lonEW As String) As BasicGeopos

        Dim latDegree As Double = Math.Floor(latD)
        Dim latMin As Double = 100 * (latD - latDegree)
        Dim lonDegree As Double = Math.Floor(lonD)
        Dim lonMin As Double = 100 * (lonD - lonDegree)

        Return FromDM(latDegree, latMin, latSN, lonDegree, lonMin, lonEW)
    End Function
#End Region

#Region "from/to any object"

    ''' <summary>
    ''' Try to construct BasicGeopos from any .Net object with Latitude, Longitude and Altitude properties or fields.
    ''' It can be UWP BasicGeoposition, .Net Framework GeoCoordinate, or MAUI Location (or even your own types)
    ''' </summary>
    ''' <returns>BasicGeopos created from input parameter, or NULL if something goes wrong (e.g. no Latitude or Longitude given - Altitude is not required)</returns>
    Public Shared Function FromObject(anyObject As Object) As BasicGeopos
        Dim oNew As New BasicGeopos(0, 0, 0)

        Dim prop As PropertyInfo
        Dim fld As FieldInfo

        Dim bLat As Boolean = False
        Dim bLon As Boolean = False

        Try

            prop = anyObject.GetType.GetRuntimeProperty("Latitude")
            If prop IsNot Nothing Then
                oNew.Latitude = prop.GetValue(anyObject)
                bLat = True
            Else
                fld = anyObject.GetType.GetRuntimeField("Latitude")
                If fld IsNot Nothing Then
                    oNew.Latitude = fld.GetValue(anyObject)
                    bLat = True
                End If
            End If

            prop = anyObject.GetType.GetRuntimeProperty("Longitude")
            If prop IsNot Nothing Then
                oNew.Longitude = prop.GetValue(anyObject)
                bLon = True
            Else
                fld = anyObject.GetType.GetRuntimeField("Longitude")
                If fld IsNot Nothing Then
                    oNew.Longitude = fld.GetValue(anyObject)
                    bLon = True
                End If
            End If

            prop = anyObject.GetType.GetRuntimeProperty("Altitude")
            If prop IsNot Nothing Then
                oNew.Altitude = prop.GetValue(anyObject)
            Else
                fld = anyObject.GetType.GetRuntimeField("Altitude")
                If fld IsNot Nothing Then oNew.Altitude = fld.GetValue(anyObject)
            End If

            If bLat AndAlso bLon Then Return oNew

        Catch ex As Exception
        End Try

        Return Nothing

    End Function

    ''' <summary>
    ''' Try to copy Latitude, Longitude and Altitude to any .Net object (to Properties or Fields).
    ''' It can be UWP BasicGeoposition, .Net Framework GeoCoordinate, or MAUI Location (or even your own types)
    ''' </summary>
    ''' <param name="anyObject">Object to insert values into</param>
    Public Sub CopyTo(anyObject As Object)

        Dim prop As PropertyInfo

        prop = anyObject.GetType.GetRuntimeProperty("Latitude")
        If prop IsNot Nothing Then prop.SetValue(anyObject, Latitude)

        prop = anyObject.GetType.GetRuntimeProperty("Longitude")
        If prop IsNot Nothing Then prop.SetValue(anyObject, Longitude)

        prop = anyObject.GetType.GetRuntimeProperty("Altitude")
        If prop IsNot Nothing Then prop.SetValue(anyObject, Altitude)

        Dim fld As FieldInfo

        fld = anyObject.GetType.GetRuntimeField("Latitude")
        If fld IsNot Nothing Then fld.SetValue(anyObject, Latitude)

        fld = anyObject.GetType.GetRuntimeField("Longitude")
        If fld IsNot Nothing Then fld.SetValue(anyObject, Longitude)

        fld = anyObject.GetType.GetRuntimeField("Altitude")
        If fld IsNot Nothing Then fld.SetValue(anyObject, Altitude)

    End Sub

#End Region


End Class

#If False Then
Public Enum MapService
    OpenStreetMap
    Bing
    Google
    WirtSzlaki
    ArcGis
End Enum
#end if