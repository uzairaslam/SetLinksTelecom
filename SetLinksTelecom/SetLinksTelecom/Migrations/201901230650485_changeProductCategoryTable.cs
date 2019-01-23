namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeProductCategoryTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductCategory", "Name", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductCategory", "Name", c => c.String());
        }
    }
}
