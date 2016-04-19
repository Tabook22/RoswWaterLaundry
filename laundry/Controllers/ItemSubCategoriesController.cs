using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using laundry.ViewModels;
using laundry.Models.DB;
using laundry.Security;

namespace laundry.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ItemSubCategoriesController : Controller
    {
        private LundryDbContext db = new LundryDbContext();

        // GET: ItemSubCategories
        public ActionResult Index()
        {
            var itemSubCategories = from M in db.ItemMainCategories
                                    join S in db.ItemSubCategories on M.Id equals S.MId
                                    select new CatSubViewData
                                    {
                                        Id =S.Id,
                                        catName =S.catName,
                                        MId=M.Id ,
                                        catMName =M.catName
                                    };
            return View(itemSubCategories.ToList());
        }

        // GET: ItemSubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemSubCategory itemSubCategory = db.ItemSubCategories.Find(id);
            if (itemSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(itemSubCategory);
        }

        // GET: ItemSubCategories/Create
        public ActionResult Create()
        {
            ViewBag.MId = new SelectList(db.ItemMainCategories, "Id", "catName");
            return View();
        }

        // POST: ItemSubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,catName,MId")] ItemSubCategory itemSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.ItemSubCategories.Add(itemSubCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MId = new SelectList(db.ItemMainCategories, "Id", "catName", itemSubCategory.MId);
            return View(itemSubCategory);
        }

        // GET: ItemSubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemSubCategory itemSubCategory = db.ItemSubCategories.Find(id);
            if (itemSubCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.MId = new SelectList(db.ItemMainCategories, "Id", "catName", itemSubCategory.MId);
            return View(itemSubCategory);
        }

        // POST: ItemSubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,catName,MId")] ItemSubCategory itemSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MId = new SelectList(db.ItemMainCategories, "Id", "catName", itemSubCategory.MId);
            return View(itemSubCategory);
        }

        // GET: ItemSubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemSubCategory itemSubCategory = db.ItemSubCategories.Find(id);
            if (itemSubCategory == null)
            {
                return HttpNotFound();
            }
            db.ItemSubCategories.Remove(itemSubCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: ItemSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemSubCategory itemSubCategory = db.ItemSubCategories.Find(id);
            db.ItemSubCategories.Remove(itemSubCategory);
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
