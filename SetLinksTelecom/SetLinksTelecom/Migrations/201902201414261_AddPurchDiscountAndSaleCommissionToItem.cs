namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchDiscountAndSaleCommissionToItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "PurDiscString", c => c.String(maxLength: 12));
            AddColumn("dbo.Item", "SaleCommString", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Item", "SaleCommString");
            DropColumn("dbo.Item", "PurDiscString");
        }
    }
}
