using laundry.Models;
using laundry.Models.DB;
using laundry.Security;
using laundry.ViewModels;
using laundryTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace laundry.Controllers
{
    public class AccountController : Controller
    {
        private LundryDbContext db = new LundryDbContext();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel avm)
        {
            AccountModel am = new AccountModel();
            //here we are going to check to see if the username is null and password is null inside the tbl_Account
            if (string.IsNullOrEmpty(avm.tbl_account.Username) || string.IsNullOrEmpty(avm.tbl_account.Password) || am.login(avm.tbl_account.Username, avm.tbl_account.Password, avm.tbl_account.Branch) == null)
            {
                ViewBag.Error = "Account's Invalid";
                return View("Login");
            }
            //if there is username and password then add the username to the session, and redirect to success view
            SessionPersister.Username = avm.tbl_account.Username;
            SessionPersister.BranchID =Convert.ToString(avm.tbl_account.Branch);
            SessionPersister.BranchName = db.Branches.Where(x => x.Id.Equals(avm.tbl_account.Branch)).FirstOrDefault().Name;
            //to get branch name
            return RedirectToAction("create","Bills");
        }

        public ActionResult Logout()
        {
            SessionPersister.Username = string.Empty;
            return RedirectToAction("Login");
            // Code disables caching by browser.
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();


        }
    }
}