using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class OdrzaniCasovi_VM_Prikaz
    {

        public List<ROw> listaPrikaza { get; set; }
        public string skolaNaziv { get; set; }
        public string nastavnikIme { get; set; }
        public int nastavnikID { get; set; }

        public class ROw
        {
            public int odrzaniCasID { get; set; }
            public string datumCasa { get; set; }
            public string odjeljenje { get; set; }
            public string skolskaGOdin { get; set; }
            public string predmet { get; set; }
            public List<string> odsutniUcenici { get; set; }
        }

    }

}
