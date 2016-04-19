using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace laundry.Models.DB
{
    public class paidbill
    {
        [Key]
        public int Id { get; set; }
        public int BillNo { get; set; }
        public string printedBill { get; set; }
        public bool IsPaid { get; set; }
    }
}
