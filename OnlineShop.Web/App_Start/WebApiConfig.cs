using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OnlineShop.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            /*config.EnableCors(new EnableCorsAttribute("http://localhost:4200", "*", "*"));*/

            /*var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            config.EnableCors(cors);*/

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            

       

        }
    }
}
