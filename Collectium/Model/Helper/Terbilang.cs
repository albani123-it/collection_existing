namespace Collectium.Model.Helper
{
    class Terbilang
    {
        readonly string[] data = { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas" };
        private bool minus = false;
        public string this[long angka] => CariIndexAngka(angka);

        private string CariIndexAngka(long angka)
        {
            string nilaiReturn;
            if (angka < 0)
            {
                minus = true;
                long abs = Math.Abs(angka);
                string coba = CariIndexAngka(abs);
                nilaiReturn = coba == string.Empty ? $"-{CariIndexAngka(abs)}" : $"minus {coba}";
            }
            else if (angka == 0)
            {
                nilaiReturn = "nol";
            }
            else if (angka < 12)
            {
                nilaiReturn = data[angka];
            }
            else if (angka < 20)
            {
                nilaiReturn = data[angka - 10] + " Belas";
            }
            else if (angka < 100)
            {
                nilaiReturn = Olah(angka, 10, "Puluh");
            }
            else if (angka < 200)
            {
                nilaiReturn = "Seratus " + CariIndexAngka(angka - 100);
            }
            // ~ 999
            else if (angka < 1000)
            {
                nilaiReturn = Olah(angka, 100, "Ratus");
            }
            // ~ 1,999
            else if (angka < 2000)
            {
                nilaiReturn = "Seribu " + CariIndexAngka(angka - 1000);
            }
            // ~ 9,999
            else if (angka < 10000)
            {
                nilaiReturn = Olah(angka, 1000, "Ribu");
            }
            // ~ 99,999
            else if (angka < 100000)
            {
                nilaiReturn = Olah(angka, 1000, "Ribu", 2);
            }
            // ~ 999,999
            else if (angka < 1000000)
            {
                nilaiReturn = Olah(angka, 1000, "Ribu", 3);
            }
            // ~ 9,999,999
            else if (angka < 10000000)
            {
                nilaiReturn = Olah(angka, 1000000, "Juta");
            }
            // ~ 99,999,999
            else if (angka < 100000000)
            {
                nilaiReturn = Olah(angka, 1000000, "Juta", 2);
            }
            // ~ 999,999,999
            else if (angka < 1000000000)
            {
                nilaiReturn = Olah(angka, 1000000, "Juta", 3);
            }
            // ~ 9,999,999,999
            else if (angka < 10000000000)
            {
                nilaiReturn = Olah(angka, 1000000000, "Milyar");
            }
            // ~ 99,999,999,999
            else if (angka < 100000000000)
            {
                nilaiReturn = Olah(angka, 1000000000, "Milyar", 2);
            }
            // ~ 999,999,999,999
            else if (angka < 1000000000000)
            {
                nilaiReturn = Olah(angka, 1000000000, "Milyar", 3);
            }
            // ~ 9,999,999,999,999
            else if (angka < 10000000000000)
            {
                nilaiReturn = Olah(angka, 1000000000000, "Triliun");
            }
            // ~ 99,999,999,999,999
            else if (angka < 100000000000000)
            {
                nilaiReturn = Olah(angka, 1000000000000, "Triliun", 2);
            }
            // ~ 999,999,999,999,999
            else if (angka < 1000000000000000)
            {
                nilaiReturn = Olah(angka, 1000000000000, "Triliun", 3);
            }
            // ~ 9,999,999,999,999,999
            else if (angka < 10000000000000000)
            {
                nilaiReturn = Olah(angka, 1000000000000000, "kuadriliun");
            }
            // ~ 99,999,999,999,999,999
            else if (angka < 100000000000000000)
            {
                nilaiReturn = Olah(angka, 1000000000000000, "kuadriliun", 2);
            }
            else
            {
                if (minus)
                {
                    nilaiReturn = string.Empty;
                }
                else
                {
                    nilaiReturn = $"{angka} di luar range";
                }
            }
            minus = false;
            return nilaiReturn;
        }

        string Olah(long a, long p, string konversi, int sub = 1)
        {
            long utama = a / (sub == 2 ? Convert.ToInt64(p * 0.1) : p);
            int depan = Convert.ToInt32(utama.ToString().Substring(0, sub));
            long belakang = a % p;
            return $"{CariIndexAngka(depan)} {konversi} {(belakang == 0 ? data[belakang] : CariIndexAngka(belakang))}";
        }
    }
}
