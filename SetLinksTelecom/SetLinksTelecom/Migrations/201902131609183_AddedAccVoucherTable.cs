namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccVoucherTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccVoucher",
                c => new
                    {
                        AccVoucherId = c.Int(nullable: false, identity: true),
                        VDate = c.DateTime(nullable: false),
                        SessionId = c.Int(nullable: false),
                        AccString = c.String(maxLength: 12),
                        VNo = c.Int(nullable: false),
                        VType = c.String(maxLength: 2),
                        VSrNo = c.Int(nullable: false),
                        VDescription = c.String(maxLength: 300),
                        Debit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Credit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserCode = c.Int(nullable: false),
                        OID = c.Int(nullable: false),
                        BID = c.Int(nullable: false),
                        CID = c.Int(nullable: false),
                        HeadCode = c.Int(nullable: false),
                        SubHeadCode = c.Int(nullable: false),
                        AccCode = c.Int(nullable: false),
                        ChequeNo = c.String(maxLength: 25),
                        InvNo = c.String(maxLength: 50),
                        InvType = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AccVoucherId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccVoucher");
        }
    }
}
