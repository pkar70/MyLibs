Imports pkar
Imports WinGeo = Windows.Devices.Geolocation
Imports WinXAML = Windows.UI.Xaml.Controls

Public Module GeoposExtensions

#Region "konwersje"

#Region "BasicGeoposition"

    ' direct konwersja (jako metody)

    <Extension()>
    Public Function ToBasicGeopos(ByVal oPos As WinGeo.BasicGeoposition) As BasicGeopos
        Return New BasicGeopos(oPos.Latitude, oPos.Longitude)
    End Function

    <Extension()>
    Public Function ToWinGeopos(ByVal oPos As BasicGeopos) As WinGeo.BasicGeoposition
        Dim oPoint As New WinGeo.BasicGeoposition With
            {
                .Latitude = oPos.Latitude,
                .Longitude = oPos.Longitude,
                .Altitude = oPos.Altitude
            }
        Return oPoint
    End Function

    ' nie da się zrobić Extension CAST
    'Public Shared Widening Operator CType(oPos As WinGeo.BasicGeoposition) As pkar.BasicGeopos
    '    Return pkar.BasicGeopos.Empty
    'End Operator

#End Region

#Region "Geopoint"

    ' z utratą części danych

    <Extension()>
    Public Function ToBasicGeopos(ByVal oPos As WinGeo.Geopoint) As BasicGeopos
        Return oPos.Position.ToBasicGeopos
    End Function


    <Extension()>
    Public Function ToWinGeopoint(ByVal oPos As BasicGeopos) As WinGeo.Geopoint
        Return New WinGeo.Geopoint(oPos.ToWinGeopos())
    End Function


#End Region

#Region "Geocoordinate"

    ' z utratą części danych

    <Extension()>
    Public Function ToBasicGeopos(ByVal oPos As WinGeo.Geocoordinate) As BasicGeopos
        Return oPos.Point.ToBasicGeopos
    End Function

    ' nie ma możliwości uzyskania new Geocoordinate

#End Region

#Region "Geocircle"

    ' z utratą części danych

    <Extension()>
    Public Function ToBasicGeopos(ByVal oPos As WinGeo.Geocircle) As BasicGeopos
        Return oPos.Center.ToBasicGeopos
    End Function
    <Extension()>
    Public Function ToWinGeocircle(ByVal oPos As BasicGeopos, radius As Double, Optional altitudeReferenceSystem As WinGeo.AltitudeReferenceSystem = WinGeo.AltitudeReferenceSystem.Unspecified) As WinGeo.Geocircle
        Return New WinGeo.Geocircle(oPos.ToWinGeopos, radius)
    End Function


#End Region

#Region "Geoposition"

    ' z utratą części danych

    <Extension()>
    Public Function ToBasicGeopos(ByVal oPos As WinGeo.Geoposition) As BasicGeopos
        Return oPos.Coordinate.ToBasicGeopos
    End Function

    ' nie ma możliwości uzyskania new Geoposition

#End Region

#Region "Geovisit"

    ' z utratą części danych

    <Extension()>
    Public Function ToBasicGeopos(ByVal oPos As WinGeo.Geovisit) As BasicGeopos
        Return oPos.Position.ToBasicGeopos
    End Function

    ' nie ma możliwości uzyskania new

#End Region

#Region "Geofence"

    ' z utratą części danych

    <Extension()>
    Public Function ToBasicGeopos(ByVal oPos As WinGeo.Geofencing.Geofence) As BasicGeopos
        Dim temp = TryCast(oPos.Geoshape, WinGeo.Geocircle)
        If temp Is Nothing Then Return Nothing
        Return temp.ToBasicGeopos
    End Function

    <Extension()>
    Public Function ToWinGeofence(ByVal oPos As BasicGeopos, id As String, radius As Double, Optional altitudeReferenceSystem As WinGeo.AltitudeReferenceSystem = WinGeo.AltitudeReferenceSystem.Unspecified) As WinGeo.Geofencing.Geofence
        Return New WinGeo.Geofencing.Geofence(id, oPos.ToWinGeocircle(radius, altitudeReferenceSystem))
    End Function

