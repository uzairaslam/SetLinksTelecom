namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedStockOutProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchase", "StockOut", c => c.Decimal(nullable: false, precision: 20, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Purchase", "StockOut", c => c.Int(nullable: false));
        }
    }
}
