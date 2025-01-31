Imports pkar.obiekty
Imports pkar.DotNetExtensions

Public Class AdresUI

    Public Property AdresFormat As String
        Get
            Return _adresFormat
        End Get
        Set(value As String)
            _adresFormat = value
            ShowShortFormat()
        End Set
    End Property

    Public Shared Sub SetCacheFolder(folder As String)
        _cachefolder = folder

        ' to jest SHARED, więc wystarcza w ten sposób
        _resolvedZip = New KodPocztowy("")
        _resolvedZip.CacheInit(_cachefolder)
        _resolvedZip.CacheLoad()

        ' to też jest SHARED - co prawda z tym logowaniem nie zadziała, ale jednak do cache wystarcza
        '_teryt = New TERYT("pkar", "t1Cv15F1t") 'TERYT("TestPubliczny", "1234abcd")
        _teryt = New TERYT("TestPubliczny", "1234abcd")
        _teryt.Cache.Init(_cachefolder)
        _teryt.Cache.Load()
    End Sub

    Private _adresFormat As String = "%z %m, %u, %d%l"

    Private Shared _cachefolder As String
    Private Shared _teryt As TERYT
    Private Shared _resolvedZip As KodPocztowy

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        uiAdresDated_SelectedDateChanged(Nothing, Nothing)
        ShowShortFormat()
    End Sub

    Private Sub uiRadioAktualnosc_Checked(sender As Object, e As RoutedEventArgs)
        If uiRadioAktHist Is Nothing Then Return ' przed inicjalizajcą

        uiAdresDated.Visibility = If(uiRadioAktHist.IsChecked, Visibility.Visible, Visibility.Collapsed)

        uiAdresDated_SelectedDateChanged(Nothing, Nothing)

    End Sub

    Private Function GetNaKiedy() As Date
        If Not uiRadioAktHist.IsChecked Then Return Date.Now
        If Not uiAdresDated.SelectedDate.HasValue Then Return Date.Now

        Return uiAdresDated.SelectedDate.Value
    End Function

    Private Async Sub uiAdresDated_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles uiAdresDated.SelectedDateChanged
        ' obojętnie czy historyczny, ważne czy w zasięgu TERC
        If Not uiRadioAktHist.IsChecked OrElse uiAdresDated.SelectedDate > New Date(1991, 1, 1) Then
            uiWojewCBox.IsEditable = False
            uiPowiatCBox.IsEditable = False
            uiGminaCBox.IsEditable = False

            uiWojewCBox.Items.Clear()

            ' lista województw jest w cache nawet jak jej nie ma :) bo z defaulta, hardcoded
            Dim wojList = If(uiRadioAktHist.IsChecked, _teryt.Cache.PobierzListeWojewodztw, Await _teryt.PobierzListeWojewodztwAsync(GetNaKiedy))
            For Each oWoj In wojList
                uiWojewCBox.Items.Add(New ComboBoxItem With {.DataContext = oWoj, .Content = oWoj.NAZWA.ToLowerInvariant})
            Next
        Else
            uiWojewCBox.IsEditable = True
            uiPowiatCBox.IsEditable = True
            uiGminaCBox.IsEditable = True
        End If

    End Sub



