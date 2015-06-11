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
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            LesPersistanceManager manager = new LesPersistanceManager();
            Gebruiker g = (Gebruiker)Session["user"];
            List<Les> lessen = manager.getAgenda(0, g);
            ViewData["lessen"] = lessen;
            ViewData["beginDate"] = DateTime.Now;
            return View();
        }

        public ActionResult SelectCalendarWeek(string week)   
        {
            System.Diagnostics.Debug.WriteLine("test");
            return null;
        }
    }
}