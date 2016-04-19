using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace laundry.Models.DB
{
    public class ItemSubCategory
    {
        [Key]
        public int Id { get; set; }
        public string catName { get; set; }
        public Nullable<int> MId { get; set; }
    }
}
