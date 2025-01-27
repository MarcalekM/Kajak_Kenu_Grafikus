using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kajak_Kenu_Grafikus
{
    class Kolcsonzes
    {
        public string Nev { get; set; }
        public int HajoAzonosito { get; set; }
        public string HajoTipusa { get; set; }
        public int SzemelyekSzama { get; set; }
        public int ElvitelOra { get; set; }
        public int ElvitelPerc { get; set; }
        public int VisszahozatalOra { get; set; }
        public int VisszahozatalPerc { get; set; }

        public bool VizenVan(string ido)
        {
            int ora = int.Parse(ido.Split(':')[0]);
            int perc = int.Parse(ido.Split(':')[1]);
            if (ElvitelOra == ora && ElvitelPerc < perc && (VisszahozatalOra > ora || VisszahozatalOra == ora && VisszahozatalPerc >= perc)) return true;
            else return false;
        }

        public string KolcsonzesIdotartam()
        {
            return $"{VisszahozatalOra - ElvitelOra} óra {VisszahozatalPerc - ElvitelPerc} perc";
        }

        public Kolcsonzes(string sor)
        {
            var temp = sor.Split(';').ToList();
            Nev = temp[0];
            HajoAzonosito = int.Parse(temp[1]);
            HajoTipusa = temp[2];
            SzemelyekSzama = int.Parse(temp[3]);
            ElvitelOra = int.Parse(temp[4]);
            ElvitelPerc = int.Parse(temp[5]);
            VisszahozatalOra = int.Parse(temp[6]);
            VisszahozatalPerc = int.Parse(temp[7]);
        }
    }
}
