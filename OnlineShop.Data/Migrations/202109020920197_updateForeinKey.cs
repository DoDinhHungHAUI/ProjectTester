namespace OnlineShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateForeinKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.OrderDetails", name: "ProductID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.OrderDetails", name: "OrderID", newName: "ProductID");
            RenameColumn(table: "dbo.OrderDetails", name: "__mig_tmp__0", newName: "OrderID");
            RenameIndex(table: "dbo.OrderDetails", name: "IX_ProductID", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.OrderDetails", name: "IX_OrderID", newName: "IX_ProductID");
            RenameIndex(table: "dbo.OrderDetails", name: "__mig_tmp__0", newName: "IX_OrderID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.OrderDetails", name: "IX_OrderID", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.OrderDetails", name: "IX_ProductID", newName: "IX_OrderID");
            RenameIndex(table: "dbo.OrderDetails", name: "__mig_tmp__0", newName: "IX_ProductID");
            RenameColumn(table: "dbo.OrderDetails", name: "OrderID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.OrderDetails", name: "ProductID", newName: "OrderID");
            RenameColumn(table: "dbo.OrderDetails", name: "__mig_tmp__0", newName: "ProductID");
        }
    }
}
