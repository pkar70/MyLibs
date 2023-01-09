
 This is library for manipulating geolocation inside ClassLibs - UWP has BasicGeolocation, Android and MAUI has Location, but in .Net we have nothing.

# constructor

    new BasicGeo(latitude, longitude, altitude = 0)

# various distance metering

    Function DistanceTo(dLatitude As Double, dLongitude As Double) As Double
    Function DistanceTo(oGeoPos As BasicGeopos) As Double
    Function DistanceKmTo(oGeoPos As BasicGeopos) As Double

    Function IsNear(oGeoPos As BasicGeopos, distanceMeters As Double) As Boolean
    Operator -(ByVal oGeoPos0 As BasicGeopos, ByVal oGeoPos1 As BasicGeopos) As Double

# checking if BasicGeopos is inside various areas

    Function IsLatitudeBetween(value0 As Double, value1 As Double) As Boolean
    Function IsLongitudeBetween(value0 As Double, value1 As Double) As Boolean

    Function IsInsideRectangle(latMin As Double, latMax As Double, lonMin As Double, lonMax As Double) As Boolean
    Function IsInsideRectangle(oGeoPos0 As BasicGeopos, oGeoPos1 As BasicGeopos) As Boolean

    Function IsInsideCircle(center As BasicGeopos, radius As Double)

# get centers of some regions

 You can treat it as another form of constructors

    GetEuropeCenter()   // "Mainland Europe"
    GetEUCenter()       // "In Europe, not including overseas territories"
    GetPolandCenter()
    GetKrakowCenter()
    Empty() // <=> (0,-150), middle of ocean; to make some scenarios easier

# tests if we are inside one of these

 These tests are done as IsInsideRectangle, not IsInsideCircle

    Function IsInsideEurope() As Boolean
    Function IsInsideEU() As Boolean
    Function IsInsidePoland() As Boolean
    Function IsInsideKrakow() As Boolean

    Function IsEmpty() As Boolean   // test if current BasicGeopos is Empty one



# serialization etc.

 Helpers, to avoid warnings that ToString should have CultureInfo,  etc. Default precision is 5 digits, it is about 10 meters.

    Function StringLat(Optional iDigits As Integer = 5) As String
    Function StringLon(Optional iDigits As Integer = 5) As String

    Function FormatLink(sBaseLink As String) As String  // replace %lat , %long with values
    Function ToOSMLink(Optional zoom As Integer = 16)

## DMS (degree, minute, second)

    Function StringLatDM(Optional sFormat As String = "%d°%m′%s″", Optional iDigits As Integer = 5) As String
    Function StringLonDM(Optional sFormat As String = "%d°%m′%s″", Optional iDigits As Integer = 5) As String

### and constructors
    FromDMS(latD As Integer, latM As Double, latS As Double, latSN As String, lonD As Integer, lonM As Double, lonS As Double, lonEW As String) 
    FromDM(latD As Integer, latM As Double, latSW As String, lonD As Integer, lonM As Double, lonEW As String)
    FromDM(latD As Double, latSW As String, lonD As Double, lonEW As String)


