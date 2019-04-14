namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionIdinSale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "TansactionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "TansactionId");
        }
    }
}
