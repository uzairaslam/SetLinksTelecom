namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedAccountsTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccAccount", "AccCode", c => c.Int(nullable: false));
            AddColumn("dbo.AccHead", "HeadCode", c => c.Int(nullable: false));
            AddColumn("dbo.AccHead", "TypeCode", c => c.Int(nullable: false));
            AddColumn("dbo.AccSubHead", "SubHeadCode", c => c.Int(nullable: false));
            AddColumn("dbo.AccSubHead", "HeadCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccSubHead", "HeadCode");
            DropColumn("dbo.AccSubHead", "SubHeadCode");
            DropColumn("dbo.AccHead", "TypeCode");
            DropColumn("dbo.AccHead", "HeadCode");
            DropColumn("dbo.AccAccount", "AccCode");
        }
    }
}
