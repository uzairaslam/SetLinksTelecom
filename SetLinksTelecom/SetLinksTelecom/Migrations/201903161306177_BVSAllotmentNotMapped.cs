namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BVSAllotmentNotMapped : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BVSAllot", "ItemId", "dbo.Item");
            DropForeignKey("dbo.BVSAllot", "PersonId", "dbo.Person");
            DropForeignKey("dbo.BVSAllotService", "BvsServiceId", "dbo.BvsService");
            DropForeignKey("dbo.BvsService", "BVSAllotService_BVSAllotServiceId", "dbo.BVSAllotService");
            DropIndex("dbo.BVSAllot", new[] { "PersonId" });
            DropIndex("dbo.BVSAllot", new[] { "ItemId" });
            DropIndex("dbo.BVSAllotService", new[] { "BvsServiceId" });
            DropIndex("dbo.BvsService", new[] { "BVSAllotService_BVSAllotServiceId" });
            DropColumn("dbo.BvsService", "BVSAllotService_BVSAllotServiceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BvsService", "BVSAllotService_BVSAllotServiceId", c => c.Int());
            CreateIndex("dbo.BvsService", "BVSAllotService_BVSAllotServiceId");
            CreateIndex("dbo.BVSAllotService", "BvsServiceId");
            CreateIndex("dbo.BVSAllot", "ItemId");
            CreateIndex("dbo.BVSAllot", "PersonId");
            AddForeignKey("dbo.BvsService", "BVSAllotService_BVSAllotServiceId", "dbo.BVSAllotService", "BVSAllotServiceId");
            AddForeignKey("dbo.BVSAllotService", "BvsServiceId", "dbo.BvsService", "BvsServiceId", cascadeDelete: true);
            AddForeignKey("dbo.BVSAllot", "PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.BVSAllot", "ItemId", "dbo.Item", "ItemId", cascadeDelete: true);
        }
    }
}
