using OnlineShop.Common.ViewModels;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Model.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace OnlineShop.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);

        IEnumerable<ReportOrderViewModel> GetReportOrder(string userId);

        IEnumerable<userOrder> getUserOrder();

    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate" , toDate)
            };
            return DbContext.Database.SqlQuery<RevenueStatisticViewModel>("GetRevenueStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<ReportOrderViewModel> GetReportOrder(string userId)
        {
            var parameters = new SqlParameter[]
            {
              new SqlParameter("@userId", userId)
            };
            return DbContext.Database.SqlQuery<ReportOrderViewModel>("GetReportOrder @userId", parameters);
        }

        public IEnumerable<userOrder> getUserOrder()
        {
            return DbContext.Database.SqlQuery<userOrder>("GetUserOrder");
        }
    }
}