using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravvvniIspitDeetalji_VM_Prikaz
    {
        public List<Row> detaljiPrikaz { get; set; }

        public class Row
        {
            public int detaljID { get; set; }
            public string ucenikIme { get; set; }
            public string odjeljenjeIme { get; set; }
            public int brojUDnevniku { get; set; }
            public bool pristupio { get; set; }
            public string rezultatPristupa { get { return pristupio ? "DA" : "NE"; } }
            public bool imaPravoPristupa { get; set; }
            public int brojBodova { get; set; }
        }

    }
}
