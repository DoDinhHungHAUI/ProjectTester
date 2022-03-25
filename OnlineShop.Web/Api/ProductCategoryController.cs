
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineShop.Model.Models;
using OnlineShop.Service;
using OnlineShop.Web.infrastructure.Core;
using OnlineShop.Web.Models;
using OnlineShop.Web.infrastructure.Extensions;
using Microsoft.AspNetCore.Cors;
using System.Web;
using System.IO;

namespace OnlineShop.Web.Api
{
    [RoutePrefix("api/productcategory")]
    //[EnableCors("AllowAllHeaders")]
    public class ProductCategoryController : ApiControllerBase
    {
        #region Initialize
        IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }
        #endregion

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetAll();
                var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        //[Route("getbyid/{id : int}")]
        //[HttpGet]

        //public HttpResponseMessage GetById(HttpRequestMessage request , int id)
        //{
        //    return CreateHttpResponse(request, () =>
        //   {
        //       var model = _productCategoryService.GetById(id);
        //       var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(model);
        //       var response = request.CreateResponse(HttpStatusCode.OK, responseData);
        //       return response;
        //   });
        //}


        [Route("create")]
        //[EnableCors("AllowAllHeaders")]
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductCategoryViewModel productCategoryVM)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                }
                else
                {
                    var newProductCategory = new ProductCategory();

                  
                    newProductCategory.UpdateProductCategory(productCategoryVM);
                    newProductCategory.CreatedDate = DateTime.Now;
                    _productCategoryService.Add(newProductCategory);
                    _productCategoryService.Save();

                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(newProductCategory);

                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]

        public HttpResponseMessage Update(HttpRequestMessage request , ProductCategoryViewModel productCategoryVm)
        {
            return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;
               if(!ModelState.IsValid)
               {
                   response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
               }
               else
               {
                   var dbProductCategory = _productCategoryService.GetById(productCategoryVm.ID);
                   dbProductCategory.UpdateProductCategory(productCategoryVm);
                   dbProductCategory.UpdatedDate = DateTime.Now;

                   _productCategoryService.Update(dbProductCategory);
                   _productCategoryService.Save();

                   var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(dbProductCategory);
                   response = request.CreateResponse(HttpStatusCode.Created, responseData);

               }
               return response;
           });
        }


        [Route("delete/{id}")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request , int id)
        {
            return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;
               if (!ModelState.IsValid)
               {
                   response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
               }
               else
               {
                   var oldProductCategory = _productCategoryService.Delete(id);
                   _productCategoryService.Save();
                   var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(oldProductCategory);
                   response = request.CreateResponse(HttpStatusCode.Created, responseData);
               }
               return response;
           });
        }

    }
}
