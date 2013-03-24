using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimplReaderBLL.BLL.Reader;

namespace SimplReaderMVC.Controllers
{
    public class GeneralController : Controller
    {
        private readonly FeedProvider feedProvider;

        public GeneralController(FeedProvider feedProvider)
        {
            this.feedProvider = feedProvider;
        }

        public ActionResult SyncFeeds()
        {
            feedProvider.SycAllFeeds();
            return Content("OK");
        }

    }
}
