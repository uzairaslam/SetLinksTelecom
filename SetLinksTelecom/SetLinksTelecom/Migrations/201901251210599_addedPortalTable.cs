namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPortalTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Purchase", "ItemId", "dbo.Item");
            DropIndex("dbo.Purchase", new[] { "ItemId" });
            CreateTable(
                "dbo.Portal",
                c => new
                    {
                        PortalId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Url = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PortalId);
            
            AddColumn("dbo.Purchase", "PortalId", c => c.Int(nullable: false));
            CreateIndex("dbo.Purchase", "PortalId");
            AddForeignKey("dbo.Purchase", "PortalId", "dbo.Portal", "PortalId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purchase", "PortalId", "dbo.Portal");
            DropIndex("dbo.Purchase", new[] { "PortalId" });
            DropColumn("dbo.Purchase", "PortalId");
            DropTable("dbo.Portal");
            CreateIndex("dbo.Purchase", "ItemId");
            AddForeignKey("dbo.Purchase", "ItemId", "dbo.Item", "ItemId", cascadeDelete: true);
        }
    }
}
