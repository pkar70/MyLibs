
Imports pkar.DotNetExtensions
Imports Winradio = Windows.Devices.Radios
Imports WinBT = Windows.Devices.Bluetooth
Imports System.Reflection.PortableExecutable

Public NotInheritable Class BThelpers

#Region "BT radio"

    ''' <summary>
    ''' Checks if Bluetooth is available
    ''' </summary>
    ''' <returns>-1 means no radio, 0 means radio is present, but off, and 1 means radio is present and on</returns>
    Public Shared Function IsBTavailableAsync() As IAsyncOperation(Of Integer)
        Return IsBTavailableAsyncTask.AsAsyncOperation
    End Function

    Private Shared Async Function IsBTavailableAsyncTask() As Task(Of Integer)

        Dim oRadios As IReadOnlyList(Of Winradio.Radio) = Await Winradio.Radio.GetRadiosAsync()

        Dim bHasBT As Boolean = False

        For Each oRadio As Winradio.Radio In oRadios
            If oRadio.Kind = Winradio.RadioKind.Bluetooth Then
                If oRadio.State = Winradio.RadioState.On Then Return 1
                bHasBT = True
            End If
        Next

        If bHasBT Then Return 0
        Return -1
    End Function

    ''' <summary>
    ''' Tries to set Bluetooth state to bOn (True: on, False: off). App should have devCapabilities "Radios" set in manifest.
    ''' This should be called from UI thread, as 
    ''' </summary>
    ''' <param name="bOn">True if we want to switch Bluetooth on, False if switching off</param>
    ''' <returns>True if switching is successful (or not needed)</returns>
    Public Shared Function TrySwitchBTAsync(bOn As Boolean) As IAsyncOperation(Of Boolean)
        Return TrySwitchBTAsyncTask(bOn).AsAsyncOperation
    End Function
    Private Shared Async Function TrySwitchBTAsyncTask(bOn As Boolean) As Task(Of Boolean)
        Dim iCurrState As Integer = Await IsBTavailableAsync()
        If iCurrState = -1 Then Return False

        ' jeśli nie trzeba przełączać... 
        If bOn AndAlso iCurrState = 1 Then Return True
        If Not bOn AndAlso iCurrState = 0 Then Return True

        Try
            ' czy mamy prawo przełączyć? (devCap=radios)
            Dim result222 As Winradio.RadioAccessStatus = Await Winradio.Radio.RequestAccessAsync()
            If result222 <> Winradio.RadioAccessStatus.Allowed Then Return False
        Catch ex As Exception
            ' no permission
            Return False
        End Try


        Dim radios As IReadOnlyList(Of Winradio.Radio) = Await Winradio.Radio.GetRadiosAsync()

        For Each oRadio As Winradio.Radio In radios
            If oRadio.Kind = Winradio.RadioKind.Bluetooth Then
                Dim oStat As Winradio.RadioAccessStatus
                If bOn Then
                    oStat = Await oRadio.SetStateAsync(Winradio.RadioState.On)
                Else
                    oStat = Await oRadio.SetStateAsync(Winradio.RadioState.Off)
                End If
                If oStat <> Winradio.RadioAccessStatus.Allowed Then Return False
            End If
        Next

        Return True
    End Function

#End Region


#Region "Bluetooth debugs"

    ''' <summary>
    ''' This is "last error message" from GetDeviceServiceCharacteristic
    ''' </summary>
    Public Shared Property errorMessage As String = ""

#Region "Device"

    ''' <summary>
    ''' Get Device with MAC address, or set errorMessage
    ''' </summary>
    ''' <param name="deviceMAC"></param>
    ''' <returns>Device, or NULL in case of error. See errorMessage for error message.</returns>
    Public Shared Function GetDeviceAsync(deviceMAC As ULong) As IAsyncOperation(Of WinBT.BluetoothLEDevice)
        Return GetDeviceAsyncTask(deviceMAC).AsAsyncOperation
    End Function
    Private Shared Async Function GetDeviceAsyncTask(deviceMAC As ULong) As Task(Of WinBT.BluetoothLEDevice)

        errorMessage = ""

        Dim oDev As WinBT.BluetoothLEDevice
        oDev = Await WinBT.BluetoothLEDevice.FromBluetoothAddressAsync(deviceMAC)
        If oDev Is Nothing Then
            errorMessage = "GetDeviceServiceCharacteristic called, cannot get device for deviceMAC = " & deviceMAC.ToHexBytesString
            Return Nothing
        End If

        Return oDev
    End Function

#End Region

