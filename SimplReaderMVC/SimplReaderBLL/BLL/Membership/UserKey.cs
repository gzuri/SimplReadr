namespace SimplReaderBLL.BLL.Membership {
	public class UserKey {
		public int ID { get; set; }
		public bool IsDeleted { get; set; }
		public int UserID { get; set; }

		public int KeyType { get; set; }

		public System.Guid Key { get; set; }

		public System.DateTime CreateDate { get; set; }

		public System.DateTime ExpireDate { get; set; }

		public virtual User User { get; set; }
	}
}
