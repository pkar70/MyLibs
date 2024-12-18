using System;

    using System.Runtime.Serialization;


#region "urzędy gmin"


[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PlacUlica", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class PlacUlica : object
    {

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string IdentyfikatorTERC;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string NazwaMiejscowosci;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string RodzajMiejscowosci;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public string IdentyfikatorSIMC;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public string IdentyfikatorPRNG;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public string NazwaPelnaUlic;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public string RodzajObiektu;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public string Nazwa1Ulic;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public string Nazwa2Ulic;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9)]
        public string IdentyfikatorULIC;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public string NumerUchwaly;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public string DataUchwaly;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 12)]
        public string OpisUchwaly;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 13)]
        public string LinkDoDokumentu;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 14)]
        public string GeometriaObiektu;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 15)]
        public string IIp;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 16)]
        public string PoczatekWersjiObiektu;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 17)]
        public string KoniecWersjiObiektu;
    }


[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "OdpowiedzTeryt", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class OdpowiedzTeryt : object
    {

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public OdpowiedzTerytStatus Status;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public string OpisBledu;
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "OdpowiedzTerytStatus", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public enum OdpowiedzTerytStatus : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Sukces = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Blad = 1,
    }


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PunktAdresowy", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class PunktAdresowy : object
    {

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string IdentyfikatorTERC;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string NazwaMiejscowosci;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string RodzajMiejscowosci;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public string IdentyfikatorSIMC;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public string NazwaPelnaUlic;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public string RodzajObiektu;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public string Nazwa1Ulic;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public string Nazwa2Ulic;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 8)]
        public string IdentyfikatorULIC;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public string NumerPorzadkowy;

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public string DataNadania;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 11)]
        public string Wspolrzedne;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 12)]
        public string IIp;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 13)]
        public string PoczatekWersjiObiektu;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 14)]
        public string KoniecWersjiObiektu;
    }

#endregion


#region "undoc pt1"

