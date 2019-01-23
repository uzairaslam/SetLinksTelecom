namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateItemTableSubName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Item", "Subname", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Item", "Subname", c => c.String());
        }
    }
}
