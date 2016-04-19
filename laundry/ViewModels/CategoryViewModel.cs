using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using laundry.ViewModels;
using laundry.Models;

namespace laundry.ViewModels
{

    //TODO: Add new main category
    public class AddNewMainCategory
    {
        public int Id { get; set; }
        public string catName { get; set; }
    }

    public class CatSubViewData
    {
        public int Id { get; set; }
        public string catName { get; set; }
        public int MId { get; set; }
        public string catMName { get; set; }
    }
}
