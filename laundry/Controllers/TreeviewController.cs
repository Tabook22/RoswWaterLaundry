using laundry.Models.DB;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace laundry.Controllers
{
    public class TreeviewController : Controller
    {
        public ActionResult OnDemand()
        {
            List<ItemMainCategory> all = new List<ItemMainCategory>();
            using (LundryDbContext db = new LundryDbContext())
            {
                all = db.ItemMainCategories.OrderBy(a => a.Id).ToList();
            }
            return View(all);
        }

        public JsonResult GetSubMenu(string pid)
        {
            LundryDbContext db = new LundryDbContext();
            // this action for Get Sub Menus from database and return as json data
            System.Threading.Thread.Sleep(1000);
            //List<Item> subMenus = new List<Item>();
            int pID = 0;
            int.TryParse(pid, out pID);
            var getSubCat = db.ItemSubCategories.Where(x => x.MId==pID).ToList();
                //subMenus = db.Items.Where(a => a.Equals(pID)).OrderBy(a => a.ItemName).ToList();
 

            return new JsonResult { Data = getSubCat, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetSubMenu2(string pid)
        {
            LundryDbContext db = new LundryDbContext();
            // this action for Get Sub Menus from database and return as json data
            //System.Threading.Thread.Sleep(1000);
            //List<Item> subMenus = new List<Item>();
            int pID = 0;
            int.TryParse(pid, out pID);
            var getItem = db.Items.Where(x => x.SId == pID).ToList();
            //subMenus = db.Items.Where(a => a.Equals(pID)).OrderBy(a => a.ItemName).ToList();


            return new JsonResult { Data = getItem, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}