using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazDetaljaDetaljVM
    {
        public List<ROw> podaciRezultat { get; set; }
        public int takmicenjeID { get; set; }
        public class ROw
        {
            public int takmicenjeDetaljID { get; set; }
            public string odjeljenjeNAziv { get; set; }
            public int brojDnevnik { get; set; }
            public bool pristupio { get; set; }
            public string rezultatPristupa { get { return pristupio ? "DA" : "NE"; } }
            public int bodovi { get; set; }
            public bool isZakljucano { get; set; }
        }
    }
}
