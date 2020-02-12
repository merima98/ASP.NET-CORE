using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class IspitDetlji
    {
        public int Id { get; set; }


        [ForeignKey(nameof(IspitId))]
        public virtual Ispit Ispit{ get; set; }
        public int IspitId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
        public int StudentId { get; set; }

        public bool isPristupio { get; set; }

        public int Ocjena { get; set; }


    }
}
