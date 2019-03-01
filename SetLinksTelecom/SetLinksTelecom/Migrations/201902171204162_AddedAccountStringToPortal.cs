namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccountStringToPortal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Portal", "AccString", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Portal", "AccString");
        }
    }
}
