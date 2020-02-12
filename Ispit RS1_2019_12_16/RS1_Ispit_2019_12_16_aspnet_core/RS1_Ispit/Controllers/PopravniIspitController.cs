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
            var model = new PrikazSkolSkolskaGodinaPredmetVM
            {
                skolskaGodina= db.SkolskaGodina.Select(s=> new SelectListItem
                {
                    Value= s.Id.ToString(), 
                    Text= s.Naziv
                }).ToList(),
                skola= db.Skola.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Naziv
                }).ToList(),
                predmet= db.Predmet.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Naziv
                }).ToList()
            };
            return View(model);
        }
        public ActionResult Odaberi(PrikazSkolSkolskaGodinaPredmetVM temp)
        {
            var model = new PopravniIspitVM_Prikaz
            {
                
                predmetNaziv= db.Predmet.Where(p=>p.Id==temp.predmetID).Select(p=> p.Naziv).FirstOrDefault(), 
                predmetID= db.Predmet.Where(p=>p.Id==temp.predmetID).Select(p=> p.Id).FirstOrDefault(), 
                skola= db.Skola.Where(s=>s.Id==temp.skolaID).Select(s=>s.Naziv).FirstOrDefault(), 
                skolaID= db.Skola.Where(s=>s.Id==temp.skolaID).Select(s=>s.Id).FirstOrDefault(), 
                skolskaGOdina= db.SkolskaGodina.Where(s=>s.Id==temp.skolskaGodinaID).Select(s=>s.Naziv).FirstOrDefault(),
                skolskaGOdinaId= db.SkolskaGodina.Where(s=>s.Id==temp.skolskaGodinaID).Select(s=>s.Id).FirstOrDefault(),
                podaciPoppravniIspit = db.PopravniIsppit.Where(p=>p.PredmetId==temp.predmetID).Select(p=> new PopravniIspitVM_Prikaz.Row()
                {
                    popravniID= p.Id,
                    datumPopravnogIspita = p.DatumIspita.ToString("dd.MM.yyyy"),
                    prviClanKomisijePredmet= db.Predmet.Where(pr=>pr.Id==p.PredmetId).Select(pr=>pr.Naziv).FirstOrDefault(), 
                    brojUcesnikaNaPopravnomIspitu= db.PopravniIspitDetalji.Where(pp=>pp.PopravniIsppitId==p.Id).Count(), 
                    brojUcesnikaaKojiSuPoloziliIspit= db.PopravniIspitDetalji.Where(pp=>pp.PopravniIsppitId==p.Id).Count(pp=>pp.bodoviIspita>50)
                }).ToList()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult DodavanjePopravnogIspita(int skolaID,  int skolskaGodinaID, int predmetId)
        {
            var model = new PopravniIspitDodavanjeVM
            {
                
                clanKomisije1= db.Nastavnik.Select(n=> new SelectListItem
                {
                    Value=n.Id.ToString(), 
                    Text=n.Ime+n.Prezime
                }).ToList(),
                clanKomisije2= db.Nastavnik.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Ime + n.Prezime
                }).ToList(),
                clanKomisije3= db.Nastavnik.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Ime + n.Prezime
                }).ToList(),
                skolaID= skolaID, 
                skolaNaziv=db.Skola.Where(s=>s.Id==skolaID).Select(s=>s.Naziv).FirstOrDefault(), 
                predmetID= predmetId, 
                predmetNaziv= db.Predmet.Where(p=>p.Id==predmetId).Select(p=>p.Naziv).FirstOrDefault(), 
                skolskaGodinaID= skolskaGodinaID, 
                skolskaGodinaNaziv= db.SkolskaGodina.Where(s=>s.Id==skolskaGodinaID).Select(s=>s.Naziv).FirstOrDefault()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult DodavanjePopravnogIspita(PopravniIspitDodavanjeVM temp)
        {
            PopravniIsppit popravniIspit = new PopravniIsppit()
            {
                DatumIspita = temp.datumPopravnogIspita,
                Nastavnik1Id = temp.clanKomisije1ID,
                Nastavnik2Id = temp.clanKomisije2ID,
                Nastavnik3Id = temp.clanKomisije3ID,
                PredmetId = temp.predmetID,
                SkolaId = temp.skolaID,
                SkolskaGodinaId = temp.skolskaGodinaID
            };
            db.Add(popravniIspit); db.SaveChanges();

            List<DodjeljenPredmet> dodijeljeniPredmet = db.DodjeljenPredmet.Where(d => d.ZakljucnoKrajGodine == 1 && d.PredmetId==temp.predmetID).ToList();

            List<OdjeljenjeStavka> odjeljenjeStavkas = db.OdjeljenjeStavka.Where(od => dodijeljeniPredmet.Any(dd => dd.OdjeljenjeStavkaId == od.Id)).ToList();

            foreach (var stavke in odjeljenjeStavkas)
            {
                if (db.DodjeljenPredmet.Where(d=>d.OdjeljenjeStavkaId==stavke.Id).Count(d=>d.ZakljucnoKrajGodine==1)>=3)
                {
                    PopravniIspitDetalji popravniIspitDetalji = new PopravniIspitDetalji
                    {
                        bodoviIspita = 0,
                        imaPracoPristupa = false,
                        isPristupio = false,
                        PopravniIsppitId = popravniIspit.Id,
                        UcenikId = stavke.UcenikId
                    };
                    db.Add(popravniIspitDetalji);db.SaveChanges();
                }
                else
                {
                    PopravniIspitDetalji popravniIspitDetalji = new PopravniIspitDetalji
                    {
                        bodoviIspita = 0,
                        imaPracoPristupa = true,
                        isPristupio = false,
                        PopravniIsppitId = popravniIspit.Id,
                        UcenikId = stavke.UcenikId
                    };
                    db.Add(popravniIspitDetalji); db.SaveChanges();
                }
            }
            return Redirect("/PopravniIspit/Index");
        }
        public ActionResult UredivanjePopravnogIspita(int popravniIspitID)
        {
            PopravniIsppit popravniIsppit = db.PopravniIsppit.Find(popravniIspitID);

            var model = new DetaljiPopravnogIspitaVM
            {
                popravniIspitID = popravniIspitID,
                clanKomicije1 = db.Nastavnik.Where(n => popravniIsppit.Nastavnik1Id == n.Id).Select(n => n.Ime + n.Prezime).FirstOrDefault(),
                clanKomisije2 = db.Nastavnik.Where(n => popravniIsppit.Nastavnik2Id == n.Id).Select(n => n.Ime + n.Prezime).FirstOrDefault(),
                clanKomisije3 = db.Nastavnik.Where(n => popravniIsppit.Nastavnik3Id == n.Id).Select(n => n.Ime + n.Prezime).FirstOrDefault(),
                datumIspita= popravniIsppit.DatumIspita.ToString("dd.MM.yyyy"),
                predmetID= popravniIsppit.PredmetId,
                predmetNaziv= db.Predmet.Where(p=>p.Id==popravniIsppit.PredmetId).Select(p=> p.Naziv).FirstOrDefault(),
                skolaID= popravniIsppit.SkolaId,
                skolaNaziv= db.Skola.Where(p => p.Id == popravniIsppit.SkolaId).Select(p => p.Naziv).FirstOrDefault(),
                skolskaGOdinaID=popravniIsppit.SkolskaGodinaId,
                skolskaGodina = db.SkolskaGodina.Where(p => p.Id == popravniIsppit.SkolskaGodinaId).Select(p => p.Naziv).FirstOrDefault()
            };

            return View(model);
        }

        public ActionResult PrikazDetaljaPopravnog(int popravniId)
        {
            PopravniIsppit popravniIsppit = db.PopravniIsppit.Find(popravniId);

            var model = new PopravniIspitDetaljiVM_Prikaz
            {
                podaciDetaljiPopravni= db.PopravniIspitDetalji.Where(p=>p.PopravniIsppitId== popravniId).Select(p=> new PopravniIspitDetaljiVM_Prikaz.Row()
                {
                    detaljiID=p.Id, 
                    ucenikIme= db.Ucenik.Where(u=>u.Id==p.UcenikId).Select(u=>u.ImePrezime).FirstOrDefault(),
                    brojUDnevniku= db.OdjeljenjeStavka.Where(o=>o.UcenikId==p.UcenikId).Select(o => o.BrojUDnevniku).FirstOrDefault(),
                    ImaPravoPristupa= p.imaPracoPristupa,
                    odjeljenjeNaziv= db.Odjeljenje.Where(o=>o.SkolaID==popravniIsppit.SkolaId).Select(o=>o.Oznaka).FirstOrDefault(),
                    pristupio= p.isPristupio,
                    rezultatiBodovi= p.bodoviIspita
                }).ToList()
            };
            return PartialView(model);
        }

        public ActionResult MijenjanjePrisutnosti(int detaljiID)
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

            int popravniID = popravniIspitDetalji.PopravniIsppitId;
            return Redirect("/PopravniIspit/PrikazDetaljaPopravnog?popravniId=" + popravniID);
        }

        [HttpGet]
        public ActionResult DodavanjeUcenika(int popraavniID)
        {
            var model = new DodavanjeUcenikaVM
            {
                popravniID = popraavniID,
                ucenik = db.Ucenik.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.ImePrezime
                }).ToList()
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult DodavanjeUcenika(DodavanjeUcenikaVM temp)
        {
            PopravniIsppit popravniIsppit = db.PopravniIsppit.Find(temp.popravniID);

            PopravniIspitDetalji popravniIspitDetalji = new PopravniIspitDetalji
            {
                bodoviIspita= temp.bodovi,
                imaPracoPristupa=true,
                isPristupio=false,
                PopravniIsppitId=popravniIsppit.Id,
                UcenikId=temp.ucenikID
            };
            db.Add(popravniIspitDetalji); db.SaveChanges();
            int popravniID = popravniIspitDetalji.PopravniIsppitId;
            return Redirect("/PopravniIspit/PrikazDetaljaPopravnog?popravniId=" + popravniID);
        }
        public ActionResult UredjivanjeUcenikaBodova(int detaljiID)
        {
            PopravniIspitDetalji popravniIspitDetalji = db.PopravniIspitDetalji.Find(detaljiID);

            var model = new UredjivanjeUcenikBodoviVM
            {
                detaljiID = detaljiID,
                ucenikImePrezime = db.Ucenik.Where(u => u.Id == popravniIspitDetalji.UcenikId).Select(u => u.ImePrezime).FirstOrDefault(),
                bodovi = popravniIspitDetalji.bodoviIspita
            };
            return PartialView(model);
        }
        public ActionResult SnimanjeUcenika(int detaljiID, int bodovi)
        {
            PopravniIspitDetalji popravniIspitDetalji = db.PopravniIspitDetalji.Find(detaljiID);
            popravniIspitDetalji.bodoviIspita = bodovi;
            db.SaveChanges();
            var popravniID = popravniIspitDetalji.PopravniIsppitId;
            return Redirect("/PopravniIspit/PrikazDetaljaPopravnog?popravniId=" + popravniID);

        }
    }
}