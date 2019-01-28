namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPurchaseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchase", "Rate", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            AddColumn("dbo.Purchase", "Total", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            AddColumn("dbo.Purchase", "Remarks", c => c.String(maxLength: 150));
            AddColumn("dbo.Purchase", "DatePurchased", c => c.DateTime(nullable: false));
            AddColumn("dbo.Purchase", "StockOut", c => c.Int(nullable: false));
            DropColumn("dbo.Purchase", "Comments");
            DropColumn("dbo.Purchase", "PaidAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Purchase", "PaidAmount", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            AddColumn("dbo.Purchase", "Comments", c => c.String(maxLength: 150));
            DropColumn("dbo.Purchase", "StockOut");
            DropColumn("dbo.Purchase", "DatePurchased");
            DropColumn("dbo.Purchase", "Remarks");
            DropColumn("dbo.Purchase", "Total");
            DropColumn("dbo.Purchase", "Rate");
        }
    }
}
