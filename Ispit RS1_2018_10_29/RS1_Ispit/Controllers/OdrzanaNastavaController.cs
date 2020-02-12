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
    public class OdrzanaNastavaController : Controller
    {
        private MojContext db;
        public OdrzanaNastavaController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = new PrikazSkolaNastavnik
            {
                podaciPrikaz= db.Odjeljenje.Select(s=> new PrikazSkolaNastavnik.Row()
                {
                    skolaID= db.Skola.Where(o=>o.Id==s.SkolaID).Select(o=>o.Id).FirstOrDefault(),
                    skolaNaziv= db.Skola.Where(o => o.Id == s.SkolaID).Select(o => o.Naziv).FirstOrDefault(),
                    nastavnikImePrezime = db.Nastavnik.Where(n=>n.Id==s.RazrednikID).Select(n=> n.Ime+n.Prezime).FirstOrDefault(),
                    nastavnikID= db.Nastavnik.Where(n => n.Id == s.RazrednikID).Select(n=> n.Id).FirstOrDefault()
                }).ToList()
            };

            return View(model);
        }
        public ActionResult PrikazCasova(int nastavnikID, int skolaID)
        {

            var model = new OdrzaniCasovi_VM_Prikaz
            {
                nastavnikID= db.Nastavnik.Where(od => od.Id == nastavnikID).Select(od => od.Id).FirstOrDefault(),
                nastavnikIme = db.Nastavnik.Where(od => od.Id == nastavnikID).Select(od => od.Ime+od.Prezime).FirstOrDefault(),
                skolaNaziv = db.Skola.Where(od => od.Id == skolaID).Select(od => od.Naziv).FirstOrDefault(),

                listaPrikaza = db.OdrzaniCas.Where(n => n.NastavnikID == nastavnikID).Select(o => new OdrzaniCasovi_VM_Prikaz.ROw()
                {
                    odrzaniCasID = o.Id,
                    datumCasa = o.DatumOdrzanogCasa.ToString("dd.MM.yyyy"),
                    odjeljenje = db.Odjeljenje.Where(od => od.Id == o.OdjeljenjeID).Select(od => od.Oznaka).FirstOrDefault(),
                    predmet = db.Predmet.Where(od => od.Id == o.PredmetId).Select(od => od.Naziv).FirstOrDefault(),
                    skolskaGOdin = db.SkolskaGodina.Where(od => od.Id == o.Odjeljenje.SkolskaGodinaID).Select(od => od.Naziv).FirstOrDefault(),
                    odsutniUcenici = db.OdrzanCasDetalji.Where(oc => oc.OdrzaniCasID == o.Id && oc.Prisutan == false).Select(oc => oc.Ucenik.ImePrezime).ToList()
                }).ToList()
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult DodavanjeCasa(int nastavnikID)
        {

            var model = new DodavanjeCasaVM
            {
                nastavnikID= nastavnikID,
                nastavnikIme= db.Nastavnik.Where(od => od.Id == nastavnikID).Select(od => od.Ime + od.Prezime).FirstOrDefault(),
                odjeljenjePredmet= db.PredajePredmet.Select(p=> new SelectListItem
                {
                    Value= p.Id.ToString(),
                    Text= $"{db.Odjeljenje.Where(o=>o.Id==p.OdjeljenjeID).Select(o=>o.Oznaka).FirstOrDefault()} / {db.Predmet.Where(o => o.Id == p.PredmetID).Select(o => o.Naziv).FirstOrDefault()}"
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult DodavanjeCasa(DodavanjeCasaVM temp)
        {
            PredajePredmet predajePredmet = db.PredajePredmet.Find(temp.odjeljenjePredmetID);
            Odjeljenje odjeljenje = db.Odjeljenje.Where(o => o.Id == predajePredmet.OdjeljenjeID).FirstOrDefault();
            Predmet predmet = db.Predmet.Where(o => o.Id == predajePredmet.PredmetID).FirstOrDefault();
            Skola skola = db.Skola.Where(s => s.Id == odjeljenje.SkolaID).FirstOrDefault();
            OdrzaniCas odrzaniCas = new OdrzaniCas
            {
                DatumOdrzanogCasa = temp.datum,
                NastavnikID = temp.nastavnikID,
                OdjeljenjeID = odjeljenje.Id,
                PredmetId = predmet.Id
            };
            db.Add(odrzaniCas); db.SaveChanges();

            List<OdjeljenjeStavka> odjeljnjeStavke = db.OdjeljenjeStavka.Where(o => o.OdjeljenjeId == odjeljenje.Id).ToList();
            foreach (var stavke in odjeljnjeStavke)
            {
                OdrzanCasDetalji odrzanCasDetalji = new OdrzanCasDetalji
                {
                    ocjena = 5,
                    OdrzaniCasID = odrzaniCas.Id,
                    opravdanoOdsutan = false,
                    Prisutan = false,
                    UcenikID = stavke.UcenikId
                };
                db.Add(odrzanCasDetalji); db.SaveChanges();
            }
            return Redirect("/OdrzanaNastava/PrikazCasova?nastavnikID="+temp.nastavnikID+ "&skolaID="+skola.Id);
        }
        public ActionResult Brisanje(int casID)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Find(casID);
            int nasatvnik = odrzaniCas.NastavnikID;
            int odjeljenjeID = odrzaniCas.OdjeljenjeID;
            Odjeljenje odjeljenje = db.Odjeljenje.Where(o => o.Id == odjeljenjeID).FirstOrDefault();
            Skola skola = db.Skola.Where(s => s.Id == odjeljenje.SkolaID).FirstOrDefault();
            db.Remove(odrzaniCas);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/PrikazCasova?nastavnikID=" + nasatvnik + "&skolaID=" + skola.Id);
        }
        public ActionResult EditovanjeCasa(int casID)
        {

            OdrzaniCas odrzaniCas = db.OdrzaniCas.Find(casID);
            var model = new EditovanjeCasaVM
            {
                datum = odrzaniCas.DatumOdrzanogCasa.ToString("dd.MM.yyyy"),
                odrzaniCasID = odrzaniCas.Id,
                odjelejnje = db.Odjeljenje.Where(o => o.Id == odrzaniCas.OdjeljenjeID).Select(o => o.Oznaka).FirstOrDefault(),
                sadrzajCasa = odrzaniCas.sadrzajCasa
            };
            return View(model);
        }
        public ActionResult SnimanjeCasa(EditovanjeCasaVM temp)
        {

            OdrzaniCas odrzaniCas = db.OdrzaniCas.Find(temp.odrzaniCasID);
            odrzaniCas.sadrzajCasa = temp.sadrzajCasa;
            Odjeljenje odjeljenje = db.Odjeljenje.Where(o => o.Id == odrzaniCas.OdjeljenjeID).FirstOrDefault();
            Skola skola = db.Skola.Where(s => s.Id == odjeljenje.SkolaID).FirstOrDefault();
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/PrikazCasova?nastavnikID=" + odrzaniCas.NastavnikID + "&skolaID=" +skola.Id);

        }
        public ActionResult PrikazDetalja( int casID)
        {

            var model = new DetaljiCasaaa_PRikazDonjaTAbela
            {
                podaciPrikaz = db.OdrzanCasDetalji.Where(o => o.OdrzaniCasID == casID).Select(o => new DetaljiCasaaa_PRikazDonjaTAbela.Row()
                {
                    detaljiID = o.Id,
                    ocjene = o.ocjena,
                    opravdanoOdsutan = o.opravdanoOdsutan,
                    prisutan = o.Prisutan,
                    ucenikIme = db.Ucenik.Where(u => u.Id == o.UcenikID).Select(u => u.ImePrezime).FirstOrDefault()
                }).ToList()
            };
            return PartialView(model);
        }
        public ActionResult MIjenjenjaeOdsutnosit(int detaljID)
        {

            OdrzanCasDetalji odrzanCasDetalji = db.OdrzanCasDetalji.Find(detaljID);
            if (odrzanCasDetalji.Prisutan == false)
            {
                odrzanCasDetalji.Prisutan = true;
            }
            else
            {
                odrzanCasDetalji.Prisutan = false;

            }
            db.SaveChanges();
            int cas = odrzanCasDetalji.OdrzaniCasID;
            return Redirect("/OdrzanaNastava/PrikazDetalja?casID=" + cas);
        }
        public ActionResult MijenjenjeOcjene(int detalj)
        {

            OdrzanCasDetalji odrzanCasDetalji = db.OdrzanCasDetalji.Find(detalj);
            var model = new UrediOcejneVM
            {
                detaljID= detalj,
                ocjena=odrzanCasDetalji.ocjena,
                ucenikIme   = db.Ucenik.Where(u=> u.Id==odrzanCasDetalji.UcenikID).Select(s=>s.ImePrezime).FirstOrDefault()
            };
            return PartialView(model);
        }

        public ActionResult SnimanjeOcjene(UrediOcejneVM temo)
        {
            OdrzanCasDetalji odrzanCasDetalji = db.OdrzanCasDetalji.Find(temo.detaljID);
            odrzanCasDetalji.ocjena = temo.ocjena;
            db.SaveChanges();
            int cas = odrzanCasDetalji.OdrzaniCasID;
            return Redirect("/OdrzanaNastava/PrikazDetalja?casID=" + cas);
        }
        public ActionResult MijenjenjeOpravdaneOdsutnosti(int detalj)
        {

            OdrzanCasDetalji odrzanCasDetalji = db.OdrzanCasDetalji.Find(detalj);

            var model = new UrediOpravdanuOdsutnostVM
            {
                detaljID = detalj,
                napomena = odrzanCasDetalji.Napomena,
                ucenikIme = db.Ucenik.Where(u => u.Id == odrzanCasDetalji.UcenikID).Select(s => s.ImePrezime).FirstOrDefault()
            };
            return PartialView(model);
        }
        public ActionResult SnimanjeOdsutnosti(UrediOpravdanuOdsutnostVM temo)
        {
            OdrzanCasDetalji odrzanCasDetalji = db.OdrzanCasDetalji.Find(temo.detaljID);
            odrzanCasDetalji.Napomena = temo.napomena;
            if (temo.opravdano==true)
            {
                odrzanCasDetalji.opravdanoOdsutan = true;
            }
            else
            {
                odrzanCasDetalji.opravdanoOdsutan = false;
            }
            db.SaveChanges();
            int cas = odrzanCasDetalji.OdrzaniCasID;
            return Redirect("/OdrzanaNastava/PrikazDetalja?casID=" + cas);
        }
    }
}