#Region "pole Zip"
    Private _ZipBefore As Boolean
    Private _ZipResolving As Boolean

    Private Sub uiAdresZIP_GotFocus(sender As Object, e As RoutedEventArgs)
        _ZipBefore = (uiAdresZIP.Text = "")
    End Sub

    Private Sub uiAdresZIP_LostFocus(sender As Object, e As RoutedEventArgs)
        ' jeśli na wejściu nie był pusty, to nie robimy auto-resolve
        If Not _ZipBefore Then Return

        ' jeśli nie spełnia maski, to nie robimy auto-resolve (bo nie ma to sensu)
        Dim kod As New KodPocztowy(uiAdresZIP.Text)
        If Not kod.IsValid Then Return

        uiAdresZipResolve_Click(Nothing, Nothing)
    End Sub

    Private Sub uiAdresZIP_TextChanged(sender As Object, e As TextChangedEventArgs)
        Dim kod As New KodPocztowy(uiAdresZIP.Text)
        uiAdresZipResolve.IsEnabled = kod.IsValid
    End Sub


    Private Async Sub uiAdresZipResolve_Click(sender As Object, e As RoutedEventArgs)
        _resolvedZip = New KodPocztowy(uiAdresZIP.Text)

        If Not _resolvedZip.IsValid Then
            MsgBox("inwalida!")
            Return
        End If

        '_resolvedZip.CacheInit(_cachefolder)
        '_resolvedZip.CacheLoad()

        Await _resolvedZip.TryResolveAsync(True)
        _resolvedZip.CacheSave()

        ' w tych ramach - nie reaguje na zmiany ComboBoxow (nie wypełnia ich zależnie od 'piętra wyżej')
        _ZipResolving = True

        ' obsługa województw
        uiWojewCBox.Items.Clear()
        Dim selItem As String = _resolvedZip.GetSingleWojewodztwo
        For Each sWoj In _resolvedZip.GetPossibleWojewodztwo
            Dim oNew As New ComboBoxItem With {.DataContext = sWoj, .Content = sWoj.ToLowerInvariant}
            If sWoj = selItem Then oNew.IsSelected = True
            uiWojewCBox.Items.Add(oNew)
        Next
        If selItem = "" Then uiWojewCBox.IsEnabled = True

        ' obsługa powiatów
        uiPowiatCBox.Items.Clear()
        selItem = _resolvedZip.GetSinglePowiat
        For Each sWoj In _resolvedZip.GetPossiblePowiat
            Dim oNew As New ComboBoxItem With {.DataContext = sWoj, .Content = sWoj}
            If sWoj = selItem Then oNew.IsSelected = True
            uiPowiatCBox.Items.Add(oNew)
        Next
        If selItem = "" Then uiPowiatCBox.IsEnabled = True

        ' obsługa gmin
        uiGminaCBox.Items.Clear()
        selItem = _resolvedZip.GetSingleGmina
        For Each sWoj In _resolvedZip.GetPossibleGmina
            Dim oNew As New ComboBoxItem With {.DataContext = sWoj, .Content = sWoj}
            If sWoj = selItem Then oNew.IsSelected = True
            uiGminaCBox.Items.Add(oNew)
        Next
        If selItem = "" Then uiGminaCBox.IsEnabled = True

        ' obsługa miejscowości
        uiMiejscCBox.Items.Clear()
        selItem = _resolvedZip.GetSingleMiejscowosc
        For Each sWoj In _resolvedZip.GetPossibleMiejscowosc
            Dim oNew As New ComboBoxItem With {.DataContext = sWoj, .Content = sWoj}
            If sWoj = selItem Then oNew.IsSelected = True
            uiMiejscCBox.Items.Add(oNew)
        Next
        If selItem = "" Then uiMiejscCBox.IsEnabled = True

        ' obsługa ulic
        uiUlicCBox.Items.Clear()
        selItem = _resolvedZip.GetSingleUlica
        For Each sWoj In _resolvedZip.GetPossibleUlica
            Dim oNew As New ComboBoxItem With {.DataContext = sWoj, .Content = sWoj}
            If sWoj = selItem Then oNew.IsSelected = True
            uiUlicCBox.Items.Add(oNew)
        Next
        If selItem = "" Then uiUlicCBox.IsEnabled = True



        _ZipResolving = False

    End Sub

#End Region


