using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

// namespace pkar.obiekty.services.TERYT

public partial class TERYT
{

    public class TerytCache
    {
        private static string _folder;
        private static string[] _terc;
        private static string[] _simc;
        private static string[] _ulic;
        private TERYT _Parent;

        internal TerytCache(TERYT parent)
        {
            _Parent = parent;
        }   

        /// <summary>
        /// Ustawienie ścieżki dla cache (plików TERYT.*.csv)
        /// </summary>
        /// <param name="folder"></param>
        public void Init(string folder)
        {
            _folder = folder;
        }

        /// <summary>
        /// wczytaj pliki cache do pamięci
        /// </summary>
        /// <param name="ktory"></param>
        public void Load(KtoryPlik ktory)
        {
            string pth;

            switch (ktory)
            {
                case KtoryPlik.None:
                    return;
                case KtoryPlik.All:
                    Load(KtoryPlik.TERC);
                    Load(KtoryPlik.SIMC);
                    Load(KtoryPlik.ULIC);
                    break;

                case KtoryPlik.TERC:
                    pth = System.IO.Path.Combine(_folder, "TERYT.TERC.csv");
                    if (!System.IO.File.Exists(pth)) return;
                    _terc = System.IO.File.ReadAllLines(pth);
                    return;
                case KtoryPlik.SIMC:
                    pth = System.IO.Path.Combine(_folder, "TERYT.SIMC.csv");
                    if (!System.IO.File.Exists(pth)) return;
                    _simc = System.IO.File.ReadAllLines(pth);
                    return;
                case KtoryPlik.ULIC:
                    pth = System.IO.Path.Combine(_folder, "TERYT.ULIC.csv");
                    if (!System.IO.File.Exists(pth)) return;
                    _ulic = System.IO.File.ReadAllLines(pth);
                    return;
            }
        }

        /// <summary>
        /// Porównaj daty plików z datami bieżąego katalogu, jeśli plik jest starszy (ale jest!) to ściągnij nowszą wersję
        /// </summary>
        public async Task<bool> SyncAsync(KtoryPlik ktory)
        {
            // ma sens tylko po logowaniu (nie w publicznym), bo potrzebujemy datę zmiany
            if (!await _Parent.CzyZalogowanyAsync()) return false;

            DateTime aktual, dtplik;
            string pth;

            switch (ktory)
            {
                case KtoryPlik.None:
                    return false;
                case KtoryPlik.All:
                    await SyncAsync(KtoryPlik.TERC);
                    await SyncAsync(KtoryPlik.SIMC);
                    await SyncAsync(KtoryPlik.ULIC);
                    return true;

                case KtoryPlik.TERC:
                    pth = System.IO.Path.Combine(_folder, "TERYT.TERC.csv");
                    if (!System.IO.File.Exists(pth)) return false;

                    aktual = await _Parent.PobierzDateAktualnegoKatTercAsync();
                    dtplik = System.IO.File.GetLastWriteTime(pth).AddHours(1);
                    if(aktual < dtplik) return false;

                    return await ForceDownloadAsync(ktory);
                case KtoryPlik.SIMC:
                    pth = System.IO.Path.Combine(_folder, "TERYT.SIMC.csv");
                    if (!System.IO.File.Exists(pth)) return false;

                    aktual = await _Parent.PobierzDateAktualnegoKatSimcAsync();
                    dtplik = System.IO.File.GetLastWriteTime(pth).AddHours(1);
                    if (aktual < dtplik) return false;

                    return await ForceDownloadAsync(ktory);
                case KtoryPlik.ULIC:
                    pth = System.IO.Path.Combine(_folder, "TERYT.ULIC.csv");
                    if (!System.IO.File.Exists(pth)) return false;

                    aktual = await _Parent.PobierzDateAktualnegoKatUlicAsync();
                    dtplik = System.IO.File.GetLastWriteTime(pth).AddHours(1);
                    if (aktual < dtplik) return false;

                    return await ForceDownloadAsync(ktory);
            }

            return false;

        }

