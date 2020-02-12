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

        [ForeignKey(nameof(PopravniIsppitId))]
        public virtual PopravniIsppit PopravniIsppit { get; set; }
        public int PopravniIsppitId { get; set; }

        [ForeignKey(nameof(UcenikId))]
        public virtual Ucenik Ucenik { get; set; }
        public int UcenikId { get; set; }

        public bool imaPracoPristupa { get; set; }
        public bool isPristupio { get; set; }
        public int bodoviIspita { get; set; }
    }
}
