using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazSkolaRazred_VM
    {
        public int skolaID { get; set; }
        public List<SelectListItem> skola{ get; set; }
        public int razredID { get; set; }
        public List<SelectListItem> razred { get; set; }
        public PrikazSkolaRazred_VM()
        {
            razred = new List<SelectListItem>();
            for (int i = 1; i < 5; i++)
            {
                razred.Add(new SelectListItem()
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }
        }
    }
}
