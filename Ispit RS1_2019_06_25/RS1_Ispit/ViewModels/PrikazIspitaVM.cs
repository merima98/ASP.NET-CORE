using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazIspitaVM
    {
        public int predmetID { get; set; }
        public string predmetNaziv { get; set; }
        public int nastavnikID { get; set; }
        public string nastavnikIme { get; set; }
        public int akademskaID { get; set; }
        public string akademskaNaziv { get; set; }

        public List<Row> podaciIspit { get; set; }
        public class Row
        {
            public int ispitID { get; set; }
            public string datumIspita { get; set; }
            public int brojStudenataKojiNisuPoloziliPredmet { get; set; }
            public int brojPrijavljenihStudenata { get; set; }
            public bool evidentiraniRezultati { get; set; }
        }
    }
}
