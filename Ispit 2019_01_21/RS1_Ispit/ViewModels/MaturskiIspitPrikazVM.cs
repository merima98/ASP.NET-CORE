using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class MaturskiIspitPrikazVM
    {
        public List<Row> MaturskiPodaci { get; set; }
        public int NastavnikID { get; set; }
        public class Row {

            public int matuskiIspitID { get; set; }
            public string datumIspita { get; set; }
            public string skolaNaziv { get; set; }
            public string Predmet { get; set; }
            public List<string> uceniciNisuPristupili { get; set; }
        }
    }
}
