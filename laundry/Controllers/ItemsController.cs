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
    public class ItemsController : Controller
    {
        private LundryDbContext db = new LundryDbContext();

        // GET: Items
        public ActionResult Index()
        {
            var getItem = from I in db.Items
                          join M in db.ItemMainCategories on I.MId equals M.Id
                          join S in db.ItemSubCategories on I.SId equals S.Id
                          select new ItemViewModel()
                          {
                              ItemId = I.ItemId,
                              MId = I.MId,
                              McatName = M.catName,
                              SId = I.SId,
                              ScatName = S.catName,
                              ItemName = I.ItemName,
                              itemImg = I.itemImg,
                              Price = I.Price,
                              Description = I.Description
                          };
            //var items = db.Items.Include(i => i.ItemMainCategory).Include(i => i.SId);
           // var items = db.Items.OrderBy(x => x.ItemName);
            return View(getItem.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            //The Edit View is expecting a model of type laundry.ViewModels.ItemViewModel.
            //To prevent MVC from getting confused, and to avoid sending a model different than the view expecting. 
            //We have to create an instance of ItemViewModel and then pass to it the values we need to display in the view

            ItemViewModel itmVM = new ItemViewModel();
            var getItem = from I in db.Items
                          where I.ItemId == id
                          join M in db.ItemMainCategories on I.MId equals M.Id
                          join S in db.ItemSubCategories on I.SId equals S.Id
                          select new
                          {
                              ItemId = I.ItemId,
                              MId = I.MId,
                              McatName = M.catName,
                              SId = I.SId,
                              ScatName = S.catName,
                              ItemName = I.ItemName,
                              itemImg = I.itemImg,
                              Price = I.Price,
                              Description = I.Description
                          };



            // Item item = db.Items.Find(id);
            if (getItem == null)
            {
                return HttpNotFound();
            }
            foreach (var itm in getItem)
            {


                itmVM.ItemId = itm.ItemId;
                itmVM.MId = itm.MId;
                itmVM.McatName = itm.McatName;
                itmVM.SId = itm.SId;
                itmVM.ScatName = itm.ScatName;
                itmVM.ItemName = itm.ItemName;
                itmVM.itemImg = itm.itemImg;
                itmVM.Price = itm.Price;
                itmVM.Description = itm.Description;

                ViewBag.MId = new SelectList(db.ItemMainCategories, "Id", "catName", itm.MId);
                ViewBag.SId = itm.SId;
            }

            //ViewBag.SId = new SelectList(db.ItemSubCategories, "Id", "catName", item.SId);

            return View(itmVM);
        }

        // GET: Items/Create
        public ActionResult Create()
        {

            ViewBag.MId = new SelectList(db.ItemMainCategories, "Id", "catName");

            // ViewBag.SId = new SelectList(db.ItemSubCategories, "Id", "catName");
            return View();
        }

        //TODO: ItemSubCategory list and then send the result ans JSON fromat
        public ActionResult getSubCategory(int mcat)
        {

            var getSubCat = from n in db.ItemSubCategories
                            where n.MId == mcat
                            select new { n.catName, n.Id };

            return Json(getSubCat.ToList(), JsonRequestBehavior.AllowGet);
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,MId,SId,ItemName,itemImg,Price,Description")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MId = new SelectList(db.ItemMainCategories, "Id", "catName", item.MId);
            ViewBag.SId = new SelectList(db.ItemSubCategories, "Id", "catName", item.SId);
            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            //The Edit View is expecting a model of type laundry.ViewModels.ItemViewModel.
            //To prevent MVC from getting confused, and to avoid sending a model different than the view expecting. 
            //We have to create an instance of ItemViewModel and then pass to it the values we need to display in the view

            ItemViewModel itmVM = new ItemViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var getItem = from I in db.Items
                          where I.ItemId == id
                          join M in db.ItemMainCategories on I.MId equals M.Id
                          join S in db.ItemSubCategories on I.SId equals S.Id
                          select new
                          {
                              ItemId = I.ItemId,
                              MId = I.MId,
                              McatName = M.catName,
                              SId = I.SId,
                              ScatName = S.catName,
                              ItemName = I.ItemName,
                              itemImg = I.itemImg,
                              Price = I.Price,
                              Description = I.Description
                          };



            // Item item = db.Items.Find(id);
            if (getItem == null)
            {
                return HttpNotFound();
            }
            foreach (var itm in getItem)
            {


                itmVM.ItemId = itm.ItemId;
                itmVM.MId = itm.MId;
                itmVM.McatName = itm.McatName;
                itmVM.SId = itm.SId;
                itmVM.ScatName = itm.ScatName;
                itmVM.ItemName = itm.ItemName;
                itmVM.itemImg = itm.itemImg;
                itmVM.Price = itm.Price;
                itmVM.Description = itm.Description;

                ViewBag.MId = new SelectList(db.ItemMainCategories, "Id", "catName", itm.MId);
                ViewBag.SId = itm.SId;
            }

            //ViewBag.SId = new SelectList(db.ItemSubCategories, "Id", "catName", item.SId);

            return View(itmVM);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemViewModel itmvm)
        {
            if (ModelState.IsValid)
            {
                Item itm = db.Items.Where(x => x.ItemId == itmvm.ItemId).FirstOrDefault();

                itm.MId = itmvm.MId;
                itm.SId = itmvm.SId;
                itm.ItemName = itmvm.ItemName;
                itm.itemImg = itmvm.itemImg;
                itm.Price = itmvm.Price;
                itm.Description = itmvm.Description;


                //db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MId = new SelectList(db.ItemMainCategories, "Id", "catName", itmvm.MId);
            ViewBag.SId = new SelectList(db.ItemSubCategories, "Id", "catName", itmvm.SId);
            return View(itmvm);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
       
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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

        public JsonResult getItemCosts(int id)
        {
            LundryDbContext db = new LundryDbContext();
            var getItmCost = db.Items.Where(x => x.ItemId == id).FirstOrDefault();
            return new JsonResult { Data = getItmCost, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
