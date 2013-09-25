using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.tools.syndicate
{

    //Util para RSS de Wordpress
    public class RSSFeed
    {
        System.ServiceModel.Syndication.SyndicationFeed _rss_feed;
        string title;
        System.Xml.XmlReader _reader;
        System.Xml.Linq.XNamespace _namespace;
        string _urlfeed;
        RSSPost[] _rss_posts;

        public string Name { get { return _rss_feed.Title.Text; }  }
        public string Description {get {return _rss_feed.Description.Text; } }
        public RSSPost[] RSSPosts { get {return _rss_posts.ToArray<RSSPost>(); } }
        public RSSFeed(Uri urlfeed) 
        {
            _urlfeed = urlfeed.ToString();
            _reader = System.Xml.XmlReader.Create(urlfeed.ToString(), null);
            _namespace = System.Xml.Linq.XNamespace.Get("http://purl.org/rss/1.0/modules/content/");
            _rss_feed = System.ServiceModel.Syndication.SyndicationFeed.Load(_reader);
            
            _rss_posts = _GetBlogFeeds().ToArray<RSSPost>();
        }


        private IList<RSSPost> _GetBlogFeeds()
		{
			//Load feed via a feedUrl.
            if (_reader == null)
            {
                _reader = System.Xml.XmlReader.Create(_urlfeed, null);
            }
			var doc = System.Xml.Linq.XDocument.Load(_urlfeed);
            System.Xml.Linq.XNamespace autorns = System.Xml.Linq.XNamespace.Get("http://purl.org/dc/elements/1.1/");
			//Get all the "items" in the feed.
			var feeds = doc.Descendants("item").Select(x =>
					new RSSPost
					{
					     //Get title, pubished date, and link elements.
						Title = x.Element("title").Value, //3
						Date = DateTime.Parse(x.Element("pubDate").Value),
						Url = x.Element("link").Value,
                        Content = x.Element("description").Value,
                        ContentEncoded = x.Element(_namespace + "encoded").Value, 
                        User = x.Element(autorns + "creator").Value
					} //  Put them into an object (FeedViewModel)
					)
					// Order them by the pubDate (FeedViewModel.PublishedDate).
					.OrderByDescending(x=> x.Date);
					//Only get the amount specified, the top (1, 2, 3, etc.) via feedCount.


 			//Convert the feeds to a List and return them.
			return feeds.ToList();
		}

    }
}
