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


namespace Inzynierka.Controllers
{
    public class ExpoesController : Controller
    {
        private ExpoesContext db = new ExpoesContext();

        // GET: Expoes
        [Authorize(Roles = "User,Wystawca")]
        public ActionResult Join(long? id)
        {
            bool exC = db.Companies.Any(p => p.Email == User.Identity.Name);
            if (exC == true)
            {
                var Expo = db.Expos.Single(p => p.Id == id);
                Invite i = new Invite()
                {
                    Email = User.Identity.Name,
                    Expo = Expo,
                    Status = status.send
                };
                var Usr = db.Companies.Single(p => p.Email == User.Identity.Name);
                
                Expo.Company.Add(Usr);
                Usr.Expo.Add(Expo);
                db.Entry(Usr).State = EntityState.Modified;
                db.Entry(Expo).State = EntityState.Modified;
                db.SaveChanges();
            }
            bool exU = db.Users.Any(p => p.Email == User.Identity.Name);
            if (exU == true)
            {
                var Usr = db.Users.Single(p => p.Email == User.Identity.Name);
                var Expo = db.Expos.Single(p => p.Id == id);
                Expo.Users.Add(Usr);
                Usr.Expo.Add(Expo);
                db.Entry(Usr).State = EntityState.Modified;
                db.Entry(Expo).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StartData = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.EndData = sortOrder == "DateEnd" ? "dateend_desc" : "DateEnd";
            ViewBag.Invites = db.Invites;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var ex = from e in db.Expos
                           select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                ex = ex.Where(s => s.Name_Expo.Contains(searchString)
                                       || s.Description.Contains(searchString)||s.Adres.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    ex = ex.OrderByDescending(s => s.Name_Expo);
                    break;
                case "Date":
                    ex = ex.OrderBy(s => s.ExpoStartData);
                    break;
                case "date_desc":
                    ex = ex.OrderByDescending(s => s.ExpoStartData);
                    break;
                case "DateEnd":
                    ex = ex.OrderBy(s => s.ExpoEndData);
                    break;
                case "dateend_desc":
                    ex = ex.OrderByDescending(s => s.ExpoEndData);
                    break;
                default:
                    ex = ex.OrderBy(s => s.Name_Expo);
                    break;
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(ex.ToPagedList(pageNumber, pageSize));
        }

        // GET: Expoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expo expo = db.Expos.Find(id);
            if (expo == null)
            {
                return HttpNotFound();
            }
            return View(expo);
        }

        // GET: Expoes/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(long promoterid)
        {
            ViewBag.promoterid = promoterid;
            return View();
        }
        // GET: Expoes/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateE()
        {
            return View();
        }


        // POST: Expoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name_Expo,Adres,Description,ExpoStartData,ExpoEndData")] Expo expo)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["plikZObrazkiem"];
                if (file != null && file.ContentLength > 0)
                {
                    expo.Photo = System.Guid.NewGuid().ToString()+".jpg";
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/Expo/") + expo.Photo);
                }
                HttpPostedFileBase file1 = Request.Files["layouts"];
                if (file1 != null && file1.ContentLength > 0)
                {
                    expo.pdflayout = System.Guid.NewGuid().ToString()+".png";
                    file1.SaveAs(HttpContext.Server.MapPath("~/Images/Expo/pdf/") + expo.pdflayout);
                }
                HttpPostedFileBase file9 = Request.Files["Map"];
                if (file9 != null && file9.ContentLength > 0)
                {
                    expo.MapPhoto = System.Guid.NewGuid().ToString() + ".jpg";
                    file9.SaveAs(HttpContext.Server.MapPath("~/Images/Expo/") + expo.MapPhoto);
                }
                UpdateModel(expo);
                if (long.Parse(Request["promoterid"].ToString()) !=null){
                    long id = long.Parse(Request["promoterid"].ToString());
                    expo.Promoter = id;
                    UpdateModel(expo);
                }
                db.Expos.Add(expo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expo);
        }
        // POST: Expoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateE([Bind(Include = "Id,Name_Expo,Adres,Description,ExpoStartData,ExpoEndData")] Expo expo)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["plikZObrazkiem"];
                if (file != null && file.ContentLength > 0)
                {
                    expo.Photo = System.Guid.NewGuid().ToString() + ".jpg";
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/Expo/") + expo.Photo);
                }
                HttpPostedFileBase file1 = Request.Files["layouts"];
                if (file1 != null && file1.ContentLength > 0)
                {
                    expo.pdflayout = System.Guid.NewGuid().ToString() + ".png";
                    file1.SaveAs(HttpContext.Server.MapPath("~/Images/Expo/pdf/") + expo.pdflayout);
                }
                HttpPostedFileBase file9 = Request.Files["Map"];
                if (file9 != null && file9.ContentLength > 0)
                {
                    expo.MapPhoto = System.Guid.NewGuid().ToString() + ".jpg";
                    file9.SaveAs(HttpContext.Server.MapPath("~/Images/Expo/") + expo.MapPhoto);
                }
                UpdateModel(expo);
                db.Expos.Add(expo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expo);
        }
        // GET: Expoes/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expo expo = db.Expos.Find(id);
            if (expo == null)
            {
                return HttpNotFound();
            }
            return View(expo);
        }

        // POST: Expoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name_Expo,Adres,Description,ExpoStartData,ExpoEndData")] Expo expo)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["plikzObrazkiem"];
                if (file != null && file.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Expo/") + expo.Photo)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Expo/") + expo.Photo);
                    }
                    expo.Photo = System.Guid.NewGuid().ToString()+".jpg";
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/Expo/") + expo.Photo);
                }
                HttpPostedFileBase file1 = Request.Files["layouts"];
                if (file != null && file.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Expo/pdf/") + expo.pdflayout)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Expo/pdf") + expo.pdflayout);
                    }
                    expo.pdflayout = System.Guid.NewGuid().ToString() + ".png";
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/Expo/pdf/") + expo.pdflayout);
                }
                HttpPostedFileBase file9 = Request.Files["Map"];
                if (file9 != null && file9.ContentLength > 0)
                {
                    if ((System.IO.File.Exists(HttpContext.Server.MapPath("~/Images/Expo/") + expo.MapPhoto)))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/Expo/") + expo.MapPhoto);
                    }
                    expo.MapPhoto = System.Guid.NewGuid().ToString() + ".jpg";
                    file9.SaveAs(HttpContext.Server.MapPath("~/Images/Expo/") + expo.MapPhoto);
                }
                UpdateModel(expo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expo);
        }

        // GET: Expoes/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expo expo = db.Expos.Find(id);
            if (expo == null)
            {
                return HttpNotFound();
            }
            return View(expo);
        }

        // POST: Expoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expo expo = db.Expos.Find(id);
            db.Expos.Remove(expo);
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
