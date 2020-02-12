using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit.Web.ViewModels
{
    public class StanjeObavezePrikazVM
    {
        public List<Row>podaciStanjeObavezePrikaz { get; set; }

        public class Row
        {
            public int stanjeObavezeID { get; set; }
            public bool isZavrseno { get; set; }
            public string naziivStanjaObaveze { get; set; }
            public float izvrsenoProcentualno { get; set; }
            public int personalnaVrijednostSaljiNotifikacijuDanaPrije { get; set; }
            public bool personalnaVrijednostRekurzijaBOOL { get; set; }

        }
    }
}
