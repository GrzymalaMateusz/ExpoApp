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
    public class promotersController : Controller
    {
        private ExpoesContext db = new ExpoesContext();

        // GET: promoters
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.Promoters.ToList());
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult List(long? id)
        {
            var promoter = db.Promoters.Single(p => p.Id == id);
            return View(db.Expos.Where(p=>p.Promoter==promoter.Id).ToList());
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult AddExpo(long? id)
        {
           
            promoter p = db.Promoters.Find(id);
            return RedirectToAction("Create","Expoes",new { promoterid=p.Id});
        }
        [Authorize(Roles = "Administrator,Organizator")]
        public ActionResult MyExpos(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promoter p = db.Promoters.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(db.Expos.Where(pr => pr.Promoter == p.Id).ToList());
        }
        // GET: promoters/Details/5
        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promoter promoter = db.Promoters.Find(id);
            if (promoter == null)
            {
                return HttpNotFound();
            }
            return View(promoter);
        }

        // GET: promoters/Create
        public ActionResult Create(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promoter promoter1 = db.Promoters.Find(id);
            if (promoter1 == null)
            {
                return HttpNotFound();
            }
            return View(promoter1);
        }

        // POST: promoters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Forname,Surname")] promoter promoter)
        {
            /*if (ModelState.IsValid)
            {
                db.Promoters.Add(promoter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(promoter);*/
            try
            {
                ExpoesContext expo = new ExpoesContext();
                var c = expo.Promoters.Single(p => p.Email == User.Identity.Name);
                UpdateModel(c);
                expo.SaveChanges();
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                //await AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
            catch (Exception exc)
            {
                return View(promoter);
            }
        }

        // GET: promoters/Edit/5
        [Authorize(Roles = "Administrator,Organizator")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promoter promoter = db.Promoters.Find(id);
            if (promoter == null)
            {
                return HttpNotFound();
            }
            return View(promoter);
        }

        // POST: promoters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Forname,Surname")] promoter promoter)
        {
            try
            {
                ExpoesContext expo = new ExpoesContext();
                var c = expo.Promoters.Single(p => p.Id == promoter.Id);
                UpdateModel(c);
                expo.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception exc)
            {
                return View(promoter);
            }
        }

        // GET: promoters/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promoter promoter = db.Promoters.Find(id);
            if (promoter == null)
            {
                return HttpNotFound();
            }
            return View(promoter);
        }

        // POST: promoters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            promoter promoter = db.Promoters.Find(id);
            db.Promoters.Remove(promoter);
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
