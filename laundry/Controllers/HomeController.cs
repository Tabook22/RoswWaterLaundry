
using laundry.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace laundry.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class HomeController : Controller
    {
       
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        // GET: Home
        public ActionResult About()
        {
            return View();
        }
        // GET: Home
        public ActionResult AdminOnly()
        {
            return View();
        }
        // GET: Home
        public ActionResult Contact()
        {
            return View();
        }
        // GET: Home
        public ActionResult Thankyou()
        {
            return View();
        }
        // GET: Home
        public ActionResult Welcome()
        {
            return View();
        }
    }
}