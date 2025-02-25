﻿

Imports System.Collections.Specialized
Imports System.Net
Imports System.Reflection
Imports System.Text.RegularExpressions

Public Class BasicGeopos

    Public Property Altitude As Double
    Public Property Latitude As Double
    Public Property Longitude As Double


    ''' <summary>
    ''' create new object, as we don't know any data, it is same as Empty(). Also, used by JSON deserialization.
    ''' </summary>
    Public Sub New()
        ' this is required for JSON deserialization, as we have more than one ctor: New(double) and New(string)
        Dim emptyGeo As BasicGeopos = Empty()
        Me.Altitude = emptyGeo.Altitude
        Me.Longitude = emptyGeo.Longitude
        Me.Latitude = emptyGeo.Latitude
    End Sub


    ''' <summary>
    ''' create new object, with data validation (ArgumentOutOfRangeException would be thrown)
    ''' </summary>
    ''' <param name="latitude">Latitude, -90 to 90</param>
    ''' <param name="longitude">Longitude, -180 to 360</param>
    ''' <param name="altitude">Altitude, -6378000 to 100000</param>
    Public Sub New(latitude As Double, longitude As Double, Optional altitude As Double = 0)
        Me.Altitude = altitude
        Me.Longitude = longitude
        Me.Latitude = latitude

        ValidateRanges()

    End Sub


    ''' <summary>
    ''' create new object, with data validation (ArgumentOutOfRangeException would be thrown), from strings. If any parameter cannot be converted to Double, values from BasicGeopos.Empty() would be used.
    ''' </summary>
    ''' <param name="latitude">Latitude, -90 to 90</param>
    ''' <param name="longitude">Longitude, -180 to 360</param>
    ''' <param name="altitude">Altitude, -6378000 to 100000</param>
    Public Sub New(latitude As String, longitude As String, Optional altitude As String = "0")
        ' najpierw defaulty
        Dim emptyGeo As BasicGeopos = Empty()
        Me.Altitude = emptyGeo.Altitude
        Me.Longitude = emptyGeo.Longitude
        Me.Latitude = emptyGeo.Latitude

        If Not Double.TryParse(latitude, Me.Latitude) Then Me.Latitude = emptyGeo.Latitude
        If Not Double.TryParse(longitude, Me.Longitude) Then Me.Longitude = emptyGeo.Longitude
        If Not Double.TryParse(altitude, Me.Altitude) Then Me.Altitude = emptyGeo.Altitude

        ValidateRanges()

    End Sub


    Private Sub ValidateRanges()
        If Altitude < -6378000 Then Throw New ArgumentOutOfRangeException("Altitude", "Altitude below center of Earth")
        If Altitude > 100000 Then Throw New ArgumentOutOfRangeException("Altitude", "Altitude above Kármán line")

        If Latitude < -90 OrElse Latitude > 90 Then Throw New ArgumentOutOfRangeException("Latitude", "Latitude should be between -90 and 90 (degrees)")
        If Longitude < -180 OrElse Longitude > 360 Then Throw New ArgumentOutOfRangeException("Longitude", "Longitude should be between -180 and 360 (degrees)")
    End Sub

    ''' <summary>
    ''' returns clone of current item
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function Clone()
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
    ''' return Altitude as string with iDigits decimal digits
    ''' </summary>
    ''' <param name="iDigits">decimal digits (max 5))</param>
    Public Function StringAlt(Optional iDigits As Integer = 0) As String
        Return Double2String(Altitude, iDigits)
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
    ''' <param name="sBaseLink">base link, use %lat, %lon, %alt, %zoom, %dmslat, %dmslon as placeholders</param>
    Public Function FormatLink(sBaseLink As String, zoomLevel As Integer) As String
        sBaseLink = sBaseLink.Replace("%lat", StringLat)
        sBaseLink = sBaseLink.Replace("%lon", StringLon)
        sBaseLink = sBaseLink.Replace("%alt", StringLon)
        sBaseLink = sBaseLink.Replace("%dmslat", StringLatDM("%d_%m_%s_%nw", 0)) ' 50_03_41_N_19_56_14_E
        sBaseLink = sBaseLink.Replace("%dmslon", StringLonDM("%d_%m_%s_%ew", 0))
        sBaseLink = sBaseLink.Replace("%bbox", Double2String(Longitude - 0.01, 5) & "," & Double2String(Latitude - 0.01, 5) & "," & Double2String(Longitude + 0.01, 5) & "," & Double2String(Latitude + 0.01, 5))
        sBaseLink = sBaseLink.Replace("%zoom", zoomLevel)
        Return sBaseLink
    End Function

    ''' <summary>
    ''' tries to create new BasicGeopos from link
    ''' </summary>
    ''' <param name="baselink">base link (with %lat, %lon, %zoom)</param>
    ''' <param name="link">real link (with data)</param>
    ''' <returns>NULL, or BasicGeopos with Latitude and Longitude set from link (no altitude nor zoom)</returns>
    Public Shared Function FromLink(baselink As String, link As String) As BasicGeopos
        If link.Length < 10 Then Return Nothing

        Dim iLat As Integer = baselink.IndexOf("%lat")
        Dim iLon As Integer = baselink.IndexOf("%lon")

        Dim sRegMask As String = baselink.Replace("%lon", "(-?[\.0-9]*)").
            Replace("%lat", "(-?[\.0-9]*)").
            Replace("%zoom", "[\.0-9]*").
            Replace("%alt", "[0-9]*")

        ' special case: geouri
        ' geo:lat,lon[,alt][;u=uncertainty]
        If link.Substring(0, 4).ToLower = "geo:" Then
            ' remove uncertainty
            Dim iInd As Integer = link.IndexOf(";")
            If iInd > 0 Then link = link.Substring(0, iInd)
        End If

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


    ' Private Const _OSMlink As String = "https://www.openstreetmap.org/#map=%zoom/%lat/%long"

    ''' <summary>
    ''' returns link to OpenStreetMap from current geoposition
    ''' </summary>
    ''' <param name="zoom">zoom level to be used</param>
    Public Function ToOSMLink(Optional zoom As Integer = 16) As String
        Return ToLink("openstreetmap", Math.Min(zoom, 19))
        ' Return FormatLink(_OSMlink, zoom)
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
        Return GetFromLink(link)
        'Return FromLink(_OSMlink, link)
    End Function

#Region "dictionary of services"
    ''' <summary>
    ''' dictionary of mapservices, as (string,string) = (name of service , link for FormatLink function)
    ''' Default version contains: osm, bing, google, wirtszlaki, arcgis
    ''' </summary>
    Public Shared MapServices As New Dictionary(Of String, String) From
        {
        {"openstreetmap", "https://www.openstreetmap.org/#map=%zoom/%lat/%lon"},
        {"geohack", "https://geohack.toolforge.org/geohack.php?params=%dmslat_%dmslon_type:landmark"},
        {"bing", "https://bing.com/maps/default.aspx?lvl=%zoom&cp=%lat~%lon"},
        {"google", "https://www.google.com/maps/@%lat,%lon,%zoomz"},
        {"arcgis", "https://www.arcgis.com/home/webmap/viewer.html?center=%lon,%lat&level=%zoom"},
        {"copernix", "https://copernix.io/#?where=%lon,%lat,%zoom&query=&map_type=roadmap&pagename=?language=en"},
        {"google.pl", "https://www.google.pl/maps/@%lat,%lon,%zoomz"},
        {"herewego", "https://wego.here.com/location/?map=%lat,%lon,15"},
{"mapquest", "https://www.waze.com/livemap/?zoom=%zoom&lat=%lat&lon=%lon"},
        {"oldmaps", "https://www.oldmapsonline.org/en/Krakow#bbox=%bbox"},
{"waze", "https://www.waze.com/livemap/?zoom=%zoom&lat=%lat&lon=%lon"},
        {"wirtszlaki", "https://mapa.wirtualneszlaki.pl/#%zoom/%lat/%lon/OSM_bw:100-Szlaki_rowerowe_osm:100-Szlaki_piesze_osm:100"},
        {"mapaturyst", "https://mapa-turystyczna.pl/#%lat/%lon/%zoom"},
        {"wikimap", "https://wikimap.toolforge.org/?lat=%lat&lon=%lon&zoom=%zoom&lang=en&wp=false"},
{"yandex", "https://www.waze.com/livemap/?zoom=%zoom&lat=%lat&lon=%lon"},
        {"geouri", "geo:%lat,%lon"},
        {"geouriAlt", "geo:%lat,%lon,%alt"}
        }



    ''' <summary>
    ''' returns link to map service from current geoposition
    ''' </summary>
    ''' <param name="mapService">which map service should be used (see MapServices)</param>
    ''' <param name="zoom">zoom level to be used</param>
    ''' <returns>Link for selected service, or for OSM if service is unknown</returns>
    Public Function ToLink(mapService As String, Optional zoom As Integer = 16) As String
        zoom = Math.Min(zoom, 19)

        mapService = mapService.ToLowerInvariant
        Dim baseLink As String = ""
        If Not MapServices.TryGetValue(mapService, baseLink) Then Return ToOSMLink(zoom)

        Return FormatLink(baseLink, zoom)
    End Function

    ''' <summary>
    ''' returns link to map service from current geoposition
    ''' </summary>
    ''' <param name="mapService">which map service should be used (see MapServices)</param>
    ''' <param name="zoom">zoom level to be used</param>
    ''' <returns>Link for selected service, or for OSM if service is unknown</returns>
    Public Function ToUri(mapService As String, Optional zoom As Integer = 16) As Uri
        Return New Uri(ToLink(mapService, zoom))
    End Function

    ''' <summary>
    ''' tries to create new BasicGeopos from link, using URL of all services from MapServices
    ''' </summary>
    ''' <param name="link">real link (with data)</param>
    ''' <returns>NULL, or BasicGeopos with Latitude and Longitude set from link (no altitude nor zoom)</returns>
    Public Shared Function GetFromLink(link As String) As BasicGeopos

        For Each oService As KeyValuePair(Of String, String) In MapServices
            Dim ret As BasicGeopos = FromLink(oService.Value, link)
            If ret IsNot Nothing Then Return ret
        Next

        Return Nothing
    End Function

