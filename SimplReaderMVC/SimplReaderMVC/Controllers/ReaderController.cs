﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SimplReaderBLL;
using SimplReaderBLL.Enumerators;
using SimplReaderMVC.Models.Reader;
using SimplReaderMVC.Resources;

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
        public ActionResult Index(long? feedID, int page = 1)
        {
            var model = new DisplaySubscription
                            {
                                SubscriptionVM = readerService.GetSubscription(feedID),
                                FeedItemVms = readerService.GetFeedItems(userID: SimplReaderBLL.CurrentUser.UserID, feedID: feedID, page: page)
                            };


            ViewBag.OnePageOfFeeds = new StaticPagedList<FeedItemVM>(model.FeedItemVms, page, Settings.FeedItemsPerPage, model.SubscriptionVM.CalculatedTotalFeedItemsCount);
            return View(model);
        }

        public ActionResult DisplayFeedItems(long? feedID, int page = 1)
        {
            var model = readerService.GetFeedItems(CurrentUser.UserID, feedID: feedID, page: page);
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


        [HttpPost]
        public ActionResult AddSubscription(SubscriptionVM model)
        {
            if (ModelState.IsValid)
            {
                var status = readerService.AddSubscription(model.SubscriptionFullURL);
               
                switch (status)
                {
                    case ReturnStatusEnum.Success:
                        AddNotification(Notifications.SuccessfullyLoggedIn, UserMessagesTypesEnum.Success);
                        return Redirect(Url.RouteUrl("DefaultReader"));
                        break;
                    case ReturnStatusEnum.UrlInWrongFormat:
                        ModelState.AddModelError("", Notifications.UrlInWrongFormatErrorMessage);
                        break;
                    case ReturnStatusEnum.NothingOnUrl:
                        ModelState.AddModelError("", Notifications.FeedNotFoundOnUrl);
                        break;
                  
                    default:
                        break;
                }
            }
            return new JsonResult
            {
                Data = new { status = ReturnStatusEnum.GenericError },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpPost]
        public ActionResult ImportFromXML(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0 && file.ContentType.Contains("xml"))
            {
                var status = readerService.ImportXML(file);
            }
            return Content("");
        }

       
    }
}