#Region "pole wojewodztwo"
    Private Sub uiAdresWojMenuOpen_Click(sender As Object, e As RoutedEventArgs)
        uiAdresWojMenuFlyout.IsOpen = Not uiAdresWojMenuFlyout.IsOpen
        'TerytWLiczbachTypAsync - to jest "per cały TERC", więc nie dotyczy
        'RaportLiczbaMiejscowosciWiejskichAsync
        'RaportLiczbaJednostekTercAsync
    End Sub

    Private Async Sub uiAdresWojStatJedn_Click(sender As Object, e As RoutedEventArgs)
        uiAdresWojMenuFlyout.IsOpen = False

        Dim lista = Await _teryt.RaportLiczbaJednostekTercAsync(GetNaKiedy)
        Dim woj As String = TryCast(uiWojewCBox.SelectedItem, ComboBoxItem)?.Content

        Dim obiekt As RLiczbaJednostkiTerc = lista.FirstOrDefault(Function(x) x.nazwa.EqualsCI(woj))
        If obiekt Is Nothing Then Return
        MsgBox($"Stan na: {GetNaKiedy.ToString("yyyy.MM.dd")}" & vbCrLf & obiekt.DumpIt)
    End Sub

    Private Async Sub uiAdresWojStatWsi_Click(sender As Object, e As RoutedEventArgs)
        uiAdresWojMenuFlyout.IsOpen = False

        Dim lista = Await _teryt.RaportLiczbaMiejscowosciWiejskichAsync(GetNaKiedy)
        Dim woj As String = TryCast(uiWojewCBox.SelectedItem, ComboBoxItem)?.Content

        Dim obiekt As RMiejscowosciWiejskie = lista.FirstOrDefault(Function(x) x.nazwa.EqualsCI(woj))
        If obiekt Is Nothing Then Return
        MsgBox($"Stan na: {GetNaKiedy.ToString("yyyy.MM.dd")}" & vbCrLf & obiekt.DumpIt)
    End Sub

    Private Async Sub uiWojewCBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles uiWojewCBox.SelectionChanged

        ' nic nie robimy jeśli to ustalanie z ZipResolve
        If _ZipResolving Then Return

        uiPowiatCBox.IsEnabled = True

        Dim cbi As ComboBoxItem = TryCast(uiWojewCBox.SelectedItem, ComboBoxItem)
        Dim jt As JednostkaTerytorialna = TryCast(cbi.DataContext, JednostkaTerytorialna)

        If jt Is Nothing Then
            ' czyli to wybór województwa z listy dopuszczalnych z kodu pocztowego
            uiPowiatCBox.Items.Clear()
            Dim selItem As String = _resolvedZip.GetSinglePowiat(cbi.Content.ToString)
            For Each sWoj In _resolvedZip.GetPossiblePowiat(cbi.Content.ToString)
                Dim oNew As New ComboBoxItem With {.DataContext = sWoj, .Content = sWoj}
                If sWoj = selItem Then oNew.IsSelected = True
                uiPowiatCBox.Items.Add(oNew)
            Next
            Return
        End If

        ' mamy woj, to teraz z Teryt

        uiPowiatCBox.Items.Clear()
        Dim pwts As JednostkaTerytorialna() = Nothing

        If Not uiRadioAktHist.IsChecked Then
            pwts = _teryt.Cache.PobierzListePowiatow(jt.WOJ)
        End If
        If pwts Is Nothing OrElse pwts.Length < 1 Then
            pwts = Await _teryt.PobierzListePowiatowAsync(jt.WOJ, GetNaKiedy)
        End If

        For Each pow In pwts
            Dim oNew As New ComboBoxItem With {.DataContext = pow, .Content = pow.NAZWA}
            uiPowiatCBox.Items.Add(oNew)
        Next

    End Sub
#End Region

#Region "powiat"

    Private Async Sub uiPowiatCBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles uiPowiatCBox.SelectionChanged
        ' nic nie robimy jeśli to ustalanie z ZipResolve
        If _ZipResolving Then Return

        uiGminaCBox.IsEnabled = True

        Dim cbi As ComboBoxItem = TryCast(uiPowiatCBox.SelectedItem, ComboBoxItem)
        Dim jt As JednostkaTerytorialna = TryCast(cbi.DataContext, JednostkaTerytorialna)

        If jt Is Nothing Then
            ' czyli to wybór z listy dopuszczalnych z kodu pocztowego
            uiGminaCBox.Items.Clear()
            Dim selItem As String = _resolvedZip.GetSingleGmina(cbi.Content.ToString)
            For Each sWoj In _resolvedZip.GetPossibleGmina(cbi.Content.ToString)
                Dim oNew As New ComboBoxItem With {.DataContext = sWoj, .Content = sWoj}
                If sWoj = selItem Then oNew.IsSelected = True
                uiGminaCBox.Items.Add(oNew)
            Next
            Return
        End If

        ' mamy woj, to teraz z Teryt

        uiGminaCBox.Items.Clear()

        Dim pwts As JednostkaTerytorialna() = Nothing


        ' dostaję wieliczka miasto oraz wieliczka obszar wiejski - co z tym zrobić...
        If Not uiRadioAktHist.IsChecked Then
            pwts = _teryt.Cache.PobierzListeGmin(jt.WOJ, jt.POW)
        End If
        If pwts Is Nothing OrElse pwts.Length < 1 Then
            pwts = Await _teryt.PobierzListeGminAsync(jt.WOJ, jt.POW, GetNaKiedy)
        End If

        For Each pow In pwts
            Dim oNew As New ComboBoxItem With {.DataContext = pow, .Content = pow.NAZWA}
            If pwts.Length = 1 Then oNew.IsSelected = True
            uiGminaCBox.Items.Add(oNew)
        Next

    End Sub
