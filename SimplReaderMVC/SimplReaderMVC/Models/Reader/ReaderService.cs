using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimplReaderBLL.BLL.Reader;
using SimplReaderBLL;
using SimplReaderBLL.Enumerators;


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
                                            DatePublished = x.DatePublished
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
    }
}