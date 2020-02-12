using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazTakmicenjaVM
    {

        public int skolaID { get; set; }
        public string skolaNaziv { get; set; }
        public int razred { get; set; }
        public List<Row> podaciTakmicenjePrikaz { get; set; }
        public class Row
        {
            public int takmicenjeID { get; set; }
            public string predmetNaziv { get; set; }
            public int razred { get; set; }
            public string datum { get; set; }
            public int brojUcenisnikaKOjiNisuPristupili { get; set; }
            public string najboljaSkola { get; set; }
            public string najboljeOdjeljenje { get; set; }
            public string najboljiUcenikIme { get; set; }
        }
    }
}
