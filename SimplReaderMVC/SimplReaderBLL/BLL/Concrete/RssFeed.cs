using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplReaderBLL.BLL.Concrete {
	public class RssFeed {
		public long RssFeedID { get; set; }
		public string FullURL { get; set; }
	    public DateTime LastSync { get; set; }

		public virtual ICollection<FeedItem> FeedItems { get; set; }
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
	}
}
