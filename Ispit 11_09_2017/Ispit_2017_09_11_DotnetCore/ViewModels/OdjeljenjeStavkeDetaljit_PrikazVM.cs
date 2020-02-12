using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class OdjeljenjeStavkeDetaljit_PrikazVM
    {
        public List<Row>podaciDetalji { get; set; }
        public int odjeljenjeID { get; set; }
        public class Row
        {
            public int detaljiID { get; set; }
            public int brojUDnevniku { get; set; }
            public string ucenikImePrezime { get; set; }
            public int brojZakljucenihOcjena { get; set; }
        }
    }
}
