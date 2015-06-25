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
            List<Gebruiker> deelnemers = new List<Gebruiker>();
             
            foreach (Reservering r in reserveringen)
            {
                deelnemers.Add(r.Deelnemer);
            }

            //serializing object
            string json = new JavaScriptSerializer().Serialize(deelnemers);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
	}
}