namespace laundry.Migrations
{
    using System.Data.Entity.Migrations;
    using Models.DB;
    using System;
    internal sealed class Configuration : DbMigrationsConfiguration<LundryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LundryDbContext context)
        {
            // add first user to the system
            context.tbl_Account.AddOrUpdate(a => a.AID,
                new tbl_Account { Username = "Nasser", Password = "Nasser", FName = "Nasser", Role = "Admin", CDate = DateTime.Now, Branch = 1 });
            context.SaveChanges();

            // add laundry branches
            context.Branches.AddOrUpdate(r => r.Name,
        new Branch { Name = "Main Office", Desc = "This is the main branch" },
        new Branch { Name = "New Salalah", Desc = "New Salalah Branch" },
        new Branch { Name = "Al-Sadaa", Desc = "Sadaa branch" },
        new Branch { Name = "Oqad", Desc = "Oqad Branch" },
        new Branch { Name = "Al-Daharize", Desc = "Al-Dharize Branch" });
            context.SaveChanges();

            //Main Category starting value
            context.ItemMainCategories.AddOrUpdate(c => c.Id,
        new ItemMainCategory {catName="Normal Wash" },
        new ItemMainCategory {catName ="Dry Cleaning" },
        new ItemMainCategory { catName = "Press Only" },
        new ItemMainCategory { catName = "Linen" });
            context.SaveChanges();

            //subcategory starting values
            context.ItemSubCategories.AddOrUpdate(s => s.Id,
        new ItemSubCategory { catName="Male", MId=1},
        new ItemSubCategory { catName = "Female", MId = 1 },
        new ItemSubCategory { catName = "Kids", MId = 1 }
                );
            context.SaveChanges();


            //add customers
            context.Customers.AddOrUpdate(cs => cs.CustId,
                new Customer { CustName = "Nasser", Tel = "1234" },
                new Customer {CustName ="Salim",Tel="12345" });
            context.SaveChanges();
            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
