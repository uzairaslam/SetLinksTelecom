namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSaleandSaleDetailsModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaleDetail",
                c => new
                    {
                        SaleDetailId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 20, scale: 2),
                        SubTotal = c.Decimal(nullable: false, precision: 20, scale: 2),
                        LineId = c.Int(nullable: false),
                        Sale_SaleId = c.Int(),
                    })
                .PrimaryKey(t => t.SaleDetailId)
                .ForeignKey("dbo.Sale", t => t.Sale_SaleId)
                .Index(t => t.Sale_SaleId);
            
            CreateTable(
                "dbo.Sale",
                c => new
                    {
                        SaleId = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        OverAllTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SaleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleDetail", "Sale_SaleId", "dbo.Sale");
            DropIndex("dbo.SaleDetail", new[] { "Sale_SaleId" });
            DropTable("dbo.Sale");
            DropTable("dbo.SaleDetail");
        }
    }
}
