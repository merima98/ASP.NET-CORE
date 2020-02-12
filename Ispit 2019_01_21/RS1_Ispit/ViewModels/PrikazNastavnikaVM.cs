using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazNastavnikaVM
    {
        public List<Row> NastavniciPodaci { get; set; }
        public int NastavnikID { get; set; }

        public class Row
        {
            public string NastavnikImePrezime { get; set; }
            public string SkolaNaziv { get; set; }
        }
    }
}