[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "RMiejscowosciWiejskie", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class RMiejscowosciWiejskie : object
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int integralneInne;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int integralneOgolem;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int integralnePrzysiolki;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int integralneWsie;

        /// <summary>
        /// nazwa województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwa;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int podstawoweInne;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int podstawoweOgolem;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int podstawoweWsie;

        /// <summary>
        /// symbol województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string woj;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "RLiczbaJednostkiTerc", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class RLiczbaJednostkiTerc : object
    {


        [System.Runtime.Serialization.DataMemberAttribute()]
        public int delegatury;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int dzielnice;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int gminy;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int gminyMW;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int gminyMiejskie;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int gminyWiejskie;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int miasta;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int miastaGMW;

        /// <summary>
        /// nazwa województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwa;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int powiaty;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int powiatyMiasta;

        /// <summary>
        /// symbol województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string symbol;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "RJednostkiTerc", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class RJednostkiTerc : object
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int checkSum;

        /// <summary>
        /// dwuznakowy symbol gminy - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string gmi;

        /// <summary>
        /// nazwa jednostki podziału terytorialnego
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwa;

        /// <summary>
        /// rodzaj gminy słownie 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwaDodatkowa;

        /// <summary>
        /// dwuznakowy symbol powiatu - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string pow;

        /// <summary>
        /// jednoznakowy symbol gminy (dla niegminy: NULL) - zob. PobierzSlownikRodzajowJednostek
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string rodz;

        /// <summary>
        /// prosta konwersja RodzDo na ENUM, ale zob. też nazwaDodatkowaDo
        /// </summary>
        public RodzajJednostki rodzajDo
        {
            get
            {

                switch (rodz)
                {
                    case "1": return RodzajJednostki.GMINA_MIEJSKA;
                    case "2": return RodzajJednostki.GMINA_WIEJSKA;
                    case "3": return RodzajJednostki.GMINA_MIEJSKO_WIEJSKA;
                    case "4": return RodzajJednostki.MIASTO;
                    case "5": return RodzajJednostki.WIES;
                    case "8": return RodzajJednostki.DZIELNICA;
                    case "9": return RodzajJednostki.DELEGATURA;
                }

                return RodzajJednostki.UNRECOGNIZED;
            }
        }

        /// <summary>
        /// dwuznakowy symbol województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string woj;
    }


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "RZmianyTerc", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class RZmianyTerc : object
    {


        [System.Runtime.Serialization.DataMemberAttribute()]
        public int checkSum;

        /// <summary>
        /// dwuznakowy symbol gminy po zmianie - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string gmiDo;

        /// <summary>
        /// dwuznakowy symbol gminy przed zmianą - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string gmiZ;

        /// <summary>
        /// nazwa jednostki po zmianie (lub NULL, gdy bez zmian)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwaDo;

        /// <summary>
        /// nazwa typu jednostki po zmianie (np. gmina miejsko-wiejska)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwaDodatkowaDo;

        /// <summary>
        /// nazwa typu jednostki przed zmianą (np. gmina wiejska)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwaDodatkowaZ;

        /// <summary>
        /// nazwa jednostki przed zmianą
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwaZ;

        /// <summary>
        /// dwuznakowy symbol powiatu po zmianie - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string powDo;

        /// <summary>
        /// dwuznakowy symbol powiatu przed zmianą - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string powZ;

        /// <summary>
        /// jednoznakowy symbol gminy (dla niegminy: NULL) po zmianie - zob. PobierzSlownikRodzajowJednostek
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string rodzDo;

        /// <summary>
        /// prosta konwersja RodzDo na ENUM, ale zob. też nazwaDodatkowaDo
        /// </summary>
        public RodzajJednostki rodzajDo
        {
            get
            {

                switch (rodzDo)
                {
                    case "1": return RodzajJednostki.GMINA_MIEJSKA;
                    case "2": return RodzajJednostki.GMINA_WIEJSKA;
                    case "3": return RodzajJednostki.GMINA_MIEJSKO_WIEJSKA;
                    case "4": return RodzajJednostki.MIASTO;
                    case "5": return RodzajJednostki.WIES;
                    case "8": return RodzajJednostki.DZIELNICA;
                    case "9": return RodzajJednostki.DELEGATURA;
                }

                return RodzajJednostki.UNRECOGNIZED;
            }
        }

        /// <summary>
        /// jednoznakowy symbol gminy (dla niegminy: NULL) przed zmianą- zob. PobierzSlownikRodzajowJednostek
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string rodzZ;

        /// <summary>
        /// prosta konwersja RodzZ na ENUM, ale zob. też nazwaDodatkowaZ
        /// </summary>
        public RodzajJednostki rodzajZ
        {
            get
            {

                switch (rodzZ)
                {
                    case "1": return RodzajJednostki.GMINA_MIEJSKA;
                    case "2": return RodzajJednostki.GMINA_WIEJSKA;
                    case "3": return RodzajJednostki.GMINA_MIEJSKO_WIEJSKA;
                    case "4": return RodzajJednostki.MIASTO;
                    case "5": return RodzajJednostki.WIES;
                    case "8": return RodzajJednostki.DZIELNICA;
                    case "9": return RodzajJednostki.DELEGATURA;
                }

                return RodzajJednostki.UNRECOGNIZED;
            }
        }


        /// <summary>
        /// dwuznakowy symbol województwa po zmianie
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string wojDo;

        /// <summary>
        /// dwuznakowy symbol województwa przed zmianą 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string wojZ;
    }

    #endregion


    #region TERC



    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "JednostkaTerytorialna", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class JednostkaTerytorialna : object
    {

        /// <summary>
        /// dwuznakowy symbol gminy - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GMI;

        // bez DataMemberAttribute nie wczytuje tego


        /// <summary>
        /// nazwa jednostki terytorialnej (2024.01.01: 3 do 23 znaki, zob. też CiekawostkiTERCAsync)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NAZWA;

        /// <summary>
        /// typ jednostki podziału terytorialnego (typ słownie)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NAZWA_DOD;

        /// <summary>
        /// dwuznakowy symbol powiatu - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string POW;

        /// <summary>
        /// jednoznakowy symbol gminy (dla niegminy: NULL) - zob. PobierzSlownikRodzajowJednostek
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RODZ;

        /// <summary>
        /// prosta konwersja RODZ na ENUM, ale zob. też NAZWA_DOD
        /// </summary>
        public RodzajJednostki rodzaj
        {
            get {

                switch (RODZ) {
                    case "1": return RodzajJednostki.GMINA_MIEJSKA;
                        case "2": return RodzajJednostki.GMINA_WIEJSKA;
                    case "3": return RodzajJednostki.GMINA_MIEJSKO_WIEJSKA;
                    case "4": return RodzajJednostki.MIASTO;
                    case "5": return RodzajJednostki.WIES;
                    case "8": return RodzajJednostki.DZIELNICA;
                    case "9": return RodzajJednostki.DELEGATURA;
                }

                return RodzajJednostki.UNRECOGNIZED;
            }
        }
            
        /// <summary>
        /// data katalogu dla tego stanu
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string STAN_NA;

        /// <summary>
        /// to samo co STAN_NA ale jako data (lub .Now gdy nie da się sparsować)
        /// </summary>
        public DateTime DataStanu
        {
            get
            {
                DateTime dt = DateTime.Now;
                DateTime.TryParse(STAN_NA, out dt);
                return dt;
            }
        }


        /// <summary>
        /// dwuznakowy symbol województwa - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WOJ;
    }


    #endregion



    #region NTS

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "JednostkaNomenklaturyNTS", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class JednostkaNomenklaturyNTS : object
    {
        /// <summary>
        /// dwuznakowy symbol gminy - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GMI;

        /// <summary>
        /// nazwa jednostki terytorialnej
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NAZWA;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NAZWA_DOD;

        /// <summary>
        /// dwuznakowy symbol podregionu - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PODREG;

        /// <summary>
        /// dwuznakowy symbol powiatu - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string POW;

        /// <summary>
        /// 1: region, 2: województwo, 3: podregion, 4: powiat,5: gmina
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string POZIOM;

        /// <summary>
        /// jednoznakowy symbol regionu - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string REGION;

        /// <summary>
        /// jednoznakowy symbol gminy (dla niegminy: NULL) - zob. PobierzSlownikRodzajowJednostek
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RODZ;

        /// <summary>
        /// prosta konwersja RODZ na ENUM, ale zob. też NAZWA_DOD
        /// </summary>
        public RodzajJednostki rodzaj
        {
            get
            {

                switch (RODZ)
                {
                    case "1": return RodzajJednostki.GMINA_MIEJSKA;
                    case "2": return RodzajJednostki.GMINA_WIEJSKA;
                    case "3": return RodzajJednostki.GMINA_MIEJSKO_WIEJSKA;
                    case "4": return RodzajJednostki.MIASTO;
                    case "5": return RodzajJednostki.WIES;
                    case "8": return RodzajJednostki.DZIELNICA;
                    case "9": return RodzajJednostki.DELEGATURA;
                }

                return RodzajJednostki.UNRECOGNIZED;
            }
        }

        /// <summary>
        /// data katalogu dla tego stanu
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string STAN_NA;

        /// <summary>
        /// to samo co STAN_NA ale jako data (lub .Now gdy nie da się sparsować)
        /// </summary>
        public DateTime DataStanu
        {
            get
            {
                DateTime dt = DateTime.Now;
                DateTime.TryParse(STAN_NA, out dt);
                return dt;
            }
        }


        /// <summary>
        /// dwuznakowy symbol województwa - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WOJ;
    }

    #endregion


    #region ULIC

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "UlicaDrzewo", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class UlicaDrzewo : object
    {
        /// <summary>
        /// ul., al., itp. - skrót cechy - zob. PobierzSlownikCechULICAsync
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Cecha;

        /// <summary>
        /// dwuznakowy symbol gminy
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmi;

        /// <summary>
        /// siedmioznakowy identyfikator miejscowości
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IdentyfikatorMiejscowosci;

        /// <summary>
        /// siedmioznakowy identyfikator miejscowości podstawowej
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IdentyfikatorMiejscowosciPodstawowej;

        /// <summary>
        /// wyznacza miejsce podziału pełnej nazwy ulicy na nazwa1 i nazwa2
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IndeksKlucza;

        /// <summary>
        /// Nazwa ulicy do sortowania (2024.12.01: Nazwa1+Nazwa2 ma 1 do 92 znaków, zob. też CiekawostkiULICAsync)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa1;

        /// <summary>
        /// Nazwa ulicy do sortowania (2024.12.01: Nazwa1+Nazwa2 ma 1 do 92 znaków, zob. też CiekawostkiULICAsync)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa2;

        /// <summary>
        /// ulica, aleja, itp. - zob. PobierzSlownikCechULICAsync
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaCechy;

        /// <summary>
        /// dwuznakowy symbol powiatu - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Pow;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzGmi;

        /// <summary>
        /// data katalogu dla tego stanu
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StanNa;

        /// <summary>
        /// to samo co STAN_NA ale jako data (lub .Now gdy nie da się sparsować)
        /// </summary>
        public DateTime DataStanu
        {
            get
            {
                DateTime dt = DateTime.Now;
                DateTime.TryParse(StanNa, out dt);
                return dt;
            }
        }

        /// <summary>
        /// pięcioznakowy symbol ulicy (tak samo się nazywające ulice w różnych miejscowościach będą miały ten sam symbol)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolUlicy;

        /// <summary>
        /// dwuznakowy symbol województwa - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Woj;
    }

    #endregion

    #region SIMC

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "Miejscowosc", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class Miejscowosc : object
    {
        /// <summary>
        /// rodzaj jednostki
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiRodzaj;

        /// <summary>
        /// prosta konwersja GmiRodzaj na ENUM
        /// </summary>
        public RodzajJednostki rodzaj
        {
            get
            {

                switch (GmiRodzaj)
                {
                    case "1": return RodzajJednostki.GMINA_MIEJSKA;
                    case "2": return RodzajJednostki.GMINA_WIEJSKA;
                    case "3": return RodzajJednostki.GMINA_MIEJSKO_WIEJSKA;
                    case "4": return RodzajJednostki.MIASTO;
                    case "5": return RodzajJednostki.WIES;
                    case "8": return RodzajJednostki.DZIELNICA;
                    case "9": return RodzajJednostki.DELEGATURA;
                }

                return RodzajJednostki.UNRECOGNIZED;
            }
        }


        /// <summary>
        /// 7 znakowy id jednostki - woj, powiat, gmina, typ razem
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiSymbol;

        /// <summary>
        /// nazwa jednostki terytorialnej (często gminy, ale i delegatury)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmina;

        /// <summary>
        /// nazwa miejscowości (ale także np. Azory), (2024.01.01: 2 do 40 znaki, zob. też CiekawostkiSIMCAsync)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa;

        /// <summary>
        /// czteroznakowy symbol powiatu (wraz z wojew) - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PowSymbol;

        /// <summary>
        /// nazwa powiatu
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat;

        /// <summary>
        /// 7 znakowy identyfikator miejscowości
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Symbol;

        /// <summary>
        /// dwuznakowy symbol województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WojSymbol;

        /// <summary>
        /// nazwa województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MiejscowoscPelna", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class MiejscowoscPelna 
    {

        #region to samo co miejscowosc, ale inheritance daje w efekcie NULLe
        /// <summary>
        /// rodzaj jednostki
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiRodzaj;

        /// <summary>
        /// prosta konwersja GmiRodzaj na ENUM
        /// </summary>
        public RodzajJednostki rodzaj
        {
            get
            {

                switch (GmiRodzaj)
                {
                    case "1": return RodzajJednostki.GMINA_MIEJSKA;
                    case "2": return RodzajJednostki.GMINA_WIEJSKA;
                    case "3": return RodzajJednostki.GMINA_MIEJSKO_WIEJSKA;
                    case "4": return RodzajJednostki.MIASTO;
                    case "5": return RodzajJednostki.WIES;
                    case "8": return RodzajJednostki.DZIELNICA;
                    case "9": return RodzajJednostki.DELEGATURA;
                }

                return RodzajJednostki.UNRECOGNIZED;
            }
        }


        /// <summary>
        /// 7 znakowy id jednostki - woj, powiat, gmina, typ razem
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiSymbol;

        /// <summary>
        /// nazwa jednostki terytorialnej (często gminy, ale i delegatury)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmina;

        /// <summary>
        /// nazwa miejscowości (ale także np. Azory), (2024.01.01: 2 do 40 znaki, zob. też CiekawostkiSIMCAsync)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa;

        /// <summary>
        /// czteroznakowy symbol powiatu (wraz z wojew) - lub NULL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PowSymbol;

        /// <summary>
        /// nazwa powiatu
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat;

        /// <summary>
        /// 7 znakowy identyfikator miejscowości
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Symbol;

        /// <summary>
        /// dwuznakowy symbol województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WojSymbol;

        /// <summary>
        /// nazwa województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo;
        #endregion

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Mz;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NMSK;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NMST;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RM;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RMNazwa;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymBM;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolPodst;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolStat;

    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "RodzajMiejscowosci", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class RodzajMiejscowosci : object
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Opis;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Symbol;
    }

    #endregion


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PlikKatalog", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class PlikKatalog : object
    {


        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwa_pliku;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string opis;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string plik_zawartosc;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PlikZmiany", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class PlikZmiany : object
    {


        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwa_pliku;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string opis;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string plik_zawartosc;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "GeoTeryt", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class GeoTeryt : object
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string kodBledu;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nazwaPliku;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string opis;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string plikZawartosc;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ZweryfikowanyAdresBezUlic", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class ZweryfikowanyAdresBezUlic : object
    {


        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HistorycznyRodzajMiejscowosci;

        /// <summary>
        /// nazwa gminy / jednostki podziału (np.Kraków-Krowodrza)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaGmi;

        /// <summary>
        /// nazwa miejscowości lub jej części (np. Azory)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaMiejscowosci;

        /// <summary>
        /// nazwa powiatu
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaPow;

        /// <summary>
        /// nazwa województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaWoj;

        /// <summary>
        /// rodzaj gminy słownie (np. delegatura)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajGmi;

        /// <summary>
        /// rodzaj miejscowości słownie (np. "część miasta")
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajMiejscowosci;

        /// <summary>
        /// dwuznakowy symbol gminy
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolGmi;

        /// <summary>
        /// siedmioznakowy symbol miejscowości
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolMiejscowosci;

        /// <summary>
        /// dwuznakowy symbol powiatu
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolPow;

        /// <summary>
        /// jednoznakowy symbol rodzaju gminy
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolRodzajuGmi;

        /// <summary>
        /// dwuznakowy symbol województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolWoj;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ZweryfikowanyAdres", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class ZweryfikowanyAdres : object
    {


        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HistorycznyRodzajMiejscowosci;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaCechy;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaGmi;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaMiejscowosci;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaPow;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaUlicyWPelnymBrzmieniu;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaWoj;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa_1;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa_2;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajGmi;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajMiejscowosci;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymUl;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolGmi;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolMiejscowosci;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolPow;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolRodzajuGmi;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolWoj;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "JednostkaPodzialuTerytorialnego", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class JednostkaPodzialuTerytorialnego : object
    {
        /// <summary>
        /// nazwa gminy / jednostki podziału (np.Kraków-Krowodrza)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiNazwa;

        /// <summary>
        /// typ jednostki podziału słownie
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiNazwaDodatkowa;

        /// <summary>
        /// jednoznakowy symbol rodzaju jednostki podziału
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiRodzaj;

        /// <summary>
        /// dwuznakowy symbol gminy
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiSymbol;

        /// <summary>
        /// dwuznakowy symbol powiatu
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PowSymbol;

        /// <summary>
        /// nazwa powiatu
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat;

        /// <summary>
        /// dwuznakowy symbol województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WojSymbol;

        /// <summary>
        /// nazwa województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "Ulica", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class Ulica : object
    {

        private string CechaField;

        private string GmiRodzajField;

        private string GmiSymbolField;

        private string GminaField;

        private string IdentyfikatorMiejscowosciField;

        private string IdentyfikatorUlicyField;

        private string NazwaField;

        private string NazwaMiejscowosciField;

        private string PowSymbolField;

        private string PowiatField;

        private string WojSymbolField;

        private string WojewodztwoField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Cecha
        {
            get
            {
                return this.CechaField;
            }
            set
            {
                this.CechaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiRodzaj
        {
            get
            {
                return this.GmiRodzajField;
            }
            set
            {
                this.GmiRodzajField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GmiSymbol
        {
            get
            {
                return this.GmiSymbolField;
            }
            set
            {
                this.GmiSymbolField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmina
        {
            get
            {
                return this.GminaField;
            }
            set
            {
                this.GminaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IdentyfikatorMiejscowosci
        {
            get
            {
                return this.IdentyfikatorMiejscowosciField;
            }
            set
            {
                this.IdentyfikatorMiejscowosciField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IdentyfikatorUlicy
        {
            get
            {
                return this.IdentyfikatorUlicyField;
            }
            set
            {
                this.IdentyfikatorUlicyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa
        {
            get
            {
                return this.NazwaField;
            }
            set
            {
                this.NazwaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaMiejscowosci
        {
            get
            {
                return this.NazwaMiejscowosciField;
            }
            set
            {
                this.NazwaMiejscowosciField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PowSymbol
        {
            get
            {
                return this.PowSymbolField;
            }
            set
            {
                this.PowSymbolField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat
        {
            get
            {
                return this.PowiatField;
            }
            set
            {
                this.PowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WojSymbol
        {
            get
            {
                return this.WojSymbolField;
            }
            set
            {
                this.WojSymbolField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo
        {
            get
            {
                return this.WojewodztwoField;
            }
            set
            {
                this.WojewodztwoField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "identyfikatory", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class identyfikatory : object
    {

        private string simcField;

        private string tercField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string simc
        {
            get
            {
                return this.simcField;
            }
            set
            {
                this.simcField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string terc
        {
            get
            {
                return this.tercField;
            }
            set
            {
                this.tercField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "JednostkaPodzialuTerytorialnegoDoSortowania", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class JednostkaPodzialuTerytorialnegoDoSortowania : object
    {

        private string NazwaDodatkowaWyszukanaField;

        private string NazwaGminyField;

        private string NazwaPowiatField;

        private string NazwaWojewodztwaField;

        private string NazwaWyszukanaField;

        private string RodzajGminyField;

        private int SortGminyField;

        private int SortPowiatField;

        private string SymbolGminyField;

        private string SymbolPowiatField;

        private string SymbolWojewodztwaField;

        private string SymbolWyszukanaField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaDodatkowaWyszukana
        {
            get
            {
                return this.NazwaDodatkowaWyszukanaField;
            }
            set
            {
                this.NazwaDodatkowaWyszukanaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaGminy
        {
            get
            {
                return this.NazwaGminyField;
            }
            set
            {
                this.NazwaGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaPowiat
        {
            get
            {
                return this.NazwaPowiatField;
            }
            set
            {
                this.NazwaPowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaWojewodztwa
        {
            get
            {
                return this.NazwaWojewodztwaField;
            }
            set
            {
                this.NazwaWojewodztwaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaWyszukana
        {
            get
            {
                return this.NazwaWyszukanaField;
            }
            set
            {
                this.NazwaWyszukanaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajGminy
        {
            get
            {
                return this.RodzajGminyField;
            }
            set
            {
                this.RodzajGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SortGminy
        {
            get
            {
                return this.SortGminyField;
            }
            set
            {
                this.SortGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SortPowiat
        {
            get
            {
                return this.SortPowiatField;
            }
            set
            {
                this.SortPowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolGminy
        {
            get
            {
                return this.SymbolGminyField;
            }
            set
            {
                this.SymbolGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolPowiat
        {
            get
            {
                return this.SymbolPowiatField;
            }
            set
            {
                this.SymbolPowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolWojewodztwa
        {
            get
            {
                return this.SymbolWojewodztwaField;
            }
            set
            {
                this.SymbolWojewodztwaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolWyszukana
        {
            get
            {
                return this.SymbolWyszukanaField;
            }
            set
            {
                this.SymbolWyszukanaField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "WyszukanaMiejscowosc", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class WyszukanaMiejscowosc : object
    {

        private string DataStanuField;

        private string GmiField;

        private string GminaField;

        private string MzField;

        private string NazwaField;

        private string PowField;

        private string PowiatField;

        private string RmField;

        private string RodzajGminyField;

        private string RodzajMiejscowosciField;

        private string SymbolField;

        private string SymbolPodstField;

        private string WojField;

        private string WojewodztwoField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataStanu
        {
            get
            {
                return this.DataStanuField;
            }
            set
            {
                this.DataStanuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmi
        {
            get
            {
                return this.GmiField;
            }
            set
            {
                this.GmiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmina
        {
            get
            {
                return this.GminaField;
            }
            set
            {
                this.GminaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Mz
        {
            get
            {
                return this.MzField;
            }
            set
            {
                this.MzField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa
        {
            get
            {
                return this.NazwaField;
            }
            set
            {
                this.NazwaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Pow
        {
            get
            {
                return this.PowField;
            }
            set
            {
                this.PowField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat
        {
            get
            {
                return this.PowiatField;
            }
            set
            {
                this.PowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Rm
        {
            get
            {
                return this.RmField;
            }
            set
            {
                this.RmField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajGminy
        {
            get
            {
                return this.RodzajGminyField;
            }
            set
            {
                this.RodzajGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajMiejscowosci
        {
            get
            {
                return this.RodzajMiejscowosciField;
            }
            set
            {
                this.RodzajMiejscowosciField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Symbol
        {
            get
            {
                return this.SymbolField;
            }
            set
            {
                this.SymbolField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolPodst
        {
            get
            {
                return this.SymbolPodstField;
            }
            set
            {
                this.SymbolPodstField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Woj
        {
            get
            {
                return this.WojField;
            }
            set
            {
                this.WojField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo
        {
            get
            {
                return this.WojewodztwoField;
            }
            set
            {
                this.WojewodztwoField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "WyszukanaUlica", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class WyszukanaUlica : object
    {

        private string CechaField;

        private string DataStanuField;

        private string GmiField;

        private string GminaField;

        private string MiejscowoscField;

        private string NazwaField;

        private string Nazwa1Field;

        private string Nazwa2Field;

        private string PowField;

        private string PowiatField;

        private string RodzajGminyField;

        private string SymbolField;

        private string SymbolSimcField;

        private string WojField;

        private string WojewodztwoField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Cecha
        {
            get
            {
                return this.CechaField;
            }
            set
            {
                this.CechaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataStanu
        {
            get
            {
                return this.DataStanuField;
            }
            set
            {
                this.DataStanuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmi
        {
            get
            {
                return this.GmiField;
            }
            set
            {
                this.GmiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmina
        {
            get
            {
                return this.GminaField;
            }
            set
            {
                this.GminaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Miejscowosc
        {
            get
            {
                return this.MiejscowoscField;
            }
            set
            {
                this.MiejscowoscField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa
        {
            get
            {
                return this.NazwaField;
            }
            set
            {
                this.NazwaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa1
        {
            get
            {
                return this.Nazwa1Field;
            }
            set
            {
                this.Nazwa1Field = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa2
        {
            get
            {
                return this.Nazwa2Field;
            }
            set
            {
                this.Nazwa2Field = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Pow
        {
            get
            {
                return this.PowField;
            }
            set
            {
                this.PowField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat
        {
            get
            {
                return this.PowiatField;
            }
            set
            {
                this.PowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajGminy
        {
            get
            {
                return this.RodzajGminyField;
            }
            set
            {
                this.RodzajGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Symbol
        {
            get
            {
                return this.SymbolField;
            }
            set
            {
                this.SymbolField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolSimc
        {
            get
            {
                return this.SymbolSimcField;
            }
            set
            {
                this.SymbolSimcField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Woj
        {
            get
            {
                return this.WojField;
            }
            set
            {
                this.WojField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo
        {
            get
            {
                return this.WojewodztwoField;
            }
            set
            {
                this.WojewodztwoField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "WyszukanaMiejscowoscZPodstawowa", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class WyszukanaMiejscowoscZPodstawowa : object
    {

        private string DataStanuField;

        private string GmiField;

        private string GminaField;

        private string MzField;

        private string NazwaField;

        private string NazwaPodstField;

        private string NazwaRodzajuGminyField;

        private string PowField;

        private string PowiatField;

        private string RmField;

        private string RodzajGminyField;

        private string RodzajMiejscowosciField;

        private string RodzajMiejscowosciPodstField;

        private string SymbolField;

        private string SymbolPodstField;

        private string WojField;

        private string WojewodztwoField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataStanu
        {
            get
            {
                return this.DataStanuField;
            }
            set
            {
                this.DataStanuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmi
        {
            get
            {
                return this.GmiField;
            }
            set
            {
                this.GmiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmina
        {
            get
            {
                return this.GminaField;
            }
            set
            {
                this.GminaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Mz
        {
            get
            {
                return this.MzField;
            }
            set
            {
                this.MzField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa
        {
            get
            {
                return this.NazwaField;
            }
            set
            {
                this.NazwaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaPodst
        {
            get
            {
                return this.NazwaPodstField;
            }
            set
            {
                this.NazwaPodstField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaRodzajuGminy
        {
            get
            {
                return this.NazwaRodzajuGminyField;
            }
            set
            {
                this.NazwaRodzajuGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Pow
        {
            get
            {
                return this.PowField;
            }
            set
            {
                this.PowField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat
        {
            get
            {
                return this.PowiatField;
            }
            set
            {
                this.PowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Rm
        {
            get
            {
                return this.RmField;
            }
            set
            {
                this.RmField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajGminy
        {
            get
            {
                return this.RodzajGminyField;
            }
            set
            {
                this.RodzajGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajMiejscowosci
        {
            get
            {
                return this.RodzajMiejscowosciField;
            }
            set
            {
                this.RodzajMiejscowosciField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajMiejscowosciPodst
        {
            get
            {
                return this.RodzajMiejscowosciPodstField;
            }
            set
            {
                this.RodzajMiejscowosciPodstField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Symbol
        {
            get
            {
                return this.SymbolField;
            }
            set
            {
                this.SymbolField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolPodst
        {
            get
            {
                return this.SymbolPodstField;
            }
            set
            {
                this.SymbolPodstField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Woj
        {
            get
            {
                return this.WojField;
            }
            set
            {
                this.WojField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo
        {
            get
            {
                return this.WojewodztwoField;
            }
            set
            {
                this.WojewodztwoField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "WyszukanaUlicaZPodstawowa", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class WyszukanaUlicaZPodstawowa : object
    {

        private string CechaField;

        private string DataStanuField;

        private string GmiField;

        private string GminaField;

        private string MiejscowoscField;

        private string MiejscowoscPodstField;

        private string NazwaField;

        private string Nazwa1Field;

        private string Nazwa2Field;

        private string NazwaRodzajuGminyField;

        private string PowField;

        private string PowiatField;

        private string RodzajGminyField;

        private string RodzajMiejscowosciField;

        private string RodzajMiejscowosciPodstField;

        private string SymbolField;

        private string SymbolSimcField;

        private string SymbolSimcPodstField;

        private string WojField;

        private string WojewodztwoField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Cecha
        {
            get
            {
                return this.CechaField;
            }
            set
            {
                this.CechaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataStanu
        {
            get
            {
                return this.DataStanuField;
            }
            set
            {
                this.DataStanuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmi
        {
            get
            {
                return this.GmiField;
            }
            set
            {
                this.GmiField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmina
        {
            get
            {
                return this.GminaField;
            }
            set
            {
                this.GminaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Miejscowosc
        {
            get
            {
                return this.MiejscowoscField;
            }
            set
            {
                this.MiejscowoscField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MiejscowoscPodst
        {
            get
            {
                return this.MiejscowoscPodstField;
            }
            set
            {
                this.MiejscowoscPodstField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa
        {
            get
            {
                return this.NazwaField;
            }
            set
            {
                this.NazwaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa1
        {
            get
            {
                return this.Nazwa1Field;
            }
            set
            {
                this.Nazwa1Field = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa2
        {
            get
            {
                return this.Nazwa2Field;
            }
            set
            {
                this.Nazwa2Field = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaRodzajuGminy
        {
            get
            {
                return this.NazwaRodzajuGminyField;
            }
            set
            {
                this.NazwaRodzajuGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Pow
        {
            get
            {
                return this.PowField;
            }
            set
            {
                this.PowField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat
        {
            get
            {
                return this.PowiatField;
            }
            set
            {
                this.PowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajGminy
        {
            get
            {
                return this.RodzajGminyField;
            }
            set
            {
                this.RodzajGminyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajMiejscowosci
        {
            get
            {
                return this.RodzajMiejscowosciField;
            }
            set
            {
                this.RodzajMiejscowosciField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajMiejscowosciPodst
        {
            get
            {
                return this.RodzajMiejscowosciPodstField;
            }
            set
            {
                this.RodzajMiejscowosciPodstField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Symbol
        {
            get
            {
                return this.SymbolField;
            }
            set
            {
                this.SymbolField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolSimc
        {
            get
            {
                return this.SymbolSimcField;
            }
            set
            {
                this.SymbolSimcField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolSimcPodst
        {
            get
            {
                return this.SymbolSimcPodstField;
            }
            set
            {
                this.SymbolSimcPodstField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Woj
        {
            get
            {
                return this.WojField;
            }
            set
            {
                this.WojField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo
        {
            get
            {
                return this.WojewodztwoField;
            }
            set
            {
                this.WojewodztwoField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "Statystki", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class Statystki : object
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] nazwy;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string tytul;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "Licznosc", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class Licznosc : object
    {

        /// <summary>
        /// liczba gmin dla każdego województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] LiczbaGmin;

        /// <summary>
        /// liczba miejscowości dla każdego województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] LiczbaMiejscowosci;

        /// <summary>
        /// liczba powiatów dla każdego województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] LiczbaPowiatow;

        /// <summary>
        /// liczba ulic dla każdego województwa
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] LiczbaUlic;

        /// <summary>
        /// zawsze mam null
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Wojewodztwa;
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ObiektyZZ", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class ObiektyZZ : object
    {

        private string CechaField;

        private string GminaField;

        private string LiczbaMiejscField;

        private string MiejscowoscField;

        private string NazwaOzzField;

        private string NazwaUlicyField;

        private string Nazwa_1Field;

        private string Nazwa_2Field;

        private string NrBudWaField;

        private string NrDomuField;

        private string NrOzzField;

        private string NrbNierField;

        private string ObrebGeodezyjnyField;

        private string ObwodsSpisowyField;

        private string OpisSymboluOzzField;

        private string PowiatField;

        private string RejonStatystycznyField;

        private string RodzajField;

        private string RodzajBudynkuField;

        private string SymbolOzzField;

        private string WojewodztwoField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Cecha
        {
            get
            {
                return this.CechaField;
            }
            set
            {
                this.CechaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmina
        {
            get
            {
                return this.GminaField;
            }
            set
            {
                this.GminaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LiczbaMiejsc
        {
            get
            {
                return this.LiczbaMiejscField;
            }
            set
            {
                this.LiczbaMiejscField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Miejscowosc
        {
            get
            {
                return this.MiejscowoscField;
            }
            set
            {
                this.MiejscowoscField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaOzz
        {
            get
            {
                return this.NazwaOzzField;
            }
            set
            {
                this.NazwaOzzField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaUlicy
        {
            get
            {
                return this.NazwaUlicyField;
            }
            set
            {
                this.NazwaUlicyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa_1
        {
            get
            {
                return this.Nazwa_1Field;
            }
            set
            {
                this.Nazwa_1Field = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa_2
        {
            get
            {
                return this.Nazwa_2Field;
            }
            set
            {
                this.Nazwa_2Field = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NrBudWa
        {
            get
            {
                return this.NrBudWaField;
            }
            set
            {
                this.NrBudWaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NrDomu
        {
            get
            {
                return this.NrDomuField;
            }
            set
            {
                this.NrDomuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NrOzz
        {
            get
            {
                return this.NrOzzField;
            }
            set
            {
                this.NrOzzField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NrbNier
        {
            get
            {
                return this.NrbNierField;
            }
            set
            {
                this.NrbNierField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObrebGeodezyjny
        {
            get
            {
                return this.ObrebGeodezyjnyField;
            }
            set
            {
                this.ObrebGeodezyjnyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObwodsSpisowy
        {
            get
            {
                return this.ObwodsSpisowyField;
            }
            set
            {
                this.ObwodsSpisowyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OpisSymboluOzz
        {
            get
            {
                return this.OpisSymboluOzzField;
            }
            set
            {
                this.OpisSymboluOzzField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat
        {
            get
            {
                return this.PowiatField;
            }
            set
            {
                this.PowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RejonStatystyczny
        {
            get
            {
                return this.RejonStatystycznyField;
            }
            set
            {
                this.RejonStatystycznyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Rodzaj
        {
            get
            {
                return this.RodzajField;
            }
            set
            {
                this.RodzajField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajBudynku
        {
            get
            {
                return this.RodzajBudynkuField;
            }
            set
            {
                this.RodzajBudynkuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SymbolOzz
        {
            get
            {
                return this.SymbolOzzField;
            }
            set
            {
                this.SymbolOzzField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo
        {
            get
            {
                return this.WojewodztwoField;
            }
            set
            {
                this.WojewodztwoField = value;
            }
        }
    }

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Runtime.Serialization.DataContractAttribute(Name = "AdresoBudynki", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
public partial class AdresoBudynki : object
{

    private string CechaField;

    private string GminaField;

    private string MiejscowoscField;

    private string NazwaUlicyField;

    private string Nazwa_1Field;

    private string Nazwa_2Field;

    private string NrBudWaField;

    private string NrDomuField;

    private string NrbNierField;

    private string ObwodsSpisowyField;

    private string OkreslenieBudynkuNiemieszkalnegoField;

    private string PowiatField;

    private string RejonStatystycznyField;

    private string RodzajField;

    private string RodzajBudynkuField;

    private string WojewodztwoField;

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Cecha
    {
        get
        {
            return this.CechaField;
        }
        set
        {
            this.CechaField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Gmina
    {
        get
        {
            return this.GminaField;
        }
        set
        {
            this.GminaField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Miejscowosc
    {
        get
        {
            return this.MiejscowoscField;
        }
        set
        {
            this.MiejscowoscField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string NazwaUlicy
    {
        get
        {
            return this.NazwaUlicyField;
        }
        set
        {
            this.NazwaUlicyField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Nazwa_1
    {
        get
        {
            return this.Nazwa_1Field;
        }
        set
        {
            this.Nazwa_1Field = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Nazwa_2
    {
        get
        {
            return this.Nazwa_2Field;
        }
        set
        {
            this.Nazwa_2Field = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string NrBudWa
    {
        get
        {
            return this.NrBudWaField;
        }
        set
        {
            this.NrBudWaField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string NrDomu
    {
        get
        {
            return this.NrDomuField;
        }
        set
        {
            this.NrDomuField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string NrbNier
    {
        get
        {
            return this.NrbNierField;
        }
        set
        {
            this.NrbNierField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string ObwodsSpisowy
    {
        get
        {
            return this.ObwodsSpisowyField;
        }
        set
        {
            this.ObwodsSpisowyField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string OkreslenieBudynkuNiemieszkalnego
    {
        get
        {
            return this.OkreslenieBudynkuNiemieszkalnegoField;
        }
        set
        {
            this.OkreslenieBudynkuNiemieszkalnegoField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Powiat
    {
        get
        {
            return this.PowiatField;
        }
        set
        {
            this.PowiatField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string RejonStatystyczny
    {
        get
        {
            return this.RejonStatystycznyField;
        }
        set
        {
            this.RejonStatystycznyField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Rodzaj
    {
        get
        {
            return this.RodzajField;
        }
        set
        {
            this.RodzajField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string RodzajBudynku
    {
        get
        {
            return this.RodzajBudynkuField;
        }
        set
        {
            this.RodzajBudynkuField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Wojewodztwo
    {
        get
        {
            return this.WojewodztwoField;
        }
        set
        {
            this.WojewodztwoField = value;
        }
    }
}

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "AdresoBudynkiMieszkania", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
    public partial class AdresoBudynkiMieszkania : object
    {

        private string CechaField;

        private string GminaField;

        private string KodNiezamieszkaniaField;

        private string MiejscowoscField;

        private string NazwaUlicyField;

        private string Nazwa_1Field;

        private string Nazwa_2Field;

        private string NrBudWaField;

        private string NrDomuField;

        private string NrMieszkaniaField;

        private string NrbNierField;

        private string ObwodsSpisowyField;

        private string PowiatField;

        private string RejonStatystycznyField;

        private string RodzajField;

        private string RodzajBudynkuField;

        private string RodzajMieszkaniaField;

        private string WojewodztwoField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Cecha
        {
            get
            {
                return this.CechaField;
            }
            set
            {
                this.CechaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gmina
        {
            get
            {
                return this.GminaField;
            }
            set
            {
                this.GminaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string KodNiezamieszkania
        {
            get
            {
                return this.KodNiezamieszkaniaField;
            }
            set
            {
                this.KodNiezamieszkaniaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Miejscowosc
        {
            get
            {
                return this.MiejscowoscField;
            }
            set
            {
                this.MiejscowoscField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NazwaUlicy
        {
            get
            {
                return this.NazwaUlicyField;
            }
            set
            {
                this.NazwaUlicyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa_1
        {
            get
            {
                return this.Nazwa_1Field;
            }
            set
            {
                this.Nazwa_1Field = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nazwa_2
        {
            get
            {
                return this.Nazwa_2Field;
            }
            set
            {
                this.Nazwa_2Field = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NrBudWa
        {
            get
            {
                return this.NrBudWaField;
            }
            set
            {
                this.NrBudWaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NrDomu
        {
            get
            {
                return this.NrDomuField;
            }
            set
            {
                this.NrDomuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NrMieszkania
        {
            get
            {
                return this.NrMieszkaniaField;
            }
            set
            {
                this.NrMieszkaniaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NrbNier
        {
            get
            {
                return this.NrbNierField;
            }
            set
            {
                this.NrbNierField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObwodsSpisowy
        {
            get
            {
                return this.ObwodsSpisowyField;
            }
            set
            {
                this.ObwodsSpisowyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Powiat
        {
            get
            {
                return this.PowiatField;
            }
            set
            {
                this.PowiatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RejonStatystyczny
        {
            get
            {
                return this.RejonStatystycznyField;
            }
            set
            {
                this.RejonStatystycznyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Rodzaj
        {
            get
            {
                return this.RodzajField;
            }
            set
            {
                this.RodzajField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajBudynku
        {
            get
            {
                return this.RodzajBudynkuField;
            }
            set
            {
                this.RodzajBudynkuField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RodzajMieszkania
        {
            get
            {
                return this.RodzajMieszkaniaField;
            }
            set
            {
                this.RodzajMieszkaniaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Wojewodztwo
        {
            get
            {
                return this.WojewodztwoField;
            }
            set
            {
                this.WojewodztwoField = value;
            }
        }
    }

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Runtime.Serialization.DataContractAttribute(Name = "PlikZbioryNOBC", Namespace = "http://schemas.datacontract.org/2004/07/TerytUslugaWs1")]
public partial class PlikZbioryNOBC : object
{

    private string nazwa_plikuField;

    private string opisField;

    private string plik_zawartoscField;

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string nazwa_pliku
    {
        get
        {
            return this.nazwa_plikuField;
        }
        set
        {
            this.nazwa_plikuField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string opis
    {
        get
        {
            return this.opisField;
        }
        set
        {
            this.opisField = value;
        }
    }

    [System.Runtime.Serialization.DataMemberAttribute()]
    public string plik_zawartosc
    {
        get
        {
            return this.plik_zawartoscField;
        }
        set
        {
            this.plik_zawartoscField = value;
        }
    }
}

public enum RodzajJednostki
{
    GMINA_MIEJSKA = 1,
    GMINA_WIEJSKA = 2,
    GMINA_MIEJSKO_WIEJSKA = 3,
    /// <summary>
    /// miasto w gminie miejsko-wiejskiej
    /// </summary>
    MIASTO = 4,
    /// <summary>
    /// obszar wiejski w gminie miejsko-wiejskiej
    /// </summary>
    WIES = 5,
    /// <summary>
    /// dzielnice m. st. Warszawy
    /// </summary>
    DZIELNICA = 8,
    /// <summary>
    /// delegatury miast: Kraków, Łódź, Poznań, Wrocław
    /// </summary>
    DELEGATURA = 9,

    /// <summary>
    /// jeśli cyferka jest spoza zakresu
    /// </summary>
    UNRECOGNIZED
}

public enum TerytWliczbachTyp
{
    NAJKROTSZE_NAZWY_JEDNOSTEK_PODZIALU_TERYT = 1,
    NAJKROTSZE_NAZWY_MIEJSCOWOSCI = 2,
    NAJKROTSZE_NAZWY_ULIC = 3,

    NAJDLUZSZE_NAZWY_JEDNOSTEK_PODZIALU_TERYT = 4,
    NAJDLUZSZE_NAZWY_MIEJSCOWOSCI = 5,
    NAJDLUZSZE_NAZWY_ULIC = 6,

    NAJCZESTSZE_NAZWY_ULIC = 7,

    WOJ_NAJMNIEJ_ULIC = 8,
    WOJ_NAJMNIEJ_MIEJSCOWOSCI = 9,
    WOJ_NAJMNIEJ_MIAST = 10,

    WOJ_NAJWIECEJ_ULIC = 11,
    WOJ_NAJWIECEJ_MIEJSCOWOSCI = 12,
    WOJ_NAJWIECEJ_MIAST = 13
}


