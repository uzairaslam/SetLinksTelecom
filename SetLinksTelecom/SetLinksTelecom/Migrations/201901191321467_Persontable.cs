namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Persontable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        FatherName = c.String(),
                        Gender = c.String(),
                        CNIC = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        CNICIssueDate = c.DateTime(nullable: false),
                        CurrentAddress = c.String(),
                        PermanentAddress = c.String(),
                        MobileBusiness = c.String(nullable: false),
                        MobilePersonal = c.String(nullable: false),
                        Qualification = c.String(),
                        DesignationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Designation", t => t.DesignationId, cascadeDelete: true)
                .Index(t => t.DesignationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Person", "DesignationId", "dbo.Designation");
            DropIndex("dbo.Person", new[] { "DesignationId" });
            DropTable("dbo.Person");
        }
    }
}
