
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplReaderMVC.Models.Reader
{
    public class FeedItemVM
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePublished { get; set; }
    }
}