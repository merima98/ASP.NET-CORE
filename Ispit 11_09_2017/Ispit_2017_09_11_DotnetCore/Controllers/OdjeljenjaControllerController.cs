using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.EntityModels;
using Ispit_2017_09_11_DotnetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ispit_2017_09_11_DotnetCore.Controllers
{
    public class OdjeljenjaController : Controller
    {
        private MojContext db;
        public OdjeljenjaController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = new Odjeljenje_Prikaz_VM()
            {
                OdjeljenjePodaci = db.Odjeljenje.Select(o => new Odjeljenje_Prikaz_VM.Row()
                {
                    OdjeljenjeID= o.Id, 
                    skolskaGodina= o.SkolskaGodina, 
                    Razred= o.Razred, 
                    oznakaOdjeljenja= o.Oznaka, 
                    NastavnikImePrezime= db.Nastavnik.Where(n=>n.NastavnikID==o.NastavnikID).Select(n=>n.ImePrezime).FirstOrDefault(), 
                    isPrebacenUViseOdjeljenje= o.IsPrebacenuViseOdjeljenje, 
                    ProsjekOcjena= db.DodjeljenPredmet.Where(d=>d.OdjeljenjeStavka.OdjeljenjeId==o.Id).Average(d=>(int?)d.ZakljucnoKrajGodine)??0,
                    najboljiUcenik = db.OdjeljenjeStavka.Where(s => s.OdjeljenjeId == o.Id).Select(s => s.DodijeljeniPredmetStavke.OrderByDescending(d => d.ZakljucnoKrajGodine).Select(d => d.OdjeljenjeStavka.Ucenik.ImePrezime).FirstOrDefault()).FirstOrDefault()
                    //najboljiUcenik ="???"
                }).ToList()
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult DodavanjeOdjeljenja()
        {
            var model = new OdjeljenjeADD_VM()
            {
                Nastavnik = db.Nastavnik.Select(o => new SelectListItem
                {
                    Value = o.NastavnikID.ToString(),
                    Text = o.ImePrezime
                }).ToList(),
                NizeOdjeljenje = db.Odjeljenje.Where(p=>p.IsPrebacenuViseOdjeljenje==false).Select(o=> new SelectListItem
                {
                    Value= o.Id.ToString(), 
                    Text=$"{o.SkolskaGodina}, {o.Oznaka}"
                }).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult DodavanjeOdjeljenja(OdjeljenjeADD_VM temp)
        {
            //dodavanje odjeljenja:
            Odjeljenje odjeljenje = new Odjeljenje()
            {
                IsPrebacenuViseOdjeljenje = false,
                NastavnikID = temp.NastavnikID,
                Oznaka = temp.OznakaOdjeljenja,
                Razred = temp.Razred,
                SkolskaGodina = temp.SkolskaGodina
            };
            db.Add(odjeljenje); db.SaveChanges();
            
            //ako korisnik klikne na nize odjeljenje:
            if(temp.NizeOdjeljenjeID>0)
            {
                int brojUDnevniku = 0;
                List<OdjeljenjeStavka> odjeljenjeStavka = db.OdjeljenjeStavka.Where(o => o.OdjeljenjeId == odjeljenje.Id).ToList();

                foreach (var stavke in odjeljenjeStavka)
                {
                    if (db.DodjeljenPredmet.Where(d=>d.OdjeljenjeStavkaId==stavke.Id).Count(d=>d.ZakljucnoKrajGodine==1)==0) //na ovaj nacin cu provjeriti da li je pozitivan uspjeh
                    {
                        OdjeljenjeStavka odjeljenjStavke2 = new OdjeljenjeStavka()
                        {
                            BrojUDnevniku = ++brojUDnevniku,
                            OdjeljenjeId = odjeljenje.Id,
                            UcenikId = stavke.UcenikId
                        };
                        db.Add(odjeljenjStavke2); db.SaveChanges();
                       
                    }
                }
                odjeljenje.IsPrebacenuViseOdjeljenje = true;
                db.SaveChanges();
            }
            return Redirect("/Odjeljenja/Index");
        }
        public ActionResult DetaljiOdjeljenja(int odjeljenjeID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Find(odjeljenjeID);
            var model = new OdjeljenjeDetalji_VM()
            {
                OdjeljenjeID= odjeljenjeID, 
                skolskaGodina= odjeljenje.SkolskaGodina, 
                razred= odjeljenje.Razred, 
                oznaka= odjeljenje.Oznaka, 
                nastavnikaImePrezime= db.Nastavnik.Where(n=>n.NastavnikID==odjeljenje.NastavnikID).Select(n=>n.ImePrezime).FirstOrDefault(), 
                brojRazreda= db.Predmet.Count(p=>p.Razred==odjeljenje.Razred)
            };

            return View(model);
        }
        public ActionResult BrisanjeOdjeljenja(int odjeljenjeID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Find(odjeljenjeID);
            db.Remove(odjeljenje);
            db.SaveChanges();
            return Redirect("/Odjeljenja/Index");
        }
        public ActionResult PrikazOdjeljenjeStavke(int odjeljenjeID)
        {
       
            Odjeljenje odjeljenje = db.Odjeljenje.Find(odjeljenjeID);
            var model = new OdjeljenjeStavkeDetaljit_PrikazVM()
            {
                odjeljenjeID= odjeljenjeID  ,
                podaciDetalji = db.OdjeljenjeStavka.Where(o=>o.OdjeljenjeId==odjeljenje.Id).Select(o=> new OdjeljenjeStavkeDetaljit_PrikazVM.Row()
                {
                    detaljiID= o.Id,
                    brojUDnevniku= o.BrojUDnevniku,
                    ucenikImePrezime= db.Ucenik.Where(u=>u.Id==o.UcenikId).Select(u=>u.ImePrezime).FirstOrDefault(),
                    brojZakljucenihOcjena= db.DodjeljenPredmet.Count(d=>d.OdjeljenjeStavkaId==o.Id)
                }).ToList()
            };
            return PartialView(model);
        }
        [HttpGet]
        public ActionResult DodavanjeUcenikaUOdjeljenje(int odjeljenjeID)
        {
            var model = new DodavanjeUcenikaVM()
            {
                odjeljenjeID = odjeljenjeID,
                //detaljiID= db.OdjeljenjeStavka.Where(o=>o.OdjeljenjeId==odjeljenjeID).Select(o=>o.Id).FirstOrDefault(),
                Ucenik = db.Ucenik.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.ImePrezime
                }).ToList(),
                BrojUDnevniku = db.OdjeljenjeStavka.Where(o => o.OdjeljenjeId == odjeljenjeID).Count(o => o.BrojUDnevniku > 0) + 1
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult SnimanjeUcenika(int odjeljenjeID, int detaljiID, int ucenikID, int BrojUDnevniku)
        {
         
            OdjeljenjeStavka odjeljenjeStavka;
            if (detaljiID == 0)
            {
                //ako tek dodajemo:
                odjeljenjeStavka = new OdjeljenjeStavka();
                db.OdjeljenjeStavka.Add(odjeljenjeStavka);
            }
            else
            {
                odjeljenjeStavka = db.OdjeljenjeStavka.Find(detaljiID);
            }

            odjeljenjeStavka.BrojUDnevniku = BrojUDnevniku;
            odjeljenjeStavka.OdjeljenjeId = odjeljenjeID;
            odjeljenjeStavka.UcenikId = ucenikID;

            db.SaveChanges();
            return Redirect("/Odjeljenja/PrikazOdjeljenjeStavke?odjeljenjeID="+odjeljenjeID);
        }

        public ActionResult BrisanjeDetalja(int detaljiID )
        {
            OdjeljenjeStavka odjeljenjeStavka = db.OdjeljenjeStavka.Find(detaljiID);
            int odjeljenjeID = odjeljenjeStavka.OdjeljenjeId;
            db.Remove(odjeljenjeStavka); db.SaveChanges();
            return Redirect("/Odjeljenja/PrikazOdjeljenjeStavke?odjeljenjeID=" + odjeljenjeID);
        }

        public ActionResult Uredi(int odjeljenjeStavkeID) //kada imam Uredjivanje na ovakav nacin, potrebno je da drugi put proslijedim OdjeljenjeStavkeID, te da preucmjerim partitialView na Dodavanje, jer ce mi se tu automatski pozivati i snimanje..
        {
            OdjeljenjeStavka odjeljenjeStavka = db.OdjeljenjeStavka.Find(odjeljenjeStavkeID);
            var model = new DodavanjeUcenikaVM()
            {
                odjeljenjeID = odjeljenjeStavka.OdjeljenjeId,
                detaljiID = odjeljenjeStavka.Id,
                UcenikID = odjeljenjeStavka.UcenikId,
                Ucenik = db.Ucenik.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.ImePrezime
                }).ToList(),
                BrojUDnevniku = odjeljenjeStavka.BrojUDnevniku
            };
            return PartialView("DodavanjeUcenikaUOdjeljenje", model);
        }
    }
}