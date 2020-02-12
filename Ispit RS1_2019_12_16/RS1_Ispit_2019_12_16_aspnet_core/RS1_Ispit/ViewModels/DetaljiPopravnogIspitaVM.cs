using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DetaljiPopravnogIspitaVM
    {
        public int popravniIspitID { get; set; }
        public string clanKomicije1 { get; set; }
        public string clanKomisije2 { get; set; }
        public string clanKomisije3 { get; set; }
        public string datumIspita { get; set; }
        public string skolaNaziv { get; set; }
        public int skolaID { get; set; }
        public string skolskaGodina { get; set; }
        public int skolskaGOdinaID { get; set; }
        public string predmetNaziv { get; set; }
        public int predmetID { get; set; }
    }
}
