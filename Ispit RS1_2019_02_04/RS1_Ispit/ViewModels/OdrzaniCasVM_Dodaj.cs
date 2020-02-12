using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class OdrzaniCasVM_Dodaj
    {
        public string NastavnikImePrezime { get; set; }
        public int nastavnikID { get; set; }
        public DateTime datumCasa { get; set; }
        public int odjeljenjeSkolaPredmetID { get; set; }
        public List<SelectListItem> odjeljenjeSkolaPredmet { get; set; } //ovdje cemo pogranjivati skolu,odjeljenje,razred, a pristupaat cemo preko odjeljenja
        public string sadrzajCasa { get; set; }
    }
}
