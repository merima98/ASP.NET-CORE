using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS1.Ispit.Web.Models;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class UputnicaController : Controller
    {
        private MojContext db;
        public UputnicaController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = new EvidentiraneUputnice_VM_prikaz
            {
                podaciPrikaz= db.Uputnica.Select(u=> new EvidentiraneUputnice_VM_prikaz.Row()
                {
                    uputnicaID= u.Id,
                    datumUputnice= u.DatumUputnice.ToString("dd.MM.yyyy"),
                    datumEvidentiranjaRezultataPretrage = u.DatumRezultata,
                    doktorIme = db.Ljekar.Where(l=>l.Id==u.UputioLjekarId).Select(l=>l.Ime).FirstOrDefault(),
                    pacijentIme= db.Pacijent.Where(p=>p.Id==u.PacijentId).Select(p=>p.Ime).FirstOrDefault(),
                    vrstaPretrage= db.VrstaPretrage.Where(v=>v.Id==u.VrstaPretrageId).Select(v=>v.Naziv).FirstOrDefault()
                }).ToList()
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult DodavanjeUputnice()
        {
            var model = new DodavanjeUputniceVM
            {
                uputioLjekar = db.Ljekar.Select(l=>new SelectListItem
                {
                    Value=l.Id.ToString(), 
                    Text=l.Ime
                }).ToList(),
                pacijent = db.Pacijent.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Ime
                }).ToList(),
                vrsstaPretrage= db.VrstaPretrage.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Naziv
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult DodavanjeUputnice(DodavanjeUputniceVM temp)
        {
            Uputnica uputnica = new Uputnica
            {
                DatumUputnice = temp.datumUputnice,
                IsGotovNalaz = false,
                PacijentId = temp.pacijentID,
                UputioLjekarId = temp.uputioLjekarID,
                VrstaPretrageId = temp.vrsstaPretrageID
            };
            db.Add(uputnica); db.SaveChanges();

            List<LabPretraga> labPretragas = db.LabPretraga.Where(l => l.VrstaPretrageId == temp.vrsstaPretrageID).ToList();

            foreach (var LAB in labPretragas)
            {
                RezultatPretrage rezultatPretrage = new RezultatPretrage
                {
                    LabPretragaId = LAB.Id,
                    UputnicaId = uputnica.Id
                };
                db.Add(rezultatPretrage); db.SaveChanges();
            }
            return Redirect("/Uputnica/Index");
        }
        public ActionResult DetaljiUputnice(int uputnicaID)
        {
            Uputnica uputnica = db.Uputnica.Find(uputnicaID);
            var model = new DetlajiUputniceVM_Prikaz
            {

                uputnicaID= uputnicaID,
                datumRezultataUputnice= uputnica.DatumRezultata,
                datumUputnice= uputnica.DatumUputnice.ToString("dd.MM.yyyy"),
                pacijentIme= db.Pacijent.Where(p=>p.Id==uputnica.PacijentId).Select(p=>p.Ime).FirstOrDefault()
            };
            return View(model);
        }
        public  ActionResult PrikazRezultata(int uputnicaID)
        {
            Uputnica uputnica = db.Uputnica.Find(uputnicaID);
            var model = new RezultatPretrageVM_PRikaz
            {
                podaciRezultatiPrikaz= db.RezultatPretrage.Where(l=>l.UputnicaId==uputnica.Id).Select(l=> new RezultatPretrageVM_PRikaz.Row()
                {
                    rezultatID= l.Id,
                    labPretragaNaziv= db.LabPretraga.Where(p=>p.Id==l.LabPretragaId).Select(p=>p.Naziv).FirstOrDefault(),
                    izmjerenaNumerickaVrijednost= l.NumerickaVrijednost,
                    mjernaJednicica= db.LabPretraga.Where(p => p.Id == l.LabPretragaId).Select(p => p.MjernaJedinica).FirstOrDefault(),
                    akoJeModalitetNaziv = db.Modalitet.Where(m=>m.LabPretragaId==l.LabPretragaId).Select(m=>m.Opis).FirstOrDefault(),
                     vrstaVrijednosti = db.LabPretraga.Where(p => p.Id == l.LabPretragaId).Select(p => p.VrstaVr).FirstOrDefault(),
                }).ToList()
            };
            return PartialView(model);
        }
        public ActionResult UredjivanjeNumericko(int labID)
        {
         
            RezultatPretrage labPretraga = db.RezultatPretrage.Find(labID);
            var model = new UredivanjeNumerockoVM
            {
                labID= labID,
                naziv= db.LabPretraga.Where(l=>l.Id==labPretraga.LabPretragaId).Select(l=>l.Naziv).FirstOrDefault(),
                izmjerenaVrijednosti= db.RezultatPretrage.Where(l=>l.Id==labID).Select(d=>d.NumerickaVrijednost).FirstOrDefault()
            };
            return PartialView(model);
        }
        public ActionResult SnimanjeNumericko(int labID, double izmjerenaVrijednosti)
        {

            RezultatPretrage rezultatPretrage = db.RezultatPretrage.Find(labID);
            rezultatPretrage.NumerickaVrijednost = izmjerenaVrijednosti;
            db.SaveChanges();

            return Redirect("/Uputnica/PrikazRezultata?uputnicaID=" + rezultatPretrage.UputnicaId);
        }

        public ActionResult UredjivanjeModalitet(int labID)
        {
 
            RezultatPretrage labPretraga = db.RezultatPretrage.Find(labID);

            var model = new UredjivanjeModaliteta
            {
                labID= labID,
                nazivOpis= db.LabPretraga.Where(m=>m.Id==labPretraga.LabPretragaId).Select(m=>m.Naziv).FirstOrDefault(),
                naziv= db.Modalitet.Select(m=> new SelectListItem
                {
                    Value=m.Id.ToString(),
                    Text= m.Opis
                }).ToList()
            };
            return PartialView(model);
        }
        public ActionResult SnimanjeModaliteta(int labID, int nazivID)
        {

            RezultatPretrage rezultatPretrage = db.RezultatPretrage.Find(labID);
            Modalitet modalitet = db.Modalitet.Where(m => m.Id == rezultatPretrage.ModalitetId).FirstOrDefault();
            if (nazivID==1)
            {
                modalitet.Opis = "bistar";
            }
            else if (nazivID == 2)
            {
                modalitet.Opis = "zamucen";
            }
            else if (nazivID == 3)
            {
                modalitet.Opis = "zut";

            }
            else if (nazivID == 4)
            {
                modalitet.Opis = "nema";
            }
            else if (nazivID == 5)
            {
                modalitet.Opis = "nesto";
            }
            else
            {
                modalitet.Opis = "mnogo";
            }
            db.SaveChanges();

            return Redirect("/Uputnica/PrikazRezultata?uputnicaID=" + rezultatPretrage.UputnicaId);
        }
    }
}