using System;

namespace SimplReaderBLL.BLL.Reader {
	public class FeedItem
    {
        #region EF
        public long FeedItemID { get; set; }
		public string FullURL { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
	    public string Author { get; set; }
		public DateTime DatePublished { get; set; }
		public DateTime DateCollected { get; set; }

		public virtual RssFeed RssFeed { get; set; }
		public long RssFeedID { get; set; }
        #endregion

	    
    }
}
