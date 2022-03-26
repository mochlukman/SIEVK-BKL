using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIEVK.Service
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("{WebPage}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{WebForm}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );


            ////////////////////////////////////////////////
            ///
            //routes.RouteExistingFiles = true;

            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.IgnoreRoute("Content/{*pathInfo}");
            //routes.IgnoreRoute("Scripts/{*pathInfo}");
            //routes.IgnoreRoute("{WebPage}.aspx/{*pathInfo}");
            //routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            //routes.MapRoute(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults

            //);
        }
    }
}
