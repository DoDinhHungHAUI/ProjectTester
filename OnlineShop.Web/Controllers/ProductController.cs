using AutoMapper;
using OnlineShop.Common;
using OnlineShop.Model.Models;
using OnlineShop.Service;
using OnlineShop.Web.infrastructure.Core;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Controllers
{
    public class ProductController : Controller
    {

        IProductService _productService;
        IProductCategoryService _productCategoryService;

        public ProductController(IProductService productService , IProductCategoryService productCategoryService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
        }

        // GET: Product
        [Authorize(Roles = "ManagerProduct")]
        public ActionResult Detail(int id)
        {
            var productModel = _productService.GetById(id);
            var viewModel = Mapper.Map<Product, ProductViewModel>(productModel);

            var realteProducts  = _productService.GetReatedProducts(id, 8);

            ViewBag.listRecommentProduct = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(realteProducts);

            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_productService.GetListTagByProductId(id));

            return View(viewModel);
        }

        public ActionResult Category(int id , int page = 1 , string sort = "")
        {
            int pageSize = ConfigHelper.pageSize;
            int totalRow = 0;
            var productModel = _productService.GetListProductByCategoryIdPaging(id, page, pageSize , sort, out totalRow);

            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var category = _productCategoryService.GetById(id);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);

            var paginationSet = new PaginationSet<ProductViewModel>
            {
                Items = productViewModel,
                MaxPage = ConfigHelper.MaxPage,
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public JsonResult GetListProductByName(string keyword)
        {
            var model = _productService.GetListProductByName(keyword);

            var viewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

            List<string> listNameSearch = new List<string>();
            foreach(var item in viewModel)
            {
                listNameSearch.Add(item.Name);
            }

            return Json(new
            {
                data = listNameSearch
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string keyword, int page = 1, string sort = "")
        {
            int pageSize = ConfigHelper.pageSize;
            int totalRow = 0;
            var productModel = _productService.Search(keyword, page, pageSize, sort, out totalRow);

            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.keyword = keyword;

            var paginationSet = new PaginationSet<ProductViewModel>
            {
                Items = productViewModel,
                MaxPage = ConfigHelper.MaxPage,
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public ActionResult ListByTag(string tagId , int page = 1)
        {
            int pageSize = ConfigHelper.pageSize;
            int totalRow = 0;
            var productModel = _productService.GetListProductByTag(tagId, page, pageSize, out totalRow);//Lấy ra danh sách product dựa vào tag (id)
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            ViewBag.Tag = Mapper.Map<Tag, TagViewModel>(_productService.GetTag(tagId));
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = ConfigHelper.MaxPage,
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }



    }
}