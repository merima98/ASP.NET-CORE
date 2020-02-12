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
        public bool Prisutan { get; set; }
        public bool opravdanoOdsutan { get; set; }

        [ForeignKey(nameof(OdrzaniCasID))]
        public virtual OdrzaniCas OdrzaniCas { get; set; }
        public int OdrzaniCasID { get; set; }

        public int ocjena { get; set; }

        [ForeignKey(nameof(UcenikID))]
        public virtual Ucenik Ucenik { get; set; }
        public int UcenikID { get; set; }

        public string Napomena { get; set; }
    }
}
