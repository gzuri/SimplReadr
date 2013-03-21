using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SimplReaderBLL.Helpers {
	public static class EmailHelper {
		public static bool IsValidEmail(string emailaddress) {
			try {
				MailAddress m = new MailAddress(emailaddress);
				return true;
			} catch (FormatException) {
				return false;
			}
		}

		/// <summary>
		/// Sends message using default email settings (system messages etc)
		/// </summary>
		/// <param name="mailTo"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static bool SendSystemEmail(string mailTo, string subject, string message, bool sendAsync = true)
		{
			
				MailMessage mailMessage = new MailMessage(Settings.DefaultEmailAddress, mailTo, subject, message);
				mailMessage.BodyEncoding = Encoding.UTF8;
				mailMessage.SubjectEncoding = Encoding.UTF8;

				SmtpClient smtpClient = new SmtpClient(Settings.DefaultEmailServer, Settings.DefaultEmailOutgoingPort);
				smtpClient.EnableSsl = Settings.DefaultEmailOutgoingUseSSL;
				smtpClient.Credentials = new NetworkCredential(Settings.DefaultEmailUsername, Settings.DefaultEmailPassword);
				smtpClient.SendAsync(mailMessage, "");
				try {
					if (sendAsync) {
						smtpClient.SendCompleted += (s, e) => {
							smtpClient.Dispose();
							mailMessage.Dispose();
						};
						smtpClient.SendAsync(mailMessage, null);
					} else {
						smtpClient.Send(mailMessage);
					}
					return true;
				} catch (Exception e) {
					Elmah.ErrorSignal.FromCurrentContext().Raise(e);
					return false;
				}
		}
	}
}