#End Region

#Region "gmina"
    Private Async Sub uiGminaCBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles uiGminaCBox.SelectionChanged
        ' nic nie robimy jeśli to ustalanie z ZipResolve
        If _ZipResolving Then Return

        uiMiejscCBox.IsEnabled = True

        Dim cbi As ComboBoxItem = TryCast(uiGminaCBox.SelectedItem, ComboBoxItem)
        Dim jt As JednostkaTerytorialna = TryCast(cbi.DataContext, JednostkaTerytorialna)

        If jt Is Nothing Then
            ' czyli to wybór z listy dopuszczalnych z kodu pocztowego
            uiMiejscCBox.Items.Clear()
            Dim selItem As String = _resolvedZip.GetSingleMiejscowosc(cbi.Content.ToString)
            For Each sWoj In _resolvedZip.GetPossibleMiejscowosc(cbi.Content.ToString)
                Dim oNew As New ComboBoxItem With {.DataContext = sWoj, .Content = sWoj}
                If sWoj = selItem Then oNew.IsSelected = True
                uiMiejscCBox.Items.Add(oNew)
            Next
            Return
        End If

        ' mamy woj, to teraz z Teryt

        uiMiejscCBox.Items.Clear()

        Dim pwts As Miejscowosc() = Nothing

        ' dostaję wieliczka miasto oraz wieliczka obszar wiejski - co z tym zrobić...
        If Not uiRadioAktHist.IsChecked Then
            pwts = _teryt.Cache.PobierzListeMiejscowosci(jt.WOJ, jt.POW, jt.GMI)
        End If

        If pwts Is Nothing OrElse pwts.Count < 1 Then
            'Dim woj As String = uiWojewCBox.Text ' TryCast(uiWojewCBox.SelectedItem, ComboBoxItem)?.Content.ToString
            'Dim pow As String = uiPowiatCBox.Text
            'Dim gmi As String = uiGminaCBox.Text
            pwts = Await _teryt.PobierzListeMiejscowosciWGminieAsync(uiWojewCBox.Text, uiPowiatCBox.Text, uiGminaCBox.Text)
        End If

        Dim msc As List(Of Miejscowosc) = pwts.Where(Function(x) x.GmiRodzaj < "6").ToList

        For Each pow In msc
            Dim oNew As New ComboBoxItem With {.DataContext = pow, .Content = pow.Nazwa}
            If msc.Count = 1 Then oNew.IsSelected = True
            uiMiejscCBox.Items.Add(oNew)
        Next


    End Sub

#End Region

