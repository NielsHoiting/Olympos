using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Persistance;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("test");
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            System.Diagnostics.Debug.WriteLine("iemand logt in met sco_nummer" + username + " en wachtwoord " + password);
            GebruikersPersistenceManager manager = new GebruikersPersistenceManager();
            Gebruiker gebruiker = manager.Login(username, password);
            if(gebruiker != null){
                Session["user"] = gebruiker;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}