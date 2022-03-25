namespace OnlineShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateForeinKeyOrderDetails : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderDetails", "ProductID", "dbo.ProductCategories");
            AddForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products");
            AddForeignKey("dbo.OrderDetails", "ProductID", "dbo.ProductCategories", "ID", cascadeDelete: true);
        }
    }
}
