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
        public bool imaPravoPristupa { get; set; }
        public int rezultatiMaturskogBodovi { get; set; }

        [ForeignKey(nameof(PopravniIspitId))]
        public virtual PopravniIspit PopravniIspit { get; set; }
        public int PopravniIspitId { get; set; }

        [ForeignKey(nameof(UcenikId))]
        public virtual Ucenik Ucenik { get; set; }
        public int UcenikId { get; set; }
    }
}
