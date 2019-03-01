namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedLineModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PersonLine", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.PersonLine", "Line_LineId", "dbo.Line");
            DropIndex("dbo.PersonLine", new[] { "Person_PersonId" });
            DropIndex("dbo.PersonLine", new[] { "Line_LineId" });
            AddColumn("dbo.Line", "Percentage", c => c.Decimal(nullable: false, precision: 20, scale: 2));
            DropTable("dbo.PersonLine");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PersonLine",
                c => new
                    {
                        Person_PersonId = c.Int(nullable: false),
                        Line_LineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_PersonId, t.Line_LineId });
            
            DropColumn("dbo.Line", "Percentage");
            CreateIndex("dbo.PersonLine", "Line_LineId");
            CreateIndex("dbo.PersonLine", "Person_PersonId");
            AddForeignKey("dbo.PersonLine", "Line_LineId", "dbo.Line", "LineId", cascadeDelete: true);
            AddForeignKey("dbo.PersonLine", "Person_PersonId", "dbo.Person", "PersonId", cascadeDelete: true);
        }
    }
}
