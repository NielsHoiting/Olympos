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
            return View();
        }

        [HttpPost]
        public ActionResult Login(string gebruikersnaam, string wachtwoord, bool onthouden)
        {
            AccountPersistenceManager manager = new AccountPersistenceManager();
            Gebruiker gebruiker = manager.Login(gebruikersnaam, wachtwoord);

            if(gebruiker != null){
                Session["user"] = gebruiker;
                // todo: Ingelogd blijven cookie genereren en toevoegen
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["message"] = "Wachtwoord/SCO-Nummer onjuist";
                return RedirectToAction("Index", "Login");
            }
            
        }
    }
}