using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravniIspit_VM_PRIKAZ
    {

        public int skolaID { get; set; }
        public string skolaNaziv { get; set; }

        public int odjeljenjeID { get; set; }
        public string odjeljenjeNAziv { get; set; }
        public int skGodinaID { get; set; }
        public string skGOdinaNAziv { get; set; }

        public List<ROw> detaljiPopravniPrikaz { get; set; }
        public class ROw
        {
            public int popravniID { get; set; }
            public string datumPopravnog { get; set; }
            public string predmet { get; set; }
            public int brojUcenikaNaPopravnomIspitu { get; set; }
            public int brojPolozili { get; set; }
        }
    }
}
