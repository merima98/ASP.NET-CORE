﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class TakmicenjeUcesnik
    {
        public int Id { get; set; }

        [ForeignKey(nameof(TakmicenjeId))]
        public virtual Takmicenje Takmicenje { get; set; }
        public int TakmicenjeId { get; set; }

        [ForeignKey(nameof(UcenikId))]
        public virtual Ucenik Ucenik { get; set; }
        public int UcenikId { get; set; }
        public bool PristupioTakmicenju { get; set; }
        public int Bodovi { get; set; }
    }
}
