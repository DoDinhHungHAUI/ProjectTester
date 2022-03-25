namespace OnlineShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Color", c => c.String());
            AddColumn("dbo.Products", "Model", c => c.String());
            AddColumn("dbo.Products", "whereProduct", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "whereProduct");
            DropColumn("dbo.Products", "Model");
            DropColumn("dbo.Products", "Color");
        }
    }
}
