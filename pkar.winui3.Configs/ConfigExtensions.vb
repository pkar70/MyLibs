Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports Microsoft.UI.Xaml.Controls

Imports VBlib = pkar.NetConfigs
Imports System.Runtime.CompilerServices

Public Module Extensions

#Region "text block"

    ''' <summary>
    ''' Read setting and place it in UI, use empty string as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As TextBlock)
        oItem.GetSettingsString("", "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use empty string as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As TextBlock, sName As String)
        oItem.GetSettingsString(sName, "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use given default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="sDefault">default value</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As TextBlock, sName As String, sDefault As String)
        If sName = "" Then sName = oItem.Name
        Dim sTxt As String = VBlib.GetSettingsString(sName, sDefault)
        oItem.Text = sTxt
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As TextBlock)
        oItem.SetSettingsString("", False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As TextBlock, sName As String)
        oItem.SetSettingsString(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As TextBlock, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.Name
        VBlib.SetSettingsString(sName, oItem.Text, useRoam)
    End Sub
#End Region

#Region "TextBox"


#Region "as string"
    ''' <summary>
    ''' Read setting and place it in UI, use empty string as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As TextBox)
        oItem.GetSettingsString("", "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use empty string as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As TextBox, sName As String)
        oItem.GetSettingsString(sName, "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use given default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="sDefault">default value</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As TextBox, sName As String, sDefault As String)
        If sName = "" Then sName = oItem.Name
        Dim sTxt As String = VBlib.GetSettingsString(sName, sDefault)
        oItem.Text = sTxt
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As TextBox)
        oItem.SetSettingsString("", False)
    End Sub


    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As TextBox, sName As String)
        oItem.SetSettingsString(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As TextBox, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.Name
        VBlib.SetSettingsString(sName, oItem.Text, useRoam)
    End Sub

#End Region

#Region "as int"

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As TextBox)
        oItem.SetSettingsInt("", False, 1)
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As TextBox, sName As String)
        oItem.SetSettingsInt(sName, False, 1)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As TextBox, sName As String, useRoam As Boolean)
        oItem.SetSettingsInt(sName, useRoam, 1)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming; using scaling
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    ''' <param name="dScale">scale factor; for storing double from currency, use scale 100</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As TextBox, sName As String, useRoam As Boolean, dScale As Double)
        If sName = "" Then sName = oItem.Name
        Dim dTmp As Double
        If Not Double.TryParse(oItem.Text, dTmp) Then Return
        dTmp *= dScale
        VBlib.SetSettingsInt(sName, dTmp, useRoam)
    End Sub

    ''' <summary>
    ''' Read setting and place it in UI, use 0 as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As TextBox)
        oItem.GetSettingsInt("", 1)
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use 0 as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As TextBox, sName As String)
        oItem.GetSettingsInt(sName, 1)
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use 0 as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="dScale">scale factor; for storing double from currency, use scale 100</param>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As TextBox, sName As String, dScale As Double)
        If sName = "" Then sName = oItem.Name
        Dim dTmp As Integer = VBlib.GetSettingsInt(sName)
        dTmp /= dScale
        oItem.Text = dTmp
    End Sub


#End Region

#End Region

#Region "PasswordBox"
    ''' <summary>
    ''' Read setting and place it in UI, use empty string as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As PasswordBox)
        oItem.GetSettingsString("", "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use empty string as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As PasswordBox, sName As String)
        oItem.GetSettingsString(sName, "")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use given default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="sDefault">default value</param>
    <Extension()>
    Public Sub GetSettingsString(ByVal oItem As PasswordBox, sName As String, sDefault As String)
        If sName = "" Then sName = oItem.Name
        Dim sTxt As String = VBlib.GetSettingsString(sName, sDefault)
        oItem.Password = sTxt
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As PasswordBox)
        oItem.SetSettingsString("", False)
    End Sub


    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As PasswordBox, sName As String)
        oItem.SetSettingsString(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsString(ByVal oItem As PasswordBox, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.Name
        VBlib.SetSettingsString(sName, oItem.Password, useRoam)
    End Sub

#End Region

#Region "ToggleButton"

    ''' <summary>
    ''' Read setting and place it in UI, use FALSE as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsBool(ByVal oItem As Primitives.ToggleButton)
        oItem.GetSettingsBool("", False)
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use FALSE as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsBool(ByVal oItem As Primitives.ToggleButton, sName As String)
        oItem.GetSettingsBool(sName, False)
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use given default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="bDefault">default value</param>
    <Extension()>
    Public Sub GetSettingsBool(ByVal oItem As Primitives.ToggleButton, sName As String, bDefault As Boolean)
        If sName = "" Then sName = oItem.Name
        Dim bBool As Boolean = VBlib.GetSettingsBool(sName, bDefault)
        oItem.IsChecked = bBool
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsBool(ByVal oItem As Primitives.ToggleButton)
        oItem.SetSettingsBool("", False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsBool(ByVal oItem As Primitives.ToggleButton, sName As String)
        oItem.SetSettingsBool(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsBool(ByVal oItem As Primitives.ToggleButton, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.Name
        VBlib.SetSettingsBool(sName, oItem.IsChecked, useRoam)
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
        If sName = "" Then sName = oItem.Name
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
        If sName = "" Then sName = oItem.Name
        oItem.Value = VBlib.GetSettingsInt(sName)
    End Sub
#End Region

#Region "ComboBox"

    ''' <summary>
    ''' Save UI content (index of selected item) in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As ComboBox)
        oItem.SetSettingsInt("", False)
    End Sub

    ''' <summary>
    ''' Save UI content (index selected of item) in sName local setting.
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As ComboBox, sName As String)
        oItem.SetSettingsInt(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content (index selected of item) in sName locally or roaming.
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsInt(ByVal oItem As ComboBox, sName As String, useRoam As Boolean)
        If sName = "" Then sName = oItem.Name
        VBlib.SetSettingsInt(sName, oItem.SelectedIndex, useRoam)
    End Sub

    ''' <summary>
    ''' Read setting (index selected of item) and place it in UI, use empty string as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As ComboBox)
        oItem.GetSettingsInt("")
    End Sub

    ''' <summary>
    ''' Read setting (index selected of item) sName and place it in UI, use empty string as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsInt(ByVal oItem As ComboBox, sName As String)
        If sName = "" Then sName = oItem.Name
        Dim temp As Integer = VBlib.GetSettingsInt(sName)
        If temp < oItem.Items.Count Then
            oItem.SelectedIndex = temp
        Else
            oItem.SelectedIndex = -1
        End If
    End Sub

#End Region

#Region "CalendarDatePicker"

    ''' <summary>
    ''' Save UI content in sName local setting. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub SetSettingsDate(ByVal oItem As CalendarDatePicker)
        oItem.SetSettingsDate("", False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName local setting
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub SetSettingsDate(ByVal oItem As CalendarDatePicker, sName As String)
        oItem.SetSettingsDate(sName, False)
    End Sub

    ''' <summary>
    ''' Save UI content in sName setting, locally or roaming
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    ''' <param name="useRoam">True if value should be placed also in roaming settings</param>
    <Extension()>
    Public Sub SetSettingsDate(ByVal oItem As CalendarDatePicker, sName As String, useRoam As Boolean)
        If oItem.Date Is Nothing Then Return

        If sName = "" Then sName = oItem.Name
        VBlib.SetSettingsDate(sName, oItem.Date.Value, useRoam)
    End Sub

    ''' <summary>
    ''' Read setting and place it in UI, use empty string as default. Setting name (key) is same as UI element name.
    ''' </summary>
    <Extension()>
    Public Sub GetSettingsDate(ByVal oItem As CalendarDatePicker)
        oItem.GetSettingsDate("")
    End Sub

    ''' <summary>
    ''' Read setting sName and place it in UI, use empty string as default
    ''' </summary>
    ''' <param name="sName">setting name (key)</param>
    <Extension()>
    Public Sub GetSettingsDate(ByVal oItem As CalendarDatePicker, sName As String)
        If sName = "" Then sName = oItem.Name
        Dim dDTOff As DateTimeOffset = VBlib.GetSettingsDate(sName)
        oItem.Date = New Date(dDTOff.Ticks)
    End Sub

#End Region

End Module

