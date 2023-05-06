Public Module GeoposExtensions

#Region "BasicGeoposition"


    ' cast operator, w obie strony, bo wszystko przechodzi
    <Extension()>
    Public Function ToMyGeopos(ByVal oPos As Windows.Devices.Geolocation.BasicGeoposition) As BasicGeopos
        Return New BasicGeopos(oPos.Latitude, oPos.Longitude)
    End Function

    <Extension()>
    Public Function ToWinGeopos(ByVal oPos As BasicGeopos) As Windows.Devices.Geolocation.BasicGeoposition
        Dim oPoint As New Windows.Devices.Geolocation.BasicGeoposition With
            {
                .Latitude = oPos.Latitude,
                .Longitude = oPos.Longitude,
                .Altitude = oPos.Altitude
            }
        Return oPoint
    End Function

#End Region

#Region "Geopoint"

    <Extension()>
    Public Function ToWinGeopoint(ByVal oPos As BasicGeopos) As Windows.Devices.Geolocation.Geopoint
        Return New Windows.Devices.Geolocation.Geopoint(oPos.ToWinGeopos())
    End Function

#End Region




    <Extension()>
    Public Function DistanceTo(ByVal oGeocoord0 As Windows.Devices.Geolocation.Geocoordinate, oGeocoord1 As Windows.Devices.Geolocation.Geocoordinate) As Double
        Return oGeocoord0.Point.Position.ToMyGeopos().DistanceTo(oGeocoord1.Point.Position.ToMyGeopos())
    End Function

    <Extension()>
    Public Function DistanceTo(ByVal oGeopos0 As Windows.Devices.Geolocation.Geoposition, oGeopos1 As Windows.Devices.Geolocation.Geoposition) As Double
        Return oGeopos0.Coordinate.DistanceTo(oGeopos1.Coordinate)
    End Function


    <Extension()>
    Public Function DistanceTo(ByVal oGeopos0 As Windows.Devices.Geolocation.BasicGeoposition, oGeopos1 As Windows.Devices.Geolocation.BasicGeoposition) As Integer
        Return oGeopos0.ToMyGeopos.DistanceTo(oGeopos1.ToMyGeopos)
    End Function




End Module
