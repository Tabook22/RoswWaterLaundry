using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace laundry.Models.DB
{
    public class YearMaxBillNo
    {
        [Key]
        public int id { get; set; }
        public string year { get; set; }
        public int maxbillno { get; set; }
    }
}