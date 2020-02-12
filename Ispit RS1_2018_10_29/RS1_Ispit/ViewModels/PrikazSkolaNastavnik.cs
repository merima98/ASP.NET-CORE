using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazSkolaNastavnik
    {
        public List<Row> podaciPrikaz { get; set; }
        public class Row
        {
            public int skolaID { get; set; }
            public string skolaNaziv { get; set; }
            public int nastavnikID { get; set; }
            public string nastavnikImePrezime { get; set; }
        }
    }
}
