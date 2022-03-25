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
    [RoutePrefix("api/ReportUserOrder")]
    public class ReportOrderUserController : ApiControllerBase
    {
        IReportOrderUserService _reportOrderUserService;
        public ReportOrderUserController(IErrorService errorService, IReportOrderUserService reportOrderUserService) : base(errorService)
        {
            _reportOrderUserService = reportOrderUserService;
        }

        [Route("getUserOrder")]
        [HttpGet]
        public HttpResponseMessage GetUserOrder(HttpRequestMessage request)
        {
            var model = _reportOrderUserService.getUserOrder();
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
            return response; ;
        }


        [Route("getReportOrder/{userId}")]
        [HttpGet]
        public HttpResponseMessage GetReportUserOrder(HttpRequestMessage request , string userId)
        {
            var model = _reportOrderUserService.GetReportOrder(userId).ToList();
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
            return response;
        }

    }
}
