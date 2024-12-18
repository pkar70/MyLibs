using System;

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName = "ITerytWs1")]
public interface ITerytWs1
{

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/CzyZalogowany", ReplyAction = "http://tempuri.org/ITerytWs1/CzyZalogowanyResponse")]
    System.Threading.Tasks.Task<bool> CzyZalogowanyAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/Zweryfikowany", ReplyAction = "http://tempuri.org/ITerytWs1/ZweryfikowanyResponse")]
    System.Threading.Tasks.Task<bool> ZweryfikowanyAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/AktualizujUliceEMUiA", ReplyAction = "http://tempuri.org/ITerytWs1/AktualizujUliceEMUiAResponse")]
    System.Threading.Tasks.Task<OdpowiedzTeryt> AktualizujUliceEMUiAAsync(PlacUlica placUlica);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/AktualizujPunktAdresowyEMUiA", ReplyAction = "http://tempuri.org/ITerytWs1/AktualizujPunktAdresowyEMUiAResponse")]
    System.Threading.Tasks.Task<OdpowiedzTeryt> AktualizujPunktAdresowyEMUiAAsync(PunktAdresowy punktAdresowy);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/RaportLiczbaMiejscowosciWiejskich", ReplyAction = "http://tempuri.org/ITerytWs1/RaportLiczbaMiejscowosciWiejskichResponse")]
    System.Threading.Tasks.Task<RMiejscowosciWiejskie[]> RaportLiczbaMiejscowosciWiejskichAsync(string dataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/RaportLiczbaJednostekTerc", ReplyAction = "http://tempuri.org/ITerytWs1/RaportLiczbaJednostekTercResponse")]
    System.Threading.Tasks.Task<RLiczbaJednostkiTerc[]> RaportLiczbaJednostekTercAsync(string dataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/RaportPorownanieTercNoweJednostki", ReplyAction = "http://tempuri.org/ITerytWs1/RaportPorownanieTercNoweJednostkiResponse")]
    System.Threading.Tasks.Task<RJednostkiTerc[]> RaportPorownanieTercNoweJednostkiAsync(string dataOd, string dataDo);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/RaportPorownanieTercUsunieteJednostki", ReplyAction = "http://tempuri.org/ITerytWs1/RaportPorownanieTercUsunieteJednostkiResponse")]
    System.Threading.Tasks.Task<RJednostkiTerc[]> RaportPorownanieTercUsunieteJednostkiAsync(string dataOd, string dataDo);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/RaportPorownanieTercZmienioneNazwy", ReplyAction = "http://tempuri.org/ITerytWs1/RaportPorownanieTercZmienioneNazwyResponse")]
    System.Threading.Tasks.Task<RZmianyTerc[]> RaportPorownanieTercZmienioneNazwyAsync(string dataOd, string dataDo);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/RaportPorownanieTercZmienioneSymbole", ReplyAction = "http://tempuri.org/ITerytWs1/RaportPorownanieTercZmienioneSymboleResponse")]
    System.Threading.Tasks.Task<RZmianyTerc[]> RaportPorownanieTercZmienioneSymboleAsync(string dataOd, string dataDo);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/RaportPorownanieTercZmienioneSymboleINazwy", ReplyAction = "http://tempuri.org/ITerytWs1/RaportPorownanieTercZmienioneSymboleINazwyResponse")]
    System.Threading.Tasks.Task<RZmianyTerc[]> RaportPorownanieTercZmienioneSymboleINazwyAsync(string dataOd, string dataDo);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeStanowTerc", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeStanowTercResponse")]
    System.Threading.Tasks.Task<string[]> PobierzListeStanowTercAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeStanowSimc", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeStanowSimcResponse")]
    System.Threading.Tasks.Task<string[]> PobierzListeStanowSimcAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeStanowUlic", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeStanowUlicResponse")]
    System.Threading.Tasks.Task<string[]> PobierzListeStanowUlicAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzDateAktualnegoKatTerc", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzDateAktualnegoKatTercResponse")]
    System.Threading.Tasks.Task<System.DateTime> PobierzDateAktualnegoKatTercAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzDateAktualnegoKatNTS", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzDateAktualnegoKatNTSResponse")]
    System.Threading.Tasks.Task<System.DateTime> PobierzDateAktualnegoKatNTSAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzDateAktualnegoKatSimc", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzDateAktualnegoKatSimcResponse")]
    System.Threading.Tasks.Task<System.DateTime> PobierzDateAktualnegoKatSimcAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzDateAktualnegoKatUlic", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzDateAktualnegoKatUlicResponse")]
    System.Threading.Tasks.Task<System.DateTime> PobierzDateAktualnegoKatUlicAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeWojewodztw", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeWojewodztwResponse")]
    System.Threading.Tasks.Task<JednostkaTerytorialna[]> PobierzListeWojewodztwAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListePowiatow", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListePowiatowResponse")]
    System.Threading.Tasks.Task<JednostkaTerytorialna[]> PobierzListePowiatowAsync(string Woj, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeGmin", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeGminResponse")]
    System.Threading.Tasks.Task<JednostkaTerytorialna[]> PobierzListeGminAsync(string Woj, string Pow, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzGminyiPowDlaWoj", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzGminyiPowDlaWojResponse")]
    System.Threading.Tasks.Task<JednostkaTerytorialna[]> PobierzGminyiPowDlaWojAsync(string Woj, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeRegionow", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeRegionowResponse")]
    System.Threading.Tasks.Task<JednostkaNomenklaturyNTS[]> PobierzListeRegionowAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListePodregionow", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListePodregionowResponse")]
    System.Threading.Tasks.Task<JednostkaNomenklaturyNTS[]> PobierzListePodregionowAsync(string Woj, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeWojewodztwWRegionie", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeWojewodztwWRegionieResponse")]
    System.Threading.Tasks.Task<JednostkaNomenklaturyNTS[]> PobierzListeWojewodztwWRegionieAsync(string Reg, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListePowiatowWPodregionie", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListePowiatowWPodregionieResponse")]
    System.Threading.Tasks.Task<JednostkaNomenklaturyNTS[]> PobierzListePowiatowWPodregionieAsync(string Podreg, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeGminPowiecie", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeGminPowiecieResponse")]
    System.Threading.Tasks.Task<JednostkaNomenklaturyNTS[]> PobierzListeGminPowiecieAsync(string Pow, string Podreg, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeUlicDlaMiejscowosci", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeUlicDlaMiejscowosciResponse")]
    System.Threading.Tasks.Task<UlicaDrzewo[]> PobierzListeUlicDlaMiejscowosciAsync(string woj, string pow, string gmi, string rodzaj, string msc, bool czyWersjaUrzedowa, bool czyWersjaAdresowa, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeMiejscowosciWGminie", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeMiejscowosciWGminieResponse")]
    System.Threading.Tasks.Task<Miejscowosc[]> PobierzListeMiejscowosciWGminieAsync(string Wojewodztwo, string Powiat, string Gmina, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeMiejscowosciWGminieZSymbolem", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeMiejscowosciWGminieZSymbolemResponse")]
    System.Threading.Tasks.Task<MiejscowoscPelna[]> PobierzListeMiejscowosciWGminieZSymbolemAsync(string Woj, string Pow, string Gmi, string Rodz, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzListeMiejscowosciWRodzajuGminy", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzListeMiejscowosciWRodzajuGminyResponse")]
    System.Threading.Tasks.Task<Miejscowosc[]> PobierzListeMiejscowosciWRodzajuGminyAsync(string symbolWoj, string symbolPow, string symbolGmi, string symbolRodz, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzSlownikRodzajowJednostek", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzSlownikRodzajowJednostekResponse")]
    System.Threading.Tasks.Task<string[]> PobierzSlownikRodzajowJednostekAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzSlownikRodzajowSIMC", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzSlownikRodzajowSIMCResponse")]
    System.Threading.Tasks.Task<RodzajMiejscowosci[]> PobierzSlownikRodzajowSIMCAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzSlownikCechULIC", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzSlownikCechULICResponse")]
    System.Threading.Tasks.Task<string[]> PobierzSlownikCechULICAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogTERCAdr", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogTERCAdrResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogTERCAdrAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogTERC", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogTERCResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogTERCAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogNTS", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogNTSResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogNTSAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogSIMCAdr", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogSIMCAdrResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogSIMCAdrAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogSIMC", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogSIMCResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogSIMCAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogSIMCStat", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogSIMCStatResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogSIMCStatAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogULIC", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogULICResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogULICAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogULICAdr", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogULICAdrResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogULICAdrAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogULICBezDzielnic", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogULICBezDzielnicResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogULICBezDzielnicAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzStaryKatalogULIC", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzStaryKatalogULICResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzStaryKatalogULICAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzKatalogWMRODZ", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzKatalogWMRODZResponse")]
    System.Threading.Tasks.Task<PlikKatalog> PobierzKatalogWMRODZAsync(System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzZmianyTercUrzedowy", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzZmianyTercUrzedowyResponse")]
    System.Threading.Tasks.Task<PlikZmiany> PobierzZmianyTercUrzedowyAsync(System.DateTime stanod, System.DateTime stando);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzZmianyTercAdresowy", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzZmianyTercAdresowyResponse")]
    System.Threading.Tasks.Task<PlikZmiany> PobierzZmianyTercAdresowyAsync(System.DateTime stanod, System.DateTime stando);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzZmianyNTS", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzZmianyNTSResponse")]
    System.Threading.Tasks.Task<PlikZmiany> PobierzZmianyNTSAsync(System.DateTime stanod, System.DateTime stando);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzZmianySimcUrzedowy", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzZmianySimcUrzedowyResponse")]
    System.Threading.Tasks.Task<PlikZmiany> PobierzZmianySimcUrzedowyAsync(System.DateTime stanod, System.DateTime stando);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzZmianySimcAdresowy", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzZmianySimcAdresowyResponse")]
    System.Threading.Tasks.Task<PlikZmiany> PobierzZmianySimcAdresowyAsync(System.DateTime stanod, System.DateTime stando);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzZmianySimcStatystyczny", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzZmianySimcStatystycznyResponse")]
    System.Threading.Tasks.Task<PlikZmiany> PobierzZmianySimcStatystycznyAsync(System.DateTime stanod, System.DateTime stando);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzZmianyUlicUrzedowy", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzZmianyUlicUrzedowyResponse")]
    System.Threading.Tasks.Task<PlikZmiany> PobierzZmianyUlicUrzedowyAsync(System.DateTime stanod, System.DateTime stando);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzZmianyUlicAdresowy", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzZmianyUlicAdresowyResponse")]
    System.Threading.Tasks.Task<PlikZmiany> PobierzZmianyUlicAdresowyAsync(System.DateTime stanod, System.DateTime stando);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzZmianyUlicBezDzielnic", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzZmianyUlicBezDzielnicResponse")]
    System.Threading.Tasks.Task<PlikZmiany> PobierzZmianyUlicBezDzielnicAsync(System.DateTime stanod, System.DateTime stando);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzGeoTerytPlikPelny", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzGeoTerytPlikPelnyResponse")]
    System.Threading.Tasks.Task<GeoTeryt> PobierzGeoTerytPlikPelnyAsync(string rok, string kwartal, string kodTerytorialny);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/PobierzGeoTerytPlikRoznicowy", ReplyAction = "http://tempuri.org/ITerytWs1/PobierzGeoTerytPlikRoznicowyResponse")]
    System.Threading.Tasks.Task<GeoTeryt> PobierzGeoTerytPlikRoznicowyAsync(string rok, string kwartal, string kodTerytorialny);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WeryfikujAdresDlaMiejscowosci", ReplyAction = "http://tempuri.org/ITerytWs1/WeryfikujAdresDlaMiejscowosciResponse")]
    System.Threading.Tasks.Task<ZweryfikowanyAdresBezUlic[]> WeryfikujAdresDlaMiejscowosciAsync(string symbolMsc);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WeryfikujAdresDlaMiejscowosciAdresowy", ReplyAction = "http://tempuri.org/ITerytWs1/WeryfikujAdresDlaMiejscowosciAdresowyResponse")]
    System.Threading.Tasks.Task<ZweryfikowanyAdresBezUlic[]> WeryfikujAdresDlaMiejscowosciAdresowyAsync(string symbolMsc);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WeryfikujAdresDlaUlic", ReplyAction = "http://tempuri.org/ITerytWs1/WeryfikujAdresDlaUlicResponse")]
    System.Threading.Tasks.Task<ZweryfikowanyAdres[]> WeryfikujAdresDlaUlicAsync(string symbolMsc, string SymUl);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WeryfikujAdresDlaUlicAdresowy", ReplyAction = "http://tempuri.org/ITerytWs1/WeryfikujAdresDlaUlicAdresowyResponse")]
    System.Threading.Tasks.Task<ZweryfikowanyAdres[]> WeryfikujAdresDlaUlicAdresowyAsync(string symbolMsc, string SymUl);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WeryfikujAdresWmiejscowosci", ReplyAction = "http://tempuri.org/ITerytWs1/WeryfikujAdresWmiejscowosciResponse")]
    System.Threading.Tasks.Task<ZweryfikowanyAdresBezUlic[]> WeryfikujAdresWmiejscowosciAsync(string Wojewodztwo, string Powiat, string Gmina, string Miejscowosc, string Rodzaj);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WeryfikujAdresWmiejscowosciAdresowy", ReplyAction = "http://tempuri.org/ITerytWs1/WeryfikujAdresWmiejscowosciAdresowyResponse")]
    System.Threading.Tasks.Task<ZweryfikowanyAdresBezUlic[]> WeryfikujAdresWmiejscowosciAdresowyAsync(string Wojewodztwo, string Powiat, string Gmina, string Miejscowosc, string Rodzaj);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WeryfikujNazwaAdresUlic", ReplyAction = "http://tempuri.org/ITerytWs1/WeryfikujNazwaAdresUlicResponse")]
    System.Threading.Tasks.Task<ZweryfikowanyAdres[]> WeryfikujNazwaAdresUlicAsync(string Wojewodztwo, string Powiat, string Gmina, string Miejscowosc, string Rodzaj, string NazwaUlicy);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WeryfikujNazwaAdresUlicAdresowy", ReplyAction = "http://tempuri.org/ITerytWs1/WeryfikujNazwaAdresUlicAdresowyResponse")]
    System.Threading.Tasks.Task<ZweryfikowanyAdres[]> WeryfikujNazwaAdresUlicAdresowyAsync(string nazwaWoj, string nazwaPow, string nazwaGmi, string nazwaMiejscowosc, string rodzajMiejsc, string nazwaUlicy);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajJPT", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajJPTResponse")]
    System.Threading.Tasks.Task<JednostkaPodzialuTerytorialnego[]> WyszukajJPTAsync(string nazwa);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajMiejscowosc", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscResponse")]
    System.Threading.Tasks.Task<Miejscowosc[]> WyszukajMiejscowoscAsync(string nazwaMiejscowosci, string identyfikatorMiejscowosci);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWJPT", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWJPTResponse")]
    System.Threading.Tasks.Task<Miejscowosc[]> WyszukajMiejscowoscWJPTAsync(string nazwaWoj, string nazwaPow, string nazwaGmi, string nazwaMiejscowosci, string identyfikatorMiejscowosci);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajUlice", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajUliceResponse")]
    System.Threading.Tasks.Task<Ulica[]> WyszukajUliceAsync(string nazwaulicy, string cecha, string nazwamiejscowosci);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajJednostkeWRejestrze", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajJednostkeWRejestrzeResponse")]
    System.Threading.Tasks.Task<JednostkaPodzialuTerytorialnego[]> WyszukajJednostkeWRejestrzeAsync(string nazwa, identyfikatory[] identyfiks, string kategoria, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajJednostkeWRejestrzeWebLS", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajJednostkeWRejestrzeWebLSResponse")]
    System.Threading.Tasks.Task<JednostkaPodzialuTerytorialnego[]> WyszukajJednostkeWRejestrzeWebLSAsync(string nazwa, identyfikatory[] identyfiks, string kategoria, bool zawezenieRekordow, int odKtoregoRekordu, int iloscRekordow, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajJednostkeWRejestrzeWebLSCount", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajJednostkeWRejestrzeWebLSCountResponse")]
    System.Threading.Tasks.Task<int> WyszukajJednostkeWRejestrzeWebLSCountAsync(string nazwa, identyfikatory[] identyfiks, string kategoria, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajJednostkeWRejestrzeWebLSZSortowaniem", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajJednostkeWRejestrzeWebLSZSortowaniemResponse" +
        "")]
    System.Threading.Tasks.Task<JednostkaPodzialuTerytorialnegoDoSortowania[]> WyszukajJednostkeWRejestrzeWebLSZSortowaniemAsync(string nazwa, identyfikatory[] identyfiks, string kategoria, bool zawezenieRekordow, int odKtoregoRekordu, int iloscRekordow, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWRejestrze", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWRejestrzeResponse")]
    System.Threading.Tasks.Task<WyszukanaMiejscowosc[]> WyszukajMiejscowoscWRejestrzeAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] identyfiks, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajUliceWRejestrze", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajUliceWRejestrzeResponse")]
    System.Threading.Tasks.Task<WyszukanaUlica[]> WyszukajUliceWRejestrzeAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWeb", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWebResponse")]
    System.Threading.Tasks.Task<WyszukanaMiejscowosc[]> WyszukajMiejscowoscWebAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] identyfiks, bool czyPelnaNazwa, int iloscRekordow, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWebLS", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWebLSResponse")]
    System.Threading.Tasks.Task<WyszukanaMiejscowoscZPodstawowa[]> WyszukajMiejscowoscWebLSAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] idents, bool czyPelnaNazwa, bool czyFragmentNazwy, bool zawezenieRekordow, int odKtoregoRekordu, int iloscRekordow, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWebCount", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWebCountResponse")]
    System.Threading.Tasks.Task<int> WyszukajMiejscowoscWebCountAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] identyfiks, bool czyPelnaNazwa, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWebCountLS", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajMiejscowoscWebCountLSResponse")]
    System.Threading.Tasks.Task<int> WyszukajMiejscowoscWebCountLSAsync(string nazwa, string rodzajMiejscowosci, string symbol, identyfikatory[] identyfiks, bool czyPelnaNazwa, bool czyFragmentNazwy, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajUliceWeb", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajUliceWebResponse")]
    System.Threading.Tasks.Task<WyszukanaUlica[]> WyszukajUliceWebAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, bool czyPelnaNazwa, int iloscRekordow, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajUliceWebCount", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajUliceWebCountResponse")]
    System.Threading.Tasks.Task<int> WyszukajUliceWebCountAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, bool czyPelnaNazwa, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajUliceWebLS", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajUliceWebLSResponse")]
    System.Threading.Tasks.Task<WyszukanaUlicaZPodstawowa[]> WyszukajUliceWebLSAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, bool czyPelnaNazwa, bool czyFragmentNazwy, bool zawezenieRekordow, int odKtoregoRekordu, int iloscRekordow, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/WyszukajUliceWebLSCount", ReplyAction = "http://tempuri.org/ITerytWs1/WyszukajUliceWebLSCountResponse")]
    System.Threading.Tasks.Task<int> WyszukajUliceWebLSCountAsync(string nazwa, string cecha, string identyfikator, identyfikatory[] identyfiks, bool czyPelnaNazwa, bool czyFragmentNazwy, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/TerytWLiczbach", ReplyAction = "http://tempuri.org/ITerytWs1/TerytWLiczbachResponse")]
    System.Threading.Tasks.Task<Statystki> TerytWLiczbachAsync(int wybierz);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/CiekawostkiTERC", ReplyAction = "http://tempuri.org/ITerytWs1/CiekawostkiTERCResponse")]
    System.Threading.Tasks.Task<string> CiekawostkiTERCAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/CiekawostkiSIMC", ReplyAction = "http://tempuri.org/ITerytWs1/CiekawostkiSIMCResponse")]
    System.Threading.Tasks.Task<string> CiekawostkiSIMCAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/CiekawostkiULIC", ReplyAction = "http://tempuri.org/ITerytWs1/CiekawostkiULICResponse")]
    System.Threading.Tasks.Task<string> CiekawostkiULICAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/LicznoscJednostek", ReplyAction = "http://tempuri.org/ITerytWs1/LicznoscJednostekResponse")]
    System.Threading.Tasks.Task<Licznosc> LicznoscJednostekAsync();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/ObiektyZZ", ReplyAction = "http://tempuri.org/ITerytWs1/ObiektyZZResponse")]
    System.Threading.Tasks.Task<ObiektyZZ[]> ObiektyZZAsync(string woj, string pow, string gmi, string rodz, string symbolMsc, string SymUl);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/AdresBudynkow", ReplyAction = "http://tempuri.org/ITerytWs1/AdresBudynkowResponse")]
    System.Threading.Tasks.Task<AdresoBudynki[]> AdresBudynkowAsync(string woj, string pow, string gmi, string rodz, string symbolMsc, string SymUl);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/AdresBudynkowMieszkania", ReplyAction = "http://tempuri.org/ITerytWs1/AdresBudynkowMieszkaniaResponse")]
    System.Threading.Tasks.Task<AdresoBudynkiMieszkania[]> AdresBudynkowMieszkaniaAsync(string woj, string pow, string gmi, string rodz, string symbolMsc, string SymUl);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/ZbiorObiektowZZ", ReplyAction = "http://tempuri.org/ITerytWs1/ZbiorObiektowZZResponse")]
    System.Threading.Tasks.Task<PlikZbioryNOBC> ZbiorObiektowZZAsync(string woj, string pow, string gmi, string rodz, string formatDanych, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/AdresyBudynkowZIdentyfikatoremAdresu", ReplyAction = "http://tempuri.org/ITerytWs1/AdresyBudynkowZIdentyfikatoremAdresuResponse")]
    System.Threading.Tasks.Task<PlikZbioryNOBC> AdresyBudynkowZIdentyfikatoremAdresuAsync(string woj, string pow, string gmi, string rodz, string formatDanych, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/AdresyBudynkowZIdentyfikatoremBudynku", ReplyAction = "http://tempuri.org/ITerytWs1/AdresyBudynkowZIdentyfikatoremBudynkuResponse")]
    System.Threading.Tasks.Task<PlikZbioryNOBC> AdresyBudynkowZIdentyfikatoremBudynkuAsync(string woj, string pow, string gmi, string rodz, string formatDanych, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/AdresyBudynkow", ReplyAction = "http://tempuri.org/ITerytWs1/AdresyBudynkowResponse")]
    System.Threading.Tasks.Task<PlikZbioryNOBC> AdresyBudynkowAsync(string woj, string pow, string gmi, string rodz, string formatDanych, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/AdresyBudynkowZLiczbaMieszkan", ReplyAction = "http://tempuri.org/ITerytWs1/AdresyBudynkowZLiczbaMieszkanResponse")]
    System.Threading.Tasks.Task<PlikZbioryNOBC> AdresyBudynkowZLiczbaMieszkanAsync(string woj, string pow, string gmi, string rodz, string formatDanych, System.DateTime DataStanu);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ITerytWs1/AdresyBudynkowImieszkania", ReplyAction = "http://tempuri.org/ITerytWs1/AdresyBudynkowImieszkaniaResponse")]
    System.Threading.Tasks.Task<PlikZbioryNOBC> AdresyBudynkowImieszkaniaAsync(string woj, string pow, string gmi, string rodz, string formatDanych, System.DateTime DataStanu);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public interface ITerytWs1Channel : ITerytWs1, System.ServiceModel.IClientChannel
{
}


