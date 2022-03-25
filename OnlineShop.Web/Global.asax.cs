using OnlineShop.Web.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace OnlineShop.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

       /* protected void Application_BeginRequest()
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                Response.Flush();
            }
        }*/

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AutoMapperConfiguration.Configure();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }

        /*protected void Application_BeginRequest()
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }*/

        //protected void Application_BeginRequest()
        //{
        //    HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        //}


        

    }
}
