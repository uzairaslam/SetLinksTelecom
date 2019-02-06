namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedLinesincontext : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LinePerson", newName: "PersonLine");
            DropPrimaryKey("dbo.PersonLine");
            AddPrimaryKey("dbo.PersonLine", new[] { "Person_PersonId", "Line_LineId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PersonLine");
            AddPrimaryKey("dbo.PersonLine", new[] { "Line_LineId", "Person_PersonId" });
            RenameTable(name: "dbo.PersonLine", newName: "LinePerson");
        }
    }
}
