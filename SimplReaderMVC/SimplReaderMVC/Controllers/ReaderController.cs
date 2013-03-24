using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimplReaderMVC.Models.Reader;

namespace SimplReaderMVC.Controllers
{
    public class ReaderController : BaseController
    {
        private readonly ReaderService readerService;

        public ReaderController(ReaderService readerService)
        {
            this.readerService = readerService;
        }

        [Authorize]
        public ActionResult Index(long? feedID)
        {
            var model = readerService.GetFeedItems(SimplReaderBLL.CurrentUser.UserID, feedID);
            return View(model);
        }

        
        public ActionResult DisplaySubscriptions()
        {
            var model = readerService.GetUserSubscriptions(SimplReaderBLL.CurrentUser.UserID);
            return PartialView(model);
        }

        [Authorize]
        public ActionResult ManageSubscriptions()
        {
            return View();
        }

        public ActionResult ManageSubscriptionsTable()
        {
            var model = readerService.GetUserSubscriptions(SimplReaderBLL.CurrentUser.UserID);
            return PartialView(model);
        }
    }
}
