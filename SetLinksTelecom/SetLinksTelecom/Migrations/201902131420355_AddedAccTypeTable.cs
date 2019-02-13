namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccTypeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccType",
                c => new
                    {
                        AccTypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.AccTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccType");
        }
    }
}
