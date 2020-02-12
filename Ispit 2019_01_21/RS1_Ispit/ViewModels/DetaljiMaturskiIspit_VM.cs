using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DetaljiMaturskiIspit_VM
    {
        public List<Row> DetaljiPrikaz { get; set; }
        public class Row
        {
            public int detaljiID { get; set; }
            public string ucenikImePrezime { get; set; }
            public double prosjekOcjena { get; set; }
            public bool pristupioIspitu { get; set; }
            public string RezultatPristupa { get { return pristupioIspitu ? "DA" : "NE"; } }
            public int rezultatMaturskog { get; set; }
        }
    }
}
