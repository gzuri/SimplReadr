using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Extensions.Logging;
using QDFeedParser;
using SimplReaderBLL.BLL.Concrete;
using SimplReaderBLL.BLL.Providers;

namespace SimplReaderBLL.BLL.Reader
{
    public class FeedProvider
    {
        private readonly DbContext context;
        private readonly ILogger logger;
        private readonly CacheProvider cache;

        public FeedProvider(DbContext context, ILogger logger, CacheProvider cache)
        {
            this.context = context;
            this.logger = logger;
            this.cache = cache;
        }


        public void SycAllFeeds()
        {
            var feeds = context.RssFeeds.Select(x => x.RssFeedID).ToList();
            feeds.ForEach(SyncFeedItems);
        }

        public void SyncFeedItems(long feedID)
        {
            var feedData = context.RssFeeds.Find(feedID);
            if (feedData == null)
                return;

            var allLocalItems = context.FeedItems.Where(x => x.RssFeedID == feedID).ToList();
            var feeduri = new Uri(feedData.FullURL);
            var factory = new HttpFeedFactory();
            var feed = factory.CreateFeed(feeduri);
            foreach (var item in feed.Items)
            {
                if (allLocalItems.All(x=>x.FullURL != item.Link))
                {
                    context.FeedItems.Add(new FeedItem
                                              {
                                                  Author = item.Author,
                                                  DatePublished = item.DatePublished,
                                                  FullURL = item.Link,
                                                  ShortDescription = item.Content,
                                                  Title = item.Title,
                                                  RssFeedID = feedID,
                                                  DateCollected = DateTime.UtcNow
                                              });
                }
            }
            feedData.LastSync = DateTime.UtcNow;
            context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID">Returns for user</param>
        /// <param name="feedID">If null return feeds from all subscritpions</param>
        /// <returns></returns>
        public IEnumerable<FeedItem> GetFeedsForUser(int userID, long? feedID = null)
        {
            var data = (from feedItems in context.FeedItems
                        join feed in context.RssFeeds on feedItems.RssFeedID equals feed.RssFeedID
                        join subs in context.UserSubscriptions on feed.RssFeedID equals subs.RssFeedID
                        where subs.UserID == userID
                        orderby feedItems.DatePublished descending
                        select feedItems);

            if (feedID.HasValue)
                data = data.Where(x => x.RssFeedID == feedID.Value);
            
            return data;
        }
    }
}
