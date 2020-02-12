using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class UredjivanjePopravnog_VM_Prikaz
    {
        public int popravniID { get; set; }
        public int PredmetID { get; set; }
        public string PredmeNaziv { get; set; }

        public string datumPopravnog { get; set; }

        public int skolaID { get; set; }
        public string skolaNaziv { get; set; }

        public int skGOdinaID { get; set; }
        public string skGOdinaNaziv { get; set; }
        public int odjeljenjeId { get; set; }
        public string odjeljenjeNaziv { get; set; }
    }
}
