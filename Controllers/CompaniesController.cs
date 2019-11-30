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
using PagedList;
using Microsoft.AspNet.Identity;

namespace Inzynierka.Controllers
{
    public class CompaniesController : Controller
    {
        private ExpoesContext db = new ExpoesContext();

        // GET: Companies
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }
        // GET: Companies
        [Authorize(Roles = "Administrator,Organizator")]
        public ActionResult List(long? id)
        {
            return View(db.Companies.Where(p => p.Expo.Any(a => a.Id == id)).ToList());
        }
        // GET: Users
        [Authorize(Roles = "Wystawca")]
        public ActionResult MyExpos(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company c = db.Companies.Find(id);
            if (c == null)
            {
                return HttpNotFound();
            }
            return View(c.Expo.ToList());
        }
        // GET: Users
        [Authorize(Roles = "Wystawca")]
        public ActionResult MyHistory(long? ids)
        {
            return RedirectToAction("List", "HistoriesW", new { id = ids });
        }
        // GET: Companies/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,CompanyName,CompanyFullName,CompanyAbout,ProductsAbout,CompanyLogo,StandPhoto,Photo1,Photo2,Photo3,Photo4,Photo5,Facebook,Instagram,Snapchat,Youtube,Phone,ContactEmail,www,Adress,NIP,ForName_And_SurName,UserPhone,User_ID")] Company company)
        {
            /* if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }*/
            try
            {
                ExpoesContext expo = new ExpoesContext();
                Company c = expo.Companies.Single(p => p.Email == User.Identity.Name);
                HttpPostedFileBase logos = Request.Files["Logo"];
                if (logos != null && logos.ContentLength > 0)
                {
                    Char delimiter = '.';
                    string[] pom = logos.FileName.ToString().Split(delimiter);
                    var Filename = System.Guid.NewGuid().ToString() + "." + pom[1];
                    c.CompanyLogo = Filename;
                    logos.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Stand = Request.Files["Stand"];
                if (Stand != null && Stand.ContentLength > 0)
                {
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.StandPhoto = Filename;
                    Stand.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo = Request.Files["Photos1"];
                if (Photo != null && Photo.ContentLength > 0)
                {
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo1 = Filename;
                    Photo.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo2 = Request.Files["Photos2"];
                if (Photo2 != null && Photo2.ContentLength > 0)
                {
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo2 = Filename;
                    Photo2.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo3 = Request.Files["Photos3"];
                if (Photo3 != null && Photo3.ContentLength > 0)
                {
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo3 = Filename;
                    Photo3.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo4 = Request.Files["Photos4"];
                if (Photo4 != null && Photo4.ContentLength > 0)
                {
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo4 = Filename;
                    Photo4.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo5 = Request.Files["Photos5"];
                if (Photo5 != null && Photo5.ContentLength > 0)
                {
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo5 = Filename;
                    Photo5.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                UpdateModel(c);
                expo.SaveChanges();
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                //await AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
            catch (Exception exc)
            {
                return View(company);
            }
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "Administrator,Wystawca")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,CompanyName,CompanyFullName,CompanyAbout,ProductsAbout,CompanyLogo,StandPhoto,Photo1,Photo2,Photo3,Photo4,Photo5,Facebook,Instagram,Snapchat,Youtube,Phone,ContactEmail,www,Adress,NIP,ForName_And_SurName,UserPhone,User_ID")] Company company)
        {
            /* if (ModelState.IsValid)
             {
                 db.Entry(company).State = EntityState.Modified;
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }*/
            try
            {
                ExpoesContext expo = new ExpoesContext();
                Company c = expo.Companies.Single(p => p.Id == company.Id);
                HttpPostedFileBase logos = Request.Files["Logo"];
                if (logos != null && logos.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Company/") + c.CompanyLogo)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Company/") + c.CompanyLogo);
                    }
                    Char delimiter = '.';
                    string[] pom=logos.FileName.ToString().Split(delimiter);
                    var Filename = System.Guid.NewGuid().ToString() + "."+pom[1];
                    c.CompanyLogo = Filename;
                    logos.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Stand = Request.Files["Stand"];
                if (Stand != null && Stand.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Company/") + c.StandPhoto)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Company/") + c.StandPhoto);
                    }
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.StandPhoto = Filename;
                    Stand.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo = Request.Files["Photos1"];
                if (Photo != null && Photo.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo1)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo1);
                    }
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo1 = Filename;
                    Photo.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo2 = Request.Files["Photos2"];
                if (Photo2 != null && Photo2.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo2)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo2);
                    }
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo2 = Filename;
                    Photo2.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo3 = Request.Files["Photos3"];
                if (Photo3 != null && Photo3.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo3)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo3);
                    }
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo3 = Filename;
                    Photo3.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo4 = Request.Files["Photos4"];
                if (Photo4 != null && Photo4.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo4)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo4);
                    }
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo4 = Filename;
                    Photo4.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                HttpPostedFileBase Photo5 = Request.Files["Photos5"];
                if (Photo5 != null && Photo5.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo5)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Company/") + c.Photo5);
                    }
                    var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                    c.Photo5 = Filename;
                    Photo5.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
                }
                UpdateModel(c);
                expo.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception exc)
            {
                return View(company);
            }
        }

        // GET: Companies/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