#Region "miejscowosc"

    Private Async Sub uiAdresMscSrch_Click(sender As Object, e As RoutedEventArgs)

        If uiAdresMscSrchFlyout.IsOpen Then
            uiAdresMscSrchFlyout.IsOpen = False
            Return
        End If

        Dim srchRes = Await _teryt.WyszukajMiejscowoscWJPTAsync(uiWojewCBox.Text, uiPowiatCBox.Text, uiGminaCBox.Text, uiMiejscCBox.Text, "")

        If srchRes.Length = 1 Then
            SetMiejscowosc(srchRes(0))
            Return
        End If

        uiMscSrchMenu.Items.Clear()
        For Each oRes In srchRes.Take(12)
            Dim oNew As New MenuItem
            oNew.Header = $"{oRes.Nazwa} ({oRes.Gmina} « {oRes.Powiat} « {oRes.Wojewodztwo.ToLowerInvariant})"
            oNew.DataContext = oRes
            AddHandler oNew.Click, AddressOf uiAdresSrchSelected
            uiMscSrchMenu.Items.Add(oNew)
        Next
        If srchRes.Length > 12 Then
            uiMscSrchMenu.Items.Add("...")
        End If
        uiAdresMscSrchFlyout.IsOpen = True
    End Sub

    Private Sub uiAdresSrchSelected(sender As Object, e As RoutedEventArgs)
        uiAdresMscSrchFlyout.IsOpen = False

        Dim oMI As MenuItem = TryCast(sender, MenuItem)
        Dim oMsc As Miejscowosc = TryCast(oMI?.DataContext, Miejscowosc)
        If oMsc Is Nothing Then Return

        SetMiejscowosc(oMsc)
    End Sub

    Private Sub SetMiejscowosc(msc As Miejscowosc)
        _ZipResolving = True
        uiWojewCBox.Items.Clear()
        uiWojewCBox.Items.Add(New ComboBoxItem With {.Content = msc.Wojewodztwo.ToLowerInvariant, .IsSelected = True})
        uiPowiatCBox.Items.Clear()
        uiPowiatCBox.Items.Add(New ComboBoxItem With {.Content = msc.Powiat, .IsSelected = True})
        uiGminaCBox.Items.Clear()
        uiGminaCBox.Items.Add(New ComboBoxItem With {.Content = msc.Gmina, .IsSelected = True})
        uiMiejscCBox.Items.Clear()
        uiMiejscCBox.Items.Add(New ComboBoxItem With {.Content = msc.Nazwa, .IsSelected = True, .DataContext = msc})
        _ZipResolving = False
    End Sub

    Private Async Sub uiMiejscCBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles uiMiejscCBox.SelectionChanged
        ' nic nie robimy jeśli to ustalanie z ZipResolve
        If _ZipResolving Then Return

        uiUlicCBox.IsEnabled = True

        Dim cbi As ComboBoxItem = TryCast(uiMiejscCBox.SelectedItem, ComboBoxItem)
        Dim jt As Miejscowosc = TryCast(cbi.DataContext, Miejscowosc)

        If jt Is Nothing Then
            ' czyli to wybór z listy dopuszczalnych z kodu pocztowego
            uiUlicCBox.Items.Clear()
            Dim selItem As String = _resolvedZip.GetSingleUlica(cbi.Content.ToString)
            For Each sWoj In _resolvedZip.GetPossibleUlica(cbi.Content.ToString)
                Dim oNew As New ComboBoxItem With {.DataContext = sWoj, .Content = sWoj}
                If sWoj = selItem Then oNew.IsSelected = True
                uiUlicCBox.Items.Add(oNew)
            Next
            Return
        End If

        ' mamy woj, to teraz z Teryt

        uiUlicCBox.Items.Clear()

        Dim pwts As UlicaDrzewo() = Nothing

        If pwts Is Nothing OrElse pwts.Count < 1 Then
            'Dim woj As String = uiWojewCBox.Text ' TryCast(uiWojewCBox.SelectedItem, ComboBoxItem)?.Content.ToString
            'Dim pow As String = uiPowiatCBox.Text
            'Dim gmi As String = uiGminaCBox.Text
            pwts = Await _teryt.PobierzListeUlicDlaMiejscowosciAsync(jt.WojSymbol, jt.PowSymbol, jt.GmiSymbol, jt.GmiRodzaj, jt.Symbol, False)
        End If

        'Dim msc As List(Of UlicaDrzewo) = pwts.Where(Function(x) x.GmiRodzaj < "6").ToList

        For Each pow In pwts.Take(26)
            Dim oNew As New ComboBoxItem With {.DataContext = pow, .Content = $"{pow.Nazwa1} ({pow.Cecha} {pow.Nazwa2})"}
            If pwts.Count = 1 Then oNew.IsSelected = True
            uiUlicCBox.Items.Add(oNew)
        Next
        If pwts.Length > 12 Then
            uiUlicCBox.Items.Add("...")
        End If

    End Sub


