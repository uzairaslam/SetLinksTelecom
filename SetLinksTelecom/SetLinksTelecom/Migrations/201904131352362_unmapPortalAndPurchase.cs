namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unmapPortalAndPurchase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Purchase", "PortalId", "dbo.Portal");
            DropIndex("dbo.Purchase", new[] { "PortalId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Purchase", "PortalId");
            AddForeignKey("dbo.Purchase", "PortalId", "dbo.Portal", "PortalId", cascadeDelete: true);
        }
    }
}
