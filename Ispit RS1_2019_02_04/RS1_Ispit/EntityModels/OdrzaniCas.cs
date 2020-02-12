using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class OdrzaniCas
    {
        public int Id { get; set; }

        [ForeignKey(nameof(NastavnikID))]
        public virtual Nastavnik Nastavnik { get; set; }
        public int NastavnikID { get; set; }

        public DateTime DatumOdrzanogCasa { get; set; }

        [ForeignKey(nameof(SkolaID))]
        public virtual Skola Skola { get; set; }
        public int SkolaID { get; set; }

        [ForeignKey(nameof(OdjeljenjeID))]
        public virtual Odjeljenje Odjeljenje { get; set; }
        public int OdjeljenjeID { get; set; }

        [ForeignKey(nameof(PredmetID))]
        public virtual Predmet Predmet { get; set; }
        public int PredmetID { get; set; }

        public string SadrzajCasa { get; set; }
    }
}
