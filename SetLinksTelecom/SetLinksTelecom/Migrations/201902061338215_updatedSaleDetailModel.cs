namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedSaleDetailModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SaleDetail", "Sale_SaleId", "dbo.Sale");
            DropIndex("dbo.SaleDetail", new[] { "Sale_SaleId" });
            RenameColumn(table: "dbo.SaleDetail", name: "Sale_SaleId", newName: "SaleId");
            AlterColumn("dbo.SaleDetail", "SaleId", c => c.Int(nullable: false));
            CreateIndex("dbo.SaleDetail", "SaleId");
            AddForeignKey("dbo.SaleDetail", "SaleId", "dbo.Sale", "SaleId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleDetail", "SaleId", "dbo.Sale");
            DropIndex("dbo.SaleDetail", new[] { "SaleId" });
            AlterColumn("dbo.SaleDetail", "SaleId", c => c.Int());
            RenameColumn(table: "dbo.SaleDetail", name: "SaleId", newName: "Sale_SaleId");
            CreateIndex("dbo.SaleDetail", "Sale_SaleId");
            AddForeignKey("dbo.SaleDetail", "Sale_SaleId", "dbo.Sale", "SaleId");
        }
    }
}