#End Region

#Region "Geopath"

    ' z utratą części danych

    <Extension()>
    Public Function ToBasicGeoposList(ByVal oPos As WinGeo.Geopath) As List(Of BasicGeopos)
        Dim ret As New List(Of BasicGeopos)

        For Each oItem In oPos.Positions
            ret.Add(oItem.ToBasicGeopos)
        Next

        Return ret
    End Function

    <Extension()>
    Public Function ToWinGeopath(ByVal oPos As List(Of BasicGeopos), Optional altitudeReferenceSystem As WinGeo.AltitudeReferenceSystem = WinGeo.AltitudeReferenceSystem.Unspecified) As WinGeo.Geopath
        Dim path As New List(Of WinGeo.BasicGeoposition)

        For Each oItem In oPos
            path.Add(oItem.ToWinGeopos)
        Next

        Return New WinGeo.Geopath(path, altitudeReferenceSystem)
    End Function

#End Region

#Region "MapIcon"
    ' z utratą części danych

    <Extension()>
    Public Function ToBasicGeopos(ByVal oPos As Windows.UI.Xaml.Controls.Maps.MapIcon) As BasicGeopos
        Return oPos.Location.ToBasicGeopos
    End Function
    <Extension()>
    Public Function ToWinMapIcon(ByVal oPos As BasicGeopos) As Windows.UI.Xaml.Controls.Maps.MapIcon
        Return New Windows.UI.Xaml.Controls.Maps.MapIcon With {
            .Location = oPos.ToWinGeopoint
        }
    End Function
#End Region

#End Region

#Region "odleglosci"

    <Extension()>
    Public Function DistanceTo(ByVal oPos As WinGeo.BasicGeoposition, oPos1 As WinGeo.BasicGeoposition) As Double
        Return oPos.ToBasicGeopos.DistanceTo(oPos1.ToBasicGeopos)
    End Function

    <Extension()>
    Public Function DistanceTo(ByVal oPos As WinGeo.Geopoint, oPos1 As WinGeo.Geopoint) As Double
        Return oPos.ToBasicGeopos.DistanceTo(oPos1.ToBasicGeopos)
    End Function


    <Extension()>
    Public Function DistanceTo(ByVal oPos As WinGeo.Geocoordinate, oPos1 As WinGeo.Geocoordinate) As Double
        Return oPos.ToBasicGeopos.DistanceTo(oPos1.ToBasicGeopos)
    End Function

    <Extension()>
    Public Function DistanceTo(ByVal oPos As WinGeo.Geocircle, oPos1 As WinGeo.Geocircle) As Double
        Return oPos.ToBasicGeopos.DistanceTo(oPos1.ToBasicGeopos)
    End Function

    <Extension()>
    Public Function DistanceTo(ByVal oPos As WinGeo.Geoposition, oPos1 As WinGeo.Geoposition) As Double
        Return oPos.ToBasicGeopos.DistanceTo(oPos1.ToBasicGeopos)
    End Function

    <Extension()>
    Public Function DistanceTo(ByVal oPos As WinGeo.Geovisit, oPos1 As WinGeo.Geovisit) As Double
        Return oPos.ToBasicGeopos.DistanceTo(oPos1.ToBasicGeopos)
    End Function

    <Extension()>
    Public Function DistanceTo(ByVal oPos As WinGeo.Geofencing.Geofence, oPos1 As WinGeo.Geofencing.Geofence) As Double
        Return oPos.ToBasicGeopos.DistanceTo(oPos1.ToBasicGeopos)
    End Function


    <Extension()>
    Public Function DistanceTo(ByVal oPos As WinXAML.Maps.MapIcon, oPos1 As WinXAML.Maps.MapIcon) As Double
        Return oPos.ToBasicGeopos.DistanceTo(oPos1.ToBasicGeopos)
    End Function

#End Region

End Module
