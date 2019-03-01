namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRevenueAndCostStrings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "RevString", c => c.String(maxLength: 12));
            AddColumn("dbo.Item", "CosString", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Item", "CosString");
            DropColumn("dbo.Item", "RevString");
        }
    }
}
