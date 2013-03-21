using System;
using System.Collections.Generic;

namespace SimplReaderBLL.BLL.Concrete {
	public class User {

		public int UserID { get; set; }
		public string Username { get; set; }

		public string Email {
			get;
			set;
		}

		public bool IsActivated {
			get;
			set;
		}

		public bool IsBlocked {
			get;
			set;
		}

		public string BlockedReason {
			get;
			set;
		}

		public string Password {
			get;
			set;
		}

		public DateTime CreatedDate {
			get;
			set;
		}

		public DateTime? LastLogin {
			get;
			set;
		}

		public int? DefaultLangID {
			get;
			set;
		}

		public string Name { get; set; }


		public virtual ICollection<UserKey> UserKeys { get; set; }
		public int UserType { get; set; }
	}
}
