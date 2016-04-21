using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace laundry.Models.DB
{
    public partial class LundryDbContext : DbContext
    {
        public LundryDbContext()
            : base("name=altersanahConnection")
        {
        }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<tempBill> tempBills { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<paidbill> paidbills { get; set; }
        public virtual DbSet<Disc> Discs { get; set; }
        public virtual DbSet<ItemMainCategory> ItemMainCategories { get; set; }
        public virtual DbSet<ItemSubCategory> ItemSubCategories { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<tbl_images> tbl_images { get; set; }
        public virtual DbSet<tbl_Account> tbl_Account{set;get;}
        public virtual DbSet<tbl_Account_role> tbl_Account_role{set; get;}
        public virtual DbSet<tbl_Role> tbl_Role {set; get;}
        public virtual DbSet<YearMaxBillNo> yearmaxbillno { get; set; }
    }

}