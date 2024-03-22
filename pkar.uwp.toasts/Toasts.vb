
Imports Microsoft.Toolkit.Uwp.Notifications


Imports Windows.UI.Notifications

#If NETFX_CORE Then
Imports Windows.Foundation.Metadata
#Else
' to jest pusta definicja, żeby mógł być <DefaultOverload> podany - nie można go #IF zrobić, bo to nie działa
<AttributeUsage(AttributeTargets.Method)>
Public Class DefaultOverloadAttribute
    Inherits Attribute
End Class
#End If

Public Module Toasts

    ''' <summary>
    ''' Set number on app badge
    ''' </summary>
    ''' <param name="number"></param>
    Public Sub SetBadgeNo(number As Integer)
        ' https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/tiles-and-notifications-badges

        Dim oXmlBadge As Windows.Data.Xml.Dom.XmlDocument
        oXmlBadge = BadgeUpdateManager.GetTemplateContent(
                BadgeTemplateType.BadgeNumber)

        Dim oXmlNum As Windows.Data.Xml.Dom.XmlElement
        oXmlNum = CType(oXmlBadge.SelectSingleNode("/badge"), Windows.Data.Xml.Dom.XmlElement)
        oXmlNum.SetAttribute("value", number.ToString)

        BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(
                New BadgeNotification(oXmlBadge))
    End Sub

    ''' <summary>
    ''' Create string to be used as part of Toast XML string
    ''' </summary>
    ''' <param name="activationType">to be used as activationType</param>
    ''' <param name="action">to be used in arguments, as action&amp;elementId</param>
    ''' <param name="elementId">to be used in arguments, as action&amp;elementId</param>
    ''' <param name="content">to be used as content</param>
    ''' <returns></returns>
    Public Function ToastAction(activationType As String, action As String, elementId As String, content As String) As String
        Dim sTmp As String = content
        If sTmp <> "" Then sTmp = Localize.TryGetResManString(sTmp)

        Dim sTxt As String = "<action " &
            "activationType=""" & activationType & """ " &
            "arguments=""" & action & elementId & """ " &
            "content=""" & sTmp & """/> "
        Return sTxt
    End Function

    ''' <summary>
    ''' make simple toast with two message texts
    ''' </summary>
    Public Sub MakeToast(message As String, message2 As String)
        message = Localize.TryGetResManString(message)

#If PK_WPF Then
        Dim cb As New Microsoft.Toolkit.Uwp.Notifications.ToastContentBuilder()
        cb.AddText(message)
        If Not String.IsNullOrWhiteSpace(message2) Then
            cb.AddText(Localize.TryGetResManString(message2))
        End If
        cb.Show()
#Else
        Dim sXml = "<visual><binding template='ToastGeneric'><text>" & XmlSafeString(message)
        If message2 <> "" Then
            message2 = Localize.TryGetResManString(message2)
            sXml = sXml & "</text><text>" & XmlSafeString(message2)
        End If

        sXml &= "</text></binding></visual>"
        Dim oXml = New Windows.Data.Xml.Dom.XmlDocument
        oXml.LoadXml("<toast>" & sXml & "</toast>")
        Dim oToast = New ToastNotification(oXml)
        ToastNotificationManager.CreateToastNotifier().Show(oToast)
#End If
    End Sub

    ''' <summary>
    ''' make simple toast with message text
    ''' </summary>
    Public Sub MakeToast(message As String)
        MakeToast(message, "")
    End Sub

    ''' <summary>
    ''' make simple scheduled toast with message text
    ''' </summary>
    <DefaultOverload>
    Public Sub MakeToast(forDate As DateTimeOffset, message As String)
        MakeToast(forDate, message, "")
    End Sub


    ''' <summary>
    ''' make simple scheduled toast with two message texts
    ''' </summary>
    Public Sub MakeToast(forDate As DateTimeOffset, message As String, message2 As String)
        message = Localize.TryGetResManString(message)

#If PK_WPF Then
        Dim cb As New Microsoft.Toolkit.Uwp.Notifications.ToastContentBuilder()
        cb.AddText(message)
        If Not String.IsNullOrWhiteSpace(message2) Then
            cb.AddText(Localize.TryGetResManString(message2))
        End If
        cb.Schedule(forDate)
#Else

        Dim sXml = "<visual><binding template='ToastGeneric'><text>" & XmlSafeString(message)
        If message2 <> "" Then
            message2 = Localize.TryGetResManString(message2)
            sXml = sXml & "</text><text>" & XmlSafeString(message2)
        End If
        sXml &= "</text></binding></visual>"
        Dim oXml = New Windows.Data.Xml.Dom.XmlDocument
        oXml.LoadXml("<toast>" & sXml & "</toast>")
        Try
            ' Dim oToast = New Windows.UI.Notifications.ScheduledToastNotification(oXml, forDate, TimeSpan.FromHours(1), 10)
            Dim oToast = New ScheduledToastNotification(oXml, forDate)
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(oToast)
        Catch ex As Exception

        End Try
#End If
    End Sub

    ''' <summary>
    ''' remove all scheduled toasts
    ''' </summary>
    Public Sub RemoveScheduledToasts()
        Try
#If PK_WPF Then
            While ToastNotificationManagerCompat.CreateToastNotifier().GetScheduledToastNotifications().Count > 0
                ToastNotificationManagerCompat.CreateToastNotifier().RemoveFromSchedule(ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().Item(0))
            End While

#Else
            While ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().Count > 0
                ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().Item(0))
            End While
#End If
        Catch ex As Exception
            ' ponoc na desktopm nie dziala
        End Try

    End Sub

    ''' <summary>
    ''' remove all current toasts
    ''' </summary>
    Public Sub RemoveCurrentToasts()
#If PK_WPF Then
        ToastNotificationManagerCompat.History.Clear()
#Else
        ToastNotificationManager.History.Clear()
#End If
    End Sub

    Private Function XmlSafeString(sInput As String) As String
        Return New XText(sInput).ToString().Replace("""", "&quote;")
    End Function

    Public Function DumpToasts() As String
        Dim sResult As String = ""
#If PK_WPF Then
        For Each oToast As ScheduledToastNotification
            In ToastNotificationManagerCompat.CreateToastNotifier().GetScheduledToastNotifications()
#Else
        For Each oToast As ScheduledToastNotification
            In ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications()
#End If

            sResult = sResult & oToast.DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss") & vbCrLf
        Next

        If sResult = "" Then
            sResult = "(no toasts scheduled)"
        Else
            sResult = "Toasts scheduled for dates: " & vbCrLf & sResult
        End If

        Return sResult
    End Function

End Module
