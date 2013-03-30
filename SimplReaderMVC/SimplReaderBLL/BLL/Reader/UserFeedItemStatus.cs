using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplReaderBLL.BLL.Membership;

namespace SimplReaderBLL.BLL.Reader
{
    public class UserFeedItemStatus
    {
        #region EF
        public long ID { get; set; }
        public long FeedItemID { get; set; }
        public virtual FeedItem FeedItem { get; set; }

        public long UserSubscriptionID { get; set; }
        public virtual UserSubscription UserSubscription { get; set; }

        /// <summary>
        /// Date when a user read the item
        /// </summary>
        public DateTime? DateRead { get; set; }

        /// <summary>
        /// Date a user stared the item
        /// </summary>
        public DateTime? DateStared { get; set; }
        #endregion
    }
}
