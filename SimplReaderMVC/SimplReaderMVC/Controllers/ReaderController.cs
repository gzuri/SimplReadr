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
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult DisplaySubscriptions()
        {
            var model = readerService.GetUserSubscriptions(SimplReaderBLL.CurrentUser.UserID);
            return PartialView(model);
        }
    }
}
