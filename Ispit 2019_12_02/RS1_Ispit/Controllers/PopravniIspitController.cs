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
            var model = new OdjeljenjeSkolaPrikazVM
            {
                podaciOdjeljenje = db.Odjeljenje.Select(o => new OdjeljenjeSkolaPrikazVM.Row()
                {
                    odjeljenjeID = o.Id,
                    odjeljenjeOznaka = o.Oznaka,
                    skolaNaziv = db.Skola.Where(s => s.Id == o.SkolaID).Select(s => s.Naziv).FirstOrDefault(),
                    skolskaGOdina = db.SkolskaGodina.Where(s => s.Id == o.SkolskaGodinaID).Select(s => s.Naziv).FirstOrDefault()
                }).ToList()
            };
            return View(model);
        }
        public ActionResult PrikazPopravnogIspita(int odjeljenjeID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Find(odjeljenjeID);
            var model = new PopravniIspit_VM_Prikaz
            {
                odjeljenjeID = odjeljenjeID,
                odjeljenjeOznaka = odjeljenje.Oznaka,
                skolskaGodina = db.SkolskaGodina.Where(s => s.Id == odjeljenje.SkolskaGodinaID).Select(s => s.Naziv).FirstOrDefault(),
                skolskaGodinaID = db.SkolskaGodina.Where(s => s.Id == odjeljenje.SkolskaGodinaID).Select(s => s.Id).FirstOrDefault(),
                naszivSkole = db.Skola.Where(s => s.Id == odjeljenje.SkolaID).Select(s => s.Naziv).FirstOrDefault(),
                skolaID = db.Skola.Where(s => s.Id == odjeljenje.SkolaID).Select(s => s.Id).FirstOrDefault(),
                podaciPopravniIspit = db.PopravniIspit.Where(p => p.OdjeljenjeID == odjeljenjeID).Select(p => new PopravniIspit_VM_Prikaz.Row()
                {
                    popravniISppitID = p.Id,
                    datumPopravnogIspita = p.DatumIspita.ToString("dd.MM.yyyy"),

                    predmet = db.Predmet.Where(pp => pp.Id == p.PredmetID).Select(pp => pp.Naziv).FirstOrDefault(),
                    brojUcenikaNaPopravnomIspitu = db.PopravniIspitDetalji.Where(pp => pp.PopravniIspitID == p.Id).Count(),
                    brojUcenikaKojiSUPoloziliPopravniIspit = db.PopravniIspitDetalji.Where(pp => pp.PopravniIspitID == p.Id).Count(pp => pp.RezultatiMaturskogIspita > 50)
                }).ToList()
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult DodavanjePopravnogIspita(int odjeljenjeID, int skolaID, int skolskaGodinaID)
        {
            var model = new PopravniIspit_DOdavanje_VM
            {
                Predmet = db.Predmet.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Naziv
                }).ToList(),
                skolaID = skolaID,
                skolaNaziv = db.Skola.Where(s => s.Id == skolaID).Select(s => s.Naziv).FirstOrDefault(),
                odjeljenjeID = odjeljenjeID,
                odjeljenjeOznaka = db.Odjeljenje.Where(o => o.Id == odjeljenjeID).Select(o => o.Oznaka).FirstOrDefault(),
                skolskaGOdinaINT = skolskaGodinaID,
                skolskaGOdinaNaziv = db.SkolskaGodina.Where(s => s.Id == skolskaGodinaID).Select(s => s.Naziv).FirstOrDefault()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult DodavanjePopravnogIspita(PopravniIspit_DOdavanje_VM temp)
        {
            //dodavanje popravnog:
            PopravniIspit popravniIspit = new PopravniIspit
            {
                DatumIspita = temp.datumPopravnogIspita,
                OdjeljenjeID = temp.odjeljenjeID,
                PredmetID = temp.PredmetID,
                SkolaID = temp.skolaID,
                SkolskaGodinaID = temp.skolskaGOdinaINT
            };
            db.Add(popravniIspit); db.SaveChanges();

            List<DodjeljenPredmet> dodijeljeniPredmet = db.DodjeljenPredmet.Where(d => d.PredmetId == temp.PredmetID && d.ZakljucnoKrajGodine == 1).ToList();
            List<OdjeljenjeStavka> odjeljenjeStavka = db.OdjeljenjeStavka.Where(os => dodijeljeniPredmet.Any(dd => dd.OdjeljenjeStavkaId == os.Id)).ToList();

            foreach (var stavke in odjeljenjeStavka)
            {
                if (db.DodjeljenPredmet.Where(d => d.OdjeljenjeStavkaId == stavke.Id).Count(d => d.ZakljucnoKrajGodine == 1) >= 3) //ako imaju negativne 1 ili vise
                {
                    PopravniIspitDetalji popravniIspitDetalji = new PopravniIspitDetalji
                    {
                        imePravoPristupa = false,
                        isPristupio = false,
                        OdjeljenjeStavkaId = stavke.Id,
                        PopravniIspitID = popravniIspit.Id,
                        RezultatiMaturskogIspita = 0
                    };
                    db.Add(popravniIspitDetalji); db.SaveChanges();
                }
                else
                {
                    PopravniIspitDetalji popravniIspitDetalji = new PopravniIspitDetalji
                    {
                        imePravoPristupa = true,
                        isPristupio = false,
                        OdjeljenjeStavkaId = stavke.Id,
                        PopravniIspitID = popravniIspit.Id,
                        RezultatiMaturskogIspita = 0
                    };
                    db.Add(popravniIspitDetalji); db.SaveChanges();
                }
            }
            return Redirect("/PopravniIspit/PrikazPopravnogIspita?odjeljenjeID=" + temp.odjeljenjeID);
        }

        public ActionResult UrediPopravni(int popravniID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Find(popravniID);
            var model = new PopravniIspit_VM_Uredi
            {
                popravniID = popravniID,
                predmetNaziv = db.Predmet.Where(p => p.Id == popravniIspit.PredmetID).Select(p => p.Naziv).FirstOrDefault(),
                datumIspita = popravniIspit.DatumIspita.ToString("dd.MM.yyyy"),
                skolaNaziv = db.Skola.Where(p => p.Id == popravniIspit.SkolaID).Select(p => p.Naziv).FirstOrDefault(),
                skolskaGOdina = db.SkolskaGodina.Where(p => p.Id == popravniIspit.SkolskaGodinaID).Select(p => p.Naziv).FirstOrDefault(),
                odjeljenjNaziv = db.Odjeljenje.Where(p => p.Id == popravniIspit.OdjeljenjeID).Select(p => p.Oznaka).FirstOrDefault()
            };
            return View(model);
        }
        public ActionResult DetaljiPopravniPrikaz(int popravniIspitID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Find(popravniIspitID);
            var model = new PopravniDetalji_VM_Prikaz
            {
                podaciDetaljiPopravni = db.PopravniIspitDetalji.Where(p => p.PopravniIspitID == popravniIspitID).Select(p => new PopravniDetalji_VM_Prikaz.Row()
                {
                    popravniDetaljiID = p.Id,
                    ucenikImePrezime = db.OdjeljenjeStavka.Where(os => os.Id == p.OdjeljenjeStavkaId).Select(os => os.Ucenik.ImePrezime).FirstOrDefault(),
                    brojUDnevniku = db.OdjeljenjeStavka.Where(os => os.Id == p.OdjeljenjeStavkaId).Select(os => os.BrojUDnevniku).FirstOrDefault(),
                    odjeljenjeNaziv = db.Odjeljenje.Where(o => o.Id == p.OdjeljenjeStavka.OdjeljenjeId).Select(o => o.Oznaka).FirstOrDefault(),
                    imaPravoPristupa = p.imePravoPristupa,
                    prisutan = p.isPristupio,
                    rezultatMaturskog = p.RezultatiMaturskogIspita
                }).ToList()
            };
            return PartialView(model);
        }
        public ActionResult MijenjenjePrisutnosti(int detaljiID)
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
            int popravniID = popravniIspitDetalji.PopravniIspitID;
            return Redirect("/PopravniIspit/DetaljiPopravniPrikaz?popravniIspitID="+popravniID);
        }

        public ActionResult MijenjanjeBodova(int detaljiID)
        {
            PopravniIspitDetalji popravniIspitDetalji = db.PopravniIspitDetalji.Find(detaljiID);
            var model = new UredivanjeBodova_VM
            {
                detaljiID= detaljiID,
                ucenikIme= db.OdjeljenjeStavka.Where(os=>os.Id==popravniIspitDetalji.OdjeljenjeStavkaId).Select(os=>os.Ucenik.ImePrezime).FirstOrDefault(), 
                Bodovi= popravniIspitDetalji.RezultatiMaturskogIspita
            };
            return PartialView(model);
        }
        public ActionResult SnimanjeBodova(int detaljiID, int Bodovi)
        {
            PopravniIspitDetalji popravniIspitDetalji = db.PopravniIspitDetalji.Find(detaljiID);
            popravniIspitDetalji.RezultatiMaturskogIspita = Bodovi;
            db.SaveChanges();
            int popravni = popravniIspitDetalji.PopravniIspitID;
            return Redirect("/PopravniIspit/DetaljiPopravniPrikaz?popravniIspitID=" + popravni);
        }
    }
}