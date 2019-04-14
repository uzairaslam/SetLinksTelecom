namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalePurchaseStockOutMap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalePurchaseStockOutMap",
                c => new
                    {
                        SalePurchaseStockOutMapId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        SaleId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 20, scale: 4),
                    })
                .PrimaryKey(t => t.SalePurchaseStockOutMapId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SalePurchaseStockOutMap");
        }
    }
}
