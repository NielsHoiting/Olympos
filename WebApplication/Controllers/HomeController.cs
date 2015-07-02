using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication.Models;
using WebApplication.Persistance;
using System.Diagnostics;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        //Homepagina
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //Lessen voor persoonlijk lesoverzicht ophalen
            Gebruiker g = (Gebruiker)Session["user"];
            LesPersistanceManager manager = new LesPersistanceManager();
            List<Les> lessen = manager.getAgenda(DateTime.Now,DateTime.Now.AddDays(7), g);

            //lessen in ViewData zetten
            ViewData["lessen"] = lessen;
            ViewData["beginDate"] = DateTime.Now;
            ViewData["eindDate"] = DateTime.Now.AddDays(7);
            return View();
        }
    }
}