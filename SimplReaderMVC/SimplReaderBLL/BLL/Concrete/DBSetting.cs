using System.ComponentModel.DataAnnotations;

namespace SimplReaderBLL.BLL.Concrete {
	public class DBSetting {
		[Key]
		public int ID { get; set; }
		public string SettingsKey { get; set; }
		public string Value { get; set; }
		public string Description { get; set; }
	}
}
