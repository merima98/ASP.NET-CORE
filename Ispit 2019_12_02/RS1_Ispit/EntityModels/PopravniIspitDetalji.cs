using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class PopravniIspitDetalji
    {
        public int Id { get; set; }

        public bool isPristupio { get; set; }
        public bool imePravoPristupa { get; set; }
        public int RezultatiMaturskogIspita { get; set; }

        [ForeignKey(nameof(OdjeljenjeStavkaId))]
        public virtual OdjeljenjeStavka OdjeljenjeStavka { get; set; }
        public int OdjeljenjeStavkaId { get; set; }


        [ForeignKey(nameof(PopravniIspitID))]
        public virtual PopravniIspit PopravniIspit { get; set; }
        public int PopravniIspitID { get; set; }
    }
}
