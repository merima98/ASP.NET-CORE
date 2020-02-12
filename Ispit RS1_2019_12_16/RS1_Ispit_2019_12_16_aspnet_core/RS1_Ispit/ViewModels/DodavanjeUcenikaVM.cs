using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodavanjeUcenikaVM
    {
        public int popravniID { get; set; }
        public int ucenikID { get; set; }
        public List<SelectListItem> ucenik { get; set; }
        public int bodovi { get; set; }
    }
}
