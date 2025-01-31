Imports pkar.obiekty
Imports pkar.DotNetExtensions
Imports pkar
Imports System.Reflection.Metadata
Imports pkar.UI.Extensions
Imports System.Globalization


Class MainWindow
    Private Sub uiPesel1_TextChanged(sender As Object, e As TextChangedEventArgs)

        Dim p1 As New PESEL(uiPesel1.Text)
        Dim p2 As New PESEL(uiPesel2.Text)

        uiPesel1valid.IsChecked = p1.IsValid
        uiPesel2valid.IsChecked = p2.IsValid

        uiPesel1data.Text = p1.BirthDate.ToExifString
        uiPesel2data.Text = p2.BirthDate.ToExifString

        uiPesel1male.IsChecked = p1.IsMale
        uiPesel2male.IsChecked = p2.IsMale

        uiPesel1sex.Text = ""
        uiPesel2sex.Text = ""
        For Each typ As PESEL.IconType In [Enum].GetValues(GetType(PESEL.IconType))
            uiPesel1sex.Text &= p1.SexIcon(typ)
            uiPesel2sex.Text &= p2.SexIcon(typ)
        Next


        uiPesel1age.Text = p1.GetAge.ToString("##0.00")
        uiPesel2age.Text = p2.GetAge.ToString("##0.00")

        uiPesel1adult.IsChecked = p1.IsAdult
        uiPesel2adult.IsChecked = p2.IsAdult

        uiPesel1samesex.IsChecked = p1.SameSex(p2)
        uiPesel2samesex.IsChecked = p2.SameSex(p1)

        uiPesel1sameday.IsChecked = p1.SameBirthday(p2)
        uiPesel2sameday.IsChecked = p2.SameBirthday(p1)


    End Sub


    Private Shared EANsvcslist As String() = {
"https://www.gs1.org/services/verified-by-gs1/results?gtin=%EAN",
"https://www.ean-search.org/?q=%EAN",
"https://www.szukaj-ean.pl/EAN-%EAN",
"https://pl.product-search.net/?q=%EAN",
"https://www.eprodukty.gs1.pl/catalog/0%EAN",
"https://www.barcodelookup.com/%EAN"
        }

    Private Sub uiEAN_TextChanged(sender As Object, e As TextChangedEventArgs)
        Dim kod As New EAN(uiEAN.Text)

        uiEANvalid.IsChecked = kod.IsValid
        uiEANkraj.Text = kod.GetCountry

        If kod.IsValid Then
            uiLinkList.Children.Clear()
            For Each linek In EANsvcslist
                uiLinkList.Children.Add(New TextBlock With {.Text = linek.Replace("%EAN", kod.v)})
            Next
        End If

    End Sub

    Private Sub uiNIP_TextChanged(sender As Object, e As TextChangedEventArgs) Handles uiNIP.TextChanged
        Dim kod As New NIP(uiNIP.Text)

        uiNIPvalid.IsChecked = kod.IsValid
        uiNIPkraj.Text = kod.GetUrzSkarb
    End Sub

    Private Async Sub uiTerytTry_Click(sender As Object, e As RoutedEventArgs)

        Dim klient As New TERYT("pkar", "t1Cv15F1t")
        'Dim klient As New TERYT("TestPubliczny", "1234abcd")
        If Not Await klient.CzyZalogowanyAsync Then Return

        ' wojew bez zmian od 1/1/1999

        'For iWoj = 2 To 8 Step 2


        '    Dim wold = (Await klient.PobierzListePowiatowAsync("0" & iWoj, New Date(2015, 1, 1))).OrderBy(Of String)(Function(x) x.POW)
        '    Dim wnew = (Await klient.PobierzListePowiatowAsync("0" & iWoj)).OrderBy(Of String)(Function(x) x.POW)

        '    Dim str1 As String = ""
        '    If wold.Count <> wnew.Count Then
        '        str1 = "rozne liczby"
        '    Else
        '        For iLp = 0 To wold.Count - 1
        '            If wold(iLp).NAZWA <> wnew(iLp).NAZWA Then
        '                str1 &= "diff"
        '            End If
        '            If wold(iLp).NAZWA_DOD <> wnew(iLp).NAZWA_DOD Then
        '                str1 &= "diff"
        '            End If
        '            If wold(iLp).WOJ <> wnew(iLp).WOJ Then
        '                str1 &= "diff"
        '            End If
        '            If wold(iLp).POW <> wnew(iLp).POW Then
        '                str1 &= "diff"
        '            End If
        '        Next
        '    End If

        'Next

        klient.Cache.Init(GetCommonCachePath)
        'Await klient.Cache.SyncAsync()   '.ForceDownloadAsync(TERYT.TerytCache.KtoryPlik.TERC, False)
        'klient.Cache.Load()

        'Dim t1 = Await klient.TerytWLiczbachTypAsync()
        'Dim t2 = Await klient.RaportLiczbaMiejscowosciWiejskichAsync
        'Dim t4 = Await klient.RaportLiczbaJednostekTercAsync

        'Dim rv = klient.Cache.PobierzListeWojewodztw


        'uiTerytTERC.Text = (Await klient.PobierzDateAktualnegoKatTercAsync).ToString("yyyy.MM.dd")
        'uiTerytNTS.Text = (Await klient.PobierzDateAktualnegoKatNTSAsync).ToString("yyyy.MM.dd")
        'uiTerytSIMC.Text = (Await klient.PobierzDateAktualnegoKatSimcAsync).ToString("yyyy.MM.dd")
        'uiTerytULIC.Text = (Await klient.PobierzDateAktualnegoKatUlicAsync).ToString("yyyy.MM.dd")


        'Dim regsy = Await klient.PobierzListeRegionowAsync(New Date(2005, 1, 1))
        'Dim cnt = regsy.Count

        'Dim wojsy = Await klient.PobierzListeWojewodztwWRegionieAsync("2", Date.Now)
        ' Dim podr = Await klient.PobierzListePodregionowAsync("12", Date.Now)
        ' Dim powsy = Await klient.PobierzListePowiatowWPodregionieAsync("21", Date.Now)
        'Dim gms = Await klient.PobierzListeMiejscowosciWGminieAsync("mał", "kraków", "kraków", Date.Now) ' 0950470
        'Dim gms1 = Await klient.PobierzListeUlicDlaMiejscowosciAsync("12", "61", "02", "9", "0950463", True, False, Date.Now)

        'Dim gm1 = Await klient.PobierzListeMiejscowosciWRodzajuGminyAsync("12", "61", "02", "1") ' 0950470
        'Dim gs1 = Await klient.WyszukajMiejscowoscWJPTAsync("", "", "", "Załęże", "")
        'Dim gs1 = Await klient.WyszukajUliceAsync("Na Błonie", "", "")

        Dim gs1 = Await klient.AdresBudynkowAsync("12", "61", "02", "9", "0950463", "")

        Dim a = 1
        'Dim gs1 = Await klient.TerytWLiczbachTypAsync(TerytWliczbachTyp.NAJCZESTSZE_NAZWY_ULIC)
        'Dim gs1 = Await klient.WeryfikujAdresWmiejscowosciAsync("12", "61", "02", "0950486", "9")
        'Dim gs2 = Await klient.RaportLiczbaMiejscowosciWiejskichAsync("1/1/2024")
        'Dim gs1 = Await klient.RaportPorownanieTercNoweJednostkiAsync("1/1/2023")
        'Dim gs2 = Await klient.RaportPorownanieTercZmienioneSymboleAsync("1/1/2023", "12/12/2024")
        'Dim gs3 = Await klient.RaportPorownanieTercZmienioneNazwyAsync("1/1/2023", "12/12/2024")
        'Dim gs2 = Await klient.PobierzListeDatStanowSimcAsync
        'Dim gs3 = Await klient.PobierzListeDatStanowUlicAsync

        'Dim dt = New Date(2020, 1, 1)
        'Dim gm = Await klient.PobierzZmianyTercUrzedowyAsync(dt, Date.Now)
        'Dim bajty = Convert.FromBase64String(gm.plik_zawartosc)
        'IO.File.WriteAllBytes("c:\temp\" & gm.nazwa_pliku & ".zip", bajty)

        '        Dim moHttp = New Net.Http.HttpClient()
        '        Dim oResp As Net.Http.HttpResponseMessage

        '        ' przygotuj pContent, będzie przy redirect używany ponownie
        '        Dim pContent As Net.Http.StringContent = Nothing    ' żeby nie krzyczał że używam nieinicjalizowanego
        '        pContent = New Net.Http.StringContent("__EVENTTARGET=ctl00$body$BTERCUrzedowyPobierz", Text.Encoding.UTF8, "application/x-www-form-urlencoded")
        '        Dim oUri = New Uri("https://eteryt.stat.gov.pl/eTeryt/rejestr_teryt/udostepnianie_danych/baza_teryt/uzytkownicy_indywidualni/pobieranie/pliki_pelne.aspx")
        '        Try
        '            ' ISSUE: reference to a compiler-generated method
        '            oResp = Await moHttp.PostAsync(oUri, pContent)

        '        Catch ex As Exception
        '            pContent?.Dispose()
        '        End Try

        '        If Not oResp.IsSuccessStatusCode Then
        '            Dim a1 = 1
        '        End If

        '        pContent?.Dispose()

        '        Try
        '            Dim sPage = Await oResp.Content.ReadAsStringAsync()
        '#Disable Warning CA1031 ' Do not catch general exception types
        '        Catch ex As Exception
        '#Enable Warning CA1031 ' Do not catch general exception types
        '        End Try


        '        Dim a = 1


        '        ? gms(0)
        '{ServiceReference.Miejscowosc}
        '    GmiRodzaj: "9"
        '    GmiSymbol: "1261029"
        '    Gmina: "Kraków-Krowodrza"
        '    KrsP_Nazwa: "Azory"
        '    PowSymbol: "1261"
        '    Powiat: "Kraków"
        '    Symbol: "0950486"
        '    WojSymbol: "12"
        '    Wojewodztwo: "MAŁOPOLSKIE"
        '? gms(1)
        '{ServiceReference.Miejscowosc}
        '    GmiRodzaj: "9"
        '    GmiSymbol: "1261029"
        '    Gmina: "Kraków-Krowodrza"
        '    KrsP_Nazwa: "Bielany"
        '    PowSymbol: "1261"
        '    Powiat: "Kraków"
        '    Symbol: "0950492"
        '    WojSymbol: "12"
        '    Wojewodztwo: "MAŁOPOLSKIE"
        '? gms(100)
        '{ServiceReference.Miejscowosc}
        '    GmiRodzaj: "9"
        '    GmiSymbol: "1261039"
        '    Gmina: "Kraków-Nowa Huta"
        '    KrsP_Nazwa: "Wyciąże"
        '    PowSymbol: "1261"
        '    Powiat: "Kraków"
        '    Symbol: "0950948"
        '    WojSymbol: "12"
        '    Wojewodztwo: "MAŁOPOLSKIE"
        '? gms.Select(Of String)(Function(x) x.Gmina).Distinct
        '{System.Linq.Enumerable.DistinctIterator(Of String)}
        '    (0): "Kraków-Krowodrza"
        '    (1): "Kraków-Nowa Huta"
        '    (2): "Kraków-Podgórze"
        '    (3): "Kraków-Śródmieście"
        '    (4): "Kraków"


        'Dim wojsy = Await klient.PobierzListeWojewodztwAsync()
        'For Each item In wojsy
        '    If item.NAZWA.EqualsCI("małopolskie") Then
        '        Dim powsy = Await klient.PobierzListePowiatowAsync(item.WOJ)

        '        For Each item1 In powsy
        '            If item1.NAZWA.StartsWithCI("kraków") Then
        '                Dim gminsy = Await klient.PobierzListeGminAsync(item.WOJ, item1.POW)
        '            End If

        '        Next


        '    End If
        'Next

    End Sub


    Private Function GetCommonCachePath() As String
        Dim path As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Return IO.Path.Combine(path, "pkar.obiekty")
    End Function

    Private Async Sub uiZip_Click(sender As Object, e As RoutedEventArgs)
        Dim kod As New KodPocztowy(uiZipCode.Text)

        kod.CacheInit(GetCommonCachePath)
        kod.CacheLoad()

        If Not kod.IsValid Then
            MsgBox("inwalida!")
            Return
        End If

        Await kod.TryResolveAsync(True)

        kod.CacheSave()

        Dim pc As String = kod.GetSingleMiejscowosc
        If pc <> "" Then
            MsgBox("Hurra, jedna miejscowość: " & pc)
            Return
        End If

        MsgBox("Możliwości: " & String.Join(", ", kod.GetPossibleMiejscowosc))
    End Sub


    Private _KrsOdpis As Krs_OdpisAktualny

    Private Async Sub uiKrsResolve_Click(sender As Object, e As RoutedEventArgs)

        Dim oPos As New pkar.BasicGeopos(50.061648, 19.938005)

        'Dim listaPL = Await oPos.GeoWikiGetItems("pl")
        'Dim listaEn = Await oPos.GeoWikiGetItems("en")
        'Dim listaGb = Await oPos.GeoWikiGetItems("gb")
        'Dim listaBg = Await oPos.GeoWikiGetItems("bg")
        'Dim listaUa = Await oPos.GeoWikiGetItems("ua") ' UA: kraj, ale wiki UK (lang)


        Dim cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        For Each culture In cultures
            Dim region As RegionInfo = New RegionInfo(culture.Name)
            If region.TwoLetterISORegionName = "ua" Then
                Debug.WriteLine($"Country: {region.DisplayName}, Language: {culture.TwoLetterISOLanguageName}")
            End If
        Next



        Dim krs As String = uiKrsKrs.Text
        Dim nip As String = uiKrsNip.Text
        Dim regon As String = uiKrsRegon.Text

        If krs = "" Then
            Me.MsgBox("jednak muszę mieć KRS")
            Return
        End If

        Dim kaeres As New Krs(krs)
        _KrsOdpis = Await kaeres.GetOdpisAktualny

        uiKrsFirmaName.Text = _KrsOdpis.odpis.dane.dzial1.danePodmiotu.nazwa
        uiKrsFirmaAdres.Text = _KrsOdpis.odpis.dane.dzial1.siedzibaIAdres.adres.kraj & ", " &
            _KrsOdpis.odpis.dane.dzial1.siedzibaIAdres.adres.miejscowosc & ", " &
            _KrsOdpis.odpis.dane.dzial1.siedzibaIAdres.adres.ulica & " " &
            _KrsOdpis.odpis.dane.dzial1.siedzibaIAdres.adres.nrDomu


        uiKrsFirmaForma.Text = _KrsOdpis.odpis.dane.dzial1.danePodmiotu.formaPrawna

        Dim sumPKD As String = ""
        For Each pkd In _KrsOdpis.odpis.dane.dzial3.przedmiotDzialalnosci.przedmiotPrzewazajacejDzialalnosci
            sumPKD &= ", " & pkd.opis
        Next

        uiKrsFirmaPKDmain.Text = sumPKD

        sumPKD = ""
        For Each pkd In _KrsOdpis.odpis.dane.dzial3.przedmiotDzialalnosci.przedmiotPozostalejDzialalnosci
            sumPKD &= ", " & pkd.opis
        Next

        uiKrsFirmaPKDsecond.Text = sumPKD


    End Sub


    Private Sub uiKrsSendToClip_Click(sender As Object, e As RoutedEventArgs)
        If _KrsOdpis Is Nothing Then Return

        _KrsOdpis.DumpAsJSON.SendToClipboard
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        AdresUI.SetCacheFolder(GetCommonCachePath)
    End Sub
End Class
