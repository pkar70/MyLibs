
 This is library for manipulating geolocation inside ClassLibs - UWP has BasicGeolocation, Android and MAUI has Location, but in .Net we have nothing.
 For file that can be included in your project to map between UWP structs/classes, see https://github.com/pkar70/MyLibs/blob/master/UWPappTel/UwpGeopos.vb (it cannot be packed to Nuget, as UWP Runtime Libraries doesn't allow types defined outside of WinRT)


# constructor (and similar)

    new BasicGeo(latitude, longitude, altitude = 0) // args are validated since v1.1.0
    new BasicGeo(latitude As String, longitude As String, altitude  As String = "0") // since v1.2.4
    new BasicGeo()  // since v1.2.6
    Function Clone  // [since v1.1.0]
    Function FromObject(anyObject as Object)    // tries to extract data from given object [since v1.1.0]
    Function FromLink(baselink, link)   // tries to create BasicGeopos from map link [since v.1.2.1]
    Function FromOSMLink(link)   // tries to create BasicGeopos from OSM link [since v.1.2.1]
    FromExifString(String) // tries to create BasicGepos from EXIF formatted string [since v1.2.7]

# various distance metering

    Function DistanceTo(dLatitude As Double, dLongitude As Double) As Double
    Function DistanceTo(oGeoPos As BasicGeopos) As Double
    Function DistanceKmTo(dLatitude As Double, dLongitude As Double) As Double // [since 1.1.2]
    Function DistanceKmTo(oGeoPos As BasicGeopos) As Double

    Function IsNear(oGeoPos As BasicGeopos, distanceMeters As Double) As Boolean
    Operator -(ByVal oGeoPos0 As BasicGeopos, ByVal oGeoPos1 As BasicGeopos) As Double

# checking if BasicGeopos is inside various areas

    Function IsLatitudeBetween(value0 As Double, value1 As Double) As Boolean
    Function IsLongitudeBetween(value0 As Double, value1 As Double) As Boolean

    Function IsInsideRectangle(latMin As Double, latMax As Double, lonMin As Double, lonMax As Double) As Boolean
    Function IsInsideRectangle(oGeoPos0 As BasicGeopos, oGeoPos1 As BasicGeopos) As Boolean

    Function IsInsideCircle(center As BasicGeopos, radius As Double) As Boolean

# get centers of some regions

 You can treat it as another form of constructors

    GetEuropeCenter()   // "Mainland Europe"
    GetEUCenter()       // "In Europe, not including overseas territories"
    GetPolandCenter()
    GetKrakowCenter()
    Empty() // <=> (0,-150), middle of ocean; to make some scenarios easier
    GetCenter(locations As List(Of BasicGeopos))    // since 1.2.3

# tests if we are inside one of these

 These tests are done as IsInsideRectangle, not IsInsideCircle

    Function IsInsideEurope() As Boolean
    Function IsInsideEU() As Boolean
    Function IsInsidePoland() As Boolean
    Function IsInsideKrakow() As Boolean

    Function IsEmpty() As Boolean   // test if current BasicGeopos is Empty one

# working with other .Net geolocation types

    Function FromObject(anyObject as Object)    // tries to extract data from given object [since v1.1.0]
    Sub CopyTo(anyObject as Object)             // tries to copy data to given object [since v1.2.1]

# serialization etc.

 Helpers, to avoid warnings that ToString should have CultureInfo,  etc. Default precision is 5 digits, it is about 10 meters.

    Function StringLat(Optional iDigits As Integer = 5) As String
    Function StringLon(Optional iDigits As Integer = 5) As String
    Function StringAlt() As String // since 1.2.4

    Function FormatLink(sBaseLink As String) As String  // replace %lat , %lon with values; since 1.2.1 also %alt
    Function FormatLink(sBaseLink As String, zoomLevel) As String  // as above, but also %zoom [since 1.2.1]
    Function ToOSMLink(Optional zoom As Integer = 16) As String
    Function ToOSMUri(Optional zoom As Integer = 16) As Uri [since 1.2.1]
    Function DumpAsJson() As String // dump as one-line JSON token [since v1.1.0]


    Shared MapServices As Dictionary(Of String, String)    // since 1.2.4
    Function ToLink(mapService As String, Optional zoom As Integer = 16) As String // since 1.2.4
    Function ToUri(mapService As String, Optional zoom As Integer = 16) As Uri // since 1.2.4
    Function GetFromLink(link As String) As BasicGeopos // since 1.2.4

## DMS (degree, minute, second)

    Function StringLatDM(Optional sFormat As String = "%d°%m′%s″", Optional iDigits As Integer = 5) As String
    Function StringLonDM(Optional sFormat As String = "%d°%m′%s″", Optional iDigits As Integer = 5) As String
    Functoin ToStringDM(format, Optional iDigits = 5) As String

### and constructors
    FromDMS(latD As Integer, latM As Double, latS As Double, latSN As String, lonD As Integer, lonM As Double, lonS As Double, lonEW As String) 
    FromDM(latD As Integer, latM As Double, latSW As String, lonD As Integer, lonM As Double, lonEW As String)
    FromDM(latD As Double, latSW As String, lonD As Double, lonEW As String)

# Working with BasicGeopos lists

 These functions are not related to one geopoint, so they are defined as shared/static.

    Public Shared Function GetCenter(locations As List(Of BasicGeopos)) As BasicGeopos  // [since v1.2.3]
    Public Shared Function GetCorners(locations As List(Of BasicGeopos)) As List(Of BasicGeopos)    // [since v1.2.3]
    Public Shared Function GetCornersAndCenter(locations As List(Of BasicGeopos)) As List(Of BasicGeopos)   // [since v1.2.3]

