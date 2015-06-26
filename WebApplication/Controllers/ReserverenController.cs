using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Persistance;
using WebApplication.Models;
using System.Globalization;
using System.Web.Script.Serialization;

namespace WebApplication.Controllers
{
    public class ReserverenController : Controller
    {
        public ActionResult Index()
        {

            return RedirectToAction("WeekOverzicht", "Reserveren");
        }
        public ActionResult KomendeLessen()
        {
            
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            LesPersistanceManager manager = new LesPersistanceManager();
            Gebruiker g = (Gebruiker)Session["user"];
            List<Les> KomendeLessen = manager.getKomendeLessen(g, 10);
            ViewData["KomendeLessen"] = KomendeLessen;
            return View();
        }
        public ActionResult MijnInteresses()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            LesPersistanceManager manager = new LesPersistanceManager();
            Gebruiker g = (Gebruiker)Session["user"];

            //TODO: variabelen duidelijker maken
            ViewData["GeinteresseerdeLessen"] = manager.getMijnInteresseLessen(g, 10, 7, false);
            return View();
        }

        public ActionResult WeekOverzicht()
        {
            //getting parameters
            string sportcode = Request.Params["sportcode"];
            string beginTijd = Request.Params["begintijd"];
            string plekkenVrij = Request.Params["plekkenvrij"];
            string bewaren = Request.Params["bewaren"];
            System.Diagnostics.Debug.WriteLine(sportcode + " " + beginTijd + " " + plekkenVrij + " ");

            LesPersistanceManager manager = new LesPersistanceManager();
            DateTime begin = DateTime.Today;
            DateTime eind = begin.AddDays(7);
            List<Les> lessen = manager.getWeekOverzicht(begin, eind);
            ViewData["lessen"] = lessen;
            ViewData["beginDate"] = begin;
            ViewData["eindDate"] = eind;
            
            return View();  
        }
        public ActionResult Les(int id)
        {
            LesPersistanceManager manager = new LesPersistanceManager();

            Les les = manager.getLes(id);
            if (les == null)
            {
                return RedirectToAction("Index", "Reserveren");
            }
            ViewData["les"] = les;
            return View();
        }
        public ActionResult ReserveerLes(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            LesPersistanceManager manager = new LesPersistanceManager();
            Les les = manager.getLes(id);
            if (les == null)
            {
                return RedirectToAction("Index", "Reserveren");
            }

            ReserveerPersistanceManager reserveerManager = new ReserveerPersistanceManager();
            reserveerManager.ReserveerLes((Gebruiker)Session["user"], les);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult GetLesData(string id)
        {
            //parsing les(string) to les(int)
            int LesId = Int32.Parse(id);

            //getting les
            LesPersistanceManager manager = new LesPersistanceManager();
            Les les = manager.getLes(LesId);

            //check if user is logged in and if les exists
            if (les == null || Session["user"] == null)
                return RedirectToAction("Index", "Reserveren");

            CultureInfo nl = new CultureInfo("nl");
            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(les.Sportaanbod.Sportcode.ToLower());

            //creating object for serializer to serialize
            var Object = new {
                lesId = les.les_no,
	            lesNaam = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(les.Sportaanbod.Sportcode.ToLower()),
                docent = les.Sportdocent.voornaam + " " + les.Sportdocent.achternaam,
                datum = les.begintijd.Date.ToString("dddd dd MMMM yyyy"),
                tijd = les.begintijd.ToString("HH:mm") + " - " + les.eindtijd.ToString("HH:mm"),
                aantalPlaatsen = les.aantal_deelnemers,
                aantalGereserveerd = les.Reserveringen.Count
            };

            //serializing object
            string json = new JavaScriptSerializer().Serialize(Object);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}