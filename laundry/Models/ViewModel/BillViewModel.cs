using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laundry.Models.ViewModel
{
    public class BillViewModel
    {
        public int CustId { get; set; }
        public int ItemId { get; set; }
        public System.DateTime Date { get; set; }
        public int Qyt { get; set; }
        public decimal Cost { get; set; }
        public string printedBill { get; set; }
        public string CustName { get; set; }
        public string Tel { get; set; }
        public int MId { get; set; }
        public string catName { get; set; }
        public int SId { get; set; }
        public string ItemName { get; set; }
        public string itemImg { get; set; }
        public decimal Price { get; set; }
    }
}
