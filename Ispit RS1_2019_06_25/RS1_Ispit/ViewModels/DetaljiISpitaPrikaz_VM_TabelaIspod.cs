using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DetaljiISpitaPrikaz_VM_TabelaIspod
    {
        public List<Row> detaljiIspitaPodaci { get; set; }
        public int ispitID { get; set; }
        public class Row
        {
            public bool zakljucaano { get; set; }
            public int detaljiID { get; set; }
            public string studentIme { get; set; }
            public bool pristupio { get; set; }
            public string rezultatPristupa { get { return pristupio ? "DA" : "NE"; } }
            public int ocjena { get; set; }
        }
    }
}
