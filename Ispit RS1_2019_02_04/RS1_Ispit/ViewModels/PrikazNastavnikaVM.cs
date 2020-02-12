using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazNastavnikaVM
    {
        public List<Row>PodaciNastavnik { get; set; }
        public class Row
        {
            public int nastavnikID { get; set; }
            public string nastavnikImePrezima { get; set; }
            public int brojCasova { get; set; }
        }
    }
}
