using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class Odjeljenje_Prikaz_VM
    {
        public List<Row>OdjeljenjePodaci { get; set; }
        public class Row
        {
            public int OdjeljenjeID { get; set; }
            public string skolskaGodina { get; set; }
            public int Razred { get; set; }
            public string oznakaOdjeljenja { get; set; }
            public string NastavnikImePrezime { get; set; }
            public bool isPrebacenUViseOdjeljenje { get; set; }
            public double ProsjekOcjena { get; set; }
            public string najboljiUcenik { get; set; }
        }
    }
}
