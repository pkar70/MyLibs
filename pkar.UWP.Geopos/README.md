
This Nuget is a UWP wrapper for plain .Net nuget, pkar.BasicGeopos.
It provides conversion between UWP geolocation structures and BasicGeopos.





can be used to make "debug dumps" of Bluetooth devices, and also has some helpers.

# BThelpers
 All methods are static (shared), so can be called without instantiation.

    Async Function IsBTavailableAsync() As Task(Of Integer)
    Async Function TrySwitchBTAsync(bOn As Boolean) As Task(Of Boolean)

Rest of methods returns object or NULL when error occurs. In this case, error message is set:

    errorMessage As String = ""

    Async Function GetDeviceAsync(deviceMAC As ULong) As Task(Of BluetoothLEDevice)

    Async Function GetServiceAsync(oDevice As BluetoothLEDevice, serviceGuid As String) As Task(Of GattDeviceService)
    Async Function GetDeviceServiceAsync(deviceMAC As ULong, serviceGuid As String) As Task(Of GattDeviceService)

    Async Function GetCharacteristicAsync(service As GattDeviceService, charGuid As String) As Task(Of GattCharacteristic)
    Async Function GetServiceCharacteristicAsync(oDevice As BluetoothLEDevice, serviceGuid As String, charGuid As String) As Task(Of GattCharacteristic)
    Async Function GetDeviceServiceCharacteristicAsync(deviceMAC As ULong, serviceGuid As String, charGuid As String) As Task(Of GattCharacteristic)


# BTextensions

 All extensions are meant to return debug string, with dumped object data.

    ToDebugString(ByVal oBuf As Windows.Storage.Streams.IBuffer, iMaxLen As Integer)
    ToDebugString(ByVal oBuf As Windows.Storage.Streams.IBuffer)** // same as above, but iMaxLen is set to 16
    ToDebugString(ByVal oAdv As BluetoothLEAdvertisement)
    ToDebugStringAsync(ByVal oDescriptor As GattDescriptor)
    ToDebugString(ByVal oProp As GattCharacteristicProperties)
    ToDebugStringAsync(ByVal oChar As GattCharacteristic)
    ToDebusStringAsync(ByVal oServ As GattDeviceService)
    ToDebusStringAsync(ByVal oDevice As BluetoothLEDevice)**

** means DefaultOverload, the one and only visible in JavaScript.

This Nuget uses my DotNetExtensions (for dumping GUIDs). I moved dumping GUIDs to DotNetExtensions, as DotNetExtensions is universal (uses .Net Standard 1.4), and this Nuget is only for UWP.