#End Region



    ''' <summary>
    ''' dumps content as one-line JSON token
    ''' </summary>
    ''' <returns></returns>
    Public Function DumpAsJson() As String
        Return "{""Latitude"": " & Latitude & ", ""Longitude"": " & Longitude & ", ""Altitude"": " & Altitude & "}"
    End Function
#End Region

#Region "DMS format"
    Private Shared Function Double2StringDMS(dWspolrzedna As Double, sFormat As String, iDigits As Integer) As String

#If Not PREV_METHOD Then

        Dim dSign As Double = Math.Sign(dWspolrzedna)
        dWspolrzedna = Math.Abs(dWspolrzedna)

        Dim iDegrees As Integer = Math.Floor(dWspolrzedna)
        dWspolrzedna -= iDegrees

        Dim sRet As String = sFormat.Replace("%d", iDegrees.ToString(System.Globalization.CultureInfo.InvariantCulture))

        Dim oTSpanM As TimeSpan = TimeSpan.FromMinutes(dWspolrzedna)

        If sFormat.Contains("%s") Then
            Dim iMins As Integer = oTSpanM.Seconds
            Dim dSecs As Double = oTSpanM.TotalSeconds - iMins

            sRet = sRet.Replace("%m", iMins.ToString(System.Globalization.CultureInfo.InvariantCulture))

            Dim oTSpanS As TimeSpan = TimeSpan.FromMinutes(dSecs)   ' dSecs < 1
            sRet = sRet.Replace("%s", Math.Round(oTSpanS.Seconds, Math.Min(5, iDigits)).ToString(System.Globalization.CultureInfo.InvariantCulture))
        Else
            sRet = sRet.Replace("%m", Math.Round(oTSpanM.TotalSeconds, Math.Min(5, iDigits)).ToString(System.Globalization.CultureInfo.InvariantCulture))
        End If

        Return sRet


