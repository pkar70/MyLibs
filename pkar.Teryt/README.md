
# Wrapper do usług GUS TERYT.

Po co masz sam bawić się w dodawanie WCF i tak dalej do swojego projektu, znacznie łatwiej dodać tego Nugeta :)

Skorzystanie z TERYT staje się tak proste jak dodanie Nuget oraz wywołanie:

        klient = New TERYT(USERNAME, PSWD)
        If Not Await klient.CzyZalogowanyAsync Then Return "Error"


Aby uzyskać dane logowania do serwisu zob. https://api.stat.gov.pl/Home/TerytApi

Teoretycznie powinno się dać korzystać z tych usług bez czytania dokumentacji, używając jedynie Intellisense, jednak mocno sugeruję sięgnięcie do strony https://api.stat.gov.pl/Home/TerytApi .

Poza funkcjami opisanymi w dokumentacji, Nuget zawiera także następujące helpery:
* większość metod wymagających podania daty ma bliźniacze metody bez daty (wtedy chodzi o stan "na Date.Now")
* dodany jest enum RodzajJednostki - "jednoznakowy symbol rodzaju jednostki", i struktury poza polem z symbolem mają też prosty 'wrapper' zmieniający symbol na enum
* dodany jest enum TerytWliczbachTyp dla metody bliźniaczej do TerytWLiczbachAsync (standardowo int, w Nuget także jako enum)


## cache

Pliki TERYT mogą być skopiowane lokalnie, i wykorzystywane zamiast odwołań online:

        void Cache.Init(string folder);    // może być wspólny dla wszystkich app, albo niezależny dla app
        void Cache.Load(KtoryPlik ktory);  // wczytanie pliku lokalnego
        async Task<bool> Cache.SyncAsync(KtoryPlik ktory); // wczytanie plików z serwera jeśli nowszy niż lokalny
        async Task<bool> Cache.ForceDownloadAsync(KtoryPlik ktory); // wymuszenie wczytania pliku

        JednostkaTerytorialna[] Cache.PobierzListeWojewodztw();    // pobierz listę województw z cache, odpowiednik teryt.PobierzListeWojewodztwAsync
        JednostkaTerytorialna[] Cache.PobierzListePowiatow(string Woj); // pobierz listę powiatów w województwie z cache, odpowiednik teryt.PobierzListePowiatowAsync, 
        JednostkaTerytorialna[] Cache.PobierzListeGmin(string Woj, string Pow); // pobierz listę gmin w powiecie z cache, odpowiednik teryt.PobierzListeGminAsync, 


Niestety, biblioteka nie może być .Net Std 1.4, minimum to .Net Std 2.0. Tak więc Nuget nie zadziała na telefonach z Windows; ale zadziała na desktop WPF/UWP/WinUI, oraz wieloplarformowo w Uno, i w MAUI...
Ograniczenie bierze się z System.ServiceModel.Primitives - po obniżeniu do wersji 4.10.3 zaczyna brakować funkcji. Analogicznie System.ServiceModel.Http.
