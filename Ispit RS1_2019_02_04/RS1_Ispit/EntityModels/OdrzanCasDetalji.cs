using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class OdrzanCasDetalji
    {
        public int Id { get; set; }

        [ForeignKey(nameof(OdrzaniCasID))]
        public virtual OdrzaniCas OdrzaniCas { get; set; }
        public int OdrzaniCasID { get; set; }

        public bool Prisutan { get; set; }
        public bool OpravdanoOdsutan { get; set; }
        public int Ocjena { get; set; }

        [ForeignKey(nameof(OdjeljenjeStavkaID))]
        public virtual OdjeljenjeStavka OdjeljenjeStavka { get; set; }
        public int OdjeljenjeStavkaID { get; set; }

        public string Napomena { get; set; }
    }
}