#Else

        ' od .Net Core 3.0 jest Round(x,x,ToZero)
        Dim iDegrees As Integer = If(dVal < 0, Math.Ceiling(dVal), Math.Floor(dVal))
        Dim sRet As String = sFormat.Replace("%d", iDegrees.ToString(System.Globalization.CultureInfo.InvariantCulture))

        dVal = Math.Abs(dVal)
        dVal -= iDegrees
        ' i tu był błąd :)
        ' 0.0089 -> 
        ' 0.961

        If sFormat.Contains("%s") Then
            Dim iMins As Integer = Math.Floor(dVal * 10.0 / 6.0)

            sRet = sRet.Replace("%m", iMins.ToString(System.Globalization.CultureInfo.InvariantCulture))
            dVal -= iMins
            sRet = sRet.Replace("%s", Math.Round(dVal, Math.Min(5, iDigits)).ToString(System.Globalization.CultureInfo.InvariantCulture))
        Else
            sRet = sRet.Replace("%m", Math.Round(dVal, Math.Min(5, iDigits)).ToString(System.Globalization.CultureInfo.InvariantCulture))
        End If

        Return sRet
#End If
    End Function

    ''' <summary>
    ''' return Latitude as DMS string with iDigits decimal digits. If %ns is used, then use absolute value degrees and N/S, if not - degrees can be negative., 
    ''' </summary>
    ''' <param name="sFormat">format of value, use %d %m %s %ns as placeholders</param>
    ''' <param name="iDigits">decimal digits (max 5)</param>
    ''' <returns></returns>
    Public Function StringLatDM(Optional sFormat As String = "%d°%m′%s″", Optional iDigits As Integer = 5) As String
        If sFormat.Contains("%ns") Then
            sFormat = sFormat.Replace("%ns", If(Latitude > 0, "N", "S"))
            Return Double2StringDMS(Math.Abs(Latitude), sFormat, iDigits)
        Else
            Return Double2StringDMS(Latitude, sFormat, iDigits)
        End If
    End Function

    ''' <summary>
    ''' return Longitude as DMS string with iDigits decimal digits
    ''' </summary>
    ''' <param name="sFormat">format of value, use %d %m %s %ew as placeholders. If %ew is used, then use absolute value degrees and E/W, if not - degrees can be negative.</param>
    ''' <param name="iDigits">decimal digits (max 5)</param>
    ''' <returns></returns>
    Public Function StringLonDM(Optional sFormat As String = "%d°%m′%s″", Optional iDigits As Integer = 5) As String
        If sFormat.Contains("%ew") Then
            sFormat = sFormat.Replace("%ew", If(Latitude > 0, "E", "W"))
            Return Double2StringDMS(Math.Abs(Latitude), sFormat, iDigits)
        Else
            Return Double2StringDMS(Longitude, sFormat, iDigits)
        End If
    End Function

    ''' <summary>
    ''' return string with DMS format using iDigits decimal digits
    ''' </summary>
    ''' <param name="sFormat">format of value, use %ad %am %as as placeholders for lAtitude data, and %od %om %os as placeholders for lOngitude data. Values would be rounded (e.g. 50.9 would render as 51)</param>
    ''' <param name="iDigits">decimal digits (max 5)</param>
    ''' <returns></returns>
    Public Function StringDM(sFormat As String, Optional iDigits As Integer = 5)
        Dim ret As String = sFormat.Replace("%ad", "%d").Replace("%am", "%m").Replace("%as", "%s")
        ret = Double2StringDMS(Latitude, ret, iDigits)
        ret = ret.Replace("%od", "%d").Replace("%om", "%m").Replace("%os", "%s")
        ret = Double2StringDMS(Longitude, ret, iDigits)
        Return ret
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

