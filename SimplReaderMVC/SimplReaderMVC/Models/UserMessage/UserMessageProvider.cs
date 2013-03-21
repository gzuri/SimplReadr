using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SimplReaderBLL;
using SimplReaderBLL.Enumerators;

namespace SimplReaderMVC.Models.UserMessage
{
    /// <summary>
    /// Temporary user messages
    /// Dependant to tenantID
    /// Stores all messages to session and once displayed, messages are deleted
    /// </summary>
    public static class UserMessageProvider
    {
        private const string SessionKey = "Core_UserMessages";

        /// <summary>
        /// Stores current messages to session
        /// </summary>
        private static List<UserMessage> UserMessages
        {
            get
            {
                //checks if there is some data in session
                var data = System.Web.HttpContext.Current.Session[SessionKey];
                if (data == null) return new List<UserMessage>();

                //when returning returns as a list or creates new list to return
                return System.Web.HttpContext.Current.Session[SessionKey] as List<UserMessage> ?? new List<UserMessage>();
            }
        }

        public static void AddMessage(string message, UserMessagesTypesEnum messageType, int? timeout = 2000)
        {
            if (!timeout.HasValue)
                timeout = Settings.NotificationTimeout;
            var messages = UserMessages;
            messages.Add(new UserMessage { Message = message, MessageType = messageType, Timeout = timeout.Value });
            System.Web.HttpContext.Current.Session[SessionKey] = messages;
        }


        public static IEnumerable<UserMessage> GetMessages()
        {
            List<UserMessage> returnMessages = UserMessages.ToList();
            //TODO: ako je ajax nemoj obrisati
            System.Web.HttpContext.Current.Session[SessionKey] = UserMessages.Except(returnMessages);
            return returnMessages;
        }

        /// <summary>
        /// For each message creates a wrapper element with message in it
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString ShowUserMessages(this HtmlHelper htmlHelper)
        {
            string output = String.Empty;
            foreach (UserMessage userMessage in GetMessages())
            {
                TagBuilder messageWrapper = new TagBuilder("div");
                messageWrapper.AddCssClass("notification");
                messageWrapper.AddCssClass(userMessage.MessageType.ToString().ToLower());

                TagBuilder message = new TagBuilder("div");
                message.InnerHtml = userMessage.Message;
                messageWrapper.InnerHtml = message.ToString();
                output += MvcHtmlString.Create(messageWrapper.ToString());
            }
            return MvcHtmlString.Create(output);
        }

    }
}
