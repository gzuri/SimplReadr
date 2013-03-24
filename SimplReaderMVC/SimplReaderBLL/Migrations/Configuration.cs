using SimplReaderBLL.BLL.Concrete;
using SimplReaderBLL.BLL.Reader;
using SimplReaderBLL.Enumerators;

namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SimplReaderBLL.BLL.Concrete.DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SimplReaderBLL.BLL.Concrete.DbContext context)
        {
			  //Add default user
			  context.Users.AddOrUpdate(x => x.Email, new User { Email = "testemail@gmail.com", Password = "bba2d1bec283dd3b90add09797a9235b08069064", CreatedDate = DateTime.UtcNow, IsActivated = true, UserType = (int)UserTypeEnum.Administrator});
			  context.RssFeeds.AddOrUpdate(x => x.FullURL, new RssFeed { FullURL = "http://feeds.hanselman.com/ScottHanselman", LastSync = DateTime.UtcNow});			  
        }
    }
}
