using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class OdjeljenjeADD_VM
    {
        public string SkolskaGodina { get; set; }
        public int Razred { get; set; }
        public string OznakaOdjeljenja { get; set; }
        public int NastavnikID { get; set; }
        public List<SelectListItem> Nastavnik { get; set; }

        public int NizeOdjeljenjeID { get; set; }
        public List<SelectListItem> NizeOdjeljenje { get; set; }
    }
}
