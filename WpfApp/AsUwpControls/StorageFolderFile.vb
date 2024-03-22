
' compatibility pack

Imports System.Diagnostics.CodeAnalysis
Imports pkar.UI.Extensions

Public Class StorageFolder
    Public Property Path As String

#Disable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
    Public Async Function CreateFolderAsync(nazwa As String, opcje As CreationCollisionOption) As Task(Of StorageFolder)
#Enable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
        Dim targetPath As String = IO.Path.Combine(Path, nazwa)
        IO.Directory.CreateDirectory(targetPath)

        Return New StorageFolder(targetPath)
    End Function

    Public Sub New(folderPath As String)
        Path = folderPath
    End Sub

    Public Sub OpenExplorer()
        Path.OpenExplorer
    End Sub

#Disable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
    Public Async Function CreateFileAsync(filename As String) As Task(Of StorageFile)
#Enable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
        Dim pathname As String = IO.Path.Combine(Path, filename)
        IO.File.Create(pathname).Close()
        Return New StorageFile(pathname)
    End Function

#Disable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
    Public Shared Async Function GetFileFromPathAsync(pathname As String) As Task(Of StorageFile)
#Enable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
        Return New StorageFile(pathname)
    End Function

    Public Sub LaunchFile(filename As String)
        Dim plik As New StorageFile(Path, filename)
        plik.Launch()
    End Sub

    Public Sub FutureAccessListAddOrReplace(token As String)
        VBlib.SetSettingsString("FUTUREACCESSLIST_" & token, Path)
    End Sub

End Class

Public Class StorageFile

    ''' <summary>
    ''' full pathname of file
    ''' </summary>
    Public Property Path As String

    Public ReadOnly Property Name As String
        Get
            Return IO.Path.GetFileName(Path)
        End Get
    End Property

    Public Sub New(folderPath As String, filename As String)
        Path = IO.Path.Combine(folderPath, filename)
    End Sub

    Public Sub New(filePathname As String)
        Path = filePathname
    End Sub

#Disable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
    Public Async Function OpenStreamForWriteAsync() As Task(Of IO.Stream)
#Enable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
        Return IO.File.Open(Path, IO.FileMode.OpenOrCreate)
    End Function

#Disable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
    Public Async Function LaunchAsync() As Task
#Enable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
        Launch()
    End Function

    Public Sub Launch()
        Dim procInfo As New ProcessStartInfo(Path)
        procInfo.UseShellExecute = True
        Process.Start(procInfo)
        ' Process.Start(Path) daje
        ' System.ComponentModel.Win32Exception: 'An error occurred trying to start process 'E:\Temp\ballots\PKN\AnkietaPowszechna\prPN-prEN 18056E-Cultural Heritage -- Waterlogged archaeological wood -- Characterization of waterlogged archaeological wood to support decision-making processes for its preservation - Dziedzictwo kulturowe -- Mok__.pdf' with working directory 'H:\Home\PIOTR\VStudio\_Vs2017\Ballots\BallotsWPF\bin\Debug\net8.0-windows10.0.17763.0'. The specified executable is not a valid application for this OS platform.'
    End Sub

    Public Sub FutureAccessListAddOrReplace(token As String)
        VBlib.SetSettingsString("FUTUREACCESSLIST_" & token, Path)
    End Sub

#Disable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
    Public Shared Async Function GetFileFromPathAsync(filename As String) As Task(Of StorageFile)
#Enable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
        Return New StorageFile(filename)
    End Function

End Class

Public Class FutureAccessList

#Disable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
    Public Shared Async Function GetFolderAsync(token As String) As Task(Of StorageFolder)
#Enable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
        Return New StorageFolder(VBlib.GetSettingsString("FUTUREACCESSLIST_" & token))
    End Function
End Class


Public Enum CreationCollisionOption
    OpenIfExists
End Enum
