
Imports pkar.DotNetExtensions

Public MustInherit Class ObiektyBaseString
    Inherits pkar.BaseStruct
    Implements IObiekty

    Public ReadOnly Property v As String

    Public Sub New(tekst As String)
        v = tekst
    End Sub

    Public Overrides Function ToString() As String
        Return v
    End Function

    Public Overridable Function GetDictionaryType() As DictionaryType Implements IObiekty.GetDictionaryType
        Return DictionaryType.None
    End Function

    Public Overridable Function GetDictionary(Optional searchTerm As String = "") As String()
        Return Nothing
    End Function

    Public Overridable Function IsCacheable() As Boolean Implements IObiekty.IsCacheable
        Return False
    End Function



    ''' <summary>
    ''' sprawdza czy istnieje w słowniku lub czy pasuje do regexp, gdy nie ma ani słownika ani regexpa zwraca true
    ''' </summary>
    Public Overridable Function IsValid(Optional useonline As Boolean = False) As Boolean Implements IObiekty.IsValid
        If GetDictionaryType() = DictionaryType.Hardcoded OrElse GetDictionaryType() = DictionaryType.Downloadable Then

#If NETSTANDARD2_0_OR_GREATER Then
        Return GetDictionary.Any(Function(x) x.EqualsCIAI(v))
#Else
            Return GetDictionary.Any(Function(x) x.EqualsCI(v))
#End If

        End If

        ' nie ma słownika
        If Not HasRegexp() Then Return True

        Return Text.RegularExpressions.Regex.IsMatch(v, GetRegexp)

    End Function

    Public Overridable Function HasRegexp() As Boolean Implements IObiekty.HasRegexp
        Return False
    End Function

    Public Overridable Function GetRegexp() As String Implements IObiekty.GetRegexp
        Throw New NotImplementedException()
    End Function


    Public Shared Operator =(ByVal val1 As ObiektyBaseString, ByVal val2 As ObiektyBaseString) As Boolean
        Return val1.v.EqualsCI(val2.v)
    End Operator

    Public Shared Operator <>(ByVal val1 As ObiektyBaseString, ByVal val2 As ObiektyBaseString) As Boolean
        Return Not val1.v.EqualsCI(val2.v)
    End Operator

End Class


Public Interface IObiekty

    ''' <summary>
    ''' TRUE gdy kod/entry jest poprawne. Głównie maska, słownik, i ewentualnie online
    ''' </summary>
    ''' <param name="useonline">czy ma użyć validatorów online</param>
    ''' <returns></returns>
    Function IsValid(Optional useonline As Boolean = False) As Boolean

    ''' <summary>
    ''' TRUE gdy jest słownik z którym można sprawdzać wartość
    ''' </summary>
    ''' <returns></returns>
    Function GetDictionaryType() As DictionaryType

    ''' <summary>
    ''' czy jest możliwość cache wartości
    ''' </summary>
    ''' <returns></returns>
    Function IsCacheable() As Boolean

    ''' <summary>
    ''' czy ma regexp walidacji
    ''' </summary>
    Function HasRegexp() As Boolean
    ''' <summary>
    ''' regexp walidacji poprawności
    ''' </summary>
    Function GetRegexp() As String

End Interface

''' <summary>
''' Operacje na słowniku obiektu
''' </summary>
Public Interface IObiektyDiction
    ''' <summary>
    ''' Pobierz słownik możliwości, z ewentualnym filtrowaniem
    ''' </summary>
    ''' <param name="searchTerm"></param>
    ''' <returns></returns>
    Function GetDictionary(Optional searchTerm As String = "") As String()


End Interface



''' <summary>
''' Operacje na cache obiektu
''' </summary>
Public Interface IObiektCache
    ''' <summary>
    ''' Folder dla cache - np. common, prywatny app, itp.
    ''' </summary>
    Sub CacheInit(fold As String)

    ''' <summary>
    ''' Wczytanie cache
    ''' </summary>
    Sub CacheLoad()

    ''' <summary>
    ''' Zapis cache
    ''' </summary>
    Sub CacheSave(Optional autoTrim As Boolean = True)

    ''' <summary>
    ''' Loaded = jest coś w cache
    ''' </summary>
    ''' <returns></returns>
    Function CacheIsLoaded() As Boolean

    ''' <summary>
    ''' Przytnij cache wedle uznania
    ''' </summary>
    Sub CacheTrim()

    ''' <summary>
    ''' Wielkość pliku z cache (w bajtach)
    ''' </summary>
    ''' <returns></returns>
    Function CacheGetSize() As Integer
End Interface


Public Enum DictionaryType
    None
    Hardcoded
    Downloadable
    OnlineVerifiable
End Enum
