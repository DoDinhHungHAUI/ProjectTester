namespace OnlineShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reportOrder : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("GetReportOrder",
                p => new
                {
                    userId = p.String(),
                },
                @"select Orders.CustomerName , Orders.CustomerId , ApplicationUsers.PhoneNumber , ApplicationUsers.Address  , Products.Name , OrderDetails.Quantity, Products.Price , Orders.CreatedDate
					 from Orders inner join OrderDetails on Orders.ID = OrderDetails.OrderID
				     inner join Products on OrderDetails.ProductID = Products.ID
					 inner join ApplicationUsers on ApplicationUsers.Id = Orders.CustomerId
					 where Orders.CustomerId = @userId"
            );

        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.GetReportOrder");
        }
    }
}
