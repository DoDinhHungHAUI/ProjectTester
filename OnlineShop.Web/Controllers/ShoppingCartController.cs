using AutoMapper;
using Microsoft.AspNet.Identity;
using OnlineShop.Common;
using OnlineShop.Model.Models;
using OnlineShop.Service;
using OnlineShop.Web.App_Start;
using OnlineShop.Web.infrastructure.Extensions;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnlineShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        private ApplicationUserManager _userManager;
        IOrderService _orderService;
      
        public ShoppingCartController(IProductService productService , ApplicationUserManager useManager , IOrderService orderService)
        {
            this._productService = productService;
            this._userManager = useManager;
            this._orderService = orderService;

        }

        // GET: ShoppongCart
        public ActionResult Index()
        {
            if(Session[CommonConstants.SessionCart] == null)
            {
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            }

            var url = System.Web.HttpContext.Current.Request.Url;
            ViewBag.urlHome = url.Scheme + "://" + url.Host + ":" + url.Port;

            return View();
        }

        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            }
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true,
                Counter = TinhTongSoLuong()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productService.GetById(productId);
            if(cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }    

            if(product.Quantity == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                });
            }    

            if(cart.Any(x => x.ProductId == productId))
            {
                foreach(var item in cart)
                {
                    if(item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }    
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }
            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true,
                Counter = TinhTongSoLuong()
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if(cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true,
                    Counter = TinhTongSoLuong()
                });
            }
            return Json(new
            {
                status = false
            });
        }


        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            foreach(var item in cartSession)
            {
                foreach(var jitem in cartViewModel)
                {
                    if(item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }    
                }    
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true,
                Counter = TinhTongSoLuong()
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true,
                Counter = TinhTongSoLuong()
            });
        }

        public ActionResult CheckOut()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/gio-hang.html");
            }
            return View();
        }

        public ActionResult GetUser()
        {
            if(Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public JsonResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            var orderNew = new Order();

            orderNew.UpdateOrder(order);
            
            if(Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            bool isEnough = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = item.Product.Price;
                orderDetails.Add(detail);

                isEnough = _productService.SellProduct(item.ProductId, item.Quantity);
                break;
            }

            if(isEnough)
            {
                _orderService.Create(orderNew, orderDetails);
                _productService.Save();
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Không đủ hàng."
                });
            }
        }

        //Phương thức tính tổng Số lượng
        public double TinhTongSoLuong()
        {
            //Lấy giỏ hàng
            List<ShoppingCartViewModel> lstGioHang = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.Quantity);
        }

        public ActionResult CartPartial()
        {
            var Counter = TinhTongSoLuong();
            ViewBag.QuantityCart = Counter;

            return PartialView();
        }    


    }
}