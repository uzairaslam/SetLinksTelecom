namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedCommProfittoSaleDetails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SaleDetail", "CommProfit", c => c.Decimal(nullable: false, precision: 20, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SaleDetail", "CommProfit", c => c.Decimal(nullable: false, precision: 20, scale: 2));
        }
    }
}
