using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class UrediOpravdanuOdsutnostVM
    {
        public int detaljID { get; set; }
        public string ucenikIme { get; set; }
        public string napomena { get; set; }
        public bool opravdano{ get; set; }
    }
}
