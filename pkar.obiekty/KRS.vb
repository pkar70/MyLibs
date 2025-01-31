

Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text.RegularExpressions



Public Class Krs
    Inherits ObiektyBaseString
    Implements IObiekty, IObiektCache


    Public Sub New(krs As String)
        MyBase.New(krs)
    End Sub

    Public Shared Function FromNip(nip As String) As Krs
        Dim nipek As New NIP(nip)
        If Not nipek.IsValid Then Return Nothing

        ' *TODO* zamień NIP na KRS
        Dim tmp As String

        Return New Krs(tmp)
    End Function

    Public Shared Function FromRegon(regon As String) As Krs
        Return Nothing
    End Function


#Region "ogólne, to co zawsze"

    Private _cacheContent As BaseList(Of Krs_OdpisAktualny)

    Public Sub CacheInit(fold As String) Implements IObiektCache.CacheInit
        _cacheContent = New BaseList(Of Krs_OdpisAktualny)(fold, "KRS.OdpAkt.json")
    End Sub

    Public Sub CacheLoad() Implements IObiektCache.CacheLoad
        _cacheContent.Load()
    End Sub

    Public Sub CacheSave(Optional autoTrim As Boolean = True) Implements IObiektCache.CacheSave
        _cacheContent.Save()
    End Sub

    Public Sub CacheTrim() Implements IObiektCache.CacheTrim

        If _cacheContent.Count < 100 Then Return

        While _cacheContent.Count < 100
            _cacheContent.RemoveAt(0)
        End While

    End Sub

    Public Overloads Function IsCacheable() As Boolean Implements IObiekty.IsCacheable
        Return True
    End Function

    Public Overloads Function HasRegexp() As Boolean Implements IObiekty.HasRegexp
        Return True
    End Function

    Public Overloads Function GetRegexp() As String Implements IObiekty.GetRegexp
        Return "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
    End Function

    Public Function CacheIsLoaded() As Boolean Implements IObiektCache.CacheIsLoaded
        If _cacheContent Is Nothing Then Return False
        Return _cacheContent.Count > 0
    End Function

    Public Function CacheGetSize() As Integer Implements IObiektCache.CacheGetSize
        Return _cacheContent.GetFileSize
    End Function

    ''' <summary>
    ''' zwraca pierwszy znaleziony numer KRS (musi być poprawny), albo NULL
    ''' </summary>
    ''' <param name="tekst">tekst w którym trzeba szukać kodu</param>
    Public Shared Function TryParse(tekst As String) As Krs
        Dim macze As MatchCollection = Regex.Matches(tekst, New KodPocztowy("").GetRegexp)

        For Each macz As Match In macze
            Dim ret As New Krs(macz.Value)
            If ret.IsValid Then Return ret
        Next

        Return Nothing
    End Function


#End Region


    ' dokumentacja: https://prs.ms.gov.pl/krs/openApi

    Public Async Function GetOdpisAktualny(Optional canUseOnline As Boolean = True, Optional forceOnline As Boolean = False) As Task(Of Krs_OdpisAktualny)

        If forceOnline AndAlso Not canUseOnline Then Throw New ArgumentException("Nie można żądać online oraz jednocześnie blokować online")

        If Not forceOnline AndAlso CacheIsLoaded() Then
            Dim item As Krs_OdpisAktualny = _cacheContent.FirstOrDefault(Function(x) x.odpis.naglowekA.numerKRS = v)
            If item IsNot Nothing Then Return item
        End If

        If Not canUseOnline Then Return Nothing

        ' no to ściągnij
        Dim webcln As New HttpClient
        webcln.DefaultRequestHeaders.Accept.Clear()
        webcln.DefaultRequestHeaders.Accept.TryParseAdd("application/json")
        Dim link As String = $"https://api-krs.ms.gov.pl/api/krs/OdpisAktualny/{v}?rejestr=P&format=json"

        Dim str As String = Await webcln.GetStringAsync(link)

        Dim nowe As New BaseList(Of Krs_OdpisAktualny)(Nothing)
        Try
            ' najprościej - import udawanej listy jednoelementowej
            nowe.Import("[" & str & "]")
        Catch ex As Exception
        End Try

        If nowe.Count < 1 Then Return Nothing

        nowe(0).gotAt = Date.Now
        _cacheContent.Add(nowe(0))
        _cacheContent.Save()

        Return nowe(0)

    End Function

    ' https://api-krs.ms.gov.pl/api/krs/OdpisAktualny/{krs}?rejestr={rejestr}&format=json
    ' P przeds, S stow

    ' https://api-krs.ms.gov.pl/api/krs/OdpisPelny/{krs}?rejestr={rejestr}&format=json

    ' interesujące są:
    ' odpis.dane.Krs_Dzial1.danePodmiotu.nazwa
    ' odpis.dane.Krs_Dzial1.danePodmiotu.formaPrawna
    ' odpis.dane.Krs_Dzial1.siedzibaIAdres.adres (.ulica, .miejscowosc itd.)

    ' odpis.dane.Krs_Dzial3.przedmiotDzialalnosci.przedmiotPrzewazajacejDzialalnosci i pozostalej

    ' generowanie linku do strony z KRS (nie JSON, tylko WWW)


