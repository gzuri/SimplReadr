using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimplReaderBLL.BLL.Reader;

namespace SimplReaderMVC.Models.Reader
{
    public class ReaderService
    {
        private readonly SubscriptionProvider subscriptionProvider;
        private readonly FeedProvider feedProvider;

        public ReaderService(SubscriptionProvider subscriptionProvider, FeedProvider feedProvider )
        {
            this.subscriptionProvider = subscriptionProvider;
            this.feedProvider = feedProvider;
        }

        public List<SubscriptionsVM> GetUserSubscriptions(int userID)
        {
            return subscriptionProvider.GetUserSubscriptions(userID).Select( x => new SubscriptionsVM {SubscriptionFullURL = x.FullURL, ID = x.RssFeedID, Title = x.Title}).ToList();
        }

        public List<FeedItemVM> GetFeedItems(int userID, long? feedID = null)
        {
            var data = feedProvider.GetFeedsForUser(userID, feedID);

            return data.Select(x => new FeedItemVM
                                        {
                                            Title = x.Title,
                                            Content = x.ShortDescription,
                                            DatePublished = x.DatePublished
                                        }).ToList();
        }
    }
}