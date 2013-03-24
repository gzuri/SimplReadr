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

        public ReaderService(SubscriptionProvider subscriptionProvider )
        {
            this.subscriptionProvider = subscriptionProvider;
        }

        public List<SubscriptionsVM> GetUserSubscriptions(int userID)
        {
            return subscriptionProvider.GetUserSubscriptions(userID).Select( x => new SubscriptionsVM {SubscriptionFullURL = x.FullURL, ID = x.RssFeedID}).ToList();
        }
    }
}