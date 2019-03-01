namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccountStringToItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "AccString", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Item", "AccString");
        }
    }
}
