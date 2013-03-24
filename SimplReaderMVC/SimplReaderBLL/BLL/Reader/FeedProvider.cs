using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Logging;
using SimplReaderBLL.BLL.Concrete;
using SimplReaderBLL.BLL.Providers;

namespace SimplReaderBLL.BLL.Reader
{
    public class FeedProvider
    {
        private readonly DbContext context;
        private readonly ILogger logger;
        private readonly CacheProvider cache;

        public FeedProvider(DbContext context, ILogger logger, CacheProvider cache)
        {
            this.context = context;
            this.logger = logger;
            this.cache = cache;
        }


    }
}
