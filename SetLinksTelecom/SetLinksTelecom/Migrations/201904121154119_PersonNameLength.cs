namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonNameLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "Name", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
