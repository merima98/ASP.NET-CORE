using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class UrediVM
    {
        public int odjeljenjeID { get; set; }
        public int detaljiID { get; set; }
        public int UcenikID { get; set; }
        public List<SelectListItem> Ucenik { get; set; }
        public int BrojUDnevniku { get; set; }
    }
}
