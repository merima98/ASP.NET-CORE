using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit.Web.ViewModels
{
    public class OznaceniNeoznaceniDogadjajiVM_PRIKAZ
    {
        public List<Row>  neoznaceniDogadjaji { get; set; }
        public List<Row>  oznaceniDogadjaji { get; set; }

        public class Row
        {
            public int DogadjajID { get; set; }
            public string datumDogadjaja { get; set; }
            public string NastavnikImePrezime { get; set; }
            public string opisDogadjaja { get; set; }
            public int BrojObaveza { get; set; }
            public float IzvrsenoProcentualno { get; set; }
        }
    }
}
