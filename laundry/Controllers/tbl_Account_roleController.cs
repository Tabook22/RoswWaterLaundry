using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using laundry.Models.DB;
using laundry.Security;

namespace laundry.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class tbl_Account_roleController : Controller
    {
        private LundryDbContext db = new LundryDbContext();

        // GET: tbl_Account_role
        public ActionResult Index()
        {
            var tbl_Account_role = db.tbl_Account_role.Include(t => t.account).Include(t => t.role);
            return View(tbl_Account_role.ToList());
        }

        // GET: tbl_Account_role/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Account_role tbl_Account_role = db.tbl_Account_role.Find(id);
            if (tbl_Account_role == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Account_role);
        }

        // GET: tbl_Account_role/Create
        public ActionResult Create()
        {
            ViewBag.AID = new SelectList(db.tbl_Account, "AID", "Username");
            ViewBag.RID = new SelectList(db.tbl_Role, "RID", "RoleName");
            return View();
        }

        // POST: tbl_Account_role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AID,RID")] tbl_Account_role tbl_Account_role)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Account_role.Add(tbl_Account_role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AID = new SelectList(db.tbl_Account, "AID", "Username", tbl_Account_role.AID);
            ViewBag.RID = new SelectList(db.tbl_Role, "RID", "RoleName", tbl_Account_role.RID);
            return View(tbl_Account_role);
        }

        // GET: tbl_Account_role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Account_role tbl_Account_role = db.tbl_Account_role.Find(id);
            if (tbl_Account_role == null)
            {
                return HttpNotFound();
            }
            ViewBag.AID = new SelectList(db.tbl_Account, "AID", "Username", tbl_Account_role.AID);
            ViewBag.RID = new SelectList(db.tbl_Role, "RID", "RoleName", tbl_Account_role.RID);
            return View(tbl_Account_role);
        }

        // POST: tbl_Account_role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AID,RID")] tbl_Account_role tbl_Account_role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Account_role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AID = new SelectList(db.tbl_Account, "AID", "Username", tbl_Account_role.AID);
            ViewBag.RID = new SelectList(db.tbl_Role, "RID", "RoleName", tbl_Account_role.RID);
            return View(tbl_Account_role);
        }

        // GET: tbl_Account_role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Account_role tbl_Account_role = db.tbl_Account_role.Find(id);
            if (tbl_Account_role == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Account_role);
        }

        // POST: tbl_Account_role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Account_role tbl_Account_role = db.tbl_Account_role.Find(id);
            db.tbl_Account_role.Remove(tbl_Account_role);
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
