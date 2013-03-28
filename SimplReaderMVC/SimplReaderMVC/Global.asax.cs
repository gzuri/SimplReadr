using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SimplReaderMVC {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication {
		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
            RegisterBundles(BundleTable.Bundles);
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

        protected void Application_BeginRequest()
        {
        }

	    protected void Application_EndRequest()
        {
        }

        static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new ScriptBundle("~/Content/js").Include(
                    "~/Scripts/jquery.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/jquery.lazyload.min.js",
                    "~/Scripts/MVC/jquery.unobtrusive-ajax.min.js",
                    "~/Scripts/MVC/jquery.validate.min.js",
                    "~/Scripts/MVC/jquery.validate.unobtrusive.min.js",
                    "~/Scripts/noty/jquery.noty.js",
                    "~/Scripts/noty/top.js",
                    "~/Scripts/qtip/jquery.qtip.min.js",
                    "~/Scripts/custom.js"
                ));
            bundles.Add(new StyleBundle("~/content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/bootstrap-responsive.css",
                    "~/Scripts/qtip/jquery.qtip.min.css"
                ));
           BundleTable.EnableOptimizations = false;
        }
	}
}