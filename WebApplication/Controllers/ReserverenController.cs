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
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            LesPersistanceManager manager = new LesPersistanceManager();
            Gebruiker g = (Gebruiker)Session["user"];
            List<Les> LastMinuteLessen = manager.getLastMinuteLessen(g);
            System.Diagnostics.Debug.WriteLine(LastMinuteLessen.Count);
            ViewData["LastMinuteLessen"] = LastMinuteLessen;
            return View();
        }

        public ActionResult Les(int id)
        {
            LesPersistanceManager manager = new LesPersistanceManager();
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

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
            return Json(json);
        }
    }
}