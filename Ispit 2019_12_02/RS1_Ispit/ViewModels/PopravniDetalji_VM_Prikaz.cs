using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravniDetalji_VM_Prikaz
    {
        public List<Row> podaciDetaljiPopravni { get; set; }
        public class Row
        {
            public int popravniDetaljiID { get; set; }
            public string ucenikImePrezime { get; set; }
            public string odjeljenjeNaziv { get; set; }
            public int brojUDnevniku { get; set; }
            public bool prisutan { get; set; }
            public string rezultatPrisutan { get { return prisutan ? "DA" : "NE"; } }
            public bool imaPravoPristupa { get; set; }
            public int rezultatMaturskog { get; set; }
        }
    }
}
