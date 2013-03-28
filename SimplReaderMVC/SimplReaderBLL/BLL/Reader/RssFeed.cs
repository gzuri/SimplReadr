using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SimplReaderBLL.BLL.Concrete;

namespace SimplReaderBLL.BLL.Reader {
	public class RssFeed
    {
        #region EF data
        public long RssFeedID { get; set; }
		public string FullURL { get; set; }
	    public DateTime LastSync { get; set; }
        public string HtmlURL { get; set; }
        public string Title { get; set; }

		public virtual ICollection<FeedItem> FeedItems { get; set; }
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
        #endregion
    }
}
