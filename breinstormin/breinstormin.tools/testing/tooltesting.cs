using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace breinstormin.tools.testing
{
    class tooltesting
    {
        void testcss() 
        {
            web.css.cssReader reader =
                new web.css.cssReader(@"C:\TFS.GIT\Hostaldog.com\www.hostaldog.com.site\css\estilos.css");
            web.css.Model.CSSDocument doc = reader.cssDocument;

            if (doc != null) 
            {
                foreach (web.css.Model.RuleSet rule in doc.RuleSets) 
                {
                    Console.Write(rule.ToString());
                }
            }

        }


        void testRss() 
        {
            string rssurl = "http://hostaldog-blog.azurewebsites.net/?feed=rss2";
            Uri url = new Uri(rssurl);

            syndicate.RSSEvents rss = new syndicate.RSSEvents(url);

            syndicate.RSSFeed feed = new syndicate.RSSFeed(url);

            Console.WriteLine(rss.getHTMLOutputAll());

            foreach (syndicate.RSSPost post in feed.RSSPosts) 
            {
                Console.WriteLine("******************************************************************");
                Console.Write(post.ContentEncoded);
                Console.WriteLine(post.Title);
                Console.WriteLine(post.MediaItemUrl);
                Console.WriteLine(post.User);
                Console.Write(post.Content);
                Console.WriteLine("");
                Console.Write(post.ContentEncoded);
                Console.WriteLine("");
                Console.WriteLine("******************************************************************");
            }
        }

        void testsblur() 
        {
            string path = @"C:\img";
            string blur = @"C:\img\blur";

            string[] sources = System.IO.Directory.GetFiles(path, "*.jpg", System.IO.SearchOption.TopDirectoryOnly);

            if (!System.IO.Directory.Exists(blur)) 
            {
                System.IO.Directory.CreateDirectory(blur);
            }

            foreach (string file in sources) 
            {
                string filename = Guid.NewGuid().ToString() + ".png";
                System.Drawing.Image img = System.Drawing.Image.FromFile(file);
                for (int i = 1; i < 10; i++)
                {
                    try
                    {
                        image.breinImage blurred = new image.breinImage(img);
                        blurred.ApplyFilter(new image.Filters.GaussianBlurFilter(i,5));
                        image.ImageMgr.SavePngImage(blurred.Image, blur + @"\" + i.ToString() + "_" + filename);
                        Console.WriteLine("Creado " + i.ToString() + "_" + filename);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public void testasm() 
        {
            var appDomain = AppDomain.CreateDomain("test", null,
                new AppDomainSetup
                {
                    ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                    ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                    ShadowCopyFiles = "false"
                });
            var t = typeof(LoadAssembly);

            //appDomain has no assemblies.
            var loader = (LoadAssembly)appDomain.CreateInstanceFromAndUnwrap(t.Assembly.Location, t.FullName);

            loader.LoadFromFile(@"C:\Services.NET\Hostaldog.com\WCFHostaldogService\HostalDogs.com.Common.dll");

            //issue appDomain still has no assemblies.
            Console.WriteLine(loader.GetAssemblies().Length.ToString());
        }

        public void testgeo() 
        {
            googleMaps.Geocoding.GeocodingRequest rq = new googleMaps.Geocoding.GeocodingRequest();
            rq.Address = "Palma";
            rq.Language = "ES";
            rq.Sensor = "true";

            googleMaps.Geocoding.GeocodingResponse rsp = googleMaps.Geocoding.GeocodingService.GetResponse(rq);
            if (rsp.Status == googleMaps.ServiceResponseStatus.Ok) 
            {
                foreach (googleMaps.Geocoding.GeocodingResult rs in rsp.Results) 
                {
                    Console.WriteLine(rs.FormattedAddress);

                    foreach (googleMaps.Geocoding.AddressComponent comp in rs.Components) 
                    {
                        if (comp.Types[0] == googleMaps.Geocoding.AddressType.Locality) 
                        {
                            Console.WriteLine("city: " + comp.LongName);
                        }
                        if (comp.Types[0] == googleMaps.Geocoding.AddressType.Country)
                        {
                            Console.WriteLine("country: " + comp.LongName);
                        }
                        if (comp.Types[0] == googleMaps.Geocoding.AddressType.PostalCode)
                        {
                            Console.WriteLine("CP: " + comp.LongName);
                        }
                        if (comp.Types[0] == googleMaps.Geocoding.AddressType.AdministrativeAreaLevel1)
                        {
                            Console.WriteLine("province: " + comp.LongName);
                        }
                    }

                }
            }
                
        }
    }


    public class LoadAssembly : MarshalByRefObject
    {
        public readonly AppDomain currentDomain;
        public LoadAssembly()
        {
            currentDomain = AppDomain.CurrentDomain;
        }


        public System.Reflection.Assembly[] GetAssemblies() 
        {
            return currentDomain.GetAssemblies();
        }

        public void LoadFromFile(string filePath)
        {
            //currentDomain has no assemblies:
            System.Reflection.Assembly.LoadFrom(filePath);
            //currentDomain has assemblies:

        }

        public void TestAzure() 
        {
            azure.AzureBlobStorageEngine eng = new azure.AzureBlobStorageEngine("hostaldog", 
                "XpcnoVzn2RyZkfI+8uhILK3mv0aDclGmlwAT+tolsg/r7TeOr5e/i+rDjpXBme0BVfWv1sUfyLvaBceaDvvnMw==", "http");

            Microsoft.WindowsAzure.Storage.Blob.IListBlobItem[] blobs = eng.GetDataList("images");

            foreach (Microsoft.WindowsAzure.Storage.Blob.IListBlobItem blob in blobs) 
            {
                Console.WriteLine(blob.Uri.ToString() + " - " + blob.GetType().ToString());
                //if (typeof(Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob) == blob.GetType()) 
                //{
                //    Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blblob = (Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob)blob;
                //    Console.Write(" [MIME: " + blblob.Properties.ContentType + "]");
                //    if (blblob.Uri.ToString().ToLower().Contains(".jpg") && blblob.Properties.ContentType != "image/jpeg") 
                //    {
                //        blblob.FetchAttributes();
                //        blblob.Properties.ContentType = "image/jpeg";
                //        blblob.SetProperties();
                //        Console.Write("Cambiado mime a : " + blblob.Properties.ContentType + "]");
    //            //    }
    //             <add key="AzureAccountName" value="hostaldog"/>
    //<add key="AzureAccountKey" value="XpcnoVzn2RyZkfI+8uhILK3mv0aDclGmlwAT+tolsg/r7TeOr5e/i+rDjpXBme0BVfWv1sUfyLvaBceaDvvnMw=="/>
    //            //}
            }
            string guid = Guid.NewGuid().ToString();
            Console.WriteLine("Updloading ... " + guid);
            eng.UploadData(new System.IO.FileStream(@"c:\temp\ab3f4428-856b-4660-8979-5542fa16547e.jpg", System.IO.FileMode.Open),
                "images", guid + ".jpg", "image/jpeg");

        }
        
    }

    


}
