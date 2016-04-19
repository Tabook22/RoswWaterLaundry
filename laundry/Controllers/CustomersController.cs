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
    public class CustomersController : Controller
    {

        private LundryDbContext db = new LundryDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }


        // Getting the customer details based on the tel no
        public ActionResult getCustDetails(string sOption, string sType)
        {


            if (sType == "cTel")
            {
                var getCNames = (from cst in db.Customers
                                 where cst.Tel == sOption
                                 select cst).SingleOrDefault();
                return Json(getCNames, JsonRequestBehavior.AllowGet);
            }
            if (sType == "cCode")
            {
                int cCode = Convert.ToInt32(sOption);
                var getCNames = (from cst in db.Customers
                                 where cst.CustId == cCode
                                 select cst).SingleOrDefault();
                return Json(getCNames, JsonRequestBehavior.AllowGet);
            }


            if (sType == "cstId")
            {
                int cNameCode = Convert.ToInt32(sOption);
                var getCNames = (from cst in db.Customers
                                 where cst.CustId == cNameCode
                                 select cst).SingleOrDefault();
                return Json(getCNames, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Create", "Bills");

        }
        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustId,CustName,Tel,Type")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustId,CustName,Tel,Type")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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

        public ActionResult Report()
        {


            var query = from itm in db.Items
                        join bls in db.Bills on itm.ItemId equals bls.ItemId
                        join cst in db.Customers on bls.CustId equals cst.CustId
                        group bls by new { bls.CustId, cst.CustName } into s
                        select new
                        {
                            Account = s.Key.CustId,
                            CustomerName = s.Key.CustName,
                            Amount = s.Sum(y => y.Cost)
                        };




            var itmlst = from itm in db.Items
                         join bls in db.Bills on itm.ItemId equals bls.ItemId
                         join cst in db.Customers on bls.CustId equals cst.CustId
                         group bls by new { bls.Date, cst.CustName, bls.Cost } into g
                         select new
                         {
                             Date = g.Key.Date,
                             CustomerName = g.Key.CustName,
                             Amount = g.Sum(bls => g.Key.Cost)
                         };

            var getTotals = itmlst.GroupBy(x => x.CustomerName).Select(y => new { Date = y.FirstOrDefault().Date, customerName = y.FirstOrDefault().CustomerName, Amount = y.Sum(x => x.Amount) }).ToList();
            return View(query.ToList());
            //select new billsDetails { CustId = bls.CustId, CustName = cst.CustName, ItemId = bls.ItemId, ItemName = itm.ItemName, Qyt = bls.Qyt, Cost = bls.Cost, Date = bls.Date };

        }
    }
}
