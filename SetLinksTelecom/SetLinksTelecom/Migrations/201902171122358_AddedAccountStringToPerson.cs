namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccountStringToPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "AccString", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "AccString");
        }
    }
}
