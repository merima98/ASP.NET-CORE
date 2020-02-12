using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eUniverzitet.Web.Helper;
using Ispit.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Ispit.Data;
using Ispit.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Ispit.Web.ViewModels;

namespace Ispit.Web.Controllers
{
    [Autorizacija]
    public class OznaceniDogadajiController : Controller
    {
        private MyContext db;

        public OznaceniDogadajiController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            KorisnickiNalog logiraniKOrisnik = HttpContext.GetLogiraniKorisnik();

            //Logirani student
            Student logiraniStudent = db.Student.Where(s => s.KorisnickiNalogId == logiraniKOrisnik.Id).FirstOrDefault();

            List<Dogadjaj> sviDOgadjaji = db.Dogadjaj.Include(d => d.Nastavnik).ToList(); //jer cemo koristiti i nastavnika
            List<OznacenDogadjaj> studentoviDogadjaji = db.OznacenDogadjaj.Where(od => od.StudentID == logiraniStudent.ID).ToList(); //ovdje se nallaze oznaceni studentovi dogadjaji

            List<OznaceniNeoznaceniDogadjajiVM_PRIKAZ.Row> neoznaceni = new List<OznaceniNeoznaceniDogadjajiVM_PRIKAZ.Row>();

            foreach (var svi in sviDOgadjaji)
            {
                var ima = false;
                foreach (var student in studentoviDogadjaji)
                {
                    if (svi.ID == student.DogadjajID)
                        ima = true;
                }
                if (ima==false) //ako jos nisu u bazi oznacenih dogadjaja
                {
                    OznaceniNeoznaceniDogadjajiVM_PRIKAZ.Row neoznaceniDogadjaj = new OznaceniNeoznaceniDogadjajiVM_PRIKAZ.Row();
                    neoznaceniDogadjaj.DogadjajID = svi.ID;
                    neoznaceniDogadjaj.datumDogadjaja = svi.DatumOdrzavanja.ToString("dd.MM.yyyy");
                    neoznaceniDogadjaj.NastavnikImePrezime = svi.Nastavnik.ImePrezime;
                    neoznaceniDogadjaj.opisDogadjaja = svi.Opis;
                    neoznaceniDogadjaj.BrojObaveza = db.Obaveza.Count(o => o.DogadjajID == svi.ID);
                    neoznaceni.Add(neoznaceniDogadjaj); //dodavanje u neoznacene, koji se nalaze u view modelu.
                }
            }
            var ulazniPOdaci = new OznaceniNeoznaceniDogadjajiVM_PRIKAZ
            {

                oznaceniDogadjaji = db.OznacenDogadjaj.Where(s => s.StudentID == logiraniStudent.ID).Select(o => new OznaceniNeoznaceniDogadjajiVM_PRIKAZ.Row()
                {
                    DogadjajID = o.DogadjajID,
                    datumDogadjaja = db.Dogadjaj.Where(d => d.ID == o.DogadjajID).Select(d => d.DatumOdrzavanja.ToString("dd.MM.yyyy")).FirstOrDefault(),
                    NastavnikImePrezime = db.Dogadjaj.Where(d => d.ID == o.DogadjajID).Select(d => d.Nastavnik.ImePrezime).FirstOrDefault(),
                    opisDogadjaja= db.Dogadjaj.Where(d => d.ID == o.DogadjajID).Select(d => d.Opis).FirstOrDefault(),
                    IzvrsenoProcentualno= db.StanjeObaveze.Where(so=>so.OznacenDogadjajID==o.ID).Sum(so=>so.IzvrsenoProcentualno)
                }).ToList(),
                neoznaceniDogadjaji= neoznaceni
            };

            return View(ulazniPOdaci);
        }
        public ActionResult DodavanjeUOznaceneDogadjaje(int dogadjajID)
        {
            KorisnickiNalog logiraniKorisnik = HttpContext.GetLogiraniKorisnik();
            Student logiraniStudent = db.Student.Where(s => s.KorisnickiNalogId == logiraniKorisnik.Id).FirstOrDefault();
            OznacenDogadjaj oznacenDogadjaj = new OznacenDogadjaj
            {
                DatumDodavanja = DateTime.Now,
                DogadjajID = dogadjajID,
                StudentID = logiraniStudent.ID
            };
            db.Add(oznacenDogadjaj); db.SaveChanges();

            List<Obaveza> obavezeDogadjaja = db.Obaveza.Where(od => od.DogadjajID == oznacenDogadjaj.DogadjajID).ToList();

            foreach (var obaveze in obavezeDogadjaja)
            {
                StanjeObaveze stanjeObaveze = new StanjeObaveze
                {
                    DatumIzvrsenja = DateTime.Now,
                    IsZavrseno = false,
                    IzvrsenoProcentualno = 0,
                    NotifikacijaDanaPrije = 0,
                    NotifikacijeRekurizivno = default,
                    ObavezaID = obaveze.ID,
                    OznacenDogadjajID = oznacenDogadjaj.ID
                };
                db.Add(stanjeObaveze); db.SaveChanges();
            }
            return Redirect("/OznaceniDogadaji/Index");

        }
         
