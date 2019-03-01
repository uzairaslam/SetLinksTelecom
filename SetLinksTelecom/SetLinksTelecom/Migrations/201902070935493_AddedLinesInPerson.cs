namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLinesInPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "BusinessLineMap", c => c.Int());
            AddColumn("dbo.Person", "PersonalLineMap", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "PersonalLineMap");
            DropColumn("dbo.Person", "BusinessLineMap");
        }
    }
}
