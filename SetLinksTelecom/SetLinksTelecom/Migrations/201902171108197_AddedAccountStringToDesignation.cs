namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccountStringToDesignation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Designation", "AccString", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Designation", "AccString");
        }
    }
}
