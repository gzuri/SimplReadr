using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimplReaderMVC {
	public class RouteConfig {
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "DefaultReader",
                 url: "Reader/{feedid}",
                 defaults: new { controller = "Reader", action = "Index", feedID = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "LogOn",
                 url: "LogOn",
                 defaults: new { controller = "Account", action = "LogOn" }
            );

            routes.MapRoute(
                 name: "LogOut",
                 url: "LogOut",
                 defaults: new { controller = "Account", action = "LogOut" }
            );

			routes.MapRoute(
				 name: "Default",
				 url: "{controller}/{action}/{id}",
				 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}