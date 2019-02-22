namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesQtyTypeSaleDetails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SaleDetail", "Qty", c => c.Decimal(nullable: false, precision: 20, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SaleDetail", "Qty", c => c.Int(nullable: false));
        }
    }
}
