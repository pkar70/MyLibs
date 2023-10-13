Imports System.Reflection

''' <summary>
''' inherit from this to add Clone() and dumping properties' values
''' </summary>
Public MustInherit Class BaseStruct

    ''' <summary>
    '''  to be used in debugging, dump all properties inside class as string
    ''' </summary>
    Public Function DumpAsText() As String
        Dim oTypek As Type = Me.GetType
        Dim sTxt As String = Me.ToString & ":" & vbCrLf

        For Each oProp As PropertyInfo In oTypek.GetRuntimeProperties
            sTxt = sTxt & oProp.Name & ":" & vbTab
            If oProp.GetValue(Me) Is Nothing Then
                sTxt &= " (null)"
            Else
                sTxt &= oProp.GetValue(Me).ToString
            End If
        Next

        Return sTxt
    End Function

    ''' <summary>
    '''  to be used in debugging, dump tree of all properties inside class (as JSON)
    ''' </summary>
    ''' <param name="bSkipDefaults"></param>
    ''' <returns></returns>
    Public Function DumpAsJSON(Optional bSkipDefaults As Boolean = False) As String
        Dim oSerSet As New Newtonsoft.Json.JsonSerializerSettings

        If bSkipDefaults Then
            oSerSet.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
            oSerSet.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore
        End If
        Return Newtonsoft.Json.JsonConvert.SerializeObject(Me, Newtonsoft.Json.Formatting.Indented, oSerSet)
    End Function


    ''' <summary>
    '''  deep clone of object
    ''' </summary>
    ''' <returns></returns>
    Public Function Clone() As Object
        Dim sTxt As String = DumpAsJSON()
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(sTxt, Me.GetType)
    End Function

    ''' <summary>
    ''' copy all properties and fields from current object into given object, if such properties/fields exists
    ''' </summary>
    ''' <param name="anyObject">destination object</param>
    Public Sub CopyTo(anyObject As Object)
        If anyObject Is Nothing Then Return

        For Each oPropFrom As PropertyInfo In Me.GetType.GetRuntimeProperties

            For Each oPropTo As PropertyInfo In anyObject.GetType.GetRuntimeProperties
                If oPropFrom.Name = oPropTo.Name Then
                    oPropTo.SetValue(anyObject, oPropFrom.GetValue(Me))
                    Exit For
                End If
            Next

            For Each oFldTo As FieldInfo In anyObject.GetType.GetRuntimeFields
                If oPropFrom.Name = oFldTo.Name Then
                    oFldTo.SetValue(anyObject, oPropFrom.GetValue(Me))
                    Exit For
                End If
            Next
        Next

        For Each oFieldFrom As FieldInfo In Me.GetType.GetRuntimeFields

            For Each oPropTo As PropertyInfo In anyObject.GetType.GetRuntimeProperties
                If oFieldFrom.Name = oPropTo.Name Then
                    oPropTo.SetValue(anyObject, oFieldFrom.GetValue(Me))
                    Exit For
                End If
            Next

            For Each oFldTo As FieldInfo In anyObject.GetType.GetRuntimeFields
                If oFieldFrom.Name = oFldTo.Name Then
                    oFldTo.SetValue(anyObject, oFieldFrom.GetValue(Me))
                    Exit For
                End If
            Next
        Next


    End Sub

    ''' <summary>
    ''' copy all properties and fields from given object into current object, if such properties/fields exists
    ''' </summary>
    ''' <param name="anyObject">destination object</param>
    Public Sub CopyFrom(anyObject As Object)
        If anyObject Is Nothing Then Return

        For Each oPropTo As PropertyInfo In Me.GetType.GetRuntimeProperties

            For Each oPropFrom As PropertyInfo In anyObject.GetType.GetRuntimeProperties
                If oPropTo.Name = oPropFrom.Name Then
                    oPropTo.SetValue(Me, oPropFrom.GetValue(anyObject))
                    Exit For
                End If
            Next

            For Each oFldFrom As FieldInfo In anyObject.GetType.GetRuntimeFields
                If oPropTo.Name = oFldFrom.Name Then
                    oPropTo.SetValue(Me, oFldFrom.GetValue(anyObject))
                    Exit For
                End If
            Next
        Next

        For Each oFieldTo As FieldInfo In Me.GetType.GetRuntimeFields

            For Each oPropFrom As PropertyInfo In anyObject.GetType.GetRuntimeProperties
                If oFieldTo.Name = oPropFrom.Name Then
                    oFieldTo.SetValue(Me, oPropFrom.GetValue(anyObject))
                    Exit For
                End If
            Next

            For Each oFldFrom As FieldInfo In anyObject.GetType.GetRuntimeFields
                If oFieldTo.Name = oFldFrom.Name Then
                    oFieldTo.SetValue(Me, oFldFrom.GetValue(anyObject))
                    Exit For
                End If
            Next
        Next

    End Sub



End Class



