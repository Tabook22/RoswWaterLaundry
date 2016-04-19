using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace laundry.Models.DB
{
    public class tbl_Role
    {
        [Key]
        public int RID { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<tbl_Account_role> roles { get; set; }
    }
}