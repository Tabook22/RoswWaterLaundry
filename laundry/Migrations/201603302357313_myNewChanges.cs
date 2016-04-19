namespace laundry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class myNewChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        TransId = c.Int(nullable: false, identity: true),
                        CustId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Qyt = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillNo = c.Int(nullable: false),
                        printedBill = c.String(),
                    })
                .PrimaryKey(t => t.TransId);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustId = c.Int(nullable: false, identity: true),
                        CustName = c.String(),
                        Tel = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.CustId);
            
            CreateTable(
                "dbo.Discs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisDate = c.DateTime(),
                        DisAmount = c.Int(),
                        Branche = c.Int(),
                        Allbra = c.Boolean(nullable: false),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ItemMainCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        catName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        MId = c.Int(nullable: false),
                        SId = c.Int(nullable: false),
                        ItemName = c.String(),
                        itemImg = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ItemId);
            
            CreateTable(
                "dbo.ItemSubCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        catName = c.String(),
                        MId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.paidbills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BillNo = c.Int(nullable: false),
                        printedBill = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_Account",
                c => new
                    {
                        AID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        FName = c.String(),
                        LName = c.String(),
                        Role = c.String(),
                        CDate = c.DateTime(nullable: false),
                        Branch = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AID);
            
            CreateTable(
                "dbo.tbl_Account_role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AID = c.Int(nullable: false),
                        RID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Account", t => t.AID, cascadeDelete: true)
                .ForeignKey("dbo.tbl_Role", t => t.RID, cascadeDelete: true)
                .Index(t => t.AID)
                .Index(t => t.RID);
            
            CreateTable(
                "dbo.tbl_Role",
                c => new
                    {
                        RID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.RID);
            
            CreateTable(
                "dbo.tbl_images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        imgTitle = c.String(),
                        imgL = c.String(),
                        imgS = c.String(),
                        imgUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tempBills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransId = c.Int(nullable: false),
                        CustId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Qyt = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MId = c.Int(nullable: false),
                        SId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.YearMaxBillNoes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        year = c.String(),
                        maxbillno = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_Account_role", "RID", "dbo.tbl_Role");
            DropForeignKey("dbo.tbl_Account_role", "AID", "dbo.tbl_Account");
            DropIndex("dbo.tbl_Account_role", new[] { "RID" });
            DropIndex("dbo.tbl_Account_role", new[] { "AID" });
            DropTable("dbo.YearMaxBillNoes");
            DropTable("dbo.Users");
            DropTable("dbo.tempBills");
            DropTable("dbo.tbl_images");
            DropTable("dbo.tbl_Role");
            DropTable("dbo.tbl_Account_role");
            DropTable("dbo.tbl_Account");
            DropTable("dbo.paidbills");
            DropTable("dbo.ItemSubCategories");
            DropTable("dbo.Items");
            DropTable("dbo.ItemMainCategories");
            DropTable("dbo.Discs");
            DropTable("dbo.Customers");
            DropTable("dbo.Branches");
            DropTable("dbo.Bills");
        }
    }
}
