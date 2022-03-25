namespace OnlineShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addQualityToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Quality", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Quality");
        }
    }
}
