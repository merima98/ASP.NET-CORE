using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DetlajiUputniceVM_Prikaz
    {
        public int uputnicaID { get; set; }
        public string datumUputnice { get; set; }
        public string pacijentIme { get; set; }
        public DateTime? datumRezultataUputnice { get; set; }

    }
}
