namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBvsServiceTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BvsService",
                c => new
                    {
                        BvsServiceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BvsServiceId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BvsService");
        }
    }
}