#Region "Service"
    ''' <summary>
    ''' Get GattDeviceService, or set errorMessage. Similar to obsoleted device.GetGattService 
    ''' </summary>
    ''' <returns>Service, or NULL in case of error. See errorMessage for error message.</returns>
    Public Shared Function GetServiceAsync(oDevice As WinBT.BluetoothLEDevice,
                                      serviceGuid As String) As IAsyncOperation(Of WinBT.GenericAttributeProfile.GattDeviceService)
        Return GetServiceAsyncTask(oDevice, serviceGuid).AsAsyncOperation
    End Function

    Private Shared Async Function GetServiceAsyncTask(oDevice As WinBT.BluetoothLEDevice,
                                      serviceGuid As String) As Task(Of IAsyncOperation(Of WinBT.GenericAttributeProfile.GattDeviceService))

        errorMessage = ""

        If oDevice Is Nothing Then
            errorMessage = "GetDeviceServiceCharacteristic called with oDevice = null"
            Return Nothing
        End If

        Dim oSrv = Await oDevice.GetGattServicesAsync
        If oSrv.Status <> WinBT.GenericAttributeProfile.GattCommunicationStatus.Success Then
            errorMessage = "GetDeviceServiceCharacteristic:GetGattServicesAsync.Status = " & oSrv.Status.ToString
            Return Nothing
        End If

        Dim oSvc As WinBT.GenericAttributeProfile.GattDeviceService = Nothing
        For Each oSv In oSrv.Services
            If oSv.Uuid.ToString = serviceGuid.ToLower Then
                oSvc = oSv
            End If
        Next
        If oSvc Is Nothing Then
            errorMessage = "GetDeviceServiceCharacteristic: cannot find service " & serviceGuid
            Return Nothing
        End If

        Return oSvc
    End Function


    ''' <summary>
    ''' Get GattDeviceService, or set errorMessage. Similar to obsoleted device.GetGattService 
    ''' </summary>
    ''' <returns>Service, or NULL in case of error. See errorMessage for error message.</returns>
    Public Shared Function GetDeviceServiceAsync(deviceMAC As ULong,
                                      serviceGuid As String) As IAsyncOperation(Of WinBT.GenericAttributeProfile.GattDeviceService)
        Return GetDeviceServiceAsyncTask(deviceMAC, serviceGuid).AsAsyncOperation
    End Function

    Private Shared Async Function GetDeviceServiceAsyncTask(deviceMAC As ULong,
                                      serviceGuid As String) As Task(Of WinBT.GenericAttributeProfile.GattDeviceService)

        errorMessage = ""

        Dim oDev As WinBT.BluetoothLEDevice = Await GetDeviceAsync(deviceMAC)
        If oDev Is Nothing Then Return Nothing

        Return Await GetServiceAsyncTask(oDev, serviceGuid)

    End Function

#End Region

#Region "Characteristic"

    ''' <summary>
    ''' Get Characteristic from Service from device with MAC address
    ''' </summary>
    ''' <returns>Characteristic, or NULL in case of error. See errorMessage for error message.</returns>
    Public Shared Function GetCharacteristicAsync(service As WinBT.GenericAttributeProfile.GattDeviceService,
                                      charGuid As String) As IAsyncOperation(Of WinBT.GenericAttributeProfile.GattCharacteristic)
        Return GetCharacteristicAsyncTask(service, charGuid).AsAsyncOperation
    End Function

    Private Shared Async Function GetCharacteristicAsyncTask(service As WinBT.GenericAttributeProfile.GattDeviceService,
                                      charGuid As String) As Task(Of WinBT.GenericAttributeProfile.GattCharacteristic)

        errorMessage = ""

        Dim oChars = Await service.GetCharacteristicsAsync

        If oChars Is Nothing Then
            errorMessage = "GetDeviceServiceCharacteristic:GetCharacteristicsAsync = null"
            Return Nothing
        End If

        If oChars.Status <> WinBT.GenericAttributeProfile.GattCommunicationStatus.Success Then
            errorMessage = "GetDeviceServiceCharacteristic:GetCharacteristicsAsync.Status = " & oChars.Status.ToString
            Return Nothing
        End If

        For Each oChr In oChars.Characteristics
            If oChr.Uuid.ToString = charGuid.ToLower Then Return oChr
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Get Characteristic from ServiceGUID from device
    ''' </summary>
    ''' <returns>Characteristic, or NULL in case of error. See errorMessage for error message.</returns>
    Public Shared Function GetServiceCharacteristicAsync(oDevice As WinBT.BluetoothLEDevice,
                                      serviceGuid As String, charGuid As String) As IAsyncOperation(Of WinBT.GenericAttributeProfile.GattCharacteristic)
        Return GetServiceCharacteristicAsyncTask(oDevice, serviceGuid, charGuid).AsAsyncOperation
    End Function

    Private Shared Async Function GetServiceCharacteristicAsyncTask(oDevice As WinBT.BluetoothLEDevice,
                                      serviceGuid As String, charGuid As String) As Task(Of WinBT.GenericAttributeProfile.GattCharacteristic)

        errorMessage = ""

        Dim service As WinBT.GenericAttributeProfile.GattDeviceService = Await GetServiceAsync(oDevice, serviceGuid)
        If service Is Nothing Then Return Nothing

        Return Await GetCharacteristicAsync(service, charGuid)

    End Function


    ''' <summary>
    ''' Get Characteristic from ServiceGUID from deviceMAC
    ''' </summary>
    ''' <returns>Characteristic, or NULL in case of error. See errorMessage for error message.</returns>
    Public Shared Function GetDeviceServiceCharacteristicAsync(deviceMAC As ULong,
                                      serviceGuid As String, charGuid As String) As IAsyncOperation(Of WinBT.GenericAttributeProfile.GattCharacteristic)
        Return GetDeviceServiceCharacteristicAsyncTask(deviceMAC, serviceGuid, charGuid).AsAsyncOperation
    End Function

    Private Shared Async Function GetDeviceServiceCharacteristicAsyncTask(deviceMAC As ULong,
                                      serviceGuid As String, charGuid As String) As Task(Of WinBT.GenericAttributeProfile.GattCharacteristic)

        errorMessage = ""

        Dim service As WinBT.GenericAttributeProfile.GattDeviceService = Await GetDeviceServiceAsyncTask(deviceMAC, serviceGuid)
        If service Is Nothing Then Return Nothing

        Return Await GetCharacteristicAsync(service, charGuid)

    End Function

#End Region
#End Region

End Class
