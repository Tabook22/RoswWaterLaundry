using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using laundry.ViewModels;
using laundry.Models.DB;
using laundry.Security;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace laundry.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class BillsController : Controller
    {
        private LundryDbContext db = new LundryDbContext();

        // GET: Bills
        public ActionResult Index()
        {

            return View(db.Bills.ToList());
        }
        //this action is used only to fill the table of items which were selected by the customers when the page is loaded
        public ActionResult getTempItmList()
        {
            var itmlst = from itm in db.Items
                         join bls in db.tempBills on itm.ItemId equals bls.ItemId
                         join cst in db.Customers on bls.CustId equals cst.CustId
                         select new TempBillsDetails { Id = bls.Id, CustId = bls.CustId, CustName = cst.CustName, Tel = cst.Tel, ItemId = bls.ItemId, ItemName = itm.ItemName, Qyt = bls.Qyt, Cost = bls.Cost, Date = bls.Date };

            return Json(itmlst.ToList(), JsonRequestBehavior.AllowGet);
        }

        //Add Temprory Bills

        public ActionResult AddTemp(int custId, int itemId, int qyt, decimal cost, int MId, int SId)
        {

            try
            {
                ////TODO:Save the data to Bills
                //int maxBillNo = db.Bills.Max(x => x.BillNo); // Get the Maximum BillNo 
                // Bill bill=new Bill();
                // bill.CustId = custId;
                // bill.ItemId = itemId;
                // bill.Date = DateTime.Now;
                // bill.Qyt = qyt;
                // bill.Cost = cost;
                ////bill.BillNo = maxBillNo + 1; // Increasing the BillNo by 1
                // db.Bills.Add(bill);
                // db.SaveChanges();


                // TODO: Save Data to tempBill
                tempBill tb = new tempBill();
                tb.CustId = custId;
                tb.ItemId = itemId;
                tb.Date = DateTime.Now;
                tb.Qyt = qyt;
                tb.Cost = cost;
                tb.MId = MId;
                tb.SId = SId;
                db.tempBills.Add(tb);
                db.SaveChanges();

                var itmlst = (from itm in db.Items
                              join bls in db.tempBills on itm.ItemId equals bls.ItemId
                              join cst in db.Customers on bls.CustId equals cst.CustId
                              select new TempBillsDetails { Id = bls.Id, CustId = bls.CustId, CustName = cst.CustName, ItemId = bls.ItemId, ItemName = itm.ItemName, Qyt = bls.Qyt, Cost = bls.Cost, Date = bls.Date }).ToList();

                return Json(itmlst, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return RedirectToAction("Create");
            }
        }

        // GET: Bills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // delete from temptable
        public JsonResult DeleteTemp(int id)
        {
            // int getId = int.Parse(id);
            tempBill tnv = new tempBill();
            tnv = db.tempBills.Where(x => x.Id == id).FirstOrDefault();
            db.tempBills.Remove(tnv);
            db.SaveChanges();

            var itmlst = from itm in db.Items
                         join bls in db.tempBills on itm.ItemId equals bls.ItemId
                         join cst in db.Customers on bls.CustId equals cst.CustId
                         select new TempBillsDetails { Id = bls.Id, CustId = bls.CustId, CustName = cst.CustName, ItemId = bls.ItemId, ItemName = itm.ItemName, Qyt = bls.Qyt, Cost = bls.Cost, Date = bls.Date };

            return Json(itmlst.ToList(), JsonRequestBehavior.AllowGet);
        }


        // GET: Bills/Create
        public ActionResult Create()
        {

            var getAllUsers = db.Customers.OrderBy(x => x.CustName).Select(x => new { CustId = x.CustId, CustName = x.CustName });
            var itemlst = (from itm in db.Items
                           orderby itm.ItemId descending
                           select itm).ToList();
            ViewBag.AllCust = new SelectList(getAllUsers, "CustId", "CustName");
            ViewBag.itmlst = new SelectList(db.Items, "ItemId", "ItemName");
            return View();
        }


        //TODO: Addin a new bill
        // GET: Bills/Create
        public ActionResult CreateNewBill(AddNewBill newbl)
        {
            billdetailsmanager BDM = new billdetailsmanager();
            BDM.AddNewBill(newbl);
            var getPrtBill = db.Bills.Where(x => x.printedBill.Equals(SessionPersister.printBillNo)).Select(x => new
            {
                CustId = x.CustId,
                ItemId = x.ItemId,
                Date = x.Date,
                printedBill = x.printedBill,


            });

            return RedirectToAction("billReport", new { billno= SessionPersister.printBillNo });
           
            //return RedirectToAction("printBill");
        }

        // POST: Bills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<TempBillsDetails> bdetail)
        //public ActionResult Create([Bind(Include = "TransId,CustId,ItemId,Date,Qyt,Cost")] Bill bill)
        {
            int getTotals = bdetail.Count();
            if (ModelState.IsValid)
            {
                Bill bill = new Bill();
                db.Bills.Add(bill);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Create");
        }

        // GET: Bills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransId,CustId,ItemId,Date,Qyt,Cost")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bill);
        }

        // GET: Bills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        //delete all data from tempTable, this is important to prevent any data to be shown when we open a new bill form
        public JsonResult DelAllTemp()
        {
            var getAllTemp = (from t in db.tempBills
                              select t).ToList();
            foreach (var itm in getAllTemp)
            {
                db.tempBills.Remove(itm);
            }
            return Json(new { message = true }, JsonRequestBehavior.AllowGet);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
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

        public ActionResult printBill()
        {
            return View();
        }

        public ActionResult billReport(string billno)
        {
            ViewBag.billno = billno;
            return View();
        }
    

    }
}
