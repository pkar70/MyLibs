Imports Microsoft.Maui.Controls
Imports System.Runtime.CompilerServices
Imports VBlib = pkar.NetConfigs

Public Module Extensions

#Region "label (text block)"

    ''' <summary>
    ''' Read setting and place it in UI, use empty string as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As Label)
        oItem.GetSettingsString("", "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use empty string as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As Label, sName As String)
        oItem.GetSettingsString(sName, "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use given default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="sDefault">default value</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As Label, sName As String, sDefault As String)
        If sName = "" Then sName = oItem.StyleId
        Dim sTxt As String = VBlib.GetSettingsString(sName, sDefault)
        oItem.Text = sTxt
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As Label)
        oItem.SetSettingsString("", False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As Label, sName As String)
        oItem.SetSettingsString(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As Label, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.StyleId
        VBlib.SetSettingsString(sName, oItem.Text, useRoam)
    End Sub
#End Region

#Region "Entry (TextBox)"


#Region "as string"
    ''' <summary>
    ''' Read setting and place it in UI, use empty string as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As Entry)
        oItem.GetSettingsString("", "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use empty string as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As Entry, sName As String)
        oItem.GetSettingsString(sName, "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use given default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="sDefault">default value</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As Entry, sName As String, sDefault As String)
        If sName = "" Then sName = oItem.StyleId
        Dim sTxt As String = VBlib.GetSettingsString(sName, sDefault)
        oItem.Text = sTxt
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As Entry)
        oItem.SetSettingsString("", False)
    End Sub


    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As Entry, sName As String)
        oItem.SetSettingsString(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As Entry, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.StyleId
        VBlib.SetSettingsString(sName, oItem.Text, useRoam)
    End Sub

#End Region

#Region "as int"

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Entry)
        oItem.SetSettingsInt("", False, 1)
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Entry, sName As String)
        oItem.SetSettingsInt(sName, False, 1)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Entry, sName As String, useRoam As Boolean)
        oItem.SetSettingsInt(sName, useRoam, 1)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming; using scaling
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    ''' <param name="dScale">scale factor; for storing double from currency, use scale 100</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Entry, sName As String, useRoam As Boolean, dScale As Double)
        If sName = "" Then sName = oItem.StyleId
        Dim dTmp As Double
        If Not Double.TryParse(oItem.Text, dTmp) Then Return
        dTmp *= dScale
        VBlib.SetSettingsInt(sName, dTmp, useRoam)
    End Sub

    ''' <summary>
    ''' Read setting and place it in UI, use 0 as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As Entry)
        oItem.GetSettingsInt("", 1)
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use 0 as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As Entry, sName As String)
        oItem.GetSettingsInt(sName, 1)
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use 0 as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="dScale">scale factor; for storing double from currency, use scale 100</param>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As Entry, sName As String, dScale As Double)
        If sName = "" Then sName = oItem.StyleId
        Dim dTmp As Integer = VBlib.GetSettingsInt(sName)
        dTmp /= dScale
        oItem.Text = dTmp
    End Sub


#End Region

#End Region

#Region "Switch (ToggleButton)"

    ''' <summary>
    ''' Read setting and place it in UI, use FALSE as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsBool(ByVal oItem As Switch)
        oItem.GetSettingsBool("", False)
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use FALSE as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsBool(ByVal oItem As Switch, sName As String)
        oItem.GetSettingsBool(sName, False)
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use given default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="bDefault">default value</param>
    <Extension()>
    Public Sub GetSettingsBool(ByVal oItem As Switch, sName As String, bDefault As Boolean)
        If sName = "" Then sName = oItem.StyleId
        Dim bBool As Boolean = VBlib.GetSettingsBool(sName, bDefault)
        oItem.IsToggled = bBool
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsBool(ByVal oItem As Switch)
        oItem.SetSettingsBool("", False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsBool(ByVal oItem As Switch, sName As String)
        oItem.SetSettingsBool(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsBool(ByVal oItem As Switch, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.StyleId
        VBlib.SetSettingsBool(sName, oItem.IsToggled, useRoam)
    End Sub
#End Region


#Region "Slider"

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Slider)
        oItem.SetSettingsInt("", False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Slider, sName As String)
        oItem.SetSettingsInt(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Slider, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.StyleId
        VBlib.SetSettingsInt(sName, oItem.Value, useRoam)
    End Sub

    ''' <summary>
    ''' Read setting and place it in UI, use 0 as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As Slider)
        oItem.GetSettingsInt("")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use 0 as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As Slider, sName As String)
        If sName = "" Then sName = oItem.StyleId
        oItem.Value = VBlib.GetSettingsInt(sName)
    End Sub
#End Region

#Region "Picker (ComboBox)"

    ''' <summary>
    ''' Save UI content (index of selected item) in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Picker)
        oItem.SetSettingsInt("", False)
    End Sub

    ''' <summary>
    ''' Save UI content (index selected of item) in sName local setting.
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Picker, sName As String)
        oItem.SetSettingsInt(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content (index selected of item) in sName locally or roaming.
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As Picker, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.StyleId
        VBlib.SetSettingsInt(sName, oItem.SelectedIndex, useRoam)
    End Sub

    ''' <summary>
    ''' Read setting (index selected of item) and place it in UI, use empty string as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As Picker)
        oItem.GetSettingsInt("")
    End Sub

    ''' <summary>
    ''' Read setting (index selected of item) sName and place it in UI, use empty string as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As Picker, sName As String)
        If sName = "" Then sName = oItem.StyleId
        Dim temp As Integer = VBlib.GetSettingsInt(sName)
        If temp < oItem.Items.Count Then
            oItem.SelectedIndex = temp
        Else
            oItem.SelectedIndex = -1
        End If
    End Sub

#End Region

#Region "DatePicker"

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsDate(ByVal oItem As DatePicker)
        oItem.SetSettingsDate("", False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsDate(ByVal oItem As DatePicker, sName As String)
        oItem.SetSettingsDate(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsDate(ByVal oItem As DatePicker, sName As String, useRoam As Boolean)
        'If oItem.Date Is Nothing Then Return
        If sName = "" Then sName = oItem.StyleId
        VBlib.SetSettingsDate(sName, oItem.Date, useRoam)
    End Sub

    ''' <summary>
    ''' Read setting and place it in UI, use empty string as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsDate(ByVal oItem As DatePicker)
        oItem.GetSettingsDate("")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use empty string as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsDate(ByVal oItem As DatePicker, sName As String)
        If sName = "" Then sName = oItem.StyleId
        Dim dDTOff As DateTimeOffset = VBlib.GetSettingsDate(sName)
        oItem.Date = New Date(dDTOff.Ticks)
    End Sub

#End Region

End Module
