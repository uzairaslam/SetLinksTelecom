namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPidAndNumberPurchase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchase", "Pid", c => c.Int(nullable: false));
            AddColumn("dbo.Purchase", "Number", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchase", "Number");
            DropColumn("dbo.Purchase", "Pid");
        }
    }
}
