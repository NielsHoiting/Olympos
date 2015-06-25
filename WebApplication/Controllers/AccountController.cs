using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication.Models;
using WebApplication.Persistance;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult Profiel()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult Logout()
        {
            if (Session["user"] != null)
            {
                Session.Abandon();
                return RedirectToAction("Index", "Login");
            }
            return RedirectToAction("Index", "Login");
        }
        public ActionResult ZoekGebruiker(string achternaam, string geboortedatum)
        {

            //check if user is logged in and if les exists
            if (Session["user"] == null)
                return RedirectToAction("Index", "Registratie");

            DateTime geboorteDateTime = DateTime.Now;
            geboorteDateTime = DateTime.Parse(geboortedatum);
            AccountPersistenceManager manager = new AccountPersistenceManager();
            Gebruiker g = manager.ZoekGebruiker(achternaam, geboorteDateTime.Date);
            
            string helenaam = "Gebruiker niet gevonden";
            int sco_nummer = 0;
            if (g != null)
            {
                helenaam = g.voornaam + " " + g.achternaam;
                sco_nummer = g.sco_nummer;
            }
            
            var Object = new
            {
                naam = helenaam,
                sco_nummer = sco_nummer
            };
            //creating object for serializer to serialize
            

            //serializing object
            string json = new JavaScriptSerializer().Serialize(Object);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}