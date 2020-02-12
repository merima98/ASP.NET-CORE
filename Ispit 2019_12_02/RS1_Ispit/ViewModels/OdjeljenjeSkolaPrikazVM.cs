using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class OdjeljenjeSkolaPrikazVM
    {
        public List<Row> podaciOdjeljenje { get; set; }
        public class Row
        {
            public int odjeljenjeID { get; set; }
            public string odjeljenjeOznaka { get; set; }
            public string skolskaGOdina { get; set; }
            public string skolaNaziv { get; set; }
        }
    }
}
