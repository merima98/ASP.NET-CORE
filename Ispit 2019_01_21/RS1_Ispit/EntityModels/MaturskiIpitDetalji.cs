using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class MaturskiIpitDetalji
    {
        public int Id { get; set; }

        [ForeignKey(nameof(MaturskiIspitID))]
        public virtual MaturskiIspit MaturskiIspit { get; set; }
        public int MaturskiIspitID { get; set; }

        public int RezultatiMaturskog { get; set; }
        public bool isPristupuoIspitu { get; set; }

        [ForeignKey(nameof(OdjeljenjeStavkaID))]
        public virtual OdjeljenjeStavka OdjeljenjeStavka { get; set; }
        public int OdjeljenjeStavkaID { get; set; }
    }
}
