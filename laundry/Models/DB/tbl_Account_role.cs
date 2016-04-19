using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace laundry.Models.DB
{
    public class tbl_Account_role
    {
        [Key]
        public int Id { get; set; }
        public int AID { get; set; }
        public int RID { get; set; }

        [ForeignKey("AID")]
        public virtual tbl_Account account { get; set; }

        [ForeignKey("RID")]
        public virtual tbl_Role role { get; set; }
    }
}