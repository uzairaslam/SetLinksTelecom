namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPurchasesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Purchase",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        Comments = c.String(maxLength: 150),
                        Qty = c.Int(nullable: false),
                        PaidAmount = c.Decimal(nullable: false, precision: 20, scale: 2),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseId)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purchase", "ItemId", "dbo.Item");
            DropIndex("dbo.Purchase", new[] { "ItemId" });
            DropTable("dbo.Purchase");
        }
    }
}
