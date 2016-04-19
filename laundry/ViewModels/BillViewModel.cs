using laundry.Models;
using laundry.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace laundry.ViewModels
{
    public class AddNewBill
    {
        public int TransId { get; set; }
        public int CustId { get; set; }
        public int ItemId { get; set; }
        public DateTime Date { get; set; }
        public int Qyt { get; set; }
        public decimal Cost { get; set; }
        public int BillNo { get; set; }
    }

    public class TempBillsDetails
    {
        public int Id { get; set; }
        public int TransId { get; set; }
        public int CustId { get; set; }
        public string CustName { get; set; }
        public string Tel { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public DateTime Date { get; set; }
        public int Qyt { get; set; }
        public decimal? Cost { get; set; }
    }

    public class BillViewModel
    {
        public int Id { get; set; }
        public int BillNo { get; set; }
        public bool IsPaid { get; set; }
    }

    public class BillViewData
    {
        public int TransId { get; set; }
        public int CustId { get; set; }
        public string CustName { get; set; }
        public string Tel { get; set; }
        public DateTime Date { get; set; }
        public int Qyt { get; set; }
        public decimal Cost { get; set; }
        public int BillNo { get; set; }
        public string printedBill { get; set; }
        public bool IsPaid { get; set; }
        public string BillType { get; set; }

        public List<Item> items { get; set; }
    }
}