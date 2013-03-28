using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using SimplReaderBLL.Enumerators;
using SimplReaderMVC.Models.UserMessage;

namespace SimplReaderMVC.Controllers
{
    public class BaseController : Controller
    {
        public bool IsChildAction
        {
            get
            {
                RouteData routeData = RouteData;
                if (routeData == null)
                {
                    return false;
                }
                return routeData.DataTokens.ContainsKey("ParentActionViewContext");
            }
        }

        protected ActionResult Redirect(string url, object data = null)
        {
            url = System.Web.HttpUtility.UrlDecode(url);
            if (HttpContext.Request.IsAjaxRequest())
            {
                //To check ajax this can be used controllerContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                HttpContext.Response.AddHeader("nano-action", "redirect");
                HttpContext.Response.AddHeader("nano-timeout", "0");
                HttpContext.Response.AddHeader("nano-location", url);
                HttpContext.Response.AddHeader("nano-data", JsonConvert.SerializeObject(data));
                return Json(new { status = "redirect", location = url }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                HttpContext.Response.Redirect(url);
            }
            TagBuilder link = new TagBuilder("a");
            link.MergeAttribute("href", url);
            link.InnerHtml = "link";
            return Content("To redirect manually please click on the " + link);
        }

        public void AddNotification(string message, UserMessagesTypesEnum userMessagesTypes, int? timeout = null)
        {
            UserMessageProvider.AddMessage(message, userMessagesTypes, timeout);
        }
    }
}
