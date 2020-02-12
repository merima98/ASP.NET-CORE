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
    public class OdrzaniCasController : Controller
    {
        private MojContext db;

        public OdrzaniCasController(MojContext db)
        {
            this.db = db;
        }
             
        public IActionResult Index()
        {
            var model = new PrikazNastavnikaVM()
            {
                
                PodaciNastavnik = db.Nastavnik.Select(n=> new PrikazNastavnikaVM.Row()
                {
                    nastavnikID= n.Id, 
                    nastavnikImePrezima= n.Ime+n.Prezime, 
                    brojCasova= db.OdrzaniCas.Count(ns=>ns.NastavnikID==n.Id)
                }).ToList()
            };
            return View(model);
        }
        public ActionResult PrikazCasova(int nastavnik)
        {

            var model = new OdrzaniCasoviPrikaz_VM()
            {
                nastavnikID= nastavnik, 

                podaciCasovi = db.OdrzaniCas.Where(n => n.NastavnikID == nastavnik).Select(o => new OdrzaniCasoviPrikaz_VM.Row()
                {
                    odrzaniCasID = o.Id,
                    datumCasa = o.DatumOdrzanogCasa.ToString("dd.MM.yyyy"),
                    skola = db.Skola.Where(s => s.Id == o.SkolaID).Select(s => s.Naziv).FirstOrDefault(),
                    skolskaGOdina = db.Odjeljenje.Where(od => od.Id == o.OdjeljenjeID).Select(od => od.SkolskaGodina.Naziv).FirstOrDefault(),
                    odjeljenje = db.Odjeljenje.Where(od => od.Id == o.OdjeljenjeID).Select(od => od.Oznaka).FirstOrDefault(),
                    predmet = db.Predmet.Where(p => p.Id == o.PredmetID).Select(p => p.Naziv).FirstOrDefault(),
                    odsutinUcenici = db.OdrzanCasDetalji.Where(od => od.OdrzaniCasID == o.Id && od.Prisutan == false).Select(od => od.OdjeljenjeStavka.Ucenik.ImePrezime).ToList()
                }).ToList()
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult DodavanjeCassa(int nastavnikID)
        {

            var model = new OdrzaniCasVM_Dodaj()
            {
                nastavnikID= nastavnikID, 
                NastavnikImePrezime= db.Nastavnik.Where(n=>n.Id==nastavnikID).Select(n=>n.Ime+n.Prezime).FirstOrDefault(), 
                odjeljenjeSkolaPredmet= db.Odjeljenje.Select(o=> new SelectListItem
                {
                    Value= o.Id.ToString(), 
                    Text= $"{db.Skola.Where(s=>s.Id==o.SkolaID).Select(s=>s.Naziv).FirstOrDefault()}/{o.Oznaka}/{db.PredajePredmet.Where(p=>p.OdjeljenjeID==o.Id).Select(p=>p.Predmet.Naziv).FirstOrDefault()}"

                }).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult DodavanjeCassa(OdrzaniCasVM_Dodaj temp)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Find(temp.odjeljenjeSkolaPredmetID);
            Skola skola = db.Skola.Where(s => s.Id == odjeljenje.SkolaID).FirstOrDefault();
            PredajePredmet predaje = db.PredajePredmet.Where(p => p.OdjeljenjeID == odjeljenje.Id).FirstOrDefault();
            Predmet predmet = db.Predmet.Where(p => p.Id == predaje.PredmetID).FirstOrDefault();
            OdrzaniCas odrzani = new OdrzaniCas()
            {
                DatumOdrzanogCasa=temp.datumCasa,
                NastavnikID=temp.nastavnikID, 
                OdjeljenjeID=temp.odjeljenjeSkolaPredmetID,
                SkolaID=skola.Id, 
                PredmetID=predaje.Id,
                SadrzajCasa= temp.sadrzajCasa
            };
            db.Add(odrzani); db.SaveChanges();

            List<OdjeljenjeStavka> odjeljenjStavke = db.OdjeljenjeStavka.Where(o => o.OdjeljenjeId == odjeljenje.Id).ToList();

            foreach (var stavke in odjeljenjStavke)
            {
                OdrzanCasDetalji odrzanCasDetalji = new OdrzanCasDetalji()
                {
                    Ocjena = 0,
                    OdjeljenjeStavkaID = stavke.Id,
                    OdrzaniCasID = odrzani.Id,
                    OpravdanoOdsutan = false,  //postavit cu primarno ovako, jer cu svakako kasnije mijenjati
                    Prisutan = false
                };
                db.Add(odrzanCasDetalji);db.SaveChanges();
            }
            return Redirect("/OdrzaniCas/PrikazCasova?nastavnik=" + temp.nastavnikID);
        }
        public ActionResult BrisanjeCasa(int casID)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Find(casID);
            int idNastavnik = odrzaniCas.NastavnikID;
            db.Remove(odrzaniCas); db.SaveChanges();
            return Redirect("/OdrzaniCas/PrikazCasova?nastavnik=" + idNastavnik);
        }
        public ActionResult DetaljiODrzanogCasa(int casID)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Find(casID);

            Odjeljenje odjeljenje = db.Odjeljenje.Where(o=>o.Id==odrzaniCas.OdjeljenjeID).FirstOrDefault();
            Skola skola = db.Skola.Where(s => s.Id == odjeljenje.SkolaID).FirstOrDefault();
            PredajePredmet predaje = db.PredajePredmet.Where(p => p.OdjeljenjeID == odjeljenje.Id).FirstOrDefault();
            Predmet predmet = db.Predmet.Where(p => p.Id == predaje.PredmetID).FirstOrDefault();

            var model = new DetaljiOdrzanogCasa_VM()
            {
                odrzanicasID= casID,
                datumCasa=odrzaniCas.DatumOdrzanogCasa.ToString("dd.MM.yyyy"),
                sadrzajCasa=odrzaniCas.SadrzajCasa,
                skolaOdjeljenjePredmet= $"{skola.Naziv}/{odjeljenje.Oznaka}/{predmet.Naziv}"
            };
            return View(model);
        }
        public ActionResult OdrzaniCasDetaljiPrikaz(int odrzaniCasID)
        {
           
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Find(odrzaniCasID);
            List<OdrzaniCas> oCas = db.OdrzaniCas.Where(o => o.Id == odrzaniCas.Id).ToList();
            List<OdrzanCasDetalji> detalji = db.OdrzanCasDetalji.Where(d => oCas.Any(o => o.Id == d.OdrzaniCasID)).ToList();

            var model = new OdrzaniCasDetalji_VM_Prikaz()
            {
                podaciDetaji = db.OdrzanCasDetalji.Where(o => o.OdrzaniCasID == odrzaniCasID).Select(o => new OdrzaniCasDetalji_VM_Prikaz.Row()
                {
                    odrzaniCasDetaljiID = o.Id,
                    ocjena = o.Ocjena,
                    opravdanoOdsutan = o.OpravdanoOdsutan,
                    prisutan = o.Prisutan,
                    prosjekOcjena = db.DodjeljenPredmet.Where(d => d.OdjeljenjeStavkaId == o.OdjeljenjeStavkaID).Average(d => (int?)d.ZakljucnoKrajGodine) ?? 0,
                    ucenikImePrezime = db.OdjeljenjeStavka.Where(os => os.Id == o.OdjeljenjeStavkaID).Select(os => os.Ucenik.ImePrezime).FirstOrDefault()
                }).ToList()
            };
            return PartialView(model);
        }
        public ActionResult PromijeniPrisutnost(int detaljiID)
        {
            OdrzanCasDetalji detalji = db.OdrzanCasDetalji.Find(detaljiID);
            if (detalji.Prisutan == false)
            {
                detalji.Prisutan = true;
            }
            else
            {
                detalji.Prisutan = false;
            }
            db.SaveChanges();
            int oCasID = detalji.OdrzaniCasID;
            return Redirect("/OdrzaniCas/OdrzaniCasDetaljiPrikaz?odrzaniCasID=" + oCasID);

        }
        public ActionResult MijenjanjeOcjene(int detaljiID)
        {
          OdrzanCasDetalji detalji = db.OdrzanCasDetalji.Find(detaljiID);
            var model = new MijenjenjePcjene_VM()
            {
                detaljiID = detaljiID,
                ucenikIme = db.OdjeljenjeStavka.Where(os => os.Id == detalji.OdjeljenjeStavkaID).Select(os => os.Ucenik.ImePrezime).FirstOrDefault(),
                ocjena = detalji.Ocjena
            };
            return PartialView(model);
        }
        public ActionResult SnimanjeOcjene(int detaljiID,int ocjena)
        {
            OdrzanCasDetalji detalji = db.OdrzanCasDetalji.Find(detaljiID);
            detalji.Ocjena = ocjena;
            db.SaveChanges();
            int oCasID = detalji.OdrzaniCasID;
            return Redirect("/OdrzaniCas/OdrzaniCasDetaljiPrikaz?odrzaniCasID=" + oCasID);
        }
        public ActionResult MijenjanjeOdsutnosti(int detaljiID)
        {
            OdrzanCasDetalji detalji = db.OdrzanCasDetalji.Find(detaljiID);
            var model = new MijenjanjeOdsutnosti_VM()
            {
                detaljiID = detaljiID,
                ucenikIme = db.OdjeljenjeStavka.Where(os => os.Id == detalji.OdjeljenjeStavkaID).Select(os => os.Ucenik.ImePrezime).FirstOrDefault(),
                napomena= detalji.Napomena
            };
            return PartialView(model);
        }
        public ActionResult SnimanjeOdsutan(int detaljiID, int oznaceno)
        {
            OdrzanCasDetalji detalji = db.OdrzanCasDetalji.Find(detaljiID);
            if (oznaceno==1)
            {
                detalji.OpravdanoOdsutan = true;
            }
            else
            {
                detalji.OpravdanoOdsutan = false;
            }
            db.SaveChanges();
            int oCasID = detalji.OdrzaniCasID;
            return Redirect("/OdrzaniCas/OdrzaniCasDetaljiPrikaz?odrzaniCasID=" + oCasID);
        }
    }
}