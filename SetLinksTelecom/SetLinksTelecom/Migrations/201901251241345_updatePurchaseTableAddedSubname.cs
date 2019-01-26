namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePurchaseTableAddedSubname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchase", "Subname", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchase", "Subname");
        }
    }
}
