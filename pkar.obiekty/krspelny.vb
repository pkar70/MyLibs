Imports pkar.obiekty.services

Public Class krspelny

    Public Class Krs_OdpisPelny
        Public Property odpis As KrsP_OdpisPelny
    End Class

    Public Class KrsP_OdpisPelny
        Public Property rodzaj As String
        Public Property naglowekP As KrsP_Naglowekp
        Public Property dane As KrsP_Dane
    End Class

    Public Class KrsP_Naglowekp
        Public Property rejestr As String
        Public Property numerKRS As String
        Public Property dataCzasOdpisu As String
        Public Property stanZDnia As String
        Public Property wpis() As KrsP_Wpis
        Public Property stanPozycji As Integer
    End Class

    Public Class KrsP_Wpis
        Public Property numerWpisu As Integer
        Public Property opis As String
        Public Property dataWpisu As String
        Public Property sygnaturaAktSprawyDotyczacejWpisu As String
        Public Property oznaczenieSaduDokonujacegoWpisu As String
    End Class

    Public Class KrsP_Dane
        Public Property dzial1 As KrsP_Dzial1
        Public Property dzial2 As KrsP_Dzial2
        Public Property dzial3 As KrsP_Dzial3
        Public Property dzial4 As KrsP_Dzial4
        Public Property dzial5 As KrsP_Dzial5
        Public Property dzial6 As KrsP_Dzial6
    End Class

    Public Class KrsP_Dzial1
        Public Property danePodmiotu As KrsP_Danepodmiotu
        Public Property siedzibaIAdres As KrsP_Siedzibaiadres
        Public Property jednostkiTerenoweOddzialy() As KrsP_Jednostkiterenoweoddzialy
        Public Property umowaStatut As KrsP_Umowastatut
        Public Property pozostaleInformacje As KrsP_Pozostaleinformacje
        Public Property wspolnicySpzoo() As KrsP_Wspolnicyspzoo
        Public Property kapital As KrsP_Kapital
    End Class

    Public Class KrsP_Danepodmiotu
        Public Property formaPrawna() As KrsP_Formaprawna
        Public Property identyfikatory() As KrsP_Identyfikatory
        Public Property nazwa() As KrsP_Nazwa
        Public Property czyProwadziDzialalnoscZInnymiPodmiotami() As KrsP_Czyprowadzidzialalnosczinnymipodmiotami
        Public Property czyPosiadaStatusOPP() As KrsP_Czyposiadastatusopp
    End Class

    Public Class KrsP_Formaprawna
        Public Property formaPrawna As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Identyfikatory
        Public Property identyfikatory As Krs_Identyfikatory
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class



    Public Class KrsP_Nazwa
        Public Property nazwa As String
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Czyprowadzidzialalnosczinnymipodmiotami
        Public Property czyProwadziDzialalnoscZInnymiPodmiotami As Boolean
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Czyposiadastatusopp
        Public Property czyPosiadaStatusOPP As Boolean
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Siedzibaiadres
        Public Property siedziba() As Krs_Siedziba
        Public Property adres() As Krs_Adres
        Public Property adresPocztyElektronicznej() As Object
        Public Property adresStronyInternetowej() As Object
    End Class



    Public Class KrsP_Umowastatut
        Public Property informacjaOZawarciuZmianieUmowyStatutu() As KrsP_Informacjaozawarciuzmianieumowystatutu
    End Class

    Public Class KrsP_Informacjaozawarciuzmianieumowystatutu
        Public Property pozycja() As Krs_Informacjaozawarciuzmianieumowystatutu
    End Class


    Public Class KrsP_Pozostaleinformacje
        Public Property informacjaOCzasieNaJakiZostalUtworzonyPodmiot() As KrsP_Informacjaoczasienajakizostalutworzonypodmiot
        Public Property informacjaOPismieDoOgloszen() As Object
        Public Property informacjaOLiczbieUdzialow() As KrsP_Informacjaoliczbieudzialow
    End Class

    Public Class KrsP_Informacjaoczasienajakizostalutworzonypodmiot
        Public Property czasNaJakiUtworzonyZostalPodmiot As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Informacjaoliczbieudzialow
        Public Property informacjaOLiczbieUdzialow As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Kapital
        Public Property wysokoscKapitaluZakladowego() As Krs_Wysokosckapitaluzakladowego
        Public Property wniesioneAporty As KrsP_Wniesioneaporty
    End Class

    Public Class KrsP_Wniesioneaporty
        Public Property okreslenieWartosciUdzialowObjetychZaAport() As KrsP_Okresleniewartosciudzialowobjetychzaaport
    End Class

    Public Class KrsP_Okresleniewartosciudzialowobjetychzaaport
        Public Property pozycja() As Krs_Wysokosckapitaluzakladowego
    End Class





    Public Class KrsP_Jednostkiterenoweoddzialy
        Public Property nazwa() As KrsP_Nazwa1
        Public Property siedziba() As Krs_Siedziba
        Public Property adres() As Krs_Adres
    End Class

    Public Class KrsP_Nazwa1
        Public Property nazwa As String
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class




    Public Class KrsP_Wspolnicyspzoo
        Public Property nazwa() As KrsP_Nazwa2
        Public Property identyfikator() As Krs_IdentyfikatorRegon
        Public Property krs() As KrsP_IdKrs
        Public Property posiadaneUdzialy() As KrsP_Posiadaneudzialy
        Public Property czyPosiadaCaloscUdzialow() As KrsP_Czyposiadacaloscudzialow
    End Class

    Public Class KrsP_Nazwa2
        Public Property nazwa As String
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class



    Public Class KrsP_IdKrs
        Public Property krs As String
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Posiadaneudzialy
        Public Property posiadaneUdzialy As String
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Czyposiadacaloscudzialow
        Public Property czyPosiadaCaloscUdzialow As Boolean
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Dzial2
        Public Property reprezentacja() As KrsP_Reprezentacja
        Public Property prokurenci() As KrsP_Prokurenci
    End Class

    Public Class KrsP_Reprezentacja
        Public Property nazwaOrganu() As KrsP_Nazwaorganu
        Public Property sposobReprezentacji() As KrsP_Sposobreprezentacji
        Public Property sklad() As KrsP_Sklad
    End Class

    Public Class KrsP_Nazwaorganu
        Public Property nazwaOrganu As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Sposobreprezentacji
        Public Property sposobReprezentacji As String
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Sklad
        Public Property nazwisko() As KrsP_Nazwisko
        Public Property imiona() As KrsP_Imiona
        Public Property identyfikator() As Krs_IdPesel
        Public Property funkcjaWOrganie() As KrsP_Funkcjaworganie
        Public Property czyZawieszona() As KrsP_Czyzawieszona
        Public Property dataZawieszeniaDo() As Object
    End Class

    Public Class KrsP_Nazwisko
        Public Property nazwisko As Krs_Nazwisko1
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class


    Public Class KrsP_Imiona
        Public Property imiona As KrsP_Imiona1
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Imiona1
        Public Property imie As String
        Public Property imieDrugie As String
    End Class


    Public Class KrsP_Funkcjaworganie
        Public Property funkcjaWOrganie As String
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Czyzawieszona
        Public Property czyZawieszona As Boolean
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Prokurenci
        Public Property nazwisko() As KrsP_Nazwisko2
        Public Property imiona() As KrsP_Imiona2
        Public Property identyfikator() As Krs_IdPesel
        Public Property rodzajProkury() As KrsP_Rodzajprokury
    End Class

    Public Class KrsP_Nazwisko2
        Public Property nazwisko As Krs_Nazwisko
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class


    Public Class KrsP_Imiona2
        Public Property imiona As Krs_Imiona
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class


    Public Class KrsP_Rodzajprokury
        Public Property rodzajProkury As String
        Public Property nrWpisuWykr As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Dzial3
        Public Property przedmiotDzialalnosci As KrsP_Przedmiotdzialalnosci
        Public Property wzmiankiOZlozonychDokumentach As KrsP_Wzmiankiozlozonychdokumentach
        Public Property informacjaODniuKonczacymRokObrotowy As KrsP_Informacjaodniukonczacymrokobrotowy
    End Class

    Public Class KrsP_Przedmiotdzialalnosci
        Public Property przedmiotPrzewazajacejDzialalnosci() As KrsP_Przedmiotprzewazajacejdzialalnosci
        Public Property przedmiotPozostalejDzialalnosci() As KrsP_Przedmiotpozostalejdzialalnosci
    End Class

    Public Class KrsP_Przedmiotprzewazajacejdzialalnosci
        Public Property pozycja() As Krs_PKD
    End Class


    Public Class KrsP_Przedmiotpozostalejdzialalnosci
        Public Property pozycja() As Krs_PKD
    End Class


    Public Class KrsP_Wzmiankiozlozonychdokumentach
        Public Property wzmiankaOZlozeniuRocznegoSprawozdaniaFinansowego() As KrsP_Wzmiankaozlozeniurocznegosprawozdaniafinansowego
        Public Property wzmiankaOZlozeniuOpiniiBieglegoRewidentaSprawozdaniaZBadania() As Krs_PozycjeOkresOdDo
        Public Property wzmiankaOZlozeniuUchwalyPostanowieniaOZatwierdzeniuRocznegoSprawozdaniaFinansowego() As Krs_PozycjeOkresOdDo
        Public Property wzmiankaOZlozeniuSprawozdaniaZDzialalnosci() As Krs_PozycjeOkresOdDo
    End Class

    Public Class KrsP_Wzmiankaozlozeniurocznegosprawozdaniafinansowego
        Public Property pozycja() As Krs_Wzmiankaozlozeniurocznegosprawozdaniafinansowego
    End Class


    Public Class Krs_PozycjeOkresOdDo
        Public Property pozycja() As Krs_OkresOdDo
    End Class


    Public Class KrsP_Informacjaodniukonczacymrokobrotowy
        Public Property dzienKonczacyPierwszyRokObrotowy() As Krs_Informacjaodniukonczacymrokobrotowy
    End Class



    Public Class KrsP_Dzial4
    End Class

    Public Class KrsP_Dzial5
    End Class

    Public Class KrsP_Dzial6
        Public Property rozwiazanieUniewaznienie As KrsP_Rozwiazanieuniewaznienie
        Public Property polaczeniePodzialPrzeksztalcenie() As KrsP_Polaczeniepodzialprzeksztalcenie
    End Class

    Public Class KrsP_Rozwiazanieuniewaznienie
        Public Property okreslenieOkolicznosci() As Object
    End Class

    Public Class KrsP_Polaczeniepodzialprzeksztalcenie
        Public Property okreslenieOkolicznosci() As KrsP_Okreslenieokolicznosci
        Public Property opisPolaczeniaPodzialuPrzeksztalcenia() As KrsP_Opispolaczeniapodzialuprzeksztalcenia
        Public Property podmiotyPrzejmowane() As KrsP_Podmiotyprzejmowane
    End Class

    Public Class KrsP_Okreslenieokolicznosci
        Public Property okreslenieOkolicznosci As String
        Public Property nrWpisuWprow As String
        Public Property nrWpisuWykr As String
    End Class

    Public Class KrsP_Opispolaczeniapodzialuprzeksztalcenia
        Public Property opisPolaczeniaPodzialuPrzeksztalcenia As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Podmiotyprzejmowane
        Public Property nazwa() As KrsP_Nazwa3
        Public Property krajNazwaRejestru() As KrsP_Krajnazwarejestru
        Public Property nazwaSaduProwadzacegoRejestr() As Object
        Public Property identyfikator() As KrsP_Identyfikator3
        Public Property numerWRejestrzeAlboEwidencji() As KrsP_Numerwrejestrzealboewidencji
    End Class

    Public Class KrsP_Nazwa3
        Public Property nazwa As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Krajnazwarejestru
        Public Property krajNazwaRejestru As String
        Public Property nrWpisuWprow As String
    End Class

    Public Class KrsP_Identyfikator3
        Public Property regon As String
        Public Property identyfikator As Krs_IdentyfikatorRegon
        Public Property nrWpisuWprow As String
    End Class


    Public Class KrsP_Numerwrejestrzealboewidencji
        Public Property numerWRejestrzeAlboEwidencji As String
        Public Property nrWpisuWprow As String
    End Class

End Class
