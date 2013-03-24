using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplReaderMVC.Models.Reader
{
    public class SubscriptionsVM
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string InternalURL { get; set; }
        public string SubscriptionFullURL { get; set; }
    }
}