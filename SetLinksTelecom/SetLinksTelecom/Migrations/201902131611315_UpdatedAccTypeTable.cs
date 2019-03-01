namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedAccTypeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccType", "TypeCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccType", "TypeCode");
        }
    }
}