#Region "Maidenhead Locator System / QTH Locator / IARU Locator"

    ' https://en.wikipedia.org/wiki/Maidenhead_grid_locator
    ''' <summary>
    ''' Get as QTH locator 
    ''' </summary>
    ''' <param name="depth">number of character pairs (e.q., to get JO90xb use 3), >6 is treated as 6</param>
    Public Function ToQTH(Optional depth As UInt16 = 4) As String
        Dim qth As String = ""

        Dim latit As Double = Latitude + 90
        Dim longit As Double = Longitude + 180

        ' Field, A-R, 18 values, 20×10 degrees
        qth &= ChrW(&H41 + Math.Floor(longit / 20.0))
        qth &= ChrW(&H41 + Math.Floor(latit / 10.0))

        If depth < 2 Then Return qth

        longit = Math.IEEERemainder(longit, 20)
        If longit < 0 Then longit += 20
        latit = Math.IEEERemainder(latit, 10)
        If latit < 0 Then latit += 10

        ' Square, 0-9, 10 values,  2×1 degrees			
        qth &= ChrW(&H30 + Math.Floor(longit / 2))
        qth &= ChrW(&H30 + Math.Floor(latit / 1))

        If depth < 3 Then Return qth

        longit = Math.IEEERemainder(longit, 2)
        If longit < 0 Then longit += 2
        latit = Math.IEEERemainder(latit, 1)
        If latit < 0 Then latit += 1

        ' Subsquare, a-x, 24 values, 5×2.5 minutes
        qth &= ChrW(&H61 + Math.Floor(longit * 12))
        qth &= ChrW(&H61 + Math.Floor(latit * 24))

        If depth < 4 Then Return qth

        longit = Math.IEEERemainder(longit, 1.0 / 12.0)
        If (longit < 0) Then longit += 1.0 / 12.0
        latit = Math.IEEERemainder(latit, 1.0 / 24.0)
        If (latit < 0) Then latit += 1.0 / 24.0

        ' Extended square, 0-9, 10 values, 30×15 seconds
        qth &= ChrW(&H30 + Math.Floor(longit * 12 * 10))
        qth &= ChrW(&H30 + Math.Floor(latit * 24 * 10))

        If depth < 5 Then Return qth

        longit = Math.IEEERemainder(longit, 1.0 / 12.0 / 10.0)
        If (longit < 0) Then longit += 1.0 / 12.0 / 10.0
        latit = Math.IEEERemainder(latit, 1.0 / 24.0 / 10.0)
        If (latit < 0) Then latit += 1.0 / 24.0 / 10.0

        ' Super extended square, A-X, 24 values, 1.25×0.625 seconds
        qth &= ChrW(&H61 + Math.Floor(longit * 12 * 10 * 24))
        qth &= ChrW(&H61 + Math.Floor(latit * 24 * 10 * 24))

        If depth < 6 Then Return qth

        longit = Math.IEEERemainder(longit, 1.0 / 12.0 / 10.0 / 24.0)
        If (longit < 0) Then longit += 1.0 / 12.0 / 10.0 / 24.0
        latit = Math.IEEERemainder(latit, 1.0 / 24.0 / 10.0 / 24.0)
        If (latit < 0) Then latit += 1.0 / 24.0 / 10.0 / 24.0

        ' Super extended subsquare, 0-9, 10 values, 0.125×0.0625 seconds
        qth &= ChrW(&H61 + Math.Floor(longit * 12 * 10 * 24 * 10))
        qth &= ChrW(&H61 + Math.Floor(latit * 24 * 10 * 24 * 10))


        Return qth

    End Function

    ''' <summary>
    '''  creates BasicGeopos from QTH locator
    ''' </summary>
    ''' <param name="qth">QTH locator, between 2 and 5 character pairs</param>
    ''' <returns></returns>
    Public Shared Function FromQTH(qth As String) As BasicGeopos

        qth = qth.Trim().ToUpper
        If Not Regex.IsMatch(qth, "^[A-R]{2}[0-9]{2}") Then Return Empty()

        Dim ret As pkar.BasicGeopos = Empty()
        ret.Longitude = (AscW(qth(0)) - &H41) * 20 - 180 + (AscW(qth(2)) - &H30) * 2
        ret.Latitude = (AscW(qth(1)) - &H41) * 10 - 90 + (AscW(qth(3)) - &H30)

        If Regex.IsMatch(qth, "^[A-R]{2}[0-9]{2}$") Then
            ret.Longitude += 1
            ret.Latitude += 0.5
            Return ret
        End If

        If Not Regex.IsMatch(qth, "^[A-R]{2}[0-9]{2}[A-X]{2}") Then Return Empty()
        ret.Longitude += (AscW(qth(4)) - &H41) / 12
        ret.Latitude += (AscW(qth(5)) - &H41) / 24

        If Regex.IsMatch(qth, "^[A-R]{2}[0-9]{2}[A-X]{2}$") Then
            ret.Longitude += 0.5 / 12
            ret.Latitude += 0.5 / 24
            Return ret
        End If

        If Not Regex.IsMatch(qth, "^[A-R]{2}[0-9]{2}[A-X]{2}[0-9]{2}") Then Return Empty()

        ret.Longitude += (AscW(qth(6)) - &H30) / 120
        ret.Latitude += (AscW(qth(7)) - &H30) / 240

        If Regex.IsMatch(qth, "^[A-R]{2}[0-9]{2}[A-X]{2}[0-9]{2}$") Then
            ret.Longitude += 0.5 / 120
            ret.Latitude += 0.5 / 240
            Return ret
        End If

        If Not Regex.IsMatch(qth, "^[A-R]{2}[0-9]{2}[A-X]{2}[0-9]{2}[A-X]{2}") Then Return Empty()
        ret.Longitude += (AscW(qth(8)) - &H41 + 0.5) / 120 / 24
        ret.Latitude += (AscW(qth(9)) - &H41 + 0.5) / 240 / 24

        If Regex.IsMatch(qth, "^[A-R]{2}[0-9]{2}[A-X]{2}[0-9]{2}[A-X]{2}$") Then
            ret.Longitude += 0.5 / 120 / 24
            ret.Latitude += 0.5 / 240 / 24
        End If

        If Not Regex.IsMatch(qth, "^[A-R]{2}[0-9]{2}[A-X]{2}[0-9]{2}[A-X]{2}[0-9]{2}") Then Return Empty()
        ret.Longitude += (AscW(qth(10)) - &H30) / 120 / 24 / 10
        ret.Latitude += (AscW(qth(11)) - &H30) / 240 / 24 / 10

        ' środkujemy ten poziom
        ret.Longitude += 0.5 / 120 / 24 / 10
        ret.Latitude += 0.5 / 240 / 24 / 10

        Return ret
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
    ''' Try to copy Latitude, Longitude and Altitude to any .Net object (to Properties).
    ''' It can be .Net Framework GeoCoordinate, or MAUI Location (or even your own types)
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

        ' to niestety nie działa - nie zmienia...
        Dim fld As FieldInfo

        fld = anyObject.GetType.GetRuntimeField("Latitude")
        If fld IsNot Nothing Then fld.SetValue(anyObject, Latitude)

        fld = anyObject.GetType.GetRuntimeField("Longitude")
        If fld IsNot Nothing Then fld.SetValue(anyObject, Longitude)

        fld = anyObject.GetType.GetRuntimeField("Altitude")
        If fld IsNot Nothing Then fld.SetValue(anyObject, Altitude)

    End Sub

#End Region

#Region "center and corners from list"
    ''' <summary>
    ''' Return GeoCenter point for list of locations (using Latitude, Longitude and Altitude)
    ''' </summary>
    Public Shared Function GetCenter(locations As List(Of BasicGeopos)) As BasicGeopos
        Return GetCornersAndCenter(locations).Item(2)
    End Function

    ''' <summary>
    ''' return NorthWest and SouthEast corners for list of locations. Also return altitude minimum (in NW) and maximum (in SE). It can be used for creating UWP GeoboundingBox.
    ''' </summary>
    Public Shared Function GetCorners(locations As List(Of BasicGeopos)) As List(Of BasicGeopos)

        Dim oSE As New BasicGeopos(90, -180, -6378000)
        Dim oNW As New BasicGeopos(-90, 360, 100000)

        For Each loc As BasicGeopos In locations
            oNW.Altitude = Math.Min(oNW.Altitude, loc.Altitude)
            oNW.Latitude = Math.Max(oNW.Latitude, loc.Latitude)
            oNW.Longitude = Math.Min(oNW.Longitude, loc.Longitude)

            oSE.Altitude = Math.Max(oSE.Altitude, loc.Altitude)
            oSE.Latitude = Math.Min(oSE.Latitude, loc.Latitude)
            oSE.Longitude = Math.Max(oSE.Longitude, loc.Longitude)

        Next

        Return New List(Of BasicGeopos) From {oNW, oSE}

    End Function

    ''' <summary>
    ''' return NorthWest, SouthEast corners and center point for list of locations. Also return altitude minimum (in NW) and maximum (in SE)
    ''' </summary>
    Public Shared Function GetCornersAndCenter(locations As List(Of BasicGeopos)) As List(Of BasicGeopos)

        Dim corners As List(Of BasicGeopos) = GetCorners(locations)
        Dim GeoNW As BasicGeopos = corners.Item(0)
        Dim GeoSE As BasicGeopos = corners.Item(1)

        Dim GeoCenter As New BasicGeopos(
            GeoSE.Latitude + (GeoNW.Latitude - GeoSE.Latitude) / 2,
            GeoNW.Longitude + (GeoSE.Longitude - GeoNW.Longitude) / 2,
            GeoNW.Altitude + (GeoSE.Altitude - GeoNW.Altitude) / 2)


        Return New List(Of BasicGeopos) From {GeoNW, GeoSE, GeoCenter}

    End Function

