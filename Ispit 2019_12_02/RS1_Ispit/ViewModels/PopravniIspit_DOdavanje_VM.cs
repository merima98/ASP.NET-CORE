using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravniIspit_DOdavanje_VM
    {
        public int PredmetID { get; set; }
        public List<SelectListItem> Predmet { get; set; }
        public DateTime datumPopravnogIspita { get; set; }

        public string skolaNaziv { get; set; }
        public int skolaID { get; set; }
        public string skolskaGOdinaNaziv { get; set; }
        public int skolskaGOdinaINT { get; set; }
        public int odjeljenjeID { get; set; }
        public string odjeljenjeOznaka { get; set; }

    }
}