End Class

#Region "klasy wygenerowane z JSON"


Public Class Krs_OdpisAktualny
    Inherits pkar.BaseStruct

    Public Property odpis As Krs_Odpis

    ''' <summary>
    ''' kiedy został ściągnięty - ważne potem dla cache
    ''' </summary>
    Public Property gotAt As Date
End Class

Public Class Krs_Odpis
    Public Property rodzaj As String
    Public Property naglowekA As Krs_NaglowekA
    Public Property dane As Krs_Dane
End Class

Public Class Krs_NaglowekA
    Public Property rejestr As String
    Public Property numerKRS As String
    Public Property dataCzasOdpisu As String
    Public Property stanZDnia As String
    Public Property dataRejestracjiWKRS As String
    Public Property numerOstatniegoWpisu As Integer
    Public Property dataOstatniegoWpisu As String
    Public Property sygnaturaAktSprawyDotyczacejOstatniegoWpisu As String
    Public Property oznaczenieSaduDokonujacegoOstatniegoWpisu As String
    Public Property stanPozycji As Integer
End Class

Public Class Krs_Dane
    Public Property dzial1 As Krs_Dzial1
    Public Property dzial2 As Krs_Dzial2
    Public Property dzial3 As Krs_Dzial3
    Public Property dzial4 As Krs_Dzial4
    Public Property dzial5 As Krs_Dzial5
    Public Property dzial6 As Krs_Dzial6
End Class

#Region "odpis dział 1"

Public Class Krs_Dzial1
    Public Property danePodmiotu As Krs_Danepodmiotu
    Public Property siedzibaIAdres As Krs_Siedzibaiadres
    Public Property jednostkiTerenoweOddzialy() As Krs_Jednostkiterenoweoddzialy
    Public Property umowaStatut As Krs_Umowastatut
    Public Property pozostaleInformacje As Krs_Pozostaleinformacje
    Public Property wspolnicySpzoo() As Krs_Wspolnicyspzoo
    Public Property kapital As Krs_Kapital
    Public Property sposobPowstaniaPodmiotu As Krs_Sposobpowstaniapodmiotu
    Public Property emisjeAkcji() As Krs_Emisjeakcji
    Public Property wzmiankaOUpowaznieniuDoEmisjiWarrantowSubskrypcyjnych As Krs_Wzmiankaoupowaznieniudoemisjiwarrantowsubskrypcyjnych
    Public Property wspolnicyPartnerzy() As Krs_Sklad

End Class

Public Class Krs_Wzmiankaoupowaznieniudoemisjiwarrantowsubskrypcyjnych
    Public Property czyZarzadJestUprawnionyDoEmisjiWarrantowSubskrypcyjnych As Boolean
End Class

Public Class Krs_Emisjeakcji
    Public Property nazwaSeriiAkcji As String
    Public Property liczbaAkcjiWSerii As String
    Public Property czyAkcjeUprzywilejowaneLiczbaAkcjiRodzajUprzywilejowania As String
End Class



Public Class Krs_Sposobpowstaniapodmiotu
    Public Property okolicznosciPowstania As String
    Public Property opisSposobuPowstaniaInformacjaOUchwale As String
    Public Property podmioty() As Krs_Podmiot

End Class

Public Class Krs_Podmiot
    Public Property nazwa As String
    Public Property krajNazwaRejestruEwidencji As String
    Public Property identyfikator As Krs_IdentyfikatorRegon
    Public Property nip As String
End Class


Public Class Krs_Danepodmiotu
    Public Property formaPrawna As String
    Public Property identyfikatory As Krs_Identyfikatory
    Public Property nazwa As String
    Public Property daneOWczesniejszejRejestracji As Krs_Daneowczesniejszejrejestracji
    Public Property czyProwadziDzialalnoscZInnymiPodmiotami As Boolean
    Public Property czyPosiadaStatusOPP As Boolean
End Class

Public Class Krs_Identyfikatory
    Public Property regon As String
    Public Property nip As String
End Class

Public Class Krs_Daneowczesniejszejrejestracji
    Public Property nazwaPoprzedniegoRejestru As String
    Public Property numerWPoprzednimRejestrze As String
    Public Property sadProwadzacyRejestr As String

End Class

Public Class Krs_Siedzibaiadres
    Public Property siedziba As Krs_Siedziba
    Public Property adres As Krs_Adres
    Public Property adresPocztyElektronicznej As String
    Public Property adresStronyInternetowej As String

End Class

Public Class Krs_Siedziba
    Public Property kraj As String
    Public Property wojewodztwo As String
    Public Property powiat As String
    Public Property gmina As String
    Public Property miejscowosc As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String
    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWykr As String
End Class

Public Class Krs_Adres
    Public Property ulica As String
    Public Property nrDomu As String
    Public Property miejscowosc As String
    Public Property kodPocztowy As String
    Public Property poczta As String
    Public Property kraj As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String
    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWykr As String
End Class

Public Class Krs_Umowastatut
    Public Property informacjaOZawarciuZmianieUmowyStatutu() As Krs_Informacjaozawarciuzmianieumowystatutu
End Class

Public Class Krs_Informacjaozawarciuzmianieumowystatutu
    Public Property zawarcieZmianaUmowyStatutu As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWykr As String
    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String

End Class

Public Class Krs_Pozostaleinformacje
    Public Property czasNaJakiUtworzonyZostalPodmiot As String
    Public Property informacjaOLiczbieUdzialow As String
    Public Property czyUmowaStatutPrzyznajeUprawnieniaOsobiste As Boolean
    Public Property czyObligatoriuszeMajaPrawoDoUdzialuWZysku As Boolean

End Class

Public Class Krs_Kapital
    Public Property wysokoscKapitaluZakladowego As Krs_Wysokosckapitaluzakladowego
    Public Property wniesioneAporty As Krs_Wniesioneaporty
    Public Property lacznaLiczbaAkcjiUdzialow As String
    Public Property wartoscJednejAkcji As Krs_Wysokosckapitaluzakladowego
    Public Property czescKapitaluWplaconegoPokrytego As Krs_Wysokosckapitaluzakladowego
    Public Property wartoscWarunkowegoPodwyzszeniaKapitaluZakladowego As Krs_Wysokosckapitaluzakladowego

End Class

Public Class Krs_Wysokosckapitaluzakladowego
    Public Property wartosc As String
    Public Property waluta As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWykr As String
    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String
End Class

Public Class Krs_Wniesioneaporty
    Public Property okreslenieWartosciUdzialowObjetychZaAport() As Krs_Okresleniewartosciudzialowobjetychzaaport
End Class

Public Class Krs_Okresleniewartosciudzialowobjetychzaaport
    Public Property wartosc As String
    Public Property waluta As String
End Class

Public Class Krs_Jednostkiterenoweoddzialy
    Public Property nazwa As String
    Public Property siedziba As Krs_Siedziba
    Public Property adres As Krs_Adres
End Class



Public Class Krs_Wspolnicyspzoo
    Public Property nazwa As String
    Public Property identyfikator As Krs_IdentyfikatorRegon
    Public Property krs As Krs_Krs
    Public Property posiadaneUdzialy As String
    Public Property czyPosiadaCaloscUdzialow As Boolean
End Class

Public Class Krs_IdentyfikatorRegon
    Public Property regon As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWykr As String
    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String
End Class

Public Class Krs_Krs
    Public Property krs As String
End Class

#End Region

#Region "odpis dział 2"


Public Class Krs_Dzial2
    Public Property reprezentacja As Krs_Reprezentacja
    Public Property prokurenci() As Krs_Prokurenci

    Public Property organNadzoru() As Krs_Organnadzoru

End Class

Public Class Krs_Organnadzoru
    Public Property nazwa As String
    Public Property sklad() As Krs_Sklad
End Class


Public Class Krs_Reprezentacja
    Public Property nazwaOrganu As String
    Public Property sposobReprezentacji As String
    Public Property sklad() As Krs_Sklad
End Class

Public Class Krs_Sklad
    Public Property nazwisko As Krs_Nazwisko
    Public Property imiona As Krs_Imiona
    Public Property identyfikator As Krs_IdPesel

    ''' <summary>
    ''' nie w OrganNadzoru
    ''' </summary>
    Public Property funkcjaWOrganie As String
    ''' <summary>
    ''' nie w OrganNadzoru
    ''' </summary>
    Public Property czyZawieszona As Boolean
    Public Property rodzajProkury As String
    Public Property czyPozostajeWZwiazkuMalzenskim As Boolean
    Public Property czyZawartaMalzenskaUmowaMajatkowa As Boolean
    Public Property czyPowstalaRozdzielnoscMajatkowa As Boolean
    Public Property czyMaOgraniczonaZdolnoscDoCzynnosciPrawnych As Boolean

End Class

Public Class Krs_Nazwisko
    Public Property nazwiskoICzlon As String
End Class

Public Class Krs_Imiona
    Public Property imie As String
    Public Property imieDrugie As String
End Class

Public Class Krs_Prokurenci
    Public Property nazwisko As Krs_Nazwisko1
    Public Property imiona As Krs_Imiona
    Public Property identyfikator As Krs_IdPesel
    Public Property rodzajProkury As String
End Class

Public Class Krs_Nazwisko1
    Public Property nazwiskoICzlon As String
End Class


Public Class Krs_IdPesel
    Public Property pesel As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWykr As String
    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String
End Class

#End Region

#Region "odpis dział 3"
Public Class Krs_Dzial3
    Public Property przedmiotDzialalnosci As Krs_Przedmiotdzialalnosci
    Public Property wzmiankiOZlozonychDokumentach As Krs_Wzmiankiozlozonychdokumentach
    Public Property informacjaODniuKonczacymRokObrotowy As Krs_Informacjaodniukonczacymrokobrotowy
End Class

Public Class Krs_Przedmiotdzialalnosci
    Public Property przedmiotPrzewazajacejDzialalnosci As List(Of Krs_PKD)
    Public Property przedmiotPozostalejDzialalnosci As List(Of Krs_PKD)
End Class


Public Class Krs_PKD
    Public Property opis As String
    Public Property kodDzial As String
    Public Property kodKlasa As String
    Public Property kodPodklasa As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWykr As String
    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String
End Class

Public Class Krs_Wzmiankiozlozonychdokumentach
    Public Property wzmiankaOZlozeniuRocznegoSprawozdaniaFinansowego() As Krs_Wzmiankaozlozeniurocznegosprawozdaniafinansowego
    Public Property wzmiankaOZlozeniuOpiniiBieglegoRewidentaSprawozdaniaZBadania() As Krs_OkresOdDo
    Public Property wzmiankaOZlozeniuUchwalyPostanowieniaOZatwierdzeniuRocznegoSprawozdaniaFinansowego() As Krs_OkresOdDo
    Public Property wzmiankaOZlozeniuSprawozdaniaZDzialalnosci() As Krs_OkresOdDo
End Class

Public Class Krs_Wzmiankaozlozeniurocznegosprawozdaniafinansowego
    Public Property dataZlozenia As String
    Public Property zaOkresOdDo As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String
End Class

Public Class Krs_OkresOdDo
    Public Property zaOkresOdDo As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String
End Class

Public Class Krs_Informacjaodniukonczacymrokobrotowy
    Public Property dzienKonczacyPierwszyRokObrotowy As String

    ''' <summary>
    ''' tylko w pełnym odpisie
    ''' </summary>
    Public Property nrWpisuWprow As String
