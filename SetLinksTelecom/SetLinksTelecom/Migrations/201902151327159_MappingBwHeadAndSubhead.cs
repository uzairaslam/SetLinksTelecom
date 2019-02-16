namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MappingBwHeadAndSubhead : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AccSubHead", "AccHeadId");
            AddForeignKey("dbo.AccSubHead", "AccHeadId", "dbo.AccHead", "AccHeadId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccSubHead", "AccHeadId", "dbo.AccHead");
            DropIndex("dbo.AccSubHead", new[] { "AccHeadId" });
        }
    }
}
