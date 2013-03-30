using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimplReaderMVC.Models.Reader
{
    public class SubscriptionVM
    {
        /// <summary>
        /// RssFeedID
        /// </summary>
        public long? ID { get; set; }
        public string Title { get; set; }
        public string InternalURL { get; set; }
        public int CalculatedTotalFeedItemsCount { get; set; }
        [Required]
        public string SubscriptionFullURL { get; set; }
    }
}