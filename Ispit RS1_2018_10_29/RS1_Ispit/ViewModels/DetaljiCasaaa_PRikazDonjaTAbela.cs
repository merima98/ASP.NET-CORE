    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DetaljiCasaaa_PRikazDonjaTAbela
    {
        public List<Row> podaciPrikaz { get; set; }
        public class Row
        {
            public int detaljiID { get; set; }
            public string ucenikIme { get; set; }
            public int ocjene { get; set; }
            public bool prisutan { get; set; }
            public string rezultatPrisutan { get { return prisutan ? "DA" : "NE"; }}
            public bool opravdanoOdsutan { get; set; }
        }
    }
}
