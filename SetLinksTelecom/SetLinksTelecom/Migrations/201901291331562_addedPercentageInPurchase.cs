namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPercentageInPurchase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchase", "Percentage", c => c.Decimal(nullable: false, precision: 20, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchase", "Percentage");
        }
    }
}
