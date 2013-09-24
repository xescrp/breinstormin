using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

namespace breinstormin.tools.syndicate
{
    //public class RSSPost 
    //{
    //    public string Title;
    //    public string Content;
    //    public string User;
    //    public DateTime Date;
    //    public List<string> Comments;

    //    public RSSPost() 
    //    { 
            
    //    }

    //    public RSSPost(Rss.RssItem rssitem) 
    //    {
    //        Title = rssitem.Title;
    //        Content = rssitem.Description;
    //        User = rssitem.Author;
    //        Date = rssitem.PubDate;
    //        if (rssitem.Comments != null & rssitem.Comments.Length > 0)
    //        {
    //            Comments = new List<string>(rssitem.Comments.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries));
    //        }
    //        else 
    //        { Comments = new List<string>(); }
            
    //    }
    //}
    [Serializable()]
    public class RSSEvents
    {
        //Consts
        const string post =
        "<h2 class=\"title\">%titulo%</h2>\r\n" +
        "<table><tr><td><img style=\"border:0px;\" src=\"Resources/Images/userk.png\" /></td><td>Usuario: <a><strong>%usuario%</strong></a></td><td><img src=\"Resources/Images/calend.png\" /></td><td>&nbsp;&nbsp;Fecha: <a class=\"a2\"><strong>%fecha%</strong></a></td></tr></table>\r\n"
            + "%entry%\r\n"
        + "<h3 class=\"title\"></h3>\r\n";
        //Vars
        string rssfile;
        Rss.RssFeed rss;

        public RSSEvents(string filepath, string channeldescription, string webmaster, string channeltitle, string httplink)
        {
            //rssfile = filerelpath;
            string tmprssfile = rssfile;
            rssfile = filepath;
            rss = new Rss.RssFeed();
            if (!System.IO.File.Exists(rssfile))
            {
                //System.IO.StreamWriter str = new System.IO.StreamWriter(rssfile, false);
                //str.Close();
                if (rss.Channels.Count == 0)
                {
                    Rss.RssFeed rs = new Rss.RssFeed();
                    Rss.RssChannel ch = new Rss.RssChannel();
                    string cat = "";
                    //if (rssfile.Contains(@"\Components\")) { cat = "Componente forte"; }
                    //if (rssfile.Contains(@"\Environments\")) { cat = "Entorno forte"; }
                    ch.Description = channeldescription;
                    ch.Title = channeltitle;
                    ch.WebMaster = webmaster;
                    Rss.RssItem rt = new Rss.RssItem();
                    rt.Author = webmaster;
                    rt.Description = "Seccion de eventos, canal " + channeltitle + "." + cat;
                    rt.PubDate = DateTime.Now;
                    rt.Title = "Eventos para el canal " + channeltitle;

                    ch.Items.Add(rt);
                    ch.LastBuildDate = DateTime.Now;
                    ch.PubDate = DateTime.Now;
                    ch.Link = new Uri(httplink);
                    rss.Channels.Add(ch);
                    rss.Write(rssfile);
                }
            }
            rss = Rss.RssFeed.Read(rssfile);

        }
        public RSSEvents(string filepath) 
        {
            rssfile = filepath;
            rss = new Rss.RssFeed();
            
            if (!System.IO.File.Exists(rssfile)) 
            {
                throw new Exception("El archivo indicado para sindicacion no existe o no es correcto.");
            }
            rss = Rss.RssFeed.Read(rssfile);
        }

        public RSSEvents(Uri url) 
        {
            rss = Rss.RssFeed.Read(url.ToString());
        }

        public RSSEvents(string channelDescription, string channelTittle, string webmaster) 
        {
            rss = new Rss.RssFeed();
            if (rss.Channels.Count == 0)
            {
                Rss.RssFeed rs = new Rss.RssFeed();
                Rss.RssChannel ch = new Rss.RssChannel();
                string cat = "";
                //if (rssfile.Contains(@"\Components\")) { cat = "Componente forte"; }
                //if (rssfile.Contains(@"\Environments\")) { cat = "Entorno forte"; }
                ch.Description = channelDescription;
                ch.Title = channelTittle;
                ch.WebMaster = webmaster;
                Rss.RssItem rt = new Rss.RssItem();
                rt.Author = webmaster;
                rt.Description = channelDescription;
                rt.PubDate = DateTime.Now;
                rt.Title = channelTittle;

                ch.Items.Add(rt);
                ch.LastBuildDate = DateTime.Now;
                ch.PubDate = DateTime.Now;
                //ch.Link = new Uri(httplink);
                rss.Channels.Add(ch);
                
            }
        }


        internal string getHTMLOutput()
        {
            string html = "";
            Stack<Rss.RssItem> st = new Stack<Rss.RssItem>();
            foreach (Rss.RssItem rit in rss.Channels[0].Items)
            {
                st.Push(rit);
            }
            int posts = 7;
            while (st.Count > 0 && posts > 0)
            {
                Rss.RssItem rit = st.Pop();
                string rpost = "";
                rpost = post.Replace("%titulo%", rit.Title);
                rpost = rpost.Replace("%usuario%", rit.Author);
                rpost = rpost.Replace("%fecha%", rit.PubDate.AddHours(-2).ToString("dddd, dd/MM/yyyy HH:mm"));
                rpost = rpost.Replace("%entry%", rit.Description);
                html += rpost + "\r\n";
                --posts;
            }

            return html;
        }

        public RSSPost[] getPosts() 
        { 
            Stack<Rss.RssItem> st = new Stack<Rss.RssItem>();
            List<RSSPost> list = new List<RSSPost>();
            foreach (Rss.RssItem rit in rss.Channels[0].Items)
            {
                st.Push(rit);
            }
            //int posts = 7;
            while (st.Count > 0)
            {
                Rss.RssItem rit = st.Pop();
                list.Add(new RSSPost(rit));
            }
            return list.ToArray();
        }

        internal string getHTMLOutputAll()
        {
            string html = "";
            Stack<Rss.RssItem> st = new Stack<Rss.RssItem>();
            foreach (Rss.RssItem rit in rss.Channels[0].Items)
            {
                st.Push(rit);
            }
            //int posts = 7;
            while (st.Count > 0)
            {
                Rss.RssItem rit = st.Pop();
                string rpost = "";
                rpost = post.Replace("%titulo%", rit.Title);
                rpost = rpost.Replace("%usuario%", rit.Author);
                rpost = rpost.Replace("%fecha%", rit.PubDate.AddHours(-2).ToString("dddd, dd/MM/yyyy HH:mm"));
                rpost = rpost.Replace("%entry%", rit.Description);
                html += rpost + "\r\n";
                //--posts;
            }
            return html;
        }
        public void addNewPost(string title, string usuario, string entry)
        {
            Rss.RssItem it = new Rss.RssItem();
            it.Title = title;
            it.Author = usuario;
            it.Description = entry;
            it.PubDate = DateTime.Now;
            rss.Channels[0].Items.Add(it);
            rss.Write(this.rssfile);
        }

        public void addNewPostNoSaveFile(string title, string usuario, string entry)
        {
            Rss.RssItem it = new Rss.RssItem();
            it.Title = title;
            it.Author = usuario;
            it.Description = entry;
            it.PubDate = DateTime.Now;
            rss.Channels[0].Items.Add(it);
            //rss.Write(this.rssfile);
        }
    }
}