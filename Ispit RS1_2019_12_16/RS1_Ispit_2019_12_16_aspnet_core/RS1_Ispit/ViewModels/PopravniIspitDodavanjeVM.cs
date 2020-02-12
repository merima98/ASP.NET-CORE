using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravniIspitDodavanjeVM
    {
        public int clanKomisije1ID { get; set; }
        public List<SelectListItem> clanKomisije1 { get; set; }

        public int clanKomisije2ID { get; set; }
        public List<SelectListItem> clanKomisije2 { get; set; }

        public int clanKomisije3ID { get; set; }
        public List<SelectListItem> clanKomisije3 { get; set; }

        public DateTime datumPopravnogIspita { get; set; }

        public int skolaID { get; set; }
        public string skolaNaziv { get; set; }
        public int skolskaGodinaID { get; set; }
        public string skolskaGodinaNaziv { get; set; }
        public int predmetID { get; set; }
        public string predmetNaziv { get; set; }

    }
}
