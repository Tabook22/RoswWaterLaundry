using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace laundry.Models.DB
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public int MId { get; set; }
        public int SId { get; set; }
        public string ItemName { get; set; }
        public string itemImg { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
