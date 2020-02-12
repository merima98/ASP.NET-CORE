using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class MaturskiIspitController : Controller
    {
        private MojContext db;

        public MaturskiIspitController(MojContext context)
        {
            db = context;
        }


        public IActionResult Index()
        {
            var model = new PrikazNastavnikaVM()
            {
                NastavnikID = db.Nastavnik.Select(r => r.Id).FirstOrDefault(),
                
                NastavniciPodaci = db.Odjeljenje.Select(n=> new PrikazNastavnikaVM.Row() { 
                NastavnikImePrezime= db.Nastavnik.Where(s=>s.Id== n.RazrednikID).Select(r=>r.Ime+r.Prezime).FirstOrDefault(), 
                SkolaNaziv = db.Skola.Where(s=>s.Id==n.SkolaID).Select(s=>s.Naziv).FirstOrDefault()
                }).ToList()
            };
            return View(model);
        }
        public ActionResult MaturskiIspitPrikaz(int nastavnikID) {

            var model = new MaturskiIspitPrikazVM()
            {
                NastavnikID= nastavnikID, 
                MaturskiPodaci = db.MaturskiIspit.Where(n=>n.NastavnikID==nastavnikID).Select(m=> new MaturskiIspitPrikazVM.Row()
                {
                    matuskiIspitID= m.Id, 
                    datumIspita= m.DatumMaturskogIspita.ToString("dd.MM.yyyy"), 
                    skolaNaziv = db.Skola.Where(s=>s.Id==m.SkolaID).Select(s=>s.Naziv).FirstOrDefault(), 
                    Predmet = db.Predmet.Where(p=>p.Id==m.PredmetID).Select(p=>p.Naziv).FirstOrDefault(), 
                    uceniciNisuPristupili= db.MaturskiIpitDetalji.Where(md=>md.MaturskiIspitID==m.Id && md.isPristupuoIspitu==false).Select(md=>md.OdjeljenjeStavka.Ucenik.ImePrezime).ToList()
                }).ToList()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult DodavanjeMaturskog(int nastavnikID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Where(n => n.RazrednikID == nastavnikID).FirstOrDefault();
            var model = new MaturskiIspitAdd_VM()
            {
                NastavnikID = nastavnikID,
                Skola = db.Skola.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Naziv
                }).ToList(),
                NastavnikImePrezime = db.Nastavnik.Where(n => n.Id == nastavnikID).Select(n => n.Ime + n.Prezime).FirstOrDefault(),
                SkolskaGodina = db.SkolskaGodina.Where(n => n.Id == odjeljenje.SkolskaGodinaID).Select(o => o.Naziv).FirstOrDefault(),
                SkolskaGodinaID = db.SkolskaGodina.Where(n => n.Id == odjeljenje.SkolskaGodinaID).Select(o => o.Id).FirstOrDefault(),
                Predmet = db.Predmet.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Naziv
                }).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult DodavanjeMaturskog(MaturskiIspitAdd_VM temp)
        {
            //dodavanje matuskog: 
            MaturskiIspit maturskiIspit = new MaturskiIspit()
            {
                NastavnikID = temp.NastavnikID,
                DatumMaturskogIspita = temp.DatumMaturskogIspita,
                PredmetID = temp.PredmetID,
                SkolaID = temp.SkolaID,
                SkolskaGodinaID = temp.SkolskaGodinaID
            };
            db.Add(maturskiIspit); db.SaveChanges();

            List<Odjeljenje> odjeljenjes = db.Odjeljenje.Where(s => s.SkolaID == temp.SkolaID && s.Razred==4).ToList();
            List<OdjeljenjeStavka> odjeljenjeStavke = db.OdjeljenjeStavka.Where(s => odjeljenjes.Any(o => o.Id == s.OdjeljenjeId)).ToList();

            foreach (var stavke in odjeljenjeStavke)
            {
                if(db.DodjeljenPredmet.Where(d=>d.OdjeljenjeStavkaId==stavke.Id).Count(d=>d.ZakljucnoKrajGodine==1)==0 || db.MaturskiIpitDetalji.Where(m=>m.OdjeljenjeStavkaID==stavke.Id).Count(m=>m.RezultatiMaturskog<55)!=0)
                {
                    MaturskiIpitDetalji maturskiIpitDetalji = new MaturskiIpitDetalji()
                    {
                        isPristupuoIspitu=true, 
                        MaturskiIspitID= maturskiIspit.Id, 
                        OdjeljenjeStavkaID= stavke.Id, 
                        RezultatiMaturskog= 0 //jer cemo postaviti da nema jos poena, kako nije polozio prethodni ispit
                    };
                    db.Add(maturskiIpitDetalji); db.SaveChanges();
                }
            }
            int nastavnikaID = maturskiIspit.NastavnikID;
            return Redirect("/MaturskiIspit/MaturskiIspitPrikaz?nastavnikID=" + nastavnikaID);
        }

        public ActionResult UrediMaturski(int maturskiID)
        {
 
            MaturskiIspit maturski = db.MaturskiIspit.Find(maturskiID);
            var model = new MaturskiIspit_UrediVM()
            {
                MaturskiID = maturskiID,
                DatumMaturskog = maturski.DatumMaturskogIspita.ToString("dd.MM.yyyy"),
                PredmetNaziv = db.Predmet.Where(p => p.Id == maturski.PredmetID).Select(p => p.Naziv).FirstOrDefault(),
                Napomena = maturski.Napomena
            };

            return View(model);
        }
        public ActionResult SnimanjeMaturskog(int MaturskiID, string Napomena)
        {
            MaturskiIspit maturskiIspit = db.MaturskiIspit.Find(MaturskiID);
            maturskiIspit.Napomena = Napomena;
            db.SaveChanges();
            int nastavnikID = maturskiIspit.NastavnikID;
            return Redirect("/MaturskiIspit/MaturskiIspitPrikaz?nastavnikID=" + nastavnikID);
        }
        public ActionResult DetaljiMaturskogIspita(int maturskiID)
        {
            MaturskiIspit maturskiIspit = db.MaturskiIspit.Find(maturskiID);


            var model = new DetaljiMaturskiIspit_VM()
            {
                DetaljiPrikaz    = db.MaturskiIpitDetalji.Where(m=>m.MaturskiIspitID==maturskiID).Select(m=> new DetaljiMaturskiIspit_VM.Row()
                {
                    detaljiID= m.Id, 
                    ucenikImePrezime = db.OdjeljenjeStavka.Where(o=>o.Id==m.OdjeljenjeStavkaID).Select(o=>o.Ucenik.ImePrezime).FirstOrDefault(),
                     prosjekOcjena= db.DodjeljenPredmet.Where(d=>d.OdjeljenjeStavkaId==m.OdjeljenjeStavkaID).Average(d=>(int?)d.ZakljucnoKrajGodine)??0, 
                     pristupioIspitu= m.isPristupuoIspitu, 
                     rezultatMaturskog= m.RezultatiMaturskog
                }).ToList()
            };
            return PartialView(model);
        }

        public ActionResult UrediBodove(int detaljiID)
        {
            //public int detaljiID { get; set; }
            //public string UcenikImePrezime { get; set; }
            //public int bodovi { get; set; }
            MaturskiIpitDetalji maturskiIpitDetalji = db.MaturskiIpitDetalji.Find(detaljiID);
            var model = new UrediBodove_VM()
            {
                detaljiID = detaljiID,
                UcenikImePrezime = db.OdjeljenjeStavka.Where(o => o.Id == maturskiIpitDetalji.OdjeljenjeStavkaID).Select(o => o.Ucenik.ImePrezime).FirstOrDefault(),
                bodovi = maturskiIpitDetalji.RezultatiMaturskog
            };
            return PartialView(model);
        }
        public ActionResult SnimanjeBodova(int detaljiID, int bodovi)
        {
     
            MaturskiIpitDetalji maturskiIpitDetalji = db.MaturskiIpitDetalji.Find(detaljiID);
            maturskiIpitDetalji.RezultatiMaturskog = bodovi;
            db.SaveChanges();
            int maturskiID = maturskiIpitDetalji.MaturskiIspitID;
            return Redirect("/MaturskiIspit/DetaljiMaturskogIspita?maturskiID=" + maturskiID);
        }
        public ActionResult PromjenaPrisutnosti(int detaljiID)
        {
            MaturskiIpitDetalji maturskiIpitDetalji = db.MaturskiIpitDetalji.Find(detaljiID);
            if (maturskiIpitDetalji.isPristupuoIspitu==true)
            {
                maturskiIpitDetalji.isPristupuoIspitu = false;
            }
            else
            {
                maturskiIpitDetalji.isPristupuoIspitu = true;
            }
            db.SaveChanges();
            int maturskiID = maturskiIpitDetalji.MaturskiIspitID;
            return Redirect("/MaturskiIspit/DetaljiMaturskogIspita?maturskiID=" + maturskiID);
        }
    }
}