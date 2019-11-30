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

namespace Inzynierka.Controllers
{
    public class FilesController : Controller
    {
        private ExpoesContext db = new ExpoesContext();

        // GET: Files
        public ActionResult Index()
        {
            Company c = db.Companies.Single(p => p.Email == User.Identity.Name);
            return View(c.Files.ToList());
        }

        // GET: Files/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ID")] Files files)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase Fileq = Request.Files["Filek"];
                if (Fileq != null && Fileq.ContentLength > 0)
                {
                    Char delimiter = '.';
                    string[] pom = Fileq.FileName.ToString().Split(delimiter);
                    var Filename = System.Guid.NewGuid().ToString() + "." + pom[1];
                    Fileq.SaveAs(HttpContext.Server.MapPath("~/Images/Company/Files/") + Filename);
                    files.File = Filename;
                    files.Name = pom[0];
                }
                
                Company c = db.Companies.Single(p => p.Email == User.Identity.Name);
                c.Files.Add(files);
                db.Files.Add(files);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(files);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Files files = db.Files.Find(id);
            if (files == null)
            {
                return HttpNotFound();
            }
            return View(files);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Company c = db.Companies.Single(p => p.Email == User.Identity.Name);
            Files files = db.Files.Find(id);
            if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Company/Files/") + files.File)))
            {
                System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Company/Files/") + files.File);
            }
            c.Files.Remove(files);
            db.Files.Remove(files);
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
