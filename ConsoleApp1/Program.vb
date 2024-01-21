Imports System

Module Program

    Public Class Prefix
        Public Shared Function AllPrefixes(prefixLength As Integer, words As IEnumerable(Of String)) As IEnumerable(Of String)

            Dim temp = words.Where(Function(x) x.Length >= prefixLength).Select(Of String)(Function(x) x.Substring(0, prefixLength))

            For Each oel In temp
                Console.WriteLine(oel)
            Next

            Return temp
            ' throw new InvalidOperationException("Waiting to be implemented.")
        End Function
    End Class

    Public Sub Main()
        For Each p As String In Prefix.AllPrefixes(3, {"flow", "flowers", "flew", "flag", "fm", "ala"})
            Console.WriteLine(p)
        Next
    End Sub
End Module
