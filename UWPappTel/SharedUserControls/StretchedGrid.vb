Imports pkar.DotNetExtensions


#If Not NETFX_CORE And Not PK_WPF Then
' assume WinUI3
Imports Microsoft.UI.Xaml
#End If

Public Class StretchedGrid
    Inherits Controls.Grid

    Public Property Cols As String
        Get
            Dim colwym As String = ""
            For Each col In ColumnDefinitions
                If colwym <> "" Then colwym &= ","
                colwym &= col.Width.ToString
            Next
            Return colwym
        End Get
        Set(value As String)
            Dim aArr As String() = value.Split(",")
            ColumnDefinitions.Clear()
            For Each col In aArr
                ColumnDefinitions.Add(New Controls.ColumnDefinition() With {.Width = Text2GridLen(col)})
            Next
        End Set
    End Property

    Public Property Rows As String
        Get
            Dim colwym As String = ""
            For Each col In RowDefinitions
                If colwym <> "" Then colwym &= ","
                colwym &= col.Height.ToString
            Next
            Return colwym
        End Get
        Set(value As String)
            Dim aArr As String() = value.Split(",")
            RowDefinitions.Clear()
            For Each col In aArr
                RowDefinitions.Add(New Controls.RowDefinition() With {.Height = Text2GridLen(col)})
            Next
        End Set
    End Property

    Public Property AutoRows As Boolean


    Public Sub New()
        MyBase.New

        AddHandler Me.Loaded, AddressOf MojaWczytana
    End Sub

    Private Sub MojaWczytana(sender As Object, e As RoutedEventArgs)
        If Not Me.AutoRows Then Return

        Dim currnum(5) As Integer

        ' zeruj licznik
        For iLp = 0 To 4
            currnum(iLp) = 0
        Next

        For Each oChild As FrameworkElement In Me.Children
            Dim definedCol As Integer = Grid.GetColumn(oChild)
            If definedCol > 4 Then Continue For

            Dim definedRow As Integer = Grid.GetRow(oChild)

            If definedRow > 0 Then
                currnum(definedCol) = definedRow + Grid.GetRowSpan(oChild)
                Continue For
            End If

            If Me.RowDefinitions.Count <= currnum(definedCol) Then
                If Me.RowDefinitions.Count > 0 Then
                    Dim prevRowDef = Me.RowDefinitions(Me.RowDefinitions.Count - 1)
                    ' ponizsze daje: 'value' already belongs to another 'RowDefinitionCollection'.
                    ' Me.RowDefinitions.Add(prevRowDef)
                    ' więc robimy default wszystko, i kopiujemy tylko Height (ale można byłoby narzucać A zawsze)
                    Me.RowDefinitions.Add(New RowDefinition With {.Height = prevRowDef.Height})
                Else
                    Me.RowDefinitions.Add(New RowDefinition With {.Height = New GridLength(1, GridUnitType.Auto)})
                End If
            End If

            Grid.SetRow(oChild, currnum(definedCol))
            currnum(definedCol) += Grid.GetRowSpan(oChild)
        Next

    End Sub

    '<Grid HorizontalAlignment="Stretch"  >

#If PK_WPF Then
    Public Overrides Sub OnApplyTemplate()
#Else
    Protected Overrides Sub OnApplyTemplate()
#End If
        HorizontalAlignment = HorizontalAlignment.Center

        MyBase.OnApplyTemplate()
    End Sub


    Private Shared Function Text2GridLen(text As String) As GridLength
        If text.EqualsCI("Auto") Then Return New GridLength(0, GridUnitType.Auto)
        If text.EqualsCI("A") Then Return New GridLength(0, GridUnitType.Auto)

        Dim typek As GridUnitType = GridUnitType.Pixel
        If text.Contains("*"c) Then
            typek = GridUnitType.Star
            text = text.Replace("*", "")
        End If

        Dim dbl As Double
        If Not Double.TryParse(text, dbl) Then Return New GridLength(1, GridUnitType.Star)

        Return New GridLength(dbl, typek)
    End Function





End Class
