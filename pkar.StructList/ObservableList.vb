
Imports System.Collections.Specialized
Imports System.ComponentModel

''' <summary>
''' simple implementation of ObservableList - only CollectionChanged is implemented (no PropertyChanged)
''' </summary>
''' <typeparam name="TYP"></typeparam>
Public Class ObservableList(Of TYP)
    Inherits List(Of TYP)
    Implements INotifyCollectionChanged, INotifyPropertyChanged

    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' Removes all elements from the list
    ''' </summary>
    Public Overloads Sub Clear()
        MyBase.Clear()

        RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
    End Sub

    ''' <summary>
    ''' Adds an object to the end of the list
    ''' </summary>
    ''' <param name="item">The object to be added to the end of the list. The value can be null for reference types.</param>
    Public Overloads Sub Add(item As TYP)
        MyBase.Add(item)

        Dim newItems As New List(Of TYP)
        newItems.Add(item)
        Dim eventArgs As New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems)
        RaiseEvent CollectionChanged(Me, eventArgs)
    End Sub

    ''' <summary>
    ''' Adds the elements of the specified collection to the end of the list.
    ''' </summary>
    ''' <param name="collection">The collection whose elements should be added to the end of the list. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
    Public Overloads Sub AddRange(collection As IEnumerable(Of TYP))
        MyBase.AddRange(collection)

        ' seems like only one item can be in modified items list (for data binding)
        ' https://www.codeproject.com/Articles/1004644/ObservableCollection-Simply-Explained
        For Each oItem As TYP In collection
            Dim eventArgs As New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection)
            RaiseEvent CollectionChanged(Me, eventArgs)
        Next

    End Sub

    ''' <summary>
    ''' Removes the first occurrence of a specific object from the list.
    ''' </summary>
    ''' <param name="item">The object to remove from the list. The value can be null for reference types.</param>
    Public Overloads Sub Remove(item As TYP)
        Dim oldItems As New List(Of TYP)
        oldItems.Add(item)
        Dim eventArgs As New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems)
        RaiseEvent CollectionChanged(Me, eventArgs)

        MyBase.Remove(item)
    End Sub

    ''' <summary>
    ''' Removes all the elements that match the conditions defined by the specified predicate.
    ''' </summary>
    ''' <param name="match">The delegate that defines the conditions of the elements to remove.</param>
    Public Overloads Sub RemoveAll(match As Predicate(Of TYP))

        For Each oItem As TYP In Me.FindAll(match)
            Dim oldItems As New List(Of TYP)
            oldItems.Add(oItem)
            Dim eventArgs As New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems)
            RaiseEvent CollectionChanged(Me, eventArgs)
        Next

        MyBase.RemoveAll(match)
    End Sub

    ''' <summary>
    ''' Removes the element at the specified index of the list.
    ''' </summary>
    ''' <param name="index">The zero-based index of the element to remove.</param>
    Public Overloads Sub RemoveAt(index As Int32)
        Dim oldItems As New List(Of TYP)
        oldItems.Add(Item(index))
        Dim eventArgs As New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems)
        RaiseEvent CollectionChanged(Me, eventArgs)

        MyBase.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Removes a range of elements from the list.
    ''' </summary>
    ''' <param name="index">The zero-based starting index of the range of elements to remove.</param>
    ''' <param name="count">The number of elements to remove.</param>
    Public Overloads Sub RemoveRange(index As Int32, count As Int32)
        If index < 0 Then Throw New ArgumentOutOfRangeException(NameOf(index), "cannot be less than 0")
        If count < 0 Then Throw New ArgumentOutOfRangeException(NameOf(count), "cannot be less than 0")

        ' prepare list of items to be removed
        Dim lista As New List(Of TYP)
        For iLP = index To Math.Min(index + count - 1, Me.Count)
            lista.Add(Item(iLP))
        Next

        ' and raise event for all of them
        For Each oItem As TYP In lista
            Dim oldItems As New List(Of TYP)
            oldItems.Add(oItem)
            Dim eventArgs As New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems)
            RaiseEvent CollectionChanged(Me, eventArgs)
        Next

        MyBase.RemoveRange(index, count)
    End Sub

    ''' <summary>
    ''' Reverses the order of the elements in the entire list.
    ''' </summary>
    Public Overloads Sub Reverse()
        MyBase.Reverse()
        RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
    End Sub

    ''' <summary>
    ''' Reverses the order of the elements in the specified range.
    ''' </summary>
    ''' <param name="index">Reverses the order of the elements in the specified range.</param>
    ''' <param name="count">The number of elements in the range to reverse.</param>
    Public Overloads Sub Reverse(index As Int32, count As Int32)
        MyBase.Reverse(index, count)
        RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
    End Sub

    ''' <summary>
    ''' Sorts the elements in the entire list using the default comparer.
    ''' </summary>
    Public Overloads Sub Sort()
        MyBase.Sort()
        RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
    End Sub

    ''' <summary>
    ''' Sorts the elements in the entire List using the specified Comparison.
    ''' </summary>
    ''' <param name="compar">The Comparison to use when comparing elements.</param>
    Public Overloads Sub Sort(compar As Comparison(Of TYP))
        MyBase.Sort(compar)
        RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
    End Sub

    ''' <summary>
    ''' Sorts the elements in the entire List using the specified comparer.
    ''' </summary>
    ''' <param name="compar">The IComparer implementation to use when comparing elements, or null to use the default comparer Default.</param>
    Public Overloads Sub Sort(compar As IComparer(Of TYP))
        MyBase.Sort(compar)
        RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
    End Sub

    ''' <summary>
    ''' Sorts the elements in a range of elements in List using the specified comparer.
    ''' </summary>
    ''' <param name="index">The zero-based starting index of the range to sort.</param>
    ''' <param name="count">The length of the range to sort.</param>
    ''' <param name="compar">The IComparer implementation to use when comparing elements, or null to use the default comparer Default.</param>
    Public Overloads Sub Sort(index As Int32, count As Int32, compar As IComparer(Of TYP))
        MyBase.Sort(index, count, compar)
        RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
    End Sub

End Class
