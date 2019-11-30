using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inzynierka.DAL;
using Inzynierka.Models;
using Microsoft.AspNet.Identity;

namespace Inzynierka.Controllers
{
    public class UsersController : Controller
    {
        private ExpoesContext db = new ExpoesContext();

        // GET: Users
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        // GET: Users
        [Authorize(Roles = "Administrator,Organizator")]
        public ActionResult List(long? id)
        {
            return View(db.Users.Where(p=>p.Expo.Any(a=>a.Id==id)).ToList());
        }
        // GET: Users
        [Authorize(Roles = "User")]
        public ActionResult MyExpos(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user.Expo.ToList());
        }
        // GET: Users
        [Authorize(Roles = "User")]
        public ActionResult MyHistory(long? ids)
        {
            return RedirectToAction("List","Histories",new { id=ids});
        }
        // GET: Users/Details/5
        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Email,ForName,SurName,Photo,Phone,Country,Language,CompanyName")] User user)
        {
            /*if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);*/
            try
            {
                ExpoesContext expo = new ExpoesContext();
                var c = expo.Users.Single(p => p.Email == User.Identity.Name);
                HttpPostedFileBase file = Request.Files["Photos"];
                if (file != null && file.ContentLength > 0)
                {
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo = Filename;
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/User/") + Filename);
                }
                UpdateModel(c);
                expo.SaveChanges();
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                //await AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login","Account");
            }
            catch (Exception exc)
            {
                return View(user);
            }
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Administrator,User")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Email,ForName,SurName,Photo,Phone,Country,Language,CompanyName")] User user)
        {
            /*if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);*/
            try
            {
                ExpoesContext expo = new ExpoesContext();
                var c = expo.Users.Single(p => p.ID == user.ID);
                HttpPostedFileBase file = Request.Files["Photos"];
                if (file != null && file.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/User/") + c.Photo)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/User/") + c.Photo);
                    }
                    var Filename= System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo = Filename;
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/User/") + Filename);
                }
                UpdateModel(c);
                expo.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception exc)
            {
                return View(user);
            }
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
