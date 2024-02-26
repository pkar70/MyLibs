
#If PK_WPF Then
Imports System.Windows.Data
#Else
Imports Microsoft.UI.Xaml.Data
#End If


''' <summary>
''' this class should be used to define your own ValueConverters; but it frees you from writing ConvertBack method
''' </summary>
Public MustInherit Class ValueConverterOneWay
    Implements IValueConverter

#If PK_WPF Then
    Public MustOverride Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
#Else
    Public MustOverride Function Convert(value As Object, targetType As Type, parameter As Object, lang As String) As Object Implements IValueConverter.Convert

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, lang As String) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
#End If
End Class


''' <summary>
''' this class should be used to define your own ValueConverters; but it frees you from writing ConvertBack method, and simplyfies Convert method
''' </summary>
Public MustInherit Class ValueConverterOneWaySimple
    Implements IValueConverter

    Protected MustOverride Function Convert(value As Object) As Object

#If PK_WPF Then
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Convert(value)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function

#Else
    Public Function Convert(value As Object, targetType As Type, parameter As Object, lang As String) As Object Implements IValueConverter.Convert
        Return Convert(value)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, lang As String) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
#End If

End Class

Public MustInherit Class ValueConverterOneWayWithPar
    Implements IValueConverter

    Protected MustOverride Function Convert(value As Object, param As String) As Object

#If PK_WPF Then
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
#Else
    Public Function Convert(value As Object, targetType As Type, parameter As Object, lang As String) As Object Implements IValueConverter.Convert
#End If
        Dim param As String = ""

        If parameter IsNot Nothing Then param = CType(parameter, String)

        Return Convert(value, param)

    End Function


#If PK_WPF Then
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
#Else
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, lang As String) As Object Implements IValueConverter.ConvertBack
#End If
        Throw New NotImplementedException()
    End Function

End Class