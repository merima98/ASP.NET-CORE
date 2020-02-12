using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class OdrzaniCasoviPrikaz_VM
    {
        public List<Row>podaciCasovi { get; set; }
        public int nastavnikID { get; set; }
        public class Row
        {
            public int odrzaniCasID { get; set; }
            public string datumCasa { get; set; }
            public string skola { get; set; }
            public string skolskaGOdina { get; set; }
            public string odjeljenje { get; set; }
            public string predmet { get; set; }
            public List<string> odsutinUcenici { get; set; }
        }

    }
}
