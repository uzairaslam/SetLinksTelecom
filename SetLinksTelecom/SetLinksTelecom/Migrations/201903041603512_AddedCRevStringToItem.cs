namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCRevStringToItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "CRevString", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Item", "CRevString");
        }
    }
}
