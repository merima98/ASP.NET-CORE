using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravniIspitDetaljiVM_Prikaz
    {
        public List<Row> podaciDetaljiPopravni { get; set; }
        public class Row
        {
            public int detaljiID { get; set; }
            public string ucenikIme { get; set; }
            public string odjeljenjeNaziv { get; set; }
            public int brojUDnevniku { get; set; }
            public bool pristupio { get; set; }
            public string rezultatPristupa { get { return pristupio ? "DA" : "NE"; } }

            public bool ImaPravoPristupa { get; set; }
            public int rezultatiBodovi { get; set; }
        }
    }
}
