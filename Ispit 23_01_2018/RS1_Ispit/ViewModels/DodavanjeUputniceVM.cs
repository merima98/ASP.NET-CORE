using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodavanjeUputniceVM
    {
        public int uputioLjekarID { get; set; }
        public List<SelectListItem> uputioLjekar { get; set; }

        [Required]
        public DateTime datumUputnice { get; set; }
        public int pacijentID { get; set; }
        public List<SelectListItem> pacijent { get; set; }

        public int vrsstaPretrageID { get; set; }
        public List<SelectListItem> vrsstaPretrage { get; set; }
    }
}
