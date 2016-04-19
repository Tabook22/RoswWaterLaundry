using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace laundry.Models.DB
{

    public class tempBill
    {
        [Key]
        public int Id { get; set; }
        public int TransId { get; set; }
        public int CustId { get; set; }
        public int ItemId { get; set; }
        public System.DateTime Date { get; set; }
        public int Qyt { get; set; }
        public decimal Cost { get; set; }
        public int MId { get; set; }
        public int SId { get; set; }
    }
}
