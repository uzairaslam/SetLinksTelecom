namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersontableChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "FatherName", c => c.String(maxLength: 30));
            AlterColumn("dbo.Person", "Gender", c => c.String(nullable: false, maxLength: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "Gender", c => c.String());
            AlterColumn("dbo.Person", "FatherName", c => c.String());
        }
    }
}
