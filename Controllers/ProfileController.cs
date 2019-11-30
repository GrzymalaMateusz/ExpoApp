using Inzynierka.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inzynierka.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            ExpoesContext expo = new ExpoesContext();
            bool exC = expo.Companies.Any(p => p.Email == User.Identity.Name);
            if (exC == true)
            {
                var Usr = expo.Companies.Single(p => p.Email == User.Identity.Name);
                return RedirectToAction("Details", "Companies", new { id = Usr.Id });
            }
            bool exU = expo.Users.Any(p => p.Email == User.Identity.Name);
            if (exU == true)
            {
                var Usr = expo.Users.Single(p => p.Email == User.Identity.Name);
                return RedirectToAction("Details", "Users", new { id = Usr.ID });
            }
            return View();
        }
        public ActionResult MyExpos()
        {
            ExpoesContext expo = new ExpoesContext();
            bool exC = expo.Companies.Any(p => p.Email == User.Identity.Name);
            if (exC == true)
            {
                var Usr = expo.Companies.Single(p => p.Email == User.Identity.Name);
                return RedirectToAction("MyExpos", "Companies", new { id = Usr.Id });
            }
            bool exU = expo.Users.Any(p => p.Email == User.Identity.Name);
            if (exU == true)
            {
                var Usr = expo.Users.Single(p => p.Email == User.Identity.Name);
                return RedirectToAction("MyExpos", "Users", new { id = Usr.ID });
            }
            bool exP = expo.Promoters.Any(p => p.Email == User.Identity.Name);
            if (exP == true)
            {
                var Usr = expo.Promoters.Single(p => p.Email == User.Identity.Name);
                return RedirectToAction("MyExpos", "promoters", new { id = Usr.Id });
            }
            return View();
        }
    }
}