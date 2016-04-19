using laundry.Models.DB;
using laundry.Security;
using laundry.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace laundry.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ItemMainCategoriesController : Controller
    {
       
        private LundryDbContext db = new LundryDbContext();

        // GET: ItemMainCategories
        public ActionResult Index()
        {
            return View(db.ItemMainCategories.ToList());
        }

        // GET: ItemMainCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemMainCategory category = db.ItemMainCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: ItemMainCategories/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: ItemMainCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,catName")] AddNewMainCategory category)
        {
            if (ModelState.IsValid)
            {
                ManageCateogry MC = new ManageCateogry();
                if (MC.isMainCateogryExists(category.catName) != true)
                {
                    MC.AddNewMainCat(category);
                    return RedirectToAction("Index");
                }
            }

            return View(category);
        }

        // GET: ItemMainCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemMainCategory category = db.ItemMainCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: ItemMainCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,catName")] ItemMainCategory category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: ItemMainCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemMainCategory category = db.ItemMainCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            db.ItemMainCategories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: ItemMainCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemMainCategory category = db.ItemMainCategories.Find(id);
            db.ItemMainCategories.Remove(category);
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