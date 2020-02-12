using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class EvidentiraneUputnice_VM_prikaz
    {
        public List<Row> podaciPrikaz { get; set; }
        public class Row
        {
            public int uputnicaID { get; set; }
            public string datumUputnice { get; set; }
            public string doktorIme { get; set; }
            public string pacijentIme { get; set; }
            public string vrstaPretrage { get; set; }
            public DateTime? datumEvidentiranjaRezultataPretrage { get; set; }
        }
    }
}