        public  ActionResult DetaljiDogadjaja(int dogadjajID)
        {
           
            OznacenDogadjaj oznacenDogadjaj = db.OznacenDogadjaj.Where(d => d.DogadjajID == dogadjajID).FirstOrDefault();
            var model = new DetaljiDogadjajaVM
            {
                datumDodavanja = oznacenDogadjaj.DatumDodavanja.ToString("dd.MM.yyyy"),
                datumDogadjaja = db.Dogadjaj.Where(d => d.ID == dogadjajID).Select(d => d.DatumOdrzavanja.ToString("dd.MM.yyyy")).FirstOrDefault(),
                oznaceniDogadjajID = oznacenDogadjaj.ID,
                nastavnikImePrezime = db.Dogadjaj.Where(d => d.ID == dogadjajID).Select(d => d.Nastavnik.ImePrezime).FirstOrDefault(),
                opisDogadjaja = db.Dogadjaj.Where(d => d.ID == dogadjajID).Select(d => d.Opis).FirstOrDefault()
            };
            return View(model);
        }
        public ActionResult StanjeObavezePrikaz(int oznaceniDOgadjajID)
        {
            var model = new StanjeObavezePrikazVM
            {
               podaciStanjeObavezePrikaz= db.StanjeObaveze.Where(s=>s.OznacenDogadjajID== oznaceniDOgadjajID).Include(s=>s.Obaveza).Select(s=> new StanjeObavezePrikazVM.Row()
               {
                   izvrsenoProcentualno=s.IzvrsenoProcentualno,
                   naziivStanjaObaveze= db.Obaveza.Where(ob=>ob.ID==s.ObavezaID).Select(ob=>ob.Naziv).FirstOrDefault(),
                   personalnaVrijednostRekurzijaBOOL= s.NotifikacijeRekurizivno,
                   personalnaVrijednostSaljiNotifikacijuDanaPrije= s.NotifikacijaDanaPrije,
                   stanjeObavezeID= s.Id,
                   isZavrseno= s.IsZavrseno
               }).ToList()
            };
            return PartialView(model);
        }
        public ActionResult EvidentiranjeStanjaObaveze(int stanjeObavezeID)
        {

            StanjeObaveze stanjeObaveze = db.StanjeObaveze.Find(stanjeObavezeID);
            var model = new EvidentiranjeStanjaObavezeVM
            {
                stanjeObavezeID = stanjeObavezeID,
                izvrsenoProcentualno = stanjeObaveze.IzvrsenoProcentualno,
                nazivObaveze = db.Obaveza.Where(o => o.ID == stanjeObaveze.ObavezaID).Select(o => o.Naziv).FirstOrDefault()
            };
            return PartialView(model);
        }

        public ActionResult SnimanjeProcenta(int stanjeObavezeID, float izvrsenoProcentualno)
        {
            StanjeObaveze stanjeObaveze = db.StanjeObaveze.Find(stanjeObavezeID);
            stanjeObaveze.IzvrsenoProcentualno = izvrsenoProcentualno;
            db.SaveChanges();
            int oznaceniDOgadjajID = stanjeObaveze.OznacenDogadjajID;
            return Redirect("/OznaceniDogadaji/StanjeObavezePrikaz?oznaceniDOgadjajID=" + oznaceniDOgadjajID);
        }
        public ActionResult OznaciProcitano(string sadrzajObaveze = "Obaveza 19") //JA SAM OVDJE POSTAVILA NAZIV: Obaveza 19, jer u bazi ne postoji obaveza sa nazivom "Pregledati pdf materijale"
        {
            Obaveza obaveza = db.Obaveza.Where(o => o.Naziv.Equals(sadrzajObaveze)).FirstOrDefault();

            if (obaveza != null)
            {
                StanjeObaveze stanje = db.StanjeObaveze.Where(s => s.ObavezaID == obaveza.ID).FirstOrDefault();
                List<StanjeObaveze> listaStanja = db.StanjeObaveze.Include(s=>s.Obaveza).Where(s => s.Obaveza.Naziv.Equals(sadrzajObaveze)).ToList();
                foreach (var s in listaStanja)
                {
                    s.IsZavrseno = true;
                    db.SaveChanges();
                }
            }
            return Redirect("/OznaceniDogadaji/Index");
        }
    }
}