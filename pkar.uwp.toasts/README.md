
This Nuget simplifies Toasts (adds some 'wrappers' for them)

## badges

    Public Sub SetBadgeNo(number As Integer)

## toasts

### creating toasts

    Public Sub MakeToast(message As String)
    Public Sub MakeToast(message As String, message2 As String)
    Public Sub MakeToast(forDate As DateTimeOffset, message As String)
    Public Sub MakeToast(forDate As DateTimeOffset, message As String, message2 As String)

### removing toasts

    Public Sub RemoveScheduledToasts()
    Public Sub RemoveCurrentToasts()

### debugging toasts

    Public Function DumpToasts() As String


This Nuget uses my pkar.Localize nuget (only if you would initialize Localize).