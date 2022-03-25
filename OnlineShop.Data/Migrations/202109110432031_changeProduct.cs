namespace OnlineShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Quantity", c => c.Int());
            DropColumn("dbo.Products", "Quality");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Quality", c => c.Int());
            DropColumn("dbo.Products", "Quantity");
        }
    }
}
