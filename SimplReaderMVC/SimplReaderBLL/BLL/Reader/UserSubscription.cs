using System;
using SimplReaderBLL.BLL.Membership;

namespace SimplReaderBLL.BLL.Reader
{
    public class UserSubscription
    {
        public long ID { get; set; }

        public virtual User User { get; set; }
        public int UserID { get; set; }

        public virtual RssFeed RssFeed { get; set; }
        public long RssFeedID { get; set; }

        public long? CategoryID { get; set; }
        public virtual Category Category { get; set; }

        //Title for subscription
        public string Title { get; set; }

        public DateTime SubscribeDate { get; set; }
    }
}
