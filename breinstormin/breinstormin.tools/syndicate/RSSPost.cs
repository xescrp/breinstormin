using System;
using System.Collections.Generic;
using System.Text;


namespace breinstormin.tools.syndicate
{
    [Serializable()]
    public class RSSPost
    {
        public string Title;
        public string Content;
        public string User;
        public DateTime Date;
        public List<string> Comments;

        public string MediaItemUrl;
        public string MediaType;
        public int MediaLenght;

        public RSSPost()
        {

        }

        public RSSPost(Rss.RssItem rssitem)
        {
            if (rssitem.Enclosure != null)
            {
                MediaItemUrl = rssitem.Enclosure.Url.ToString();
                MediaType = rssitem.Enclosure.Type;
                MediaLenght = rssitem.Enclosure.Length;
            }

            Title = rssitem.Title;
            Content = rssitem.Description;
            User = rssitem.Author;
            Date = rssitem.PubDate;
            if (rssitem.Comments != null & rssitem.Comments.Length > 0)
            {
                Comments = new List<string>(rssitem.Comments.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            { Comments = new List<string>(); }

        }
    }
}
