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
    public class InvitesController : Controller
    {
        private ExpoesContext db = new ExpoesContext();

        // GET: Invites
        public ActionResult InviteE(long id)
        {
            ViewBag.ExpoId = id;
            return View(db.Invites.Where(p=>p.Expo.Id==id).ToList());
        }
        public ActionResult InviteC()
        {
            List<Invite> list = db.Invites.Where(p => p.Email == User.Identity.Name).ToList();
            foreach(var item in list)
            {
                Expo ex=db.Expos.Where(p => p.Invites.Any(s => s.Id == item.Id)).First();
                item.Expo = ex;
            }
            return View(list);
        }
        // GET: Invites/Details/5
        public ActionResult approve(int id)
        {
            Invite i = db.Invites.Find(id);
            i.Status = status.approved;
            Expo ex = db.Expos.Where(p => p.Invites.Any(s => s.Id == i.Id)).First();
            i.Expo = ex;
            bool exC = db.Companies.Any(p => p.Email ==i.Email);
            if (exC == true)
            {
                var Usr = db.Companies.Single(p => p.Email == i.Email);
                ex.Company.Add(Usr);
                Usr.Expo.Add(ex);
                db.Entry(Usr).State = EntityState.Modified;
                db.Entry(ex).State = EntityState.Modified;
            }    
            db.SaveChanges();
            return RedirectToAction("InviteE", "Invites", new { @id=ex.Id});
        }
        public ActionResult canceled(int id)
        {
            Invite i = db.Invites.Find(id);
            Expo ex = db.Expos.Where(p => p.Invites.Any(s => s.Id == i.Id)).First();
            i.Status = status.canceled;
            UpdateModel(i);
            db.SaveChanges();
            return RedirectToAction("InviteE", "Invites", new { id = ex.Id });
        }
        public ActionResult approveT(int id)
        {
            Invite i = db.Invites.Find(id);
            i.Status = status.approved;
            Expo ex = db.Expos.Where(p => p.Invites.Any(s => s.Id == i.Id)).First();
            i.Expo = ex;
            bool exC = db.Companies.Any(p => p.Email == i.Email);
            if (exC == true)
            {
                var Usr = db.Companies.Single(p => p.Email == i.Email);
                ex.Company.Add(Usr);
                Usr.Expo.Add(ex);
                db.Entry(Usr).State = EntityState.Modified;
                db.Entry(ex).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("InviteC", "Invites", new { });
        }
        public ActionResult canceledT(int id)
        {
            Invite i = db.Invites.Find(id);
            i.Status = status.canceled;
            UpdateModel(i);
            db.SaveChanges();
            return RedirectToAction("InviteC", "Invites", new { });
        }
        public ActionResult send(int id)
        {
            Expo ex = db.Expos.Find(id);
            Invite i = new Invite()
            {
                Email = User.Identity.Name,
                Expo=ex,
                Status=status.send
            };
            i = db.Invites.Add(i);
            ex.Invites.Add(i);
            UpdateModel(ex);
            db.SaveChanges();
            return RedirectToAction("Index", "Expoes", new {  });
        }

        // GET: Invites/Create
        public ActionResult Create(int id)
        {
            ViewBag.ExpoId = id;
            return View();
        }

        // POST: Invites/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Status")] Invite invite,long Expo)
        {
            
            if (ModelState.IsValid)
            {
                invite.Status = status.invite;
                Expo ex = db.Expos.Find(Expo);
                invite.Expo = ex;
                Invite i=db.Invites.Add(invite);
                db.SaveChanges();
                ex.Invites.Add(i);
                db.Entry(invite).State = EntityState.Modified;
                db.Entry(ex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("InviteE","Invites",new { @id=ex.Id});
            }

            return View(invite);
        }

      

        // GET: Invites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = db.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            return View(invite);
        }

        // POST: Invites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invite invite = db.Invites.Find(id);
            Expo ex = db.Expos.Where(p => p.Invites.Any(s => s.Id == invite.Id)).First();
            db.Invites.Remove(invite);
            db.SaveChanges();
            return RedirectToAction("InviteE", "Invites", new { @id = ex.Id });
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
