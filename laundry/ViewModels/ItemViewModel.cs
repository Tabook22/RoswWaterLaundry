using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace laundry.ViewModels
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }
        public int MId { get; set; }
        public string McatName { get; set; }
        public int SId { get; set; }
        public string ScatName { get; set; }
        public string ItemName { get; set; }
        public string itemImg { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}