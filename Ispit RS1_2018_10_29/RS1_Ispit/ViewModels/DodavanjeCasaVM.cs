using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodavanjeCasaVM
    {
        public int nastavnikID { get; set; }
        public string nastavnikIme { get; set; }
        public DateTime datum { get; set; }
        public int odjeljenjePredmetID { get; set; }
        public List<SelectListItem> odjeljenjePredmet { get; set; }
    }
}