End Class

#End Region

#Region "odpis dział 4,5 (puste?)"
Public Class Krs_Dzial4
End Class

Public Class Krs_Dzial5
End Class

#End Region

#Region "odpis dział 6"
Public Class Krs_Dzial6
    Public Property polaczeniePodzialPrzeksztalcenie() As Krs_Polaczeniepodzialprzeksztalcenie
End Class

Public Class Krs_Polaczeniepodzialprzeksztalcenie
    Public Property okreslenieOkolicznosci As String
    Public Property opisPolaczeniaPodzialuPrzeksztalcenia As String
    Public Property podmiotyPrzejmowane() As Krs_Podmiotyprzejmowane
End Class

Public Class Krs_Podmiotyprzejmowane
    Public Property nazwa As String
    Public Property krajNazwaRejestru As String
    Public Property identyfikator As Krs_IdPrzejm
    Public Property nazwaSadu As String
End Class

Public Class Krs_IdPrzejm
    Public Property identyfikator As Krs_IdentyfikatorRegon
End Class

#End Region

#End Region


'  Dim query As String = "{""rejestr"": [""P""],""podmiot"": {""nip"":""" & krs & """}}"""




























'ret.nazwa
' ret.adres =


' https://prs-openapi2-prs-prod.apps.ocp.prod.ms.gov.pl/api/wyszukiwarka/krs
'      curl ^ "https://prs-openapi2-prs-prod.apps.ocp.prod.ms.gov.pl/api/wyszukiwarka/krs^" ^
'-H ^ "Accept: application/json, text/plain, */*^" ^
'-H ^ "Accept-Language: pl,en-US;q=0.9,en;q=0.8^" ^
'-H ^ "Connection: keep-alive^" ^
'-H ^ "Content-Type: application/json^" ^
'-H ^ "Origin: https://wyszukiwarka-krs.ms.gov.pl^" ^
'-H ^ "Referer: https://wyszukiwarka-krs.ms.gov.pl/^" ^
'-H ^ "Sec-Fetch-Dest: empty^" ^
'-H ^ "Sec-Fetch-Mode: cors^" ^
'-H ^ "Sec-Fetch-Site: same-site^" ^
'-H ^ "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36^" ^
'-H ^ "sec-ch-ua: ^\^"Microsoft Edge^\^";krs=^\^"131^\^", ^\^"Chromium^\^";krs=^\^"131^\^", ^\^"Not_A Brand^\^";krs=^\^"24^\^"^" ^
'-H ^"sec-ch-ua-mobile: ?0^" ^
'-H ^"sec-ch-ua-platform: ^\^"Windows^\^"^" ^
'-H ^"x-api-key: TopSecretApiKey^" ^
'--data-raw ^"^{^\^"rejestr^\^":^[^\^"P^\^"^],^\^"podmiot^\^":^{^\^"krs^\^":null,^\^"nip^\^":^\^"6182045200^\^",^\^"regon^\^":null,^\^"nazwa^\^":null,^\^"wojewodztwo^\^":null,^\^"powiat^\^":^\^"^\^",^\^"gmina^\^":^\^"^\^",^\^"miejscowosc^\^":^\^"^\^"^},^\^"status^\^":^{^\^"czyOpp^\^":null,^\^"czyWpisDotyczacyPostepowaniaUpadlosciowego^\^":null,^\^"dataPrzyznaniaStatutuOppOd^\^":null,^\^"dataPrzyznaniaStatutuOppDo^\^":null^},^\^"paginacja^\^":^{^\^"liczbaElementowNaStronie^\^":100,^\^"maksymalnaLiczbaWynikow^\^":100,^\^"numerStrony^\^":1^}^}^"


' działa także:
'curl ^ "https://prs-openapi2-prs-prod.apps.ocp.prod.ms.gov.pl/api/wyszukiwarka/krs^" ^
'-H ^"Accept: application/json, text/plain, */*^" ^
'-H ^"Content-Type: application/json^" ^
'--data-raw ^ "^{^\^"rejestr^\^":^[^\^"P^\^"^],^\^"podmiot^\^":^{^\^"nip^\^":^\^"6182045200^\^"}^}^"

' {
'    "liczbaPodmiotow" 1,
'    "listaPodmiotow": [
'        {
'            "czyOPP": false,
'            "dataPrzyznaniaStatutuOPP": null,
'            "czyUpadlosc":  false,
'            "miejscowosc": "OPATÓWEK",
'            "nazwa": "COLIAN SPÓŁKA Z OGRANICZONĄ ODPOWIEDZIALNOŚCIĄ",
'            "numer": "269526",
'            "typRejestru": "P"
'        }
'    ]
'}

' i dalej idzie z KRSu



