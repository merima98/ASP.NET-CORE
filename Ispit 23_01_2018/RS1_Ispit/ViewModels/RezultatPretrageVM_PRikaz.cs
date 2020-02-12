using RS1.Ispit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class RezultatPretrageVM_PRikaz
    {
        public List<Row> podaciRezultatiPrikaz { get; set; }
        public class Row
        {
            public int rezultatID { get; set; }
            public string labPretragaNaziv { get; set; }
            public double? izmjerenaNumerickaVrijednost { get; set; }
            public string akoJeModalitetNaziv { get; set; }
            public string mjernaJednicica { get; set; }
            public VrstaVrijednosti vrstaVrijednosti { get; set; }

        }

    }
}
