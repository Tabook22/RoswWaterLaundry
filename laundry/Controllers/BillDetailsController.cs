using laundry.Models.DB;
using laundry.Security;
using laundry.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace laundry.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class BillDetailsController : Controller
    {
        private LundryDbContext db = new LundryDbContext();
        // GET: BillDetails
        public ActionResult Index(int? pageNumber)
        {
            var getBdetails = (from b in db.Bills
                               join c in db.Customers on b.CustId equals c.CustId
                               join p in db.paidbills on b.BillNo equals p.BillNo
                               group new { b, c, p } by new { b.BillNo } into bg
                               select new BillViewData
                               {
                                   TransId = bg.FirstOrDefault().b.TransId,
                                   CustId = bg.FirstOrDefault().c.CustId,
                                   CustName = bg.FirstOrDefault().c.CustName,
                                   Date = bg.FirstOrDefault().b.Date,
                                   Qyt = bg.FirstOrDefault().b.Qyt,
                                   Cost = bg.Sum(x => x.b.Cost),
                                   BillNo = bg.FirstOrDefault().b.BillNo,
                                   IsPaid = bg.FirstOrDefault().p.IsPaid
                               }).ToList();
            return View(getBdetails);
        }

        //TODO: Find a single Bills 
        [HttpPost]
        public JsonResult getBill(int BNO)
        {
            using (LundryDbContext db = new LundryDbContext())
            {

                bool pb = db.paidbills.Where(x => x.BillNo.Equals(BNO)).Any();

                if (!pb == false)
                {
                    var getBdetails = (from b in db.Bills
                                       where b.BillNo == BNO
                                       join c in db.Customers on b.CustId equals c.CustId
                                       join p in db.paidbills on b.BillNo equals p.BillNo
                                       group new { b, c, p } by new { b.BillNo } into bg
                                       select new BillViewData
                                       {
                                           TransId = bg.FirstOrDefault().b.TransId,
                                           CustId = bg.FirstOrDefault().c.CustId,
                                           CustName = bg.FirstOrDefault().c.CustName,
                                           Date = bg.FirstOrDefault().b.Date,
                                           Qyt = bg.FirstOrDefault().b.Qyt,
                                           Cost = bg.Sum(x => x.b.Cost),
                                           BillNo = bg.FirstOrDefault().b.BillNo,
                                           IsPaid = bg.FirstOrDefault().p.IsPaid
                                       }).ToList();
                    return Json(getBdetails, JsonRequestBehavior.AllowGet);
                }
                return Json(new { message = "Sorry No Data Found!" }, JsonRequestBehavior.AllowGet);
            }

        }

        //TODO: Find Bills according to specific date
        [HttpPost]
        public JsonResult getBillByDate(string bdate)
        {
            //var bdate2=Convert.ToDateTime(bdate);
            using (LundryDbContext db = new LundryDbContext())
            {
                String format = "dd/MM/yyyy";
                // var dd = DateTime.Today.ToString("dd/MM/yyyy");
                DateTime dte = DateTime.ParseExact(bdate, format, System.Globalization.CultureInfo.InvariantCulture);

                bool pb = db.Bills.Where(x => x.Date.Equals(dte)).Any();

                if (!pb == false)
                {
                    var getBdetails = (from b in db.Bills
                                       where b.Date == dte
                                       join c in db.Customers on b.CustId equals c.CustId
                                       join p in db.paidbills on b.BillNo equals p.BillNo
                                       group new { b, c, p } by new { b.BillNo } into bg
                                       select new BillViewData
                                       {
                                           TransId = bg.FirstOrDefault().b.TransId,
                                           CustId = bg.FirstOrDefault().c.CustId,
                                           CustName = bg.FirstOrDefault().c.CustName,
                                           Date = bg.FirstOrDefault().b.Date,
                                           Qyt = bg.FirstOrDefault().b.Qyt,
                                           Cost = bg.Sum(x => x.b.Cost),
                                           BillNo = bg.FirstOrDefault().b.BillNo,
                                           IsPaid = bg.FirstOrDefault().p.IsPaid
                                       });
                    return Json(getBdetails, JsonRequestBehavior.AllowGet);
                }
                return Json(new { message = "Sorry No Data Found!" }, JsonRequestBehavior.AllowGet);
            }

        }

        //TODO: Add new Bill Details as paid or not paid
        [HttpPost]
        public ActionResult addBillNo(BillViewData blno)
        {
            paidbill pbl = new paidbill();
            pbl = db.paidbills.Where(x => x.BillNo.Equals(blno.BillNo)).FirstOrDefault();
            pbl.IsPaid = blno.IsPaid;
            db.SaveChanges();

            ViewBag.Status = "Bill Added Successfully";
            return RedirectToAction("Index");
        }

        //TODO: Get Non-paid Bills
        public ActionResult nonPaidBills()
        {
            return View();
        }

        //TODO: Get paid Bills
        public ActionResult paidBills()
        {
            var getBdetails = (from b in db.Bills
                               join c in db.Customers on b.CustId equals c.CustId
                               join p in db.paidbills on b.BillNo equals p.BillNo
                               where p.IsPaid == true
                               group new { b, c, p } by new { b.BillNo } into bg
                               select new BillViewData
                               {
                                   CustId = bg.FirstOrDefault().b.CustId,
                                   CustName = bg.FirstOrDefault().c.CustName,
                                   Date = bg.FirstOrDefault().b.Date,
                                   Cost = bg.Sum(x => x.b.Cost),
                                   BillNo = bg.FirstOrDefault().b.BillNo,
                                   IsPaid = bg.FirstOrDefault().p.IsPaid
                               }).ToList();

            return View(getBdetails);
        }

        public ActionResult DeleteSelected()
        {
            return View();
        }

        public ActionResult AddDisc()
        {
            //TODO: Fill a dropdwon list with branches Name and id
            IEnumerable<SelectListItem> braName = db.Branches.Select(
                    b => new SelectListItem { Value = (b.Id).ToString(), Text = b.Name });
            ViewBag.Branches = braName;
            return View();
        }

        //TODO:Add New Discount-----------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult AddDisc(Disc disc)
        {
            ManageDisc MD = new ManageDisc();
            if (ModelState.IsValid)
            {
                try
                {
                    //TODO:cheack to see if there is any previous general discount (Allbra)
                    if (disc.Allbra)
                    {
                        if (MD.isAllbraActive()) //check to see if there is any previous all branches was clicked
                        {
                            IEnumerable<SelectListItem> braName = db.Branches.Select(
                                b => new SelectListItem { Value = (b.Id).ToString(), Text = b.Name });
                            ViewBag.Branches = braName;
                            ViewBag.ErrorMessage = "Sorry There is General Discount added before!, please checkout the discount table for more information";
                            return View(disc);
                        }
                        MD.DisableAll(); //here we are disabling all the previous dicount before adding the major discount

                        //if there is no previous all branche is clicked then
                        disc.Branche = 0; // add 0 in brache filed to indicate that all branches was selected
                        db.Discs.Add(disc);
                        db.SaveChanges();
                        return RedirectToAction("DiscList");
                    }
                   

                    if(MD.getBranchById(disc.Branche,disc.status)) //check to see if there is a branche with previous isActive equals true
                    {
                        IEnumerable<SelectListItem> braName = db.Branches.Select(
                                                       b => new SelectListItem { Value = (b.Id).ToString(), Text = b.Name });
                        ViewBag.Branches = braName;
                        ViewBag.ErrorMessage = "Sorry The branche is all ready has an active discount added before!, please checkout the discount table for more information";
                        return View(disc);
                    }
                    else
                    {
                        db.Discs.Add(disc);
                        db.SaveChanges();
                        return RedirectToAction("DiscList");
                    }
                }
                catch
                {
                    IEnumerable<SelectListItem> braName = db.Branches.Select(
                       b => new SelectListItem { Value = (b.Id).ToString(), Text = b.Name });
                    ViewBag.Branches = braName;
                    return View(disc);
                }


            }
            else
            {
                //this is will run if the ModelState is Invalid
                IEnumerable<SelectListItem> braName = db.Branches.Select(
                         b => new SelectListItem { Value = (b.Id).ToString(), Text = b.Name });
                ViewBag.Branches = braName;
                return View(disc);
            }

        }


        //TODO: Edit Disc------------------------------------------------------------------------------------------
        public ActionResult EditDisc(int id)
        {
            //TODO:Find the disc
            Disc getDisc = db.Discs.Find(id);
            if (getDisc != null)
            {
                //TODO: Fill a dropdwon list with branches Name and id
                IEnumerable<SelectListItem> braName = db.Branches.Select(
                        b => new SelectListItem { Value = (b.Id).ToString(), Text = b.Name });
                ViewBag.Branches = braName;
                return View(getDisc);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult EditDisc(Disc disc)
        {
            ManageDisc MD = new ManageDisc();
            if (ModelState.IsValid)
            {

                //TODO:cheack to see if there is any previous general discount (Allbra)
                if (disc.Allbra)
                {
                    if (MD.isAllbraActive()) //check to see if there is any previous all branches was clicked
                    {
                        IEnumerable<SelectListItem> braName = db.Branches.Select(
                            b => new SelectListItem { Value = (b.Id).ToString(), Text = b.Name});
                        ViewBag.Branches = braName;
                        ViewBag.ErrorMessage = "Sorry There is General Discount added before!, please checkout the discount table for more information";
                        return View(disc);
                    }
                    else
                    {
                        MD.DisableAll(); //here we are disabling all the previous dicount before adding the major discount
                                         //if there is no previous all branche is clicked then
                        disc.Branche = 0; // add 0 in brache filed to indicate that all branches was selected
                    db.Entry(disc).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("DiscList");
                    }
                    
                }


                if (MD.getBranchById(disc.Branche, disc.status,disc.Id)) //check to see if there is a branche with previous isActive equals true
                {
                    IEnumerable<SelectListItem> braName = db.Branches.Select(
                                                   b => new SelectListItem { Value = (b.Id).ToString(), Text = b.Name });
                    ViewBag.Branches = braName;
                    ViewBag.ErrorMessage = "Sorry The branche is all ready has an active discount added before! or there is a grand dicount is active, please checkout the discount table for more information";
                    return View(disc);
                }
                else
                {
                    if (disc.Branche == null)
                    {
                        disc.Branche = 0; //this makes sure that the 0 value stays in the branche column for the allbra is ture or false, otherise it will be remove and a null value will palce
                    }
                    db.Entry(disc).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("DiscList");
                }


            //    //TODO:cheack to see if there is any previous general discount (Allbra)
            //    if (disc.Allbra)
            //    {
            //        if (MD.isAllbraActive()) //check to see if there is any previous all branches was clicked
            //        {
            //            IEnumerable<SelectListItem> braName = db.Branches.Select(
            //                b => new SelectListItem { Value = (b.Id).ToString(), Text = b.Name });
            //            ViewBag.Branches = braName;
            //            ViewBag.ErrorMessage = "Sorry There is General Discount added before!, please checkout the discount table for more information";
            //            return View(disc);
            //        }
            //        //if there is no previous all branche is clicked then
            //        disc.Branche = 0; // add 0 in brache filed to indicate that all branches was selected
            //    }
            //    db.Entry(disc).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("DiscList");
            }
            return View(disc);
        }
        public ActionResult DiscList()
        {
            var dlst = (from d in db.Discs
                        orderby d.DisDate descending
                        select d).ToList();
            //var dlst = db.Discs.Join(db.Branches,
            //    d=>d.Id,
            //    b=>b.Id,
            //    (d,b)=> new DiscViewModel{Id=d.Id,
            //        DisDate =d.DisDate,
            //        DisAmount =d.DisAmount,
            //        Branche =b.Id,
            //        Allbra =d.Allbra,
            //        status =d.status}).ToList();
            return View(dlst);
        }
    }
}