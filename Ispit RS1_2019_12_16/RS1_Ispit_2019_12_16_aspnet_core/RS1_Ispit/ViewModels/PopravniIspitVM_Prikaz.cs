using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravniIspitVM_Prikaz
    {
        public string predmetNaziv { get; set; }

        public int predmetID { get; set; }
        public string skola { get; set; }
        public int skolaID { get; set; }
        public string skolskaGOdina { get; set; }
        public int skolskaGOdinaId { get; set; }

        public List<Row> podaciPoppravniIspit { get; set; }
        public class Row
        {
            public string datumPopravnogIspita { get; set; }
            public int popravniID { get; set; }
            public string prviClanKomisijePredmet { get; set; }
            public int brojUcesnikaNaPopravnomIspitu { get; set; }
            public int brojUcesnikaaKojiSuPoloziliIspit { get; set; }
        }
    }
}
