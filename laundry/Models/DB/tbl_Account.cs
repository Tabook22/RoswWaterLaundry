using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace laundry.Models.DB
{
    public class tbl_Account
    {
        [Key]
        public int AID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Role { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CDate { get; set; }
        public int Branch { get; set; }
        public virtual ICollection<tbl_Account_role> accounts { get; set; }
    }
}