namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PercisionTo4DebitAndCredit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccVoucher", "Debit", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.AccVoucher", "Credit", c => c.Decimal(nullable: false, precision: 20, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccVoucher", "Credit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.AccVoucher", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
