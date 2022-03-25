using AutoMapper;
using Microsoft.Owin.Security;
using OnlineShop.Common;
using OnlineShop.Model.Models;
using OnlineShop.Service;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Controllers
{
    public class HomeController : Controller
    {
        IProductCategoryService _productCategoryService;
        ICommonService _commonService;
        IProductService _productService;
        public HomeController(IProductCategoryService productCategoryService , ICommonService commonService , IProductService productService)
        {
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _productService = productService;
        }

        // GET: Home
        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var slideModel = _commonService.GetSlides();
            var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            HomeViewModel homeViewModel = new HomeViewModel();

            var lastestLaptopModel = _productService.GetLastestLaptop(4);

            var lastestPhone = _productService.GetLastestPhone(4);

            var topSaleProductModel = _productService.GetHotProduct(12).ToList();

            var lastestLaptopViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestLaptopModel);
            var lastestPhoneViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel >>(lastestPhone);

            var topSaleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(topSaleProductModel);

            homeViewModel.LastestLaptop = lastestLaptopViewModel;
            homeViewModel.lastestPhone = lastestPhoneViewModel;

            homeViewModel.TopSaleProducts = topSaleProductViewModel;
            homeViewModel.slideView = slideView;

            try
            {
                homeViewModel.Title = _commonService.GetSystemConfig(CommonConstants.HomeTitle).ValueString;
                homeViewModel.MetaKeyword = _commonService.GetSystemConfig(CommonConstants.HomeMetaKeyword).ValueString;
                homeViewModel.MetaDescription = _commonService.GetSystemConfig(CommonConstants.HomeMetaDescription).ValueString;
            }
            catch
            {

            }

            return View(homeViewModel);
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public ActionResult SideBar()//Category
        {

            var model = _productCategoryService.GetAll().ToList();
            var listCategory = model.Where(x => x.Status).ToList();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(listCategory);

            return PartialView(listProductCategoryViewModel);
        }

        [ChildActionOnly]
        //[OutputCache(Duration =3600)]
        public ActionResult Footer()
        {

            var footerModel = _commonService.GetFooter();
            var footerViewModel = Mapper.Map<Footer, FooterViewModel>(footerModel);

            return PartialView(footerViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {

            var url = System.Web.HttpContext.Current.Request.Url;
            ViewBag.urlHome = url.Scheme + "://" + url.Host + ":" + url.Port;
            return PartialView();
        }

    }
}