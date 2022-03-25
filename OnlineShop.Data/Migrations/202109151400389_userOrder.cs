namespace OnlineShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userOrder : DbMigration
    {
        public override void Up()
        {

            CreateStoredProcedure("GetUserOrder",
               p => new
               {
               },
               @"select Orders.CustomerId , ApplicationUsers.FullName , ApplicationUsers.PhoneNumber , ApplicationUsers.Email , ApplicationUsers.Address , COUNT(*) as 'SoLuong'
                from ApplicationUsers inner join Orders on ApplicationUsers.Id = Orders.CustomerId
                group by Orders.CustomerId , ApplicationUsers.FullName , ApplicationUsers.PhoneNumber , ApplicationUsers.Email , ApplicationUsers.Address"
           );

        }

        public override void Down()
        {
            DropStoredProcedure("dbo.GetUserOrder");
        }
    }
}
