using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DetaljiTakmicenjaVM
    {
        public string skolaNaziv { get; set; }
        public string predmet { get; set; }
        public int razred { get; set; }
        public string datum { get; set; }
        public int skolaID { get; set; }
        public int predmetID { get; set; }
        public int takmicenjeID { get; set; }
    }
}
