using System.Data.Entity;

namespace SimplReaderBLL.BLL.Concrete {
	public class DbContext : System.Data.Entity.DbContext {
		public DbContext() :  base("DataSource")
		{
			Configuration.LazyLoadingEnabled = false;
			Configuration.ValidateOnSaveEnabled = false;
		}

		public IDbSet<DBSetting> Settings { get; set; }
		public IDbSet<User> Users { get; set; }
		public IDbSet<UserKey> UserKeys { get; set; }
		public IDbSet<RssFeed> RssFeeds { get; set; }
		public IDbSet<FeedItem> FeedItems { get; set; }

		protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder) {
			System.Data.Entity.Database.SetInitializer<DbContext>(null);
			modelBuilder.Entity<User>().HasKey(x => x.UserID);
			modelBuilder.Entity<DBSetting>().HasKey(x => x.ID);
			modelBuilder.Entity<UserKey>().HasKey(x => x.ID).HasRequired(x => x.User);
			modelBuilder.Entity<RssFeed>().HasKey(x => x.RssFeedID);
			modelBuilder.Entity<FeedItem>().HasKey(x => x.FeedItemID).HasRequired(x => x.RssFeed);

			base.OnModelCreating(modelBuilder);
		}
	}
}
