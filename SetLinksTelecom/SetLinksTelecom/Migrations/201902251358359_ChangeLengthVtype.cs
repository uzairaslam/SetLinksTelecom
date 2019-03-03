namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLengthVtype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccVoucher", "VType", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccVoucher", "VType", c => c.String(maxLength: 2));
        }
    }
}
