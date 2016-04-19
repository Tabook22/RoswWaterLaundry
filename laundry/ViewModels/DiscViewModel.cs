using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace laundry.ViewModels
{
    public class DiscViewModel
    {
        public int Id { get; set; }
        public DateTime? DisDate { get; set; }
        public int? DisAmount { get; set; }
        public int? Branche { get; set; }
        public bool? Allbra { get; set; }
        public bool? status { get; set; }
    }
}