namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccAccountTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccAccount",
                c => new
                    {
                        AccAccountId = c.Int(nullable: false, identity: true),
                        HeadCode = c.Int(nullable: false),
                        SubHeadCode = c.Int(nullable: false),
                        AccString = c.String(maxLength: 12),
                        AccName = c.String(maxLength: 80),
                        OID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccAccountId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccAccount");
        }
    }
}
