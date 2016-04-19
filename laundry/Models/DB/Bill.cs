using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace laundry.Models.DB
{
    public  class Bill
    {
        [Key]
        public int TransId { get; set; }
        public int CustId { get; set; }
        public int ItemId { get; set; }
        public System.DateTime Date { get; set; }
        public int Qyt { get; set; }
        public decimal Cost { get; set; }
        public int BillNo { get; set; }
        public string printedBill { get; set; }
    }
}
