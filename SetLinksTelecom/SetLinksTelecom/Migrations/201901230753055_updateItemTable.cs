namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateItemTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "Subname", c => c.String());
            AddColumn("dbo.Item", "ProductCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Item", "ProductCategoryId");
            AddForeignKey("dbo.Item", "ProductCategoryId", "dbo.ProductCategory", "ProductCategoryId", cascadeDelete: true);
            DropColumn("dbo.Item", "ItemType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Item", "ItemType", c => c.String());
            DropForeignKey("dbo.Item", "ProductCategoryId", "dbo.ProductCategory");
            DropIndex("dbo.Item", new[] { "ProductCategoryId" });
            DropColumn("dbo.Item", "ProductCategoryId");
            DropColumn("dbo.Item", "Subname");
        }
    }
}
