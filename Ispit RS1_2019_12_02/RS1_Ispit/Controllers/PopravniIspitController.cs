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
    public class PopravniIspitController : Controller
    {
        private MojContext db;
        public PopravniIspitController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = new PrikazSkolaSkGOdinaOdjeljenjeVM
            {
                podaciDetalji= db.Odjeljenje.Select(o => new PrikazSkolaSkGOdinaOdjeljenjeVM.ROw()
                {
                    skolaID= db.Skola.Where(s=>s.Id==o.SkolaID).Select(s=>s.Id).FirstOrDefault(),
                    skolaNaziv = db.Skola.Where(s => s.Id == o.SkolaID).Select(s => s.Naziv).FirstOrDefault(),
                    skGOdinaID = db.SkolskaGodina.Where(s => s.Id == o.SkolaID).Select(s => s.Id).FirstOrDefault(),
                    skGOdinaNaziv = db.SkolskaGodina.Where(s => s.Id == o.SkolaID).Select(s => s.Naziv).FirstOrDefault(),
                    odjeljenjeID= o.Id,
                    odjeljenjeNaziv = o.Oznaka
                }).ToList()
            };
            return View(model);
        }

        public ActionResult PrikazPopravnog(int odjeljenjeID, int skolaID, int skGOdinaID)
        {
         
            var model = new PopravniIspit_VM_PRIKAZ
            {
                odjeljenjeID= odjeljenjeID,
                odjeljenjeNAziv= db.Odjeljenje.Where(o=>o.Id==odjeljenjeID).Select(o=>o.Oznaka).FirstOrDefault(),
                skGodinaID= skGOdinaID,
                skGOdinaNAziv = db.SkolskaGodina.Where(o => o.Id == skGOdinaID).Select(o => o.Naziv).FirstOrDefault(),
                skolaID= skolaID,
                skolaNaziv = db.SkolskaGodina.Where(o => o.Id == skolaID).Select(o => o.Naziv).FirstOrDefault(),

                detaljiPopravniPrikaz= db.PopravniIspit.Where(p=>p.OdjeljenjeId==odjeljenjeID).Select(p=> new PopravniIspit_VM_PRIKAZ.ROw()
                {
                    popravniID= p.Id,
                    datumPopravnog= p.DatumPopravnogIspita.ToString("dd.MM.yyyy"),
                    predmet= db.Predmet.Where(pr=>pr.Id==p.PredmetId).Select(pr=>pr.Naziv).FirstOrDefault(),
                    brojPolozili= db.PopravniIspitDetalji.Where(pr=>pr.PopravniIspitId==p.Id).Count(pr=>pr.rezultatiMaturskogBodovi>50),
                    brojUcenikaNaPopravnomIspitu = db.PopravniIspitDetalji.Count(pr => pr.PopravniIspitId == p.Id)
                }).ToList()

            };
            return View(model);
        }

        [HttpGet]
        public ActionResult DodavanjePopravnig(int sskolaID, int odjljenjeId, int skGOdinaID)
        {
            var model = new PopravniISpit_VM_Dodavnje
            {
                Predmet = db.Predmet.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Naziv
                }).ToList(),
                odjeljenjeId = odjljenjeId,
                odjeljenjeNaziv = db.Odjeljenje.Where(o => o.Id == odjljenjeId).Select(o => o.Oznaka).FirstOrDefault(),
                skGOdinaID = skGOdinaID,
                skGOdinaNaziv = db.SkolskaGodina.Where(o => o.Id == skGOdinaID).Select(o => o.Naziv).FirstOrDefault(),
                skolaID= sskolaID,
                skolaNaziv = db.SkolskaGodina.Where(o => o.Id == sskolaID).Select(o => o.Naziv).FirstOrDefault()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult DodavanjePopravnig(PopravniISpit_VM_Dodavnje temp)
        {
            PopravniIspit popravniIspit = new PopravniIspit
            {
                DatumPopravnogIspita = temp.datumPopravnog,
                OdjeljenjeId = temp.odjeljenjeId,
                PredmetId = temp.PredmetID,
                SkolaId = temp.skolaID,
                SkolskaGodinaId = temp.skGOdinaID
            };
            db.Add(popravniIspit); db.SaveChanges();

            List<DodjeljenPredmet> dodjeljenPredmets = db.DodjeljenPredmet.Where(d => d.PredmetId == temp.PredmetID && d.ZakljucnoKrajGodine == 1).ToList();

            List<OdjeljenjeStavka> odjeljenjeStavkas = db.OdjeljenjeStavka.Where(s => dodjeljenPredmets.Any(d => d.OdjeljenjeStavkaId == s.Id)).ToList();

            foreach (var stavke in odjeljenjeStavkas)
            {
                if (db.DodjeljenPredmet.Where(d => d.OdjeljenjeStavkaId == stavke.Id).Count(d => d.ZakljucnoKrajGodine == 1) >= 3)
                {
                    PopravniIspitDetalji popravniIspitDetalji = new PopravniIspitDetalji
                    {
                        imaPravoPristupa = false,
                        isPristupio = false,
                        PopravniIspitId = popravniIspit.Id,
                        rezultatiMaturskogBodovi = 0,
                        UcenikId = stavke.UcenikId
                    };
                    db.Add(popravniIspitDetalji); db.SaveChanges();
                }
                else
                {
                    PopravniIspitDetalji popravniIspitDetalji = new PopravniIspitDetalji
                    {
                        imaPravoPristupa = true,
                        isPristupio = false,
                        PopravniIspitId = popravniIspit.Id,
                        rezultatiMaturskogBodovi = 0,
                        UcenikId = stavke.UcenikId
                    };
                    db.Add(popravniIspitDetalji); db.SaveChanges();
                }
            }

            return Redirect("/PopravniIspit/PrikazPopravnog?odjeljenjeID="+temp.odjeljenjeId+ "&skolaID="+temp.skolaID+ "&skGOdinaID="+temp.skolaID);
        }

        public ActionResult UredivanjePopravnig(int popravniID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Find(popravniID);
            var model = new UredjivanjePopravnog_VM_Prikaz
            {
                popravniID  = popravniID,
                PredmetID= popravniIspit.PredmetId,
                PredmeNaziv= db.Predmet.Where(p=>p.Id==popravniIspit.PredmetId).Select(p=>p.Naziv).FirstOrDefault(),
                datumPopravnog= popravniIspit.DatumPopravnogIspita.ToString("dd.MM.yyyy"),
                odjeljenjeId= popravniIspit.OdjeljenjeId,
                odjeljenjeNaziv= db.Odjeljenje.Where(p => p.Id == popravniIspit.OdjeljenjeId).Select(p => p.Oznaka).FirstOrDefault(),
                skGOdinaID= popravniIspit.SkolskaGodinaId,
                skGOdinaNaziv= db.SkolskaGodina.Where(p => p.Id == popravniIspit.SkolskaGodinaId).Select(p => p.Naziv).FirstOrDefault(),
                skolaID= popravniIspit.SkolaId,
                skolaNaziv= db.Skola.Where(p => p.Id == popravniIspit.SkolaId).Select(p => p.Naziv).FirstOrDefault()
            };
            return View(model);
        }
        public ActionResult PrikazDetaljaPopravnog(int popravniID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Find(popravniID);
            var model = new PopravvvniIspitDeetalji_VM_Prikaz
            {
                detaljiPrikaz   = db.PopravniIspitDetalji.Where(p=>p.PopravniIspitId==popravniID).Select(p=> new PopravvvniIspitDeetalji_VM_Prikaz.Row() { 
                detaljID= p.Id,
                ucenikIme= db.Ucenik.Where(u=>u.Id==p.UcenikId).Select(u=>u.ImePrezime).FirstOrDefault(),
                odjeljenjeIme= db.Odjeljenje.Where(o=>o.Id==popravniIspit.OdjeljenjeId).Select(o=>o.Oznaka).FirstOrDefault(),
                brojBodova= p.rezultatiMaturskogBodovi,
                brojUDnevniku= db.OdjeljenjeStavka.Where(os=>os.UcenikId==p.UcenikId).Select(ps=>ps.BrojUDnevniku).FirstOrDefault(),
                imaPravoPristupa= p.imaPravoPristupa,
                pristupio=p.isPristupio
                }).ToList()
            };
            return PartialView(model);
        }

        public ActionResult MijenjanjePristupa(int detaljiID)
        {
            PopravniIspitDetalji popravniIspitDetalji = db.PopravniIspitDetalji.Find(detaljiID);

            if (popravniIspitDetalji.isPristupio == false)
            {
                popravniIspitDetalji.isPristupio = true;
            }
            else
            {
                popravniIspitDetalji.isPristupio = false;
            }
            db.SaveChanges();

            int popravniID = popravniIspitDetalji.PopravniIspitId;

            return Redirect("/PopravniIspit/PrikazDetaljaPopravnog?popravniID=" + popravniID);
        }

        public ActionResult MijenjanjeBodova(int detaljiID)
        {

            PopravniIspitDetalji popravniIspitDetalji = db.PopravniIspitDetalji.Find(detaljiID);

            var model = new UredjivanjeUcenikBOdovi
            {
                detaljiID = detaljiID,
                bodovi = db.PopravniIspitDetalji.Where(p => p.Id == detaljiID).Select(p => p.rezultatiMaturskogBodovi).FirstOrDefault(),
                ucenikIme = db.Ucenik.Where(u => u.Id == popravniIspitDetalji.UcenikId).Select(u => u.ImePrezime).FirstOrDefault()
            };
            return PartialView(model);
        }

        public ActionResult SnimanjeBodova(int detaljiID, int bodovi)
        {

            PopravniIspitDetalji popravniIspitDetalji = db.PopravniIspitDetalji.Find(detaljiID);

            popravniIspitDetalji.rezultatiMaturskogBodovi = bodovi;
            db.SaveChanges();
            int popravniID = popravniIspitDetalji.PopravniIspitId;

            return Redirect("/PopravniIspit/PrikazDetaljaPopravnog?popravniID=" + popravniID);
        }
    }
}