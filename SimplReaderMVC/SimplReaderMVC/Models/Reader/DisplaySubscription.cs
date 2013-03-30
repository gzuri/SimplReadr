using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplReaderMVC.Models.Reader
{
    public class DisplaySubscription
    {
        public SubscriptionVM SubscriptionVM { get; set; }
        public List<FeedItemVM> FeedItemVms { get; set; }
    }
}