using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravniIspit_VM_Prikaz
    {
        public int odjeljenjeID { get; set; }
        public int skolaID { get; set; }
        public int skolskaGodinaID { get; set; }
        public string odjeljenjeOznaka { get; set; }
        public string naszivSkole { get; set; }
        public string skolskaGodina { get; set; }
        public List<Row> podaciPopravniIspit { get; set; }
        public class Row
        {
            public int popravniISppitID { get; set; }
            public string datumPopravnogIspita { get; set; }
            public string predmet { get; set; }
            public int brojUcenikaNaPopravnomIspitu { get; set; }
            public int brojUcenikaKojiSUPoloziliPopravniIspit { get; set; }
        }
    }
}
