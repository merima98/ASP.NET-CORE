using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class OdjeljenjeDetalji_VM
    {
        public int OdjeljenjeID { get; set; }
        public string skolskaGodina { get; set; }
        public int razred { get; set; }
        public string oznaka { get; set; }
        public string nastavnikaImePrezime { get; set; }
        public int brojRazreda { get; set; }
    }
}
