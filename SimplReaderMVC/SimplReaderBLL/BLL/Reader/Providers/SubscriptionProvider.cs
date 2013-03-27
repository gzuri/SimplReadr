using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Extensions.Logging;
using SimplReaderBLL.BLL.Concrete;
using SimplReaderBLL.BLL.Providers;
using SimplReaderBLL.Enumerators;

namespace SimplReaderBLL.BLL.Reader
{
    public class SubscriptionProvider
    {
        private readonly CacheProvider cacheProvider;
        private readonly DbContext context;
        private readonly ILogger logger;
        private readonly FeedProvider feedProvider;

        public SubscriptionProvider(CacheProvider cacheProvider, DbContext context, ILogger logger, FeedProvider feedProvider)
        {
            this.cacheProvider = cacheProvider;
            this.context = context;
            this.logger = logger;
            this.feedProvider = feedProvider;
        }

        public IEnumerable<RssFeed> GetUserSubscriptions(int userID)
        {
            var data = (from feeds in context.RssFeeds
                       join subscriptions in context.UserSubscriptions on feeds.RssFeedID equals subscriptions.RssFeedID
                       where subscriptions.UserID == userID
                       orderby subscriptions.Title
                       select new {feeds, subscriptions}).ToList().Select(x=> new RssFeed
                           {
                               Title =  !string.IsNullOrEmpty(x.subscriptions.Title) ? x.subscriptions.Title : x.feeds.Title, 
                               FullURL = x.feeds.FullURL, 
                               RssFeedID = x.subscriptions.RssFeedID, 
                               LastSync = x.feeds.LastSync
                           });
            return data;
        }

        public ReturnStatusEnum AddSubscription(int userID, Uri url, out RssFeed rssFeed, int? categoryID = null)
        {
            string absoluteURL = url.AbsoluteUri.ToLowerInvariant();
            rssFeed = new RssFeed();
            //first check if we have the same link in the database
            rssFeed = context.RssFeeds.FirstOrDefault(x => x.FullURL == absoluteURL);
            if (rssFeed == null)
            {
                var feedData = FeedProvider.GetFeedFromURL(absoluteURL);
                if (feedData == null)
                    return ReturnStatusEnum.NothingOnUrl;

                rssFeed = context.RssFeeds.Add(new RssFeed { FullURL = absoluteURL, LastSync = DateTime.UtcNow, Title = feedData.Title });
                context.SaveChanges();
            }

            long rssFeedID = rssFeed.RssFeedID;
            if (context.UserSubscriptions.Count(x => x.UserID == userID && x.RssFeedID == rssFeedID) == 0)
            {
                context.UserSubscriptions.Add(new UserSubscription
                                                  {
                                                      UserID = userID,
                                                      RssFeedID = rssFeedID,
                                                      SubscribeDate = DateTime.UtcNow
                                                  });
                context.SaveChanges();
            }
            return ReturnStatusEnum.Success;
        }
    }
}
