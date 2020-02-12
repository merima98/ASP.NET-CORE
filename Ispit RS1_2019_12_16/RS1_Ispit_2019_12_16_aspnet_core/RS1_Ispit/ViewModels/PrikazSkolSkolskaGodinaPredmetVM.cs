using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazSkolSkolskaGodinaPredmetVM
    {
        public int skolskaGodinaID { get; set; }
        public List<SelectListItem> skolskaGodina { get; set; }

        public int skolaID { get; set; }
        public List<SelectListItem> skola { get; set; }

        public int predmetID { get; set; }
        public List<SelectListItem> predmet { get; set; }

    }
}
