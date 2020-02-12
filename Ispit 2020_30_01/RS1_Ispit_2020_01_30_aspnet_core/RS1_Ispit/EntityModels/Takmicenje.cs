using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class Takmicenje
    {
        public int Id { get; set; }

        [ForeignKey(nameof(SkolaId))]
        public virtual Skola Skola { get; set; }
        public int SkolaId { get; set; }

        [ForeignKey(nameof(PredmetId))]
        public virtual Predmet Predmet { get; set; }
        public int PredmetId { get; set; }

        public int Razred { get; set; }
        public DateTime DatumTakmicenja { get; set; }
        public bool zakljucano { get; set; }
    }
}