#End Region

    Private Async Sub uiAdresUlicSrch_Click(sender As Object, e As RoutedEventArgs)
        'Await klient.WyszukajUliceAsync("Na Błonie", "", "")

        If uiUlicSrchFlyout.IsOpen Then
            uiUlicSrchFlyout.IsOpen = False
            Return
        End If

        Dim srchRes = Await _teryt.WyszukajUliceAsync(uiUlicCBox.Text, "", uiMiejscCBox.Text)
        'Dim srchRes1 = Await _teryt.WyszukajUliceWRejestrzeAsync(uiUlicCBox.Text, "", "", {New identyfikatory With {.simc = "", .terc = ""}}, Date.Now)
        If srchRes.Length = 1 Then
            SetUlica(srchRes(0))
            Return
        End If

        uiUlicSrchMenu.Items.Clear()
        For Each oRes In srchRes.Take(12)

            If oRes.GmiRodzaj = "9" Then
                Dim iInd As Integer = oRes.NazwaMiejscowosci.IndexOf("-")
                If iInd > 0 Then oRes.NazwaMiejscowosci = oRes.NazwaMiejscowosci.Substring(0, iInd)

                iInd = oRes.Gmina.IndexOf("-")
                If iInd > 0 Then oRes.Gmina = oRes.Gmina.Substring(0, iInd)

                'Dim fullMsc = Await _teryt.WyszukajMiejscowoscAsync("", oRes.IdentyfikatorMiejscowosci)
                'Dim ulc = Await _teryt.PobierzListeUlicDlaMiejscowosciAsync(oRes.WojSymbol, oRes.PowSymbol, oRes.GmiSymbol, "9", oRes.IdentyfikatorMiejscowosci, False)
                'Dim fullMsc = Await _teryt.WyszukajMiejscowoscAsync("", ulc(0).IdentyfikatorMiejscowosciPodstawowej)

                'Cecha: "ul."
                '    GmiRodzaj: "9"
                '    GmiSymbol: "02"
                '    Gmina: "Kraków-Krowodrza"
                '    IdentyfikatorMiejscowosci: "0950470"
                '    IdentyfikatorUlicy: "06500"
                '    KrsP_Nazwa: "Hamernia"
                '    NazwaMiejscowosci: "Kraków-Krowodrza"
                '    PowSymbol: "61"
                '    Powiat: "Kraków"
                '    WojSymbol: "12"
                '    Wojewodztwo: "MAŁOPOLSKIE"

                ' gdy gmirodzaj "9" to rozwiązanie - 12/61/02
            End If

            Dim oNew As New MenuItem

            If oRes.NazwaMiejscowosci <> oRes.Gmina Then
                oNew.Header = $"{oRes.Cecha} {oRes.Nazwa} ({oRes.NazwaMiejscowosci} « {oRes.Gmina} « {oRes.Powiat} « {oRes.Wojewodztwo.ToLowerInvariant})"
            Else
                ' gdy gmina miejska, to bez sensu pisać Krakow»Kraków
                oNew.Header = $"{oRes.Cecha} {oRes.Nazwa} ({oRes.Gmina} « {oRes.Powiat} « {oRes.Wojewodztwo.ToLowerInvariant})"
            End If

            oNew.DataContext = oRes
            AddHandler oNew.Click, AddressOf uiAdresUlicSrchSelected
            uiUlicSrchMenu.Items.Add(oNew)
        Next
        If srchRes.Length > 12 Then
            uiUlicSrchMenu.Items.Add("...")
        End If

        uiUlicSrchFlyout.IsOpen = True
    End Sub

    Private Sub uiAdresUlicSrchSelected(sender As Object, e As RoutedEventArgs)
        uiUlicSrchFlyout.IsOpen = False

        Dim oMI As MenuItem = TryCast(sender, MenuItem)
        Dim oMsc As Ulica = TryCast(oMI?.DataContext, Ulica)
        If oMsc Is Nothing Then Return

        SetUlica(oMsc)
    End Sub

    Private Sub SetUlica(msc As Ulica)
        _ZipResolving = True
        uiWojewCBox.Items.Clear()
        uiWojewCBox.Items.Add(New ComboBoxItem With {.Content = msc.Wojewodztwo.ToLowerInvariant, .IsSelected = True})
        uiPowiatCBox.Items.Clear()
        uiPowiatCBox.Items.Add(New ComboBoxItem With {.Content = msc.Powiat, .IsSelected = True})
        uiGminaCBox.Items.Clear()
        uiGminaCBox.Items.Add(New ComboBoxItem With {.Content = msc.Gmina, .IsSelected = True})
        uiMiejscCBox.Items.Clear()
        uiMiejscCBox.Items.Add(New ComboBoxItem With {.Content = msc.NazwaMiejscowosci, .IsSelected = True, .DataContext = msc})
        uiUlicCBox.Items.Clear()
        uiUlicCBox.Items.Add(New ComboBoxItem With {.Content = msc.Nazwa, .IsSelected = True, .DataContext = msc})
        _ZipResolving = False
    End Sub

    Private Sub uiAdresEdit_Click(sender As Object, e As RoutedEventArgs)
        uiAdresEditorFlyout.IsOpen = Not uiAdresEditorFlyout.IsOpen
        Dim addr As pkar.obiekty.AdresPL = TryCast(DataContext, pkar.obiekty.AdresPL)

        If addr Is Nothing Then
            DataContext = New pkar.obiekty.AdresPL
            Return
        End If

        _ZipResolving = True

        If addr.Dated.Year > 1500 AndAlso addr.Dated.Year < Date.Now.Year Then
            uiAdresDated.SelectedDate = addr.Dated
            uiRadioAktHist.IsChecked = True
        Else
            uiAdresDated.SelectedDate = Date.Now
            uiRadioAktHist.IsChecked = False
        End If

        If Not String.IsNullOrWhiteSpace(addr.Wojewodztwo) Then
            uiWojewCBox.Items.Clear()
            uiWojewCBox.Items.Add(New ComboBoxItem With {.Content = addr.Wojewodztwo, .IsSelected = True})
        End If

        If Not String.IsNullOrWhiteSpace(addr.Powiat) Then
            uiPowiatCBox.Items.Clear()
            uiPowiatCBox.Items.Add(New ComboBoxItem With {.Content = addr.Powiat, .IsSelected = True})
        End If

        If Not String.IsNullOrWhiteSpace(addr.Gmina) Then
            uiGminaCBox.Items.Clear()
            uiGminaCBox.Items.Add(New ComboBoxItem With {.Content = addr.Gmina, .IsSelected = True})
        End If

        If Not String.IsNullOrWhiteSpace(addr.Miejscowosc) Then
            uiMiejscCBox.Items.Clear()
            uiMiejscCBox.Items.Add(New ComboBoxItem With {.Content = addr.Miejscowosc, .IsSelected = True})
        End If

        If Not String.IsNullOrWhiteSpace(addr.Ulica) Then
            uiUlicCBox.Items.Clear()
            uiUlicCBox.Items.Add(New ComboBoxItem With {.Content = addr.Ulica, .IsSelected = True})
        End If

        uiAdresDom.Text = addr.Dom
        uiAdresLokal.Text = addr.Lokal
        uiAdresInfoDod.Text = addr.InfoDod
        uiAdresZIP.Text = addr.Zip

        _ZipResolving = False

    End Sub

    Private Sub uiAdresEditOk_Click(sender As Object, e As RoutedEventArgs)
        uiAdresEditorFlyout.IsOpen = False
        Dim addr As pkar.obiekty.AdresPL = TryCast(DataContext, pkar.obiekty.AdresPL)

        If uiRadioAktHist.IsChecked Then
            addr.Dated = uiAdresDated.SelectedDate
        Else
            addr.Dated = Date.Now
        End If

        addr.Wojewodztwo = uiWojewCBox.Text
        addr.Powiat = uiPowiatCBox.Text
        addr.Gmina = uiGminaCBox.Text
        addr.Miejscowosc = uiMiejscCBox.Text
        addr.Ulica = uiUlicCBox.Text
        addr.Dom = uiAdresDom.Text
        addr.Lokal = uiAdresLokal.Text
        addr.InfoDod = uiAdresInfoDod.Text
        addr.Zip = uiAdresZIP.Text

        ShowShortFormat()
    End Sub

    Private Sub ShowShortFormat()
        Dim addr As pkar.obiekty.AdresPL = TryCast(DataContext, pkar.obiekty.AdresPL)
        If addr Is Nothing Then
            uiShortAdres.Text = "(pusty adres)"
            Return
        End If

        Dim tekstowo As String = _adresFormat.
            Replace("%d", addr.Dated.ToString("yyyy.MM.dd")).
            Replace("%w", addr.Wojewodztwo).
            Replace("%p", addr.Powiat).
            Replace("%g", addr.Gmina).
            Replace("%m", addr.Miejscowosc).
            Replace("%u", addr.Ulica).
            Replace("%d", addr.Dom).
            Replace("%z", addr.Zip).
            Replace("%l", If(String.IsNullOrWhiteSpace(addr.Dom), "", "/" & addr.Lokal)).
            Replace("%i", addr.InfoDod)

        If tekstowo.Replace(",", "").Trim.Length < 5 Then
            uiShortAdres.Text = "(pusty adres)"
        Else
            uiShortAdres.Text = tekstowo
        End If


    End Sub

End Class
