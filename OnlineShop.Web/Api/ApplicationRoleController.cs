using AutoMapper;
using OnlineShop.Common.Exceptions;
using OnlineShop.Model.Models;
using OnlineShop.Service;
using OnlineShop.Web.infrastructure.Core;
using OnlineShop.Web.infrastructure.Extensions;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineShop.Web.Api
{
    [RoutePrefix("api/applicationRole")]
   
    public class ApplicationRoleController : ApiControllerBase
    {
        private IApplicationRoleService _appRoleService;

        public ApplicationRoleController(IErrorService errorService,
            IApplicationRoleService appRoleService) : base(errorService)
        {
            _appRoleService = appRoleService;
        }

        [Route("getList")]
        [HttpGet]

        public HttpResponseMessage GetList(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _appRoleService.GetAll();

                var responseData = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;

            });
        }    

        [Route("detail/{id}")]
        [HttpGet]

        public HttpResponseMessage Details(HttpRequestMessage request , string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest,  " Không có giá trị.");
            }
            ApplicationRole appRole = _appRoleService.GetDetail(id);

            if(appRole == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "no Group");
            }
            return request.CreateResponse(HttpStatusCode.OK, appRole);
        }    


        [HttpPost]
        [Route("add")]

        public HttpResponseMessage Create(HttpRequestMessage request , ApplicationRoleViewModel applicationRoleViewModel)
        {
            if(ModelState.IsValid)
            {
                var newAppRole = new ApplicationRole();
                newAppRole.UpdateApplicationRole(applicationRoleViewModel);
                try
                {
                    _appRoleService.Add(newAppRole);
                    _appRoleService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, applicationRoleViewModel);
                }
                catch(NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.OK, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }    

        [HttpPut]
        [Route("update")]

        public HttpResponseMessage Update(HttpRequestMessage request , ApplicationRoleViewModel applicationRoleViewModel)
        {
            if(ModelState.IsValid)
            {
                var appRole = _appRoleService.GetDetail(applicationRoleViewModel.Id);
                try
                {
                    appRole.UpdateApplicationRole(applicationRoleViewModel, "update");
                    _appRoleService.Update(appRole);
                    _appRoleService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, appRole);
                }
                catch(NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public HttpResponseMessage Delete(HttpRequestMessage request , string id)
        {
            _appRoleService.Delete(id);
            _appRoleService.Save();
            return request.CreateResponse(HttpStatusCode.OK, id);
        }
    }
}
