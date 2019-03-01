namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedLineModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Line",
                c => new
                    {
                        LineId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LineId);
            
            CreateTable(
                "dbo.LinePerson",
                c => new
                    {
                        Line_LineId = c.Int(nullable: false),
                        Person_PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Line_LineId, t.Person_PersonId })
                .ForeignKey("dbo.Line", t => t.Line_LineId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.Person_PersonId, cascadeDelete: true)
                .Index(t => t.Line_LineId)
                .Index(t => t.Person_PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LinePerson", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.LinePerson", "Line_LineId", "dbo.Line");
            DropIndex("dbo.LinePerson", new[] { "Person_PersonId" });
            DropIndex("dbo.LinePerson", new[] { "Line_LineId" });
            DropTable("dbo.LinePerson");
            DropTable("dbo.Line");
        }
    }
}
