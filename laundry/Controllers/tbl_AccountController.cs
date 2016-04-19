using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using laundry.Models.DB;
using laundry.ViewModels;
using PagedList;
using laundry.Security;

namespace laundry.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class tbl_AccountController : Controller
    {
        private LundryDbContext db = new LundryDbContext();

        // GET: tbl_Account
        public ActionResult Index()
        {
            var ulst = db.tbl_Account.
               Join(db.Branches,
               a => a.Branch, b => b.Id,
               (a, b) => new UserProfileView
               {
                   AID = a.AID,
                   Username = a.Username,
                   Password = a.Password,
                   FName = a.FName,
                   LName = a.LName,
                   CDate = a.CDate,
                   Branch = b.Id,
                   BrName= b.Name,
                   Role = a.Role
               }).ToList();
            var getBra = db.Branches.Select(x => new { Id = x.Id, Name = x.Name, Desc = x.Desc });
            ViewBag.BrList = new SelectList(getBra, "Id", "Name");
            return View(ulst);
        }

        [HttpPost]
        public ActionResult Index(int CurrentPage,int LastPage)
        {
            var ulst = db.tbl_Account.
               Join(db.Branches,
               a => a.Branch, b => b.Id,
               (a, b) => new UserProfileView
               {
                   AID = a.AID,
                   Username = a.Username,
                   Password = a.Password,
                   FName = a.FName,
                   LName = a.LName,
                   CDate = a.CDate,
                   Branch = b.Id,
                   BrName = b.Name,
                   Role = a.Role
               }).ToList();
            var getBra = db.Branches.Select(x => new { Id = x.Id, Name = x.Name, Desc = x.Desc });
            ViewBag.BrList = new SelectList(getBra, "Id", "Name");
            ViewBag.CurrentPage = CurrentPage+1;
            ViewBag.LastPage = LastPage;
            return PartialView("_UsersList",ulst.OrderBy (x=>x.FName).Skip((CurrentPage - 1) * 5).Take(5));
        }

        public JsonResult UsrList()
        {
            var getlst = db.tbl_Account.
                Join(db.Branches,
                a => a.Branch, b => b.Id,
                (a, b) => new UserProfileView
                {
                    AID = a.AID,
                    Username = a.Username,
                    Password = a.Password,
                    FName = a.FName,
                    LName = a.LName,
                    CDate = a.CDate,
                    Branch = b.Id,
                    BrName = b.Name,
                    Role = a.Role
                }).ToList();


            //var getlst = db.tbl_Account.Select(x=> new { AID = x.AID,Username=x.Username,Password=x.Password,FName=x.FName,LName=x.LName,Role=x.Role,CDate=x.CDate,Branch=x.Branch});
            //var getBra = db.Branches.Select(x => new { Id = x.Id, Name = x.Name, Desc = x.Desc });
            //ViewBag.BrList = new SelectList(getBra, "Id", "Name");

            return Json(getlst, JsonRequestBehavior.AllowGet);
        }

        //Delete users
        public JsonResult DelUsers(int id, int? pageNumber)
        {
            tbl_Account tbl_Account = db.tbl_Account.Find(id);
            db.tbl_Account.Remove(tbl_Account);
            db.SaveChanges();

            //list all users
            var showlst = db.tbl_Account.
                Join(db.Branches,
                a => a.Branch, b => b.Id,
                (a, b) => new UserProfileView
                {
                    AID = a.AID,
                    Username = a.Username,
                    Password = a.Password,
                    FName = a.FName,
                    LName = a.LName,
                    CDate = a.CDate,
                    Branch = b.Id,
                    Role = a.Role
                }).ToList();

            return Json(showlst, JsonRequestBehavior.AllowGet);
        }

        // GET: tbl_Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Account tbl_Account = db.tbl_Account.Find(id);
            if (tbl_Account == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Account);
        }

        // GET: tbl_Account/Create
        public ActionResult Create()
        {
            var getBra = db.Branches.Select(x=> new {Id=x.Id,Name=x.Name,Desc=x.Desc});
            ViewBag.BrList = new SelectList(getBra, "Id", "Name");
            return View();
        }

        public JsonResult AddNewUser(string username, string password, string FName, string LName,string Role, int Branch, int? pageNumber)
        {
            tbl_Account tbluser = new tbl_Account();
            tbluser.Username = username;
            tbluser.Password = password;
            tbluser.FName = FName;
            tbluser.LName = LName;
            tbluser.Role = Role;
            tbluser.Branch = Branch;
            tbluser.CDate = DateTime.Now;
                db.tbl_Account.Add(tbluser);
                db.SaveChanges();

            //joing two tables tbl_Account and Branche, then using the result to fill the ViewModel UserProfileView
            var getlst = db.tbl_Account.
               Join(db.Branches,
               a => a.Branch, b => b.Id,
               (a, b) => new UserProfileView
               {
                   AID = a.AID,
                   Username = a.Username,
                   Password = a.Password,
                   FName = a.FName,
                   LName = a.LName,
                   CDate = a.CDate,
                   Branch = b.Id,
                   Role = a.Role
               }).ToList().ToPagedList(pageNumber ?? 1, 3);

            // var getlst = db.tbl_Account.Select(x => new { AID = x.AID, Username = x.Username, Password = x.Password, FName = x.FName, LName = x.LName, Role = x.Role, CDate = x.CDate, Branch = x.Branch });
            return Json(getlst, JsonRequestBehavior.AllowGet);
        }

        // GET: tbl_Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Account tbl_Account = db.tbl_Account.Find(id);
            if (tbl_Account == null)
            {
                return HttpNotFound();
            }
            var getBra = db.Branches.Select(x => new { Id = x.Id, Name = x.Name, Desc = x.Desc });
            ViewBag.BrList = new SelectList(getBra, "Id", "Name");
            return View(tbl_Account);
        }

        // POST: tbl_Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AID,Username,Password,FName,LName,CDate,Branch")] tbl_Account tbl_Account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Account);
        }

        // GET: tbl_Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Account tbl_Account = db.tbl_Account.Find(id);
            if (tbl_Account == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Account);
        }

        // POST: tbl_Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Account tbl_Account = db.tbl_Account.Find(id);
            db.tbl_Account.Remove(tbl_Account);
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
