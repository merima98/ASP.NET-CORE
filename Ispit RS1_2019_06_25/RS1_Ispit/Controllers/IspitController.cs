using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class IspitController : Controller
    {
        private MojContext db;

        public IspitController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = new PrikazPredmetNastavnikAkGodina_VM
            {
                podaciDetalji= db.Angazovan.Select(a=> new PrikazPredmetNastavnikAkGodina_VM.Row()
                {
                    predmetID = db.Predmet.Where(p=>p.Id==a.PredmetId).Select(p=>p.Id).FirstOrDefault(),
                    nazivPredmet = db.Predmet.Where(p=>p.Id==a.PredmetId).Select(p=>p.Naziv).FirstOrDefault(),
                    akademskaGOdinaID = db.AkademskaGodina.Where(p=>p.Id==a.AkademskaGodinaId).Select(p=>p.Id).FirstOrDefault(),
                    akademskaGodina = db.AkademskaGodina.Where(p=>p.Id==a.AkademskaGodinaId).Select(p=>p.Opis).FirstOrDefault(),
                    nastavnikID = db.Nastavnik.Where(p=>p.Id==a.NastavnikId).Select(p=>p.Id).FirstOrDefault(),
                    nastavnikIme = db.Nastavnik.Where(p=>p.Id==a.NastavnikId).Select(p=>p.Ime+p.Prezime).FirstOrDefault(),
                    brojCasova = db.OdrzaniCas.Count(b=>b.AngazovaniId==a.Id),
                    brojUcenikaNaPredmetu= db.SlusaPredmet.Count(s=>s.AngazovanId==a.Id)
                }).ToList()
            };

            return View(model);
        }
        public ActionResult PrikazIspitnihTermina(int predmetID, int nastvnikID, int akGodinaID)
        {
            var model = new PrikazIspitaVM
            {
                predmetID = predmetID,
                akademskaID = akGodinaID,
                nastavnikID = nastvnikID,
                predmetNaziv = db.Predmet.Where(p => p.Id == predmetID).Select(p => p.Naziv).FirstOrDefault(),
                akademskaNaziv = db.AkademskaGodina.Where(p => p.Id == akGodinaID).Select(p => p.Opis).FirstOrDefault(),
                nastavnikIme = db.Nastavnik.Where(p => p.Id == nastvnikID).Select(p => p.Ime + p.Prezime).FirstOrDefault(),
                podaciIspit = db.Ispit.Where(i => i.NastavnikId == nastvnikID && i.PredmetId == predmetID && i.AkademskaGodinaId == akGodinaID).Select(i => new PrikazIspitaVM.Row()
                {
                    ispitID = i.Id,
                    datumIspita = i.DatumIspita.ToString("dd.MM.yyyy"),
                    brojStudenataKojiNisuPoloziliPredmet = db.SlusaPredmet.Where(s => s.Angazovan.PredmetId == predmetID).Count(s => s.Ocjena == 1),
                    brojPrijavljenihStudenata = db.IspitDetlji.Count(pr => pr.IspitId == i.Id),
                    evidentiraniRezultati= i.zakljucano
                }).ToList()
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult DodavanjeIspita(int nastavnikID, int predmetID, int akGodinaID)
        {
            var model = new DodavanjeIspita_VM
            {
                predmetID= predmetID, 
                nazivPredmeta= db.Predmet.Where(p=>p.Id==predmetID).Select(p=>p.Naziv).FirstOrDefault(),
                nastavnikIme= db.Nastavnik.Where(p=>p.Id==nastavnikID).Select(p=>p.Ime+p.Prezime).FirstOrDefault(),
                nastavnikID= nastavnikID,
                akademskaNaziv = db.AkademskaGodina.Where(p => p.Id == akGodinaID).Select(p => p.Opis).FirstOrDefault(),
                akademstaID=akGodinaID
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult DodavanjeIspita(DodavanjeIspita_VM temp)
        {
            Ispit ispit = new Ispit
            {
                AkademskaGodinaId = temp.akademstaID,
                DatumIspita = temp.datumIspita,
                Napomena = temp.napomena,
                NastavnikId = temp.nastavnikID,
                PredmetId = temp.predmetID,
                zakljucano = false
            };
            db.Add(ispit); db.SaveChanges();

            List<UpisGodine> upiisGOdine = db.UpisGodine.Where(s => s.AkademskaGodinaId == temp.akademstaID).ToList();

            foreach (var upis in upiisGOdine)
            {
                IspitDetlji detalji = new IspitDetlji
                {
                    IspitId = ispit.Id,
                    isPristupio = false,
                    Ocjena = 5,
                    StudentId = upis.StudentId
                };
                db.Add(detalji); db.SaveChanges(); //nije naceden niti jedan uslov, pa sam dodala studente
            }

            return Redirect("/Ispit/PrikazIspitnihTermina?predmetID="+temp.predmetID+ "&nastvnikID="+temp.nastavnikID+ "&akGodinaID="+temp.akademstaID);
        }

        public ActionResult DetaljiPopravnogspita(int popravniID)
        {
            Ispit ispit = db.Ispit.Find(popravniID);
            var model = new DetaljiPoppravnogIspitaVM
            {
                popravniID= ispit.Id,
                predmetID= db.Predmet.Where(p=>p.Id==ispit.PredmetId).Select(p=>p.Id).FirstOrDefault(),
                nazivPredmeta= db.Predmet.Where(p=>p.Id==ispit.PredmetId).Select(p=>p.Naziv).FirstOrDefault(),
                nastavnikID= db.Nastavnik.Where(p=>p.Id==ispit.NastavnikId).Select(p=>p.Id).FirstOrDefault(),
                nastavnikIme= db.Nastavnik.Where(p=>p.Id==ispit.NastavnikId).Select(p=>p.Ime+p.Prezime).FirstOrDefault(),
                akademskaNaziv= db.AkademskaGodina.Where(p=>p.Id==ispit.AkademskaGodinaId).Select(p=>p.Opis).FirstOrDefault(),
                akademstaID= db.AkademskaGodina.Where(p=>p.Id==ispit.AkademskaGodinaId).Select(p=>p.Id).FirstOrDefault(),
                datumIspita= ispit.DatumIspita.ToString("dd.MM.yyyy"),
                napomena= ispit.Napomena
                
            };
            return View(model);
        }
        public ActionResult IspitniDetalji(int ispitID)
        {

            Ispit ispit = db.Ispit.Find(ispitID);
            var model = new DetaljiISpitaPrikaz_VM_TabelaIspod
            {
                ispitID= ispitID,
                detaljiIspitaPodaci= db.IspitDetlji.Where(i=>i.IspitId==ispitID).Select(i=> new DetaljiISpitaPrikaz_VM_TabelaIspod.Row()
                {
                    zakljucaano= ispit.zakljucano,
                    detaljiID= i.Id, 
                    studentIme = db.Student.Where(s=>s.Id==i.StudentId).Select(s=>s.Ime).FirstOrDefault(), 
                    ocjena=i.Ocjena, 
                    pristupio=i.isPristupio
                }).ToList()
            };
            return PartialView(model);
        }

        public ActionResult MijenjanjePrisutnosti(int detaljiID)
        {
            IspitDetlji ispit = db.IspitDetlji.Find(detaljiID);
            if (ispit.isPristupio == false)
            {
                ispit.isPristupio = true;
            }
            else
            {
                ispit.isPristupio = false;
            }
            db.SaveChanges();
            int ispitID = ispit.IspitId;
            return Redirect("/Ispit/IspitniDetalji?ispitID=" + ispitID);
        }

        [HttpGet]
        public ActionResult DodavanjeStudenta(int ispitID)
        {
            var model = new DodavanjeStudentaNaIspitVM
            {
                ispitID = ispitID,
                student = db.Student.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Ime} {s.Prezime}"
                }).ToList()
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult DodavanjeStudenta(DodavanjeStudentaNaIspitVM temp)
        {
            Ispit ispit = db.Ispit.Find(temp.ispitID);

            IspitDetlji novi = new IspitDetlji
            {
                IspitId = ispit.Id,
                isPristupio = false,
                Ocjena = temp.bodovi,
                StudentId = temp.studentID
            };
            db.Add(novi);db.SaveChanges();
            return Redirect("/Ispit/IspitniDetalji?ispitID=" + ispit.Id);
        }

        public ActionResult UredjivanjeBodova(int detaljiID)
        {
            //public int intDetalji { get; set; }
            //public string studentIme{ get; set; }
            //public int bodovi { get; set; }

            IspitDetlji ispitDetlji = db.IspitDetlji.Find(detaljiID);
            var model = new UredjivanjeBodovaStdentu
            {
                intDetalji= detaljiID,
                bodovi= ispitDetlji.Ocjena,
                studentIme= db.Student.Where(s=>s.Id==ispitDetlji.StudentId).Select(s=>s.Ime+s.Prezime).FirstOrDefault()
            };
            return PartialView(model);
        }
        public ActionResult SnimanjeOcjene(int intDetalji, int bodovi)
        {
            IspitDetlji ispitDetlji = db.IspitDetlji.Find(intDetalji);
            ispitDetlji.Ocjena = bodovi;
            db.SaveChanges();
            return Redirect("/Ispit/IspitniDetalji?ispitID=" + ispitDetlji.IspitId);
        }
        public ActionResult Zakljucaj(int ispitID)
        {
            Ispit ispit = db.Ispit.Find(ispitID);
            ispit.zakljucano = true;
            db.SaveChanges();
            return Redirect("/Ispit/Index");
        }
    }
}