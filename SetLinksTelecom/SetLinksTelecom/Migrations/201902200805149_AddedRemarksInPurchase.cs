namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRemarksInPurchase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "Remarks", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "Remarks");
        }
    }
}
