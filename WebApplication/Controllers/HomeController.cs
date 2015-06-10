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
using System.Diagnostics;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var sconummer = model.ScoNr;
            var password = model.Password;

            System.Diagnostics.Debug.WriteLine("Nr" + sconummer);
            System.Diagnostics.Debug.WriteLine("Pass:" + password);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            

            return View(model);
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.Doos = "Hoi";

            return View();
        }

        public ActionResult Reserveren()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}