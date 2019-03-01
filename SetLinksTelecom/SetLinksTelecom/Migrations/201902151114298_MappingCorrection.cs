namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MappingCorrection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccHead", "AccTypeId", "dbo.AccHead");
        }
        
        public override void Down()
        {
            AddForeignKey("dbo.AccHead", "AccTypeId", "dbo.AccHead", "AccHeadId");
        }
    }
}
