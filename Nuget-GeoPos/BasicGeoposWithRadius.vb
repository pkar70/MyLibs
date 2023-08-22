Public Class BasicGeoposWithRadius
    Inherits BasicGeopos

    ''' <summary>
    ''' radius in meters
    ''' </summary>
    Public Property Radius As Double

    ''' <summary>
    ''' creates new Geopos with radius = 100 m or 20 km
    ''' </summary>
    ''' <param name="coarse">True if radius should be 20 km, or False if it should be 100 m</param>
    Public Sub New(geopos As pkar.BasicGeopos, coarse As Boolean)
        MyBase.New(geopos.Latitude, geopos.Longitude)
        Radius = If(coarse, 20000, 100)
    End Sub

    Public Sub New(geopos As pkar.BasicGeopos, radiusInMeters As Double)
        MyBase.New(geopos.Latitude, geopos.Longitude)
        Radius = radiusInMeters
    End Sub

    ''' <summary>
    ''' True if distance between centers is less than sum of radiuses
    ''' </summary>
    Public Overloads Function IsInsideCircle(center As BasicGeoposWithRadius) As Boolean
        Return MyBase.IsInsideCircle(center, center.Radius + Radius)
    End Function

    ''' <summary>
    ''' True if distance between centers is less than radius
    ''' </summary>
    Public Overloads Function IsInsideCircle(center As BasicGeopos) As Boolean
        Return MyBase.IsInsideCircle(center, Radius)
    End Function

End Class