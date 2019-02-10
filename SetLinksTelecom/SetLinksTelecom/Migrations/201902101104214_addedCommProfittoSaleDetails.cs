namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedCommProfittoSaleDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleDetail", "CommProfit", c => c.Decimal(nullable: false, precision: 20, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleDetail", "CommProfit");
        }
    }
}
