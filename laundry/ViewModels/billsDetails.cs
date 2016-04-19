using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace laundry.ViewModels
{
    public class billsDetails
    {
        public int Id { get; set; }
        public Nullable<int> TransId { get; set; }
        public Nullable<int> CustId { get; set; }
        public string CustName { get; set; }
        public string Tel { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Qyt { get; set; }
        public Nullable<decimal> Cost { get; set; }
    }

}