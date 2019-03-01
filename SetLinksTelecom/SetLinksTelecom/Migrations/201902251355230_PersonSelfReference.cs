namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonSelfReference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "BossId", c => c.Int());
            CreateIndex("dbo.Person", "BossId");
            AddForeignKey("dbo.Person", "BossId", "dbo.Person", "PersonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Person", "BossId", "dbo.Person");
            DropIndex("dbo.Person", new[] { "BossId" });
            DropColumn("dbo.Person", "BossId");
        }
    }
}
