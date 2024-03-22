
#If Not NETFX_CORE And Not PK_WPF Then
' assume WinUI3
Imports Microsoft.UI.Xaml
#End If

' zamiennik dla:
'<ListView ...  >
'<ListView.ItemContainerStyle>
'<Style TargetType = "ListViewItem" >
'                    <Setter Property="HorizontalContentAlignment" Value="Stretch"/>
'                </Style>
'            </ListView.ItemContainerStyle>


Imports Windows.UI


''' <summary>
''' Daje Stretch, a jak BlueItems, to także ramki
''' </summary>
Public Class StretchedListView
    Inherits Controls.ListView

    Public Property BlueItems As Boolean

#If PK_WPF Then
    Public Overrides Sub OnApplyTemplate()
#Else
    Protected Overrides Sub OnApplyTemplate()
#End If
        MyBase.OnApplyTemplate()

        Dim stylesetter As New Style(GetType(Controls.ListViewItem))
        stylesetter.Setters.Add(New Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch))

        If BlueItems Then
            stylesetter.Setters.Add(New Setter(BorderBrushProperty, New Media.SolidColorBrush(Colors.Blue)))
            stylesetter.Setters.Add(New Setter(BorderThicknessProperty, New Thickness(1)))
            stylesetter.Setters.Add(New Setter(MarginProperty, New Thickness(0, 1, 0, 1)))
        End If

        Me.ItemContainerStyle = stylesetter

#If PK_WPF Then
        Me.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled)
#End If
    End Sub

End Class
