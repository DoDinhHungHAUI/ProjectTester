using OnlineShop.Service;
using OnlineShop.Web.infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineShop.Web.Api
{

    [RoutePrefix("api/statistic")]
   
    public class StatisticController : ApiControllerBase
    {
        IStatisticService _statisticService;
        public StatisticController(IErrorService errorService , IStatisticService statisticService) : base(errorService)
        {
            _statisticService = statisticService;
        }

        [Route("getrevenue")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatistic(HttpRequestMessage request , string fromDate , string toDate)
        {
            var model = _statisticService.GetRevenueStatistic(fromDate, toDate).ToList();
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
            return response; ;
        }
    }
}
