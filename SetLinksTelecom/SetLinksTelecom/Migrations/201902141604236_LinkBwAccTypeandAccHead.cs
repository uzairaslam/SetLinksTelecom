namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkBwAccTypeandAccHead : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccHead", "AccTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.AccHead", "AccTypeId");
            AddForeignKey("dbo.AccHead", "AccTypeId", "dbo.AccHead", "AccHeadId");
            AddForeignKey("dbo.AccHead", "AccTypeId", "dbo.AccType", "AccTypeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccHead", "AccTypeId", "dbo.AccType");
            DropForeignKey("dbo.AccHead", "AccTypeId", "dbo.AccHead");
            DropIndex("dbo.AccHead", new[] { "AccTypeId" });
            DropColumn("dbo.AccHead", "AccTypeId");
        }
    }
}
