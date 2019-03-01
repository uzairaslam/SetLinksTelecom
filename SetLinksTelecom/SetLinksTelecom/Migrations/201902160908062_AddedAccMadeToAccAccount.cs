namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccMadeToAccAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccAccount", "AccMade", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccAccount", "AccMade");
        }
    }
}
