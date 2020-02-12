using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazPredmetNastavnikAkGodina_VM
    {
        public List<Row> podaciDetalji { get; set; }
        public class Row
        {
            public int predmetID { get; set; }
            public string nazivPredmet { get; set; }
            public int akademskaGOdinaID { get; set; }
            public string akademskaGodina { get; set; }
            public int nastavnikID { get; set; }
            public string nastavnikIme { get; set; }
            public int brojCasova { get; set; }
            public int brojUcenikaNaPredmetu { get; set; }

        }
    }
}
