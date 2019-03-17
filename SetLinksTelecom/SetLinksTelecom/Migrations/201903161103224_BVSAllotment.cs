namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BVSAllotment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BVSAllot",
                c => new
                    {
                        BVSAllotId = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BVSAllotId)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.BVSAllotService",
                c => new
                    {
                        BVSAllotServiceId = c.Int(nullable: false, identity: true),
                        BVSAllotId = c.Int(nullable: false),
                        BvsServiceId = c.Int(nullable: false),
                        BvsService_BvsServiceId = c.Int(),
                    })
                .PrimaryKey(t => t.BVSAllotServiceId)
                .ForeignKey("dbo.BVSAllot", t => t.BVSAllotId, cascadeDelete: true)
                .ForeignKey("dbo.BvsService", t => t.BvsService_BvsServiceId)
                .Index(t => t.BVSAllotId)
                .Index(t => t.BvsService_BvsServiceId);
            
            AddColumn("dbo.BvsService", "BVSAllotService_BVSAllotServiceId", c => c.Int());
            CreateIndex("dbo.BvsService", "BVSAllotService_BVSAllotServiceId");
            AddForeignKey("dbo.BvsService", "BVSAllotService_BVSAllotServiceId", "dbo.BVSAllotService", "BVSAllotServiceId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BvsService", "BVSAllotService_BVSAllotServiceId", "dbo.BVSAllotService");
            DropForeignKey("dbo.BVSAllotService", "BvsService_BvsServiceId", "dbo.BvsService");
            DropForeignKey("dbo.BVSAllotService", "BVSAllotId", "dbo.BVSAllot");
            DropForeignKey("dbo.BVSAllot", "PersonId", "dbo.Person");
            DropForeignKey("dbo.BVSAllot", "ItemId", "dbo.Item");
            DropIndex("dbo.BvsService", new[] { "BVSAllotService_BVSAllotServiceId" });
            DropIndex("dbo.BVSAllotService", new[] { "BvsService_BvsServiceId" });
            DropIndex("dbo.BVSAllotService", new[] { "BVSAllotId" });
            DropIndex("dbo.BVSAllot", new[] { "ItemId" });
            DropIndex("dbo.BVSAllot", new[] { "PersonId" });
            DropColumn("dbo.BvsService", "BVSAllotService_BVSAllotServiceId");
            DropTable("dbo.BVSAllotService");
            DropTable("dbo.BVSAllot");
        }
    }
}
