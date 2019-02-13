namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccHeadTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccHead",
                c => new
                    {
                        AccHeadId = c.Int(nullable: false, identity: true),
                        HeadString = c.String(maxLength: 25),
                        HeadName = c.String(maxLength: 50),
                        AccTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.AccHeadId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccHead");
        }
    }
}
