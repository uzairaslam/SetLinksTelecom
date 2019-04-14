namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleTransactionIdChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "TransactionId", c => c.String());
            DropColumn("dbo.Sale", "TansactionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sale", "TansactionId", c => c.String());
            DropColumn("dbo.Sale", "TransactionId");
        }
    }
}
