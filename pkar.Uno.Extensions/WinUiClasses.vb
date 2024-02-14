

Imports Microsoft.UI.Xaml.Data

''' <summary>
''' this class should be used to define your own ValueConverters; but it frees you from writing ConvertBack method
''' </summary>
Public MustInherit Class ValueConverterOneWay
    Implements IValueConverter

    Public MustOverride Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class



''' <summary>
''' this class should be used to define your own ValueConverters; but it frees you from writing ConvertBack method, and simplyfies Convert method
''' </summary>
Public MustInherit Class ValueConverterOneWaySimple
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Return Convert(value)
    End Function

    Protected MustOverride Function Convert(value As Object) As Object


    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class