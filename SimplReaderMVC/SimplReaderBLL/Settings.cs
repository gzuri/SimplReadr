using System.Configuration;

namespace SimplReaderBLL {
	public class Settings {
		public static string DefaultEmailAddress { get { return ConfigurationManager.AppSettings.Get("DefaultEmailAddress"); } }
		public static string DefaultEmailServer { get { return ConfigurationManager.AppSettings.Get("DefaultEmailServer"); } }
		public static int DefaultEmailOutgoingPort { get { return GetIntSetting("DefaultEmailOutgoingPort"); } }
		public static bool DefaultEmailOutgoingUseSSL { get { return GetBoolSetting("DefaultEmailOutgoingUseSSL"); } }
		public static string DefaultEmailUsername { get { return ConfigurationManager.AppSettings.Get("DefaultEmailUsername"); } }
		public static string DefaultEmailPassword { get { return ConfigurationManager.AppSettings.Get("DefaultEmailPassword"); } }
        public static int NotificationTimeout { get { return GetIntSetting("NotificationTimeout"); } }

		static bool GetBoolSetting(string key)
		{
			var value = false;
			bool.TryParse(ConfigurationManager.AppSettings.Get(key), out value);
			return value;
		}

		static int GetIntSetting(string key)
		{
			int value;
			int.TryParse(ConfigurationManager.AppSettings.Get(key), out value);
			return value;
		}
	}
}
