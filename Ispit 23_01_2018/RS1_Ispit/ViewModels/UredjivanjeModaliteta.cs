using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class UredjivanjeModaliteta
    {
        public int labID { get; set; }
        public string nazivOpis { get; set; }
        public int nazivID { get; set; }
        public List<SelectListItem> naziv { get; set; }
    }
}
