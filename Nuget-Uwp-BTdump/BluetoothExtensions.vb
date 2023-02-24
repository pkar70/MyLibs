
Imports pkar.DotNetExtensions

Partial Public Module BTextensions

    ''' <summary>
    ''' Create string with dumped IBuffer bytes as hex
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToDebugString(ByVal oBuf As Windows.Storage.Streams.IBuffer, iMaxLen As Integer) As String
        Dim sRet As String = oBuf.Length & ": "
        Dim oArr As Byte() = oBuf.ToArray

        For i As Integer = 0 To Math.Min(oBuf.Length - 1, iMaxLen)
            sRet = sRet & oArr.ElementAt(i).ToString("X2") & " "
        Next

        Return sRet & vbCrLf
    End Function

    ''' <summary>
    ''' Create string with dumped IBuffer bytes as hex, maximum 16 bytes
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <Windows.Foundation.Metadata.DefaultOverload>
    Public Function ToDebugString(ByVal oBuf As Windows.Storage.Streams.IBuffer) As String
        Return oBuf.ToDebugString(16)
    End Function

    ''' <summary>
    ''' Create string with dumped BluetoothLEAdvertisement data
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToDebugString(ByVal oAdv As Windows.Devices.Bluetooth.Advertisement.BluetoothLEAdvertisement) As String

        If oAdv Is Nothing Then
            Return "ERROR: Advertisement is Nothing, unmoglich!"
        End If

        Dim sRet As String = ""

        If oAdv.DataSections IsNot Nothing Then
            sRet = sRet & "Adverisement, number of data sections: " & oAdv.DataSections.Count & vbCrLf
            For Each oItem As Windows.Devices.Bluetooth.Advertisement.BluetoothLEAdvertisementDataSection In oAdv.DataSections
                sRet = sRet & " DataSection: " & oItem.Data.ToDebugString(32)
            Next
        End If

        If oAdv.Flags IsNot Nothing Then sRet = sRet & "Adv.Flags: " & CInt(oAdv.Flags) & vbCrLf

        sRet = sRet & "Adv local name: " & oAdv.LocalName & vbCrLf

        If oAdv.ManufacturerData IsNot Nothing Then
            For Each oItem As Windows.Devices.Bluetooth.Advertisement.BluetoothLEManufacturerData In oAdv.ManufacturerData
                sRet = sRet & " ManufacturerData.Company: " & oItem.CompanyId & vbCrLf
                sRet = sRet & " ManufacturerData.Data: " & oItem.Data.ToDebugString(32) & vbCrLf
            Next
        End If

        If oAdv.ServiceUuids IsNot Nothing Then
            For Each oItem As Guid In oAdv.ServiceUuids
                sRet = sRet & " service " & oItem.ToString & vbCrLf
            Next
        End If

        Return sRet
    End Function


    ''' <summary>
    ''' Create string with dumped GattDescriptor data
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToDebugStringAsync(ByVal oDescriptor As Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor) As IAsyncOperation(Of String)
        Return oDescriptor.ToDebugStringAsyncTask.AsAsyncOperation
    End Function

    <Runtime.CompilerServices.Extension()>
    Private Async Function ToDebugStringAsyncTask(ByVal oDescriptor As Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor) As Task(Of String)
        Dim sRet As String

        sRet = "      descriptor: " & oDescriptor.Uuid.ToString & vbTab & oDescriptor.Uuid.AsGattReservedDescriptorName & vbCrLf
        Dim oRdVal = Await oDescriptor.ReadValueAsync
        If oRdVal.Status = Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success Then
            Dim oVal = oRdVal.Value
            sRet = sRet & oVal.ToArray.ToDebugString(8) & vbCrLf
        Else
            sRet = sRet & "      ReadValueAsync status = " & oRdVal.Status.ToString & vbCrLf
        End If
        Return sRet
    End Function


    ''' <summary>
    ''' Create string with dumped GattCharacteristicProperties flags
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToDebugString(ByVal oProp As Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties) As String

        Dim sRet As String = "      CharacteristicProperties: "

        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Read) Then
            sRet &= "[read] "
        End If

        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.AuthenticatedSignedWrites) Then
            sRet &= "[AuthenticatedSignedWrites] "
        End If
        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Broadcast) Then
            sRet &= "[broadcast] "
        End If
        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Indicate) Then
            sRet &= "[indicate] "
            ' bCanRead = False
        End If
        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.None) Then
            sRet &= "[NONE] "
        End If
        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Notify) Then
            sRet &= "[notify] "
            ' bCanRead = False
        End If
        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.ReliableWrites) Then
            sRet &= "[reliableWrite] "
        End If
        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Write) Then
            sRet &= "[write] "
        End If
        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.WritableAuxiliaries) Then
            sRet &= "[WritableAuxiliaries] "
        End If
        If oProp.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.WriteWithoutResponse) Then
            sRet &= "[writeNoResponse] "
        End If

        Return sRet
    End Function

    ''' <summary>
    ''' Create string with dumped GattCharacteristic data
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToDebugStringAsync(ByVal oChar As Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic) As IAsyncOperation(Of String)
        Return oChar.ToDebugStringAsyncTask.AsAsyncOperation
    End Function
    <Runtime.CompilerServices.Extension()>
    Private Async Function ToDebugStringAsyncTask(ByVal oChar As Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic) As Task(Of String)


        Dim sRet As String = "      CharacteristicProperties: " & oChar.CharacteristicProperties.ToDebugString & vbCrLf
        Dim bCanRead As Boolean = False
        If sRet.Contains("[read]") Then bCanRead = True
        ' ewentualnie wygaszenie gdy:
        'sProp &= "[indicate] "
        ' bCanRead = False
        '   sProp &= "[notify] "
        ' bCanRead = False


        Dim oDescriptors = Await oChar.GetDescriptorsAsync
        If oDescriptors Is Nothing Then Return sRet

        If oDescriptors.Status <> Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success Then
            sRet = sRet & "      GetDescriptorsAsync.Status = " & oDescriptors.Status.ToString & vbCrLf
            Return sRet
        End If


        For Each oDescr In oDescriptors.Descriptors
            sRet = sRet & Await oDescr.ToDebugStringAsync & vbCrLf
        Next

        If bCanRead Then
            Dim oRd = Await oChar.ReadValueAsync()
            If oRd.Status <> Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success Then
                sRet = sRet & "ReadValueAsync.Status=" & oRd.Status & vbCrLf
            Else
                sRet = sRet & "      characteristic data (read):" & vbCrLf
                sRet = sRet & oRd.Value.ToArray.ToDebugString(8) & vbCrLf
            End If

        End If

        Return sRet
    End Function

    ''' <summary>
    ''' Create string with dumped GattDeviceService data
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Function ToDebusGtringAsync(ByVal oServ As Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService) As IAsyncOperation(Of String)
        Return oServ.ToDebusGtringAsyncTask.AsAsyncOperation
    End Function
    <Runtime.CompilerServices.Extension()>
    Private Async Function ToDebusGtringAsyncTask(ByVal oServ As Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService) As Task(Of String)


        If oServ Is Nothing Then Return ""

        Dim oChars = Await oServ.GetCharacteristicsAsync
        If oChars Is Nothing Then Return ""
        If oChars.Status <> Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success Then
            Return "    GetCharacteristicsAsync.Status = " & oChars.Status.ToString
        End If

        Dim sRet As String = ""
        For Each oChr In oChars.Characteristics
            sRet = sRet & vbCrLf & "    characteristic: " & oChr.Uuid.ToString & oChr.Uuid.AsGattReservedCharacteristicName & vbCrLf
            sRet = sRet & Await oChr.ToDebugStringAsync & vbCrLf
        Next

        Return sRet
    End Function

    ''' <summary>
    ''' Create string with dumped BluetoothLEDevice data
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    <Windows.Foundation.Metadata.DefaultOverload>
    Public Function ToDebugStringAsync(ByVal oDevice As Windows.Devices.Bluetooth.BluetoothLEDevice) As IAsyncOperation(Of String)
        Return oDevice.ToDebugStringAsyncTask.AsAsyncOperation
    End Function
    <Runtime.CompilerServices.Extension()>
    Private Async Function ToDebugStringAsyncTask(ByVal oDevice As Windows.Devices.Bluetooth.BluetoothLEDevice) As Task(Of String)


        Dim sRet As String = ""

        sRet = sRet & "DebugBTdevice, data dump:" & vbCrLf
        sRet = sRet & "Device name: " & oDevice.Name & vbCrLf
        sRet = sRet & "MAC address: " & oDevice.BluetoothAddress.ToHexBytesString & vbCrLf
        sRet = sRet & "Connection status: " & oDevice.ConnectionStatus.ToString & vbCrLf

        Dim oDAI = oDevice.DeviceAccessInformation
        sRet = sRet & vbCrLf & "DeviceAccessInformation:" & vbCrLf
        sRet = sRet & "  CurrentStatus: " & oDAI.CurrentStatus.ToString & vbCrLf

        Dim oDApperr = oDevice.Appearance
        sRet = sRet & vbCrLf & "Appearance:" & vbCrLf
        sRet = sRet & "  Category: " & oDApperr.Category & vbCrLf
        sRet = sRet & "  Subcategory: " & oDApperr.SubCategory & vbCrLf

        sRet = sRet & "Services: " & oDApperr.SubCategory & vbCrLf

        Dim oSrv = Await oDevice.GetGattServicesAsync
        If oSrv.Status <> Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success Then
            sRet = sRet & "  GetGattServicesAsync.Status = " & oSrv.Status.ToString & vbCrLf
            Return sRet
        End If

        For Each oSv In oSrv.Services
            sRet = sRet & vbCrLf & "  service: " & oSv.Uuid.ToString & vbTab & vbTab & oSv.Uuid.AsGattReservedServiceName & vbCrLf
            sRet = sRet & Await oSv.ToDebusGtringAsync & vbCrLf
        Next

        Return sRet

    End Function

End Module
