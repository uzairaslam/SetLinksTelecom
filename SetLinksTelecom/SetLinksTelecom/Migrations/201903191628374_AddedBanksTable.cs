namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBanksTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bank",
                c => new
                    {
                        BankId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccNumber = c.String(),
                        AccString = c.String(maxLength: 12),
                    })
                .PrimaryKey(t => t.BankId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bank");
        }
    }
}
