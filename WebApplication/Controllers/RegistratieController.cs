using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication.Models;
using WebApplication.Persistance;

namespace WebApplication.Controllers
{
    public class RegistratieController : Controller
    {
        //
        // GET: /Registratie/
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            LesPersistanceManager manager = new LesPersistanceManager();
            Gebruiker g = (Gebruiker)Session["user"];
            List<Les> lessen = manager.getRegistreerbareLessen(g);
            ViewData["lessen"] = lessen;
            return View();
        }
        public ActionResult Les(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Gebruiker g = (Gebruiker)Session["user"];
            LesPersistanceManager manager = new LesPersistanceManager();
            Les les = manager.getLes(id);
            manager.checkAanwezigheid(id);

            
            if (les == null || les.Sportdocent.sco_nummer != g.sco_nummer)
            {
                return RedirectToAction("Les", "Index");
            }
            ViewData["les"] = les;
            return View();
        }

        public ActionResult GetDeelnemers(int lesid)
        {

            //check if user is logged in and if les exists
            if (Session["user"] == null)
                return RedirectToAction("Index", "Registratie");
            LesPersistanceManager manager = new LesPersistanceManager();
            List<Reservering> reserveringen = manager.getLes(lesid).Reserveringen.ToList<Reservering>();
            List<returnGebruiker> objects = new List<returnGebruiker>();

            foreach (Reservering r in reserveringen)
            {
                System.Diagnostics.Debug.WriteLine(new returnGebruiker(r.Deelnemer.sco_nummer, r.Deelnemer.voornaam + " " + r.Deelnemer.achternaam, r.is_geweest).isAanwezig);
                objects.Add(new returnGebruiker(r.Deelnemer.sco_nummer, r.Deelnemer.voornaam + " " + r.Deelnemer.achternaam, r.is_geweest));
            }

            //serializing object
            string json = new JavaScriptSerializer().Serialize(objects);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ToggleAanwezigheid(int sco_nummer, int lesid)
        {

            LesPersistanceManager manager = new LesPersistanceManager();
            manager.ToggleAanwezigheid(sco_nummer, lesid);
            string json = new JavaScriptSerializer().Serialize(true);
            return Json(json, JsonRequestBehavior.AllowGet);
        
        
        }

        public ActionResult Inschrijven(int sco_nummer, int lesid)
        {

            LesPersistanceManager manager = new LesPersistanceManager();
            manager.Inschrijven(sco_nummer, lesid);
            return null;
        }
        class returnGebruiker{

            public int sco_nummer { get; set; }
            public string naam { get; set; }
            public bool isAanwezig { get; set; }
            public returnGebruiker(int sco_nummer, string naam, bool isAanwezig)
            {
                this.sco_nummer = sco_nummer;
                this.naam = naam;
                this.isAanwezig = isAanwezig;
            }
        }
	}
}