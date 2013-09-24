using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.tools.syndicate
{
//    public class RSSFeed
//    {
//        System.ServiceModel.Syndication.SyndicationFeed _rss_feed;
//        string title;

//        public string Name { get { return _rss_feed.Title.Text; } set { title = value; } }

//        public RSSFeed(Uri urlfeed) 
//        {
//            System.Xml.XmlReader _reader = System.Xml.XmlReader.Create(urlfeed.ToString(), null);
//            System.Xml.= XNamespace.Get(“http://purl.org/rss/1.0/modules/content/”);
//            _rss_feed = System.ServiceModel.Syndication.SyndicationFeed.Load(_reader);
            
//        }

//        static IList GetBlogFeed(string feedUrl, int feedCount)
//{
//XNamespace content = XNamespace.Get(“http://purl.org/rss/1.0/modules/content/”);

//var doc = XDocument.Load(feedUrl);
//var feeds = doc.Descendants(“item”).Select(x =>
//new FeedViewModel
//{
//Title = x.Element(“title”).Value,
//PublishedDate = DateTime.Parse(x.Element(“pubDate”).Value),
//Url = x.Element(“link”).Value,
//Description = x.Element(“description”).Value,
//Content = x.Element(content + “encoded”).Value
//}).OrderByDescending(x => x.PublishedDate).Take(feedCount);
//return feeds.ToList();
//}
//    }
}
