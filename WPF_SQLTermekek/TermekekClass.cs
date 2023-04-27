using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppSQLTermekek
{
    public class Termek
    {
        string kategoria;
        string gyarto;
        string nev;
        int ar;
        int garido;

        public Termek(string kategoria, string gyarto, string nev, int ar, int garido)
        {
            this.kategoria = kategoria;
            this.gyarto = gyarto;
            this.nev = nev;
            this.ar = ar;
            this.garido = garido;
        }

        public string Kategoria { get => kategoria; set => kategoria = value; }
        public string Gyarto { get => gyarto; set => gyarto = value; }
        public string Nev { get => nev; set => nev = value; }
        public int Ar { get => ar; set => ar = value; }
        public int Garido { get => garido; set => garido = value; }

        public string Adatok()
        {
            return $"{this.kategoria};{this.gyarto};{this.nev};{this.ar};{this.ar};{this.garido}";
        }
    }
}