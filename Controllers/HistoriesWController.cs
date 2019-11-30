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
    [Authorize(Roles = "Wystawca")]
    public class HistoriesWController : Controller
    {
        private ExpoesContext db = new ExpoesContext();

        // GET: Histories
        public ActionResult List(long? id)
        {
            List<HistoryW> list = db.HistoryW.Where(p => p.Company.Email == User.Identity.Name && p.Expo.Id == id).ToList();
            foreach (HistoryW i in list)
            {
                String[] tab = i.Search.Split(':');
                String pom = tab[1];
                if (tab[0] == "Wystawca")
                {
                    Company c = db.Companies.Single(p => p.Email == pom);
                    i.Search = "<a href=\"/Companies/Details/" + c.Id + "\">" + c.CompanyName + "</a>";
                }
                else if (tab[0] == "Uczestnik")
                {
                    User c = db.Users.Single(p => p.Email == pom);
                    i.Search = "<a href=\"/Users/Details/" + c.ID + "\">" + c.SurName + " " + c.ForName + "</a>";
                }
            };
            return View(list);
        }

        // GET: Histories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoryW history = db.HistoryW.Find(id);
            String[] tab = history.Search.Split(':');
            String pom = tab[1];
            if (tab[0] == "Wystawca")
            {
                Company c = db.Companies.Single(p => p.Email == pom);
                history.Search = "<a href=\"/Companies/Details/" + c.Id + "\">" + c.CompanyName + "</a>";
            }
            else if (tab[0] == "Uczestnik")
            {
                User c = db.Users.Single(p => p.Email == pom);
                history.Search = "<a href=\"/Users/Details/" + c.ID + "\">" + c.SurName + " " + c.ForName + "</a>";
            }
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // GET: Histories/Create
        public ActionResult Create(long? id)
        {
            HistoryW h = new HistoryW();
            h.Expo = db.Expos.Single(p => p.Id == id);
            h.Company = db.Companies.Single(p => p.Email == User.Identity.Name);
            h.Search = "Wystawca:mateuszg787@gmail.com";
            h.Description = "Reklama";
            db.HistoryW.Add(h);
            db.SaveChanges();
            return RedirectToAction("List");

        }

        // POST: Histories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: Histories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoryW history = db.HistoryW.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // POST: Histories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Search,Description")] History history)
        {
            if (ModelState.IsValid)
            {
                HistoryW h = db.HistoryW.Find(history.ID);
                long ide = h.Expo.Id;
                UpdateModel(h);
                db.SaveChanges();
                return RedirectToAction("List", new { id = ide });
            }
            return View(history);
        }

        // GET: Histories/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoryW history = db.HistoryW.Find(id);
            String[] tab = history.Search.Split(':');
            String pom = tab[1];
            if (tab[0] == "Wystawca")
            {
                Company c = db.Companies.Single(p => p.Email == pom);
                history.Search = "<a href=\"/Companies/Details/" + c.Id + "\">" + c.CompanyName + "</a>";
            }
            else if (tab[0] == "Uczestnik")
            {
                User c = db.Users.Single(p => p.Email == pom);
                history.Search = "<a href=\"/Users/Details/" + c.ID + "\">" + c.SurName + " " + c.ForName + "</a>";
            }
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // POST: Histories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            HistoryW history = db.HistoryW.Find(id);
            long ide = history.Expo.Id;
            db.HistoryW.Remove(history);
            db.SaveChanges();
            return RedirectToAction("List", new { id = ide });
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
