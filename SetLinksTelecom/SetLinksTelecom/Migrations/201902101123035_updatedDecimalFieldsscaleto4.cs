namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedDecimalFieldsscaleto4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Item", "SaleRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Purchase", "Rate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Purchase", "Total", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Purchase", "Percentage", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.SaleDetail", "Rate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.SaleDetail", "SubTotal", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Stock", "AvgRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stock", "AvgRate", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            AlterColumn("dbo.SaleDetail", "SubTotal", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            AlterColumn("dbo.SaleDetail", "Rate", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            AlterColumn("dbo.Purchase", "Percentage", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            AlterColumn("dbo.Purchase", "Total", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            AlterColumn("dbo.Purchase", "Rate", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            AlterColumn("dbo.Item", "SaleRate", c => c.Decimal(nullable: false, precision: 20, scale: 2));
        }
    }
}
