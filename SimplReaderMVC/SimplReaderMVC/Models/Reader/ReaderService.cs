using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Ninject.Extensions.Logging;
using SimplReaderBLL.BLL.Reader;
using SimplReaderBLL;
using SimplReaderBLL.Enumerators;


namespace SimplReaderMVC.Models.Reader
{
    public class ReaderService
    {
        private readonly SubscriptionProvider subscriptionProvider;
        private readonly FeedProvider feedProvider;
        private readonly ILogger logger;

        public ReaderService(SubscriptionProvider subscriptionProvider, FeedProvider feedProvider, ILogger logger )
        {
            this.subscriptionProvider = subscriptionProvider;
            this.feedProvider = feedProvider;
            this.logger = logger;
        }

        public List<SubscriptionVM> GetUserSubscriptions(int userID)
        {
            return subscriptionProvider.GetUserSubscriptions(userID).Select( x => new SubscriptionVM {SubscriptionFullURL = x.FullURL, ID = x.RssFeedID, Title = x.Title}).ToList();
        }

        public List<FeedItemVM> GetFeedItems(int userID, long? feedID = null)
        {
            var data = feedProvider.GetFeedsForUser(userID, feedID);

            return data.Select(x => new FeedItemVM
                                        {
                                            Title = x.Title,
                                            Content = x.ShortDescription,
                                            DatePublished = x.DatePublished,
                                            FullURL =  x.FullURL
                                        }).ToList();
        }

        public ReturnStatusEnum AddSubscription(string url)
        {
            try
            {
                var uri = new Uri(url);
                RssFeed rssFeed;
                return subscriptionProvider.AddSubscription(CurrentUser.UserID, uri, out rssFeed);
            }catch(Exception e)
            {

            }
            return ReturnStatusEnum.UrlInWrongFormat;
        }


        public ReturnStatusEnum ImportXML(HttpPostedFileBase file)
        {
            try
            {
                if (!Directory.Exists(Request.AppDataFolder))
                    Directory.CreateDirectory(Request.AppDataFolder);

                var filename = Path.Combine(Request.AppDataFolder, CurrentUser.UserID.ToString() + "-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName));
                file.SaveAs(filename);
                var xmlDoc = XDocument.Load(file.InputStream);
                if (xmlDoc.Element("opml") != null)
                foreach (var feed in xmlDoc.Element("opml").Element("body").Descendants("outline"))
                {
                    var feedUrl = String.Empty;
                    if (feed.Attribute("xmlUrl") != null && !String.IsNullOrEmpty(feed.Attribute("xmlUrl").Value))
                        feedUrl = feed.Attribute("xmlUrl").Value;
                    else
                    {
                        var innerFeedElement = feed.Element("outline");
                        if (innerFeedElement != null)
                        {
                            if (innerFeedElement.Attribute("xmlUrl") != null && !String.IsNullOrEmpty(innerFeedElement.Attribute("xmlUrl").Value))
                                feedUrl = innerFeedElement.Attribute("xmlUrl").Value;
                        }
                    }
                    if (!String.IsNullOrEmpty(feedUrl))
                        AddSubscription(feedUrl);
                }
            }catch(Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                logger.Fatal(e, "Error parsing input");
            }

            return ReturnStatusEnum.Success;
        }
    }
}