#End Region

#Region "some EXIF related"

    ''' <summary>
    ''' create BasicGeopos from EXIF-format string 
    ''' </summary>
    ''' <param name="exifString">string formatted as "±lat±lon±alt", can be suffixed with "/"</param>
    ''' <returns>BasicGeopos with data from string, or NULL</returns>
    Public Shared Function FromExifString(exifString As String) As BasicGeopos
        Dim iInd As Integer = exifString.IndexOfAny({"+", "-"}, 2)
        If iInd < 1 Then Return Nothing
        exifString = exifString.Replace("/", "") ' nie wiem po co on tam jest, ale jest

        Dim sLat As String = exifString.Substring(0, iInd)
        Dim sLon As String = exifString.Substring(iInd)
        iInd = sLon.IndexOfAny({"+", "-"}, 2)
        Dim sAlt As String = "0"
        If iInd > 0 Then
            sAlt = sLon.Substring(iInd)
            sLon = sLon.Substring(0, iInd)
        End If
        Return New BasicGeopos(sLat, sLon, sAlt)
    End Function
#End Region


#Region "geowiki"

    ' https://www.mediawiki.org/wiki/Extension:GeoData

    ''' <summary>
    ''' Create link for querying wikipedia by geo - use this function if you want to make HttpClient's stuff by yourself (e.g. from own http pool)
    ''' </summary>
    ''' <param name="lang">Two letter lang code (prefix for wikipedia.org), e.g. 'en' or 'pl'</param>
    ''' <param name="radiusMeters">Max distance, throws if more than 10 km or if less than 0</param>
    ''' <param name="count">Limit of returned matches (if you specify more than 20, it would be reduced to 20)</param>
    ''' <returns></returns>
    Public Function GeoWikiGetQueryUri(lang As String, Optional radiusMeters As Integer = 500, Optional count As Integer = 10) As Uri
        count = Math.Min(count, 20)
        If count < 1 Then Throw New ArgumentException("count should be >=1")
        radiusMeters = Math.Abs(radiusMeters)
        If radiusMeters = 0 Then Throw New ArgumentException("radius cannot be 0")
        If radiusMeters > 10000 Then Throw New ArgumentException("radius cannot be greater than 10k")

        Return New Uri($"https://{lang}.wikipedia.org/w/api.php?action=query&list=geosearch&gscoord={Latitude}|{Longitude}&gsradius={radiusMeters}&gslimit={count}&format=json")
    End Function

    ''' <summary>
    ''' Convert wikipedia JSON response to list - use this function if you want to make HttpClient's stuff by yourself (e.g. from own http pool)
    ''' </summary>
    ''' <param name="page">JSON response</param>
    ''' <param name="lang">Two letter lang code (prefix for wikipedia.org), e.g. 'en' or 'pl'</param>
    ''' <param name="sortmode">How to sort data</param>
    ''' <returns></returns>
    Public Function GeoWikiImport(page As String, lang As String, Optional sortmode As GeoWikiSort = GeoWikiSort.None) As List(Of GeoWikiItem)

        '{
        '"batchcomplete": "",
        '"query": {
        '    "geosearch": [
        '        {},{}]}}

        Dim iInd As Integer = page.IndexOf("[")
        If iInd < 1 Then Return Nothing
        page = page.Substring(iInd)
        iInd = page.LastIndexOf("]")
        If iInd < 1 Then Return Nothing
        page = page.Substring(0, iInd + 1)

        Dim lista As New pkar.BaseList(Of GeoWikiJSONitem)(Nothing)
        lista.Import(page)

        Dim ret As New List(Of GeoWikiItem)
        For Each oItem As GeoWikiJSONitem In lista
            ret.Add(oItem.AsGeoWikiItem(lang))
        Next

        Select Case sortmode
            Case GeoWikiSort.Name
                Return ret.OrderBy(Of String)(Function(x) x.title).ToList
            Case GeoWikiSort.Distance
                Return ret.OrderBy(Of Integer)(Function(x) x.geo.Radius).ToList
            Case Else
                Return ret
        End Select

    End Function

    ' kopia z pkarlibmodule
    Private Shared moHttp As Net.Http.HttpClient
    '    Private Const msDefaultHttpAgent As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4321.0 Safari/537.36 Edg/88.0.702.0"
    Private Const msDefaultHttpAgent As String = "pkar.BasicGeopos Nuget"

    Private Shared Async Function GeoWikiGetResultPageAsync(link As Uri) As Task(Of String)
        If moHttp Is Nothing Then
            moHttp = New Net.Http.HttpClient()
            moHttp.DefaultRequestHeaders.UserAgent.TryParseAdd(msDefaultHttpAgent)
            'moHttp.DefaultRequestHeaders.Accept.Clear()
            'moHttp.DefaultRequestHeaders.Accept.Add(New Http.Headers.MediaTypeWithQualityHeaderValue())
        End If

        Try
            Return Await moHttp.GetStringAsync(link)
        Catch
        End Try

        Return ""

    End Function


    ''' <summary>
    ''' Query wikipedia by geo
    ''' </summary>
    ''' <param name="lang">Two letter lang code (prefix for wikipedia.org), e.g. 'en' or 'pl'</param>
    ''' <param name="radiusMeters">Max distance, throws if more than 10 km or if less than 0</param>
    ''' <param name="count">Limit of returned matches (if you specify more than 20, it would be reduced to 20)</param>
    ''' <param name="sortmode">How to sort data</param>
    ''' <returns></returns>
    Public Async Function GeoWikiGetItemsAsync(lang As String, Optional radiusMeters As Integer = 500, Optional count As Integer = 10, Optional sortmode As GeoWikiSort = GeoWikiSort.None) As Task(Of List(Of GeoWikiItem))

        Dim link As Uri = GeoWikiGetQueryUri(lang, radiusMeters, count)
        Dim page As String = Await GeoWikiGetResultPageAsync(link)

        Return GeoWikiImport(page, lang, sortmode)

    End Function

    ' https://en.wikipedia.org/?curid=16699231


    Public Enum GeoWikiSort
        None
        Distance
        Name
    End Enum

    Protected Class GeoWikiJSONitem
        Public Property pageid As Integer
        'Public Property ns As Integer
        Public Property title As String
        Public Property lat As Double
        Public Property lon As Double
        Public Property dist As Double
        Public Property primary As String

        Public Function AsGeoWikiItem(lang As String) As GeoWikiItem
            Dim ret As New GeoWikiItem
            ret.pageid = pageid
            ret.title = title
            ret.pageUri = New Uri($"https://{lang}.wikipedia.org/?curid={pageid}")
            ret.primary = primary
            ret.geo = New BasicGeoposWithRadius(New BasicGeopos(lat, lon), dist)
            ret.lang = lang
            Return ret
        End Function
    End Class

    Public Class GeoWikiItem
            Public Property pageid As Integer
            Public Property title As String
            Public Property pageUri As Uri
            Public Property geo As BasicGeoposWithRadius
        Public Property lang As String
        ''' <summary>
        ''' primary coordinates define article subject's location, while secondary coordinates are other coordinates mentioned in the article. There can be only one primary coordinate per article, but as many secondaries as you like
        ''' </summary>
        ''' <returns></returns>
        Public Property primary As String

        End Class



#End Region


    End Class

