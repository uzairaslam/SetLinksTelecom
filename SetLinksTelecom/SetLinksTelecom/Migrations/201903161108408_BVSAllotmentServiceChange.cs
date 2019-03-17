namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BVSAllotmentServiceChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BVSAllotService", "BvsService_BvsServiceId", "dbo.BvsService");
            DropIndex("dbo.BVSAllotService", new[] { "BvsService_BvsServiceId" });
            DropColumn("dbo.BVSAllotService", "BvsServiceId");
            RenameColumn(table: "dbo.BVSAllotService", name: "BvsService_BvsServiceId", newName: "BvsServiceId");
            AlterColumn("dbo.BVSAllotService", "BvsServiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.BVSAllotService", "BvsServiceId");
            AddForeignKey("dbo.BVSAllotService", "BvsServiceId", "dbo.BvsService", "BvsServiceId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BVSAllotService", "BvsServiceId", "dbo.BvsService");
            DropIndex("dbo.BVSAllotService", new[] { "BvsServiceId" });
            AlterColumn("dbo.BVSAllotService", "BvsServiceId", c => c.Int());
            RenameColumn(table: "dbo.BVSAllotService", name: "BvsServiceId", newName: "BvsService_BvsServiceId");
            AddColumn("dbo.BVSAllotService", "BvsServiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.BVSAllotService", "BvsService_BvsServiceId");
            AddForeignKey("dbo.BVSAllotService", "BvsService_BvsServiceId", "dbo.BvsService", "BvsServiceId");
        }
    }
}
