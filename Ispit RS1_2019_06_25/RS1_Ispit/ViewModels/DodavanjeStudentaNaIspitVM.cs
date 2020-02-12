using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodavanjeStudentaNaIspitVM
    {
        public int ispitID { get; set; }
        public int studentID { get; set; }
        public List<SelectListItem> student { get; set; }
        public int bodovi { get; set; }
    }
}
