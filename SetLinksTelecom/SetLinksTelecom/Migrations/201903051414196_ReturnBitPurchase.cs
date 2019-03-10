namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReturnBitPurchase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchase", "Return", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchase", "Return");
        }
    }
}
