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
            return context.RssFeeds.Where(x => x.UserSubscriptions.Any(sub => sub.UserID == userID));
        }
    }
}
