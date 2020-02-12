using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DetaljiPoppravnogIspitaVM
    {
        public int popravniID { get; set; }
        public int predmetID { get; set; }
        public string nazivPredmeta { get; set; }
        public int nastavnikID { get; set; }
        public string nastavnikIme { get; set; }
        public string akademskaNaziv { get; set; }
        public int akademstaID { get; set; }
        public string datumIspita { get; set; }
        public string napomena { get; set; }
    }
}
