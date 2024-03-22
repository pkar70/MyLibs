Imports pkar.DotNetExtensions

Public Class SymbolIcon
    Inherits TextBlock

    Private Property _Symbol As String
    Public Property Symbol As String
        Get
            Return _Symbol
        End Get
        Set(value As String)
            _Symbol = value
            Text = GetCharForIconName(value)
        End Set
    End Property

    Public Sub New()
        MyBase.New

        TextAlignment = TextAlignment.Center
        HorizontalAlignment = HorizontalAlignment.Stretch
        VerticalAlignment = VerticalAlignment.Stretch
        FontSize = 20
    End Sub

    Friend Shared Function GetCharForIconName(iconName As String) As String
        ' tu będą dopisywane kolejne, nie od razu komplet, bo szkoda roboty

        Dim ikonka As String = iconName.ToLowerInvariant.Replace("controls.symbol.", "")

        Select Case ikonka
            Case "accept"
                Return "🗸"
            Case "add"
                Return "＋"
            Case "calculator"
                Return "🖩"
            Case "calendar"
                Return "📅"
            Case "camera"
                Return "📷"
            Case "character"
                Return "A"
            Case "delete"
                Return "🗑"
            Case "forward"
                Return "→"
            Case "globe"
                Return "🌍"
            Case "help"
                Return "︖"
            Case "outlinestar"
                Return "☆"
            Case "page"
                Return "📄"
            Case "permissions"
                Return "🗝"
            Case "refresh"
                Return "↻"
            Case "shuffle"
                Return "🔀"
            Case "sort"
                Return "⇅"
            Case "zoom"
                Return "🔎"

            Case "setting"
                Return "⚙"

            Case "twopage"
                Return "◫"
            Case "list"
                Return "🗏"
            Case "folder"
                Return "📂"
        End Select

        Debug.WriteLine("Nie umiem takiej ikonki w AppBarButton!")
        Return iconName
    End Function

End Class