        /// <summary>
        /// Wymuszony download; jeśli cache był w pamięci, to jest odświeżany
        /// </summary>
        public async Task<bool> ForceDownloadAsync(KtoryPlik ktory)
        {
            //if (publiczny) return false;

            if (! await _Parent.CzyZalogowanyAsync()) return false;

            switch (ktory)
            {
                case KtoryPlik.None:
                    return false;
                case KtoryPlik.All:
                    await ForceDownloadAsync(KtoryPlik.TERC);
                    await ForceDownloadAsync(KtoryPlik.SIMC);
                    await ForceDownloadAsync(KtoryPlik.ULIC);
                    return true;

                case KtoryPlik.TERC:
                    if (!DownloadZipExtract("TERYT.TERC.csv", await _Parent.PobierzKatalogTERCAsync()))
                        return false;

                    if(_terc is null) return true;

                    // wczytanie najnowszej wersji - ale tylko gdy było wczytane wcześniej
                    if (_terc.Length > 2)
                        Load(KtoryPlik.TERC);

                    return true;
                case KtoryPlik.SIMC:
                    if (!DownloadZipExtract("TERYT.SIMC.csv", await _Parent.PobierzKatalogSIMCAsync()))
                        return false;

                    if (_simc is null) return true;

                    // wczytanie najnowszej wersji - ale tylko gdy było wczytane wcześniej
                    if (_simc.Length > 2)
                        Load(KtoryPlik.SIMC);

                    return true;
                case KtoryPlik.ULIC:
                    if (!DownloadZipExtract("TERYT.ULIC.csv", await _Parent.PobierzKatalogULICAsync()))
                        return false;

                    if (_ulic is null) return true;

                    // wczytanie najnowszej wersji - ale tylko gdy było wczytane wcześniej
                    if (_ulic.Length > 2)
                        Load(KtoryPlik.ULIC);

                    return true;
            }

            return false;
        }

        private bool DownloadZipExtract(string cacheFilename, PlikKatalog pliczek)
        {
            string pth = System.IO.Path.Combine(_folder, cacheFilename);
            byte[] bajty = Convert.FromBase64String(pliczek.plik_zawartosc);
            string tempfile = System.IO.Path.GetTempFileName() + ".zip";
            System.IO.File.WriteAllBytes(tempfile, bajty);

            using (var zipek = System.IO.Compression.ZipFile.OpenRead(tempfile))
            {
                foreach (var entry in zipek.Entries)
                {
                    if (!System.IO.Path.GetExtension(entry.Name).Equals(".csv"))
                        continue;

                    entry.ExtractToFile(pth, true);
                    break;
                }
            }

            System.IO.File.Delete(tempfile);

            return System.IO.File.Exists(pth);
        }


        /// <summary>
        /// Pobierz listę województw z cache
        /// </summary>
        /// <returns>Lista województw lub NULL gdy nie ma cache</returns>
        public JednostkaTerytorialna[] PobierzListeWojewodztw()
        {
            var ret = new List<JednostkaTerytorialna>();

            if (_terc.Length < 1) return null;

            // 02;;;;DOLNOŚLĄSKIE;województwo;2024-01-01
            foreach (string linia in _terc.Where(x => x.Length>10 && x.Substring(2, 4) == ";;;;"))
            {
                ret.Add(TerytLine2JT(linia));
            }

            return ret.ToArray();
        }

        private JednostkaTerytorialna TerytLine2JT(string linia)
        {
            var pola = linia.Split(';');
            var ret = new JednostkaTerytorialna();
            // WOJ;POW;GMI;RODZ;NAZWA;NAZWA_DOD;STAN_NA
            ret.WOJ = pola[0];
            ret.POW = pola[1];
            ret.GMI = pola[2];
            ret.RODZ = pola[3];
            ret.NAZWA = pola[4];
            ret.NAZWA_DOD = pola[5];
            ret.STAN_NA = pola[6];
            return ret;
        }

        /// <summary>
        /// Pobierz listę powiatów w danym województwie z cache
        /// </summary>
        /// <param name="Woj">pole WOJ z województwa (np. "12" dla małopolski)</param>
        public JednostkaTerytorialna[] PobierzListePowiatow(string Woj)
        {
            var ret = new List<JednostkaTerytorialna>();

            if (_terc.Length < 1) return null;

            // 02;01;;;bolesławiecki;powiat;2024-01-01
            foreach (string linia in _terc.Where(x => x.StartsWith(Woj)).Where(x => x.Substring(5, 3) == ";;;"))
            {
                ret.Add(TerytLine2JT(linia));
            }

            return ret.ToArray();
        }


        /// <summary>
        /// Pobierz listę gmin w danym województwie i powiecie z cache
        /// </summary>
        /// <param name="Woj">pole WOJ z województwa (np. "12" dla małopolski)</param>
        /// <param name="Pow">pole POW z powiatu (np. "61" dla Kraków)</param>
        public JednostkaTerytorialna[] PobierzListeGmin(string Woj, string Pow)
        {
            var ret = new List<JednostkaTerytorialna>();

            if (_terc.Length < 1) return null;

            // 02;01;01;1;Bolesławiec;gmina miejska;2024-01-01
            string prefix = Woj + ";" + Pow + ";";
            foreach (string linia in _terc.Where(x => x.StartsWith(prefix)).Where(x => x.Substring(5, 1) != ";"))
            {
                ret.Add(TerytLine2JT(linia));
            }

            return ret.ToArray();
        }


        public enum KtoryPlik
        {
            None,
            TERC,
            SIMC,
            ULIC,
            All
        }



    }

}

