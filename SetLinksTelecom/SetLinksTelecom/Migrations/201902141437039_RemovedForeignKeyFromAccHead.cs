namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedForeignKeyFromAccHead : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AccHead", "AccTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccHead", "AccTypeId", c => c.Int());
        }
    }
}
