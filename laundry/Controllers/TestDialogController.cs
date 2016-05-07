using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace laundry.Controllers
{
    public class TestDialogController : Controller
    {
        // GET: TestDialog
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult testD()
        {
            return PartialView();
        }
    }
}