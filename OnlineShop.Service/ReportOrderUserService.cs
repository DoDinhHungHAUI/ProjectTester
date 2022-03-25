using OnlineShop.Common.ViewModels;
using OnlineShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Service
{

    public interface IReportOrderUserService
    {
        IEnumerable<ReportOrderViewModel> GetReportOrder(string userId);

        IEnumerable<userOrder> getUserOrder();
    }

    class ReportOrderUserService : IReportOrderUserService
    {

        IOrderRepository _orderRepository;
        public ReportOrderUserService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IEnumerable<ReportOrderViewModel> GetReportOrder(string userId)
        {
           return  _orderRepository.GetReportOrder(userId);
        }

        public IEnumerable<userOrder> getUserOrder()
        {
            return _orderRepository.getUserOrder();
        }
    }
}
