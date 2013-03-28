using System.Data.Entity;
using SimplReaderBLL.BLL.Membership;
using SimplReaderBLL.BLL.Reader;

namespace SimplReaderBLL.BLL.Concrete {
	public class DbContext : System.Data.Entity.DbContext {
		public DbContext() :  base("DataSource")
		{
			Configuration.LazyLoadingEnabled = false;
			Configuration.ValidateOnSaveEnabled = false;
		}

		public IDbSet<User> Users { get; set; }
		public IDbSet<UserKey> UserKeys { get; set; }
		public IDbSet<RssFeed> RssFeeds { get; set; }
        public IDbSet<FeedItem> FeedItems { get; set; }
        public IDbSet<UserSubscription> UserSubscriptions { get; set; }
	    public IDbSet<Category> Categories { get; set; }
	    public IDbSet<UserFeedItemStatus> UserFeedItemStatuses { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			Database.SetInitializer<DbContext>(null);
			modelBuilder.Entity<User>().HasKey(x => x.UserID);
			modelBuilder.Entity<UserKey>().HasKey(x => x.ID).HasRequired(x => x.User).WithMany(x=>x.UserKeys).HasForeignKey(x=>x.UserID).WillCascadeOnDelete(true);
		    modelBuilder.Entity<RssFeed>().HasKey(x => x.RssFeedID);
			modelBuilder.Entity<FeedItem>().HasKey(x => x.FeedItemID).HasRequired(x => x.RssFeed).WithMany(x=>x.FeedItems).HasForeignKey(x=>x.RssFeedID).WillCascadeOnDelete(true);
		    modelBuilder.Entity<UserSubscription>().HasKey(x => x.ID).HasRequired(x=>x.User).WithMany(x=>x.UserSubscriptions).HasForeignKey(x=>x.UserID).WillCascadeOnDelete(true);
		    modelBuilder.Entity<Category>().HasKey(x => x.CategoryID).HasMany(x => x.UserSubscriptions);
            modelBuilder.Entity<UserFeedItemStatus>().HasKey(x=>x.ID).HasRequired(x=>x.User).WithMany(x=>x.UserFeedItemStatuses).HasForeignKey(x=>x.UserID).WillCascadeOnDelete(true);

            //modelBuilder.Entity<UserSubscription>().HasRequired(x=>x.RssFeed).WithMany(x=>x.UserSubscriptions).HasForeignKey(x=>x.RssFeedID).WillCascadeOnDelete(true);
			base.OnModelCreating(modelBuilder);
		}
	}
}
