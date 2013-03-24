using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplReaderBLL.BLL.Concrete
{
    public class UserSubscription
    {
        public long ID { get; set; }

        public virtual User User { get; set; }
        public int UserID { get; set; }

        public virtual RssFeed RssFeed { get; set; }
        public long RssFeedID { get; set; }
        
        //Title for subscription
        public string Title { get; set; }

        public DateTime SubscribeDate { get; set; }
    }
}
