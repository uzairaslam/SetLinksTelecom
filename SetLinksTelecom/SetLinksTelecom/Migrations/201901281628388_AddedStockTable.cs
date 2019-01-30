namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStockTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stock",
                c => new
                    {
                        StockId = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        NetQty = c.Int(nullable: false),
                        AvgRate = c.Decimal(nullable: false, precision: 20, scale: 2),
                        PurchaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stock");
        }
    }
}
