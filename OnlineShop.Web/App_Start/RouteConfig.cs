using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Search",
                url: "tim-kiem.html",
                defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
            );

            routes.MapRoute(
               name: "CheckOut",
                url: "thanh-toan.html",
                defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
           );

            routes.MapRoute(
                name: "Cart",
                url: "gio-hang.html",
                defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap.html",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                 namespaces: new string[] { "OnlineShop.Web.Controllers" }
            );
            routes.MapRoute(
               name: "Register",
               url: "dang-ky.html",
               defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
           );

            routes.MapRoute(
               name: "MyProfile",
               url: "Thong-tin-tai-khoan.html",
               defaults: new { controller = "Account", action = "MyProfile", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
           );

            routes.MapRoute(
                name: "EditUser",
                url: "Sua-Tai-khoan.html",
                defaults: new { controller = "Account", action = "EditUser", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
            );


            routes.MapRoute(
                name: "Contact",
                url: "lien-he.html",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
            );

            routes.MapRoute(
                name: "About",
                url: "gioi-thieu.html",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ProductCategory",
                url: "{alias}.pc-{id}.html",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Product",
                url: "{alias}.p-{id}.html",
                defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlineShop.Web.Controllers" }
            );

            routes.MapRoute(
             name: "TagList",
             url: "tag/{tagId}.html",
             defaults: new { controller = "Product", action = "ListByTag", tagId = UrlParameter.Optional },
               namespaces: new string[] { "OnlineShop.Web.Controllers" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
