using System;
using System.Collections.Generic;
using SimplReaderBLL.BLL.Concrete;
using SimplReaderBLL.BLL.Reader;

namespace SimplReaderBLL.BLL.Membership
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActivated { get; set; }
        public bool IsBlocked { get; set; }
        public string BlockedReason { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? DefaultLangID { get; set; }
        public string Name { get; set; }
        public int UserType { get; set; }

        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<UserKey> UserKeys { get; set; }
        public virtual ICollection<UserFeedItemStatus> UserFeedItemStatuses { get; set; }
    }
}
