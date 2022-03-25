using AutoMapper;
using OnlineShop.Common.Exceptions;
using OnlineShop.Model.Models;
using OnlineShop.Service;
using OnlineShop.Web.App_Start;
using OnlineShop.Web.infrastructure.Core;
using OnlineShop.Web.infrastructure.Extensions;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace OnlineShop.Web.Api
{
    [RoutePrefix("api/applicationGroup")]
 
    public class ApplicationGroupController : ApiControllerBase
    {

        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;
        private ApplicationUserManager _userManager;

        public ApplicationGroupController(IErrorService errorService,
           IApplicationRoleService appRoleService,
           ApplicationUserManager userManager,
           IApplicationGroupService appGroupService) : base(errorService)
        {
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;
            _userManager = userManager;
        }

        [Route("getlist")]
        [HttpGet]   
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;
               var model = _appGroupService.GetAll();
               IEnumerable<ApplicationGroupViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);
               response = request.CreateResponse(HttpStatusCode.OK, modelVm);

               return response;
           });
        }    

        [Route("getRoles/{groupId}")]
        [HttpGet]
        public HttpResponseMessage GetListGroup(HttpRequestMessage request, int groupId)
        {
            try
            {
                var listGroup = _appRoleService.GetListRoleByGroupId(groupId);

                var listGroupViewModel = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(listGroup);
                return request.CreateResponse(HttpStatusCode.OK, listGroupViewModel);
            }
            catch(Exception ex)
            {
                // Log exception code goes here  
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request , int id)
        {
            if(id == 0)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + "is required");
            }
            ApplicationGroup appGroup = _appGroupService.GetDetail(id);
            var appGroupViewModel = Mapper.Map<ApplicationGroup, ApplicationGroupViewModel>(appGroup);
            if(appGroup == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "No Group");
            }
            var listRole = _appRoleService.GetListRoleByGroupId(appGroupViewModel.ID);
            appGroupViewModel.Roles = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(listRole);
            return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);

        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request , ApplicationGroupViewModel appGroupViewModel)
        {
            if(ModelState.IsValid)
            {
                var newAppGroup = new ApplicationGroup();
                newAppGroup.Name = appGroupViewModel.Name;
                try
                {
                    var appGroup = _appGroupService.Add(newAppGroup);
                    _appGroupService.Save();

                    //Save group
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach(var role in appGroupViewModel.Roles)//Lấy ra các cái roles của applicationGroup
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    }

                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _appRoleService.Save();

                    return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
                }
                catch(NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var appGroup = _appGroupService.GetDetail(appGroupViewModel.ID);
                try
                {
                    appGroup.UpdateApplicationGroup(appGroupViewModel);
                    _appGroupService.Update(appGroup);
                    //_appGroupService.Save();

                    //save group
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in appGroupViewModel.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    }
                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _appRoleService.Save();

                    //add role to user
                    var listRole = _appRoleService.GetListRoleByGroupId(appGroup.ID).ToList();
                    var listUserInGroup = _appGroupService.GetListUserByGroupId(appGroup.ID);
                    foreach (var user in listUserInGroup)
                    {
                        var listRoleName = listRole.Select(x => x.Name).ToList();
                        foreach (var roleName in listRoleName)
                        {
                            await _userManager.RemoveFromRoleAsync(user.Id, roleName);
                            await _userManager.AddToRoleAsync(user.Id, roleName);
                        }
                    }
                    return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            var appGroup = _appGroupService.Delete(id);
            _appGroupService.Save();
            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }

    
    }
}
