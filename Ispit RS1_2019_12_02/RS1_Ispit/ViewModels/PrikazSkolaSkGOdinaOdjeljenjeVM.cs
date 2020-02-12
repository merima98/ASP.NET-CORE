using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazSkolaSkGOdinaOdjeljenjeVM
    {
        public List<ROw> podaciDetalji { get; set; }
        public class ROw
        {
            public int skolaID { get; set; }
            public string skolaNaziv { get; set; }
            public int skGOdinaID { get; set; }
            public string skGOdinaNaziv { get; set; }
            public int odjeljenjeID { get; set; }
            public string odjeljenjeNaziv { get; set; }
        }
    }
}
