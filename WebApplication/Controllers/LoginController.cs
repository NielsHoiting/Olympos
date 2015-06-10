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
        public ActionResult Login(string name, string password)
        {
            GebruikersPersistenceManager manager = new GebruikersPersistenceManager();
            Gebruiker gebruiker = manager.Login(name, password);
            if(gebruiker != null){
                Session["user"] = gebruiker;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}