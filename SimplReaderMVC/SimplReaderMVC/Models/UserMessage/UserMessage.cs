using SimplReaderBLL.Enumerators;

namespace SimplReaderMVC.Models.UserMessage {
	public class UserMessage {
		public string Message { get; set; }
		public UserMessagesTypesEnum MessageType { get; set; }
		public int? Timeout { get; set; }

		public UserMessage() {

		}

		public UserMessage(string message, int tenantId, UserMessagesTypesEnum messageType) {
			this.Message = message;
			this.MessageType = messageType;
		}
	}

}
