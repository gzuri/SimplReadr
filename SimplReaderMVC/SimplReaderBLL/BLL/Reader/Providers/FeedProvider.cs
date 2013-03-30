using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
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
            feeds.ForEach(x => SyncFeedItems(x));
        }

        public static RssFeed GetFeedFromURL(string url)
        {
            try
            {
                var systemFeed = new RssFeed();
                var feeduri = new Uri(url);
                var factory = new HttpFeedFactory();
                var feed = factory.CreateFeed(feeduri);
                systemFeed.FeedItems = feed.Items.Select(item => new FeedItem
                                                                                             {
                                                                                                 Author = item.Author,
                                                                                                 DatePublished = item.DatePublished,
                                                                                                 FullURL = item.Link,
                                                                                                 ShortDescription = PrepareForLazyLoadImages(item.Content),
                                                                                                 Title = item.Title,
                                                                                                 DateCollected = DateTime.UtcNow
                                                                                             }).ToList();
                systemFeed.Title = feed.Title;
                systemFeed.LastSync = feed.LastUpdated;
                return systemFeed;
            }
            catch (Exception e)
            {

            }
            return null;
        }


        public static string PrepareForLazyLoadImages(string rawHTML)
        {
            if (String.IsNullOrEmpty(rawHTML))
                return String.Empty;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(rawHTML ?? "");
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//img");
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    // get attributes
                    string src = node.GetAttributeValue("src", null);
                    if (string.IsNullOrEmpty(src))
                        continue;
                    node.SetAttributeValue("data-original", src);
                    node.SetAttributeValue("src", "/Content/images/empty.png");
                }
            }
            return doc.DocumentNode.OuterHtml;
        }


        public void SyncFeedItems(long feedID, RssFeed feedData = null)
        {
            var feedDbData = context.RssFeeds.Find(feedID);
            if (feedDbData == null)
                return;

            var allLocalItems = context.FeedItems.Where(x => x.RssFeedID == feedID).ToList();
            if (feedData == null)
                feedData = GetFeedFromURL(feedDbData.FullURL);

            int addedItems = 0;
            foreach (var item in feedData.FeedItems.Where(x => allLocalItems.All(y => y.FullURL != x.FullURL)))
            {
                item.RssFeedID = feedID;
                context.FeedItems.Add(item);
                addedItems++;
            }

            feedDbData.CalculatedFeedItemsCount = allLocalItems.Count + addedItems;
            feedDbData.LastSync = DateTime.UtcNow;
            feedDbData.Title = feedData.Title;
            context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID">Returns for user</param>
        /// <param name="feedID">If null return feeds from all subscritpions</param>
        /// <returns></returns>
        public IEnumerable<FeedItem> GetFeeds(int? userID = null, long? feedID = null, int skip = 0, int take = 25)
        {
            var data = (from feedItems in context.FeedItems
                        join feed in context.RssFeeds on feedItems.RssFeedID equals feed.RssFeedID
                        join subs in context.UserSubscriptions on feed.RssFeedID equals subs.RssFeedID
                        where subs.UserID == userID
                        orderby feedItems.DatePublished descending
                        select feedItems);

            if (feedID.HasValue)
                data = data.Where(x => x.RssFeedID == feedID.Value);
            data = data.Skip(skip).Take(take);
            return data;
        }
    }
}
