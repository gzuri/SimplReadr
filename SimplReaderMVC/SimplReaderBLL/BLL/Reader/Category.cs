using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplReaderBLL.BLL.Membership;

namespace SimplReaderBLL.BLL.Reader
{
    public class Category
    {
        public long CategoryID { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}
