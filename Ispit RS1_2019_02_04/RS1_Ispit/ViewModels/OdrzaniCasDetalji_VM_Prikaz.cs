using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class OdrzaniCasDetalji_VM_Prikaz
    {
        public List<Row>podaciDetaji { get; set; }
        public class Row
        {
            public int odrzaniCasDetaljiID { get; set; }
            public string ucenikImePrezime { get; set; }
            public double prosjekOcjena { get; set; }
            public int ocjena { get; set; }
            public bool prisutan { get; set; }
            public string rezultatPrisutan { get { return prisutan ? "DA" : "NE"; } }
            public bool opravdanoOdsutan { get; set; }
        }
    }
}
