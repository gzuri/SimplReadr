using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplReaderBLL.BLL.Concrete {
	public class FeedItem {
		public long FeedItemID { get; set; }
		public string FullURL { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public DateTime OriginalFeedItemCreated { get; set; }
		public DateTime DateCollected { get; set; }

		public virtual RssFeed RssFeed { get; set; }
		public long RssFeedID { get; set; }
	}
}
