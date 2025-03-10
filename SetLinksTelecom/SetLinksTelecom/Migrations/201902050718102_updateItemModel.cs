namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateItemModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "SaleRate", c => c.Decimal(nullable: false, precision: 20, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Item", "SaleRate");
        }
    }
}
