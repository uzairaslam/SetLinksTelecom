namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSubnameFromItemTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Item", "Subname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Item", "Subname", c => c.String(maxLength: 50));
        }
    }
}
