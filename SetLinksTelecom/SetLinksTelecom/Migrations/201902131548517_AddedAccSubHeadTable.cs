namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccSubHeadTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccSubHead",
                c => new
                    {
                        AccSubHeadId = c.Int(nullable: false, identity: true),
                        AccHeadId = c.Int(nullable: false),
                        SubHeadString = c.String(maxLength: 7),
                        SubHeadName = c.String(maxLength: 50),
                        OID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccSubHeadId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccSubHead");
        }
    }
}
