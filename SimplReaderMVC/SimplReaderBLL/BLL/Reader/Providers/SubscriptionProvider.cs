using System.Collections.Generic;
using System.Linq;
using Ninject.Extensions.Logging;
using SimplReaderBLL.BLL.Concrete;
using SimplReaderBLL.BLL.Providers;

namespace SimplReaderBLL.BLL.Reader
{
    public class SubscriptionProvider
    {
        private readonly CacheProvider cacheProvider;
        private readonly DbContext context;
        private readonly ILogger logger;

        public SubscriptionProvider(CacheProvider cacheProvider, DbContext context, ILogger logger)
        {
            this.cacheProvider = cacheProvider;
            this.context = context;
            this.logger = logger;
        }

        public IEnumerable<RssFeed> GetUserSubscriptions(int userID)
        {
            var data = (from feeds in context.RssFeeds
                       join subscriptions in context.UserSubscriptions on feeds.RssFeedID equals subscriptions.RssFeedID
                       where subscriptions.UserID == userID
                       orderby subscriptions.Title
                       select new {feeds, subscriptions}).ToList().Select(x=> new RssFeed{Title = x.subscriptions.Title, FullURL = x.feeds.FullURL, RssFeedID = x.subscriptions.RssFeedID, LastSync = x.feeds.LastSync});
            return data;
        }

        
    }
}
