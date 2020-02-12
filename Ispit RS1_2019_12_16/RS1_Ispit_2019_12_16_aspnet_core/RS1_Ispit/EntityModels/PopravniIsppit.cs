using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class PopravniIsppit
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Nastavnik1Id))]
        public virtual Nastavnik Nastavnik1 { get; set; }
        public int Nastavnik1Id { get; set; }

        [ForeignKey(nameof(Nastavnik2Id))]
        public virtual Nastavnik Nastavnik2 { get; set; }
        public int Nastavnik2Id { get; set; }

        [ForeignKey(nameof(Nastavnik3Id))]
        public virtual Nastavnik Nastavnik3 { get; set; }
        public int Nastavnik3Id { get; set; }

        public DateTime DatumIspita { get; set; }

        [ForeignKey(nameof(SkolaId))]
        public virtual Skola Skola { get; set; }
        public int SkolaId { get; set; }

        [ForeignKey(nameof(SkolskaGodinaId))]
        public virtual SkolskaGodina SkolskaGodina { get; set; }
        public int SkolskaGodinaId { get; set; }

        [ForeignKey(nameof(PredmetId))]
        public virtual Predmet Predmet { get; set; }
        public int PredmetId { get; set; }

    }
}
