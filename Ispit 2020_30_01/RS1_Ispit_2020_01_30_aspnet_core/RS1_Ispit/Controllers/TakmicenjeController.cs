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
    public class TakmicenjeController : Controller
    {
        private MojContext db;
        public TakmicenjeController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = new PrikazSkolaRazred_VM
            {
                skola = db.Skola.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Naziv
                }).ToList()
            };
            return View(model);
        }
        public  ActionResult Odaberi(PrikazSkolaRazred_VM temp)
        {


            List<OdjeljenjeStavka> odjeljenjeStavkas = db.OdjeljenjeStavka.ToList();
            var model = new PrikazTakmicenjaVM
            {
                skolaID= temp.skolaID,
                skolaNaziv= db.Skola.Where(s=>s.Id==temp.skolaID).Select(s=>s.Naziv).FirstOrDefault(), 
                razred= temp.razredID,
                podaciTakmicenjePrikaz= db.Takmicenje.Where(s=>s.SkolaId==temp.skolaID && s.Razred==temp.razredID).Select(t=> new PrikazTakmicenjaVM.Row()
                {
                    takmicenjeID= t.Id,
                     predmetNaziv= db.Predmet.Where(p=>p.Id==t.PredmetId).Select(p=> p.Naziv).FirstOrDefault(), 
                     datum= t.DatumTakmicenja.ToString("dd.MM.yyyy"),
                      razred= t.Razred,
                      brojUcenisnikaKOjiNisuPristupili= db.TakmicenjeUcesnik.Count(tr=>tr.TakmicenjeId==t.Id && tr.PristupioTakmicenju==false),
                      najboljiUcenikIme=  db.DodjeljenPredmet.Where(d=>d.PredmetId==t.PredmetId).OrderByDescending(d=>d.ZakljucnoKrajGodine).Select(d=>d.OdjeljenjeStavka.Ucenik.ImePrezime).FirstOrDefault(),
                      najboljaSkola=  
                      db.DodjeljenPredmet.Where(d=> odjeljenjeStavkas.Any(s=> s.Id==d.OdjeljenjeStavkaId)).OrderByDescending(d=>d.ZakljucnoKrajGodine).Select(d=>d.OdjeljenjeStavka.Odjeljenje.Skola.Naziv).FirstOrDefault(),
                      najboljeOdjeljenje= db.DodjeljenPredmet.Where(d => odjeljenjeStavkas.Any(s => s.Id == d.OdjeljenjeStavkaId)).OrderByDescending(d => d.ZakljucnoKrajGodine).Select(d => d.OdjeljenjeStavka.Odjeljenje.Oznaka).FirstOrDefault()
                }).ToList()
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult DodavanjeTakmicenje(int skolaID)
        {
       
            var model = new DodavanjeTakmicenjaVM
            {
                skolaID= skolaID,
                skolaNaziv= db.Skola.Where(s=>s.Id==skolaID).Select(s=>s.Naziv).FirstOrDefault(),
                predmet= db.Predmet.Select(p=> new SelectListItem()
                {
                    Value=p.Id.ToString(),
                    Text=p.Naziv
                }).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult DodavanjeTakmicenje(DodavanjeTakmicenjaVM temp)
        {

            Takmicenje takmicenje = new Takmicenje
            {
                DatumTakmicenja = temp.datum,
                PredmetId = temp.predmetID,
                Razred = temp.razredID,
                SkolaId = temp.skolaID,
                zakljucano= false
            };
            db.Add(takmicenje); db.SaveChanges();

            List<DodjeljenPredmet> dodjeljenPredmets = db.DodjeljenPredmet.Where(d => d.ZakljucnoKrajGodine == 5 && d.PredmetId==temp.predmetID).ToList();
            List<OdjeljenjeStavka> odjeljenjeStavkas = db.OdjeljenjeStavka.Where(o => dodjeljenPredmets.Any(d => d.OdjeljenjeStavkaId == o.Id)).ToList();
            foreach (var stavke in odjeljenjeStavkas)
            {
                if (db.DodjeljenPredmet.Where(d=>d.OdjeljenjeStavkaId==stavke.Id).Average(d=>d.ZakljucnoKrajGodine)>4)
                {
                    TakmicenjeUcesnik takmicenjeUcesnik = new TakmicenjeUcesnik
                    {
                        Bodovi = 0,
                        PristupioTakmicenju = false,
                        TakmicenjeId = takmicenje.Id,
                        UcenikId = stavke.UcenikId
                    };
                    db.Add(takmicenjeUcesnik); db.SaveChanges();
                }
            }
            return Redirect("/Takmicenje/Index");
        }
        public ActionResult PrikazDetaljaTakmicenja(int tID)
        {

            Takmicenje takmicenje = db.Takmicenje.Find(tID);
            var model = new DetaljiTakmicenjaVM
            {
                takmicenjeID= tID,
                skolaID= takmicenje.SkolaId,
                skolaNaziv= db.Skola.Where(s=>s.Id==takmicenje.SkolaId).Select(s=>s.Naziv).FirstOrDefault(), 
                predmet=db.Predmet.Where(p=>p.Id==takmicenje.PredmetId).Select(p=>p.Naziv).FirstOrDefault(), 
                predmetID= takmicenje.PredmetId,
                datum=takmicenje.DatumTakmicenja.ToString("dd.MM.yyyy"),
                razred= takmicenje.Razred
            };
            return View(model);

        }
        public ActionResult PrikazDetaljaTakmicenjeDetalji(int takmicenjeID)
        {
            Takmicenje takmicenje = db.Takmicenje.Find(takmicenjeID);
            var model = new PrikazDetaljaDetaljVM
            {
                takmicenjeID= takmicenjeID,
                podaciRezultat= db.TakmicenjeUcesnik.Where(t=>t.TakmicenjeId==takmicenjeID).Select(t=> new PrikazDetaljaDetaljVM.ROw()
                {
                    isZakljucano= takmicenje.zakljucano,
                    takmicenjeDetaljID= t.Id, 
                    bodovi= t.Bodovi,
                    pristupio= t.PristupioTakmicenju,
                    brojDnevnik= db.OdjeljenjeStavka.Where(s=>s.UcenikId==t.UcenikId).Select(s=>s.BrojUDnevniku).FirstOrDefault(), 
                    odjeljenjeNAziv= db.OdjeljenjeStavka.Where(s=>s.UcenikId==t.UcenikId).Select(s=>s.Odjeljenje.Oznaka).FirstOrDefault()
                }).ToList()
            };
            return PartialView(model);
        }
        public  ActionResult  MijenjanjeStatusa(int detaljiID)
        {
            TakmicenjeUcesnik takmicenjeUcesnik = db.TakmicenjeUcesnik.Find(detaljiID);
            if (takmicenjeUcesnik.PristupioTakmicenju == false)
            {
                takmicenjeUcesnik.PristupioTakmicenju = true;
            }
            else
            {
                takmicenjeUcesnik.PristupioTakmicenju = false;
            }
            db.SaveChanges();

            return Redirect("/Takmicenje/PrikazDetaljaTakmicenjeDetalji?takmicenjeID="+takmicenjeUcesnik.TakmicenjeId);
        }
        [HttpGet]
        public ActionResult DodavanjeUcesnikaNaTakmicenje(int takmicenjeID)
        {
            var model = new DodavanjeUceniskaVM
            {
                takmicenjeID = takmicenjeID,
                ucenik = db.Ucenik.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.ImePrezime
                }).ToList()
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult DodavanjeUcesnikaNaTakmicenje(DodavanjeUceniskaVM temp)
        {
            int takmicenje = temp.takmicenjeID;
            TakmicenjeUcesnik takmicenjeUcesnik = new TakmicenjeUcesnik
            {
                Bodovi = temp.bodovi,
                PristupioTakmicenju = true,
                TakmicenjeId = temp.takmicenjeID,
                UcenikId = temp.ucenikID
            };
            db.Add(takmicenjeUcesnik); db.SaveChanges();
            return Redirect("/Takmicenje/PrikazDetaljaTakmicenjeDetalji?takmicenjeID=" + takmicenje);

        }
        [HttpGet]
        public ActionResult UredjivaenjUcesnikaBodovi(int detalji)
        {
       
            TakmicenjeUcesnik takmicenjeUcesnik = db.TakmicenjeUcesnik.Find(detalji);
            var model = new UredjivanjeUcesnikaDetaljiVM {
                detaljiID = detalji,
                bodovi= takmicenjeUcesnik.Bodovi,
                ucenik = db.Ucenik.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.ImePrezime
                }).ToList()
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult UredjivaenjUcesnikaBodovi(UredjivanjeUcesnikaDetaljiVM temp)
        {
            TakmicenjeUcesnik takmicenjeUcesnik = db.TakmicenjeUcesnik.Find(temp.detaljiID);
            takmicenjeUcesnik.Bodovi = temp.bodovi;
            db.SaveChanges();
            return Redirect("/Takmicenje/PrikazDetaljaTakmicenjeDetalji?takmicenjeID=" + takmicenjeUcesnik.TakmicenjeId);
        }
        public ActionResult SnimanjeBodova(int detaljiiD, int  bodoovi)
        {
            TakmicenjeUcesnik takmicenjeUcesnik = db.TakmicenjeUcesnik.Find(detaljiiD);
            takmicenjeUcesnik.Bodovi = bodoovi;
            db.SaveChanges();
            return Redirect("/Takmicenje/PrikazDetaljaTakmicenjeDetalji?takmicenjeID=" + takmicenjeUcesnik.TakmicenjeId);
        }
        public ActionResult Zakljucaj(int takmicenjeID)
        {
            Takmicenje takmicenje = db.Takmicenje.Find(takmicenjeID);
            takmicenje.zakljucano = true;
            db.SaveChanges();
            return Redirect("/Takmicenje/Index");
        }

    }
}