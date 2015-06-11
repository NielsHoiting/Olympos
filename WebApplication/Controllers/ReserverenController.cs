using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Persistance;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ReserverenController : Controller
    {
        public ActionResult Index()
        {
            LesPersistanceManager manager = new LesPersistanceManager();
            Gebruiker g = (Gebruiker)Session["user"];
            List<Les> LastMinuteLessen = manager.getLastMinuteLessen(g);
            System.Diagnostics.Debug.WriteLine(LastMinuteLessen.Count);
            ViewData["LastMinuteLessen"] = LastMinuteLessen;
            return View();
        }

        public ActionResult lesDetail()
        {
            return View();
        }
    }
}