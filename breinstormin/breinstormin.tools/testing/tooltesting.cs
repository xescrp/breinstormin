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
            azure.AzureBlobStorageEngine eng = new azure.AzureBlobStorageEngine("@accountname", "keyvalue", "http") ;

            Microsoft.WindowsAzure.Storage.Blob.IListBlobItem[] blobs = eng.GetDataList("images");

            foreach (Microsoft.WindowsAzure.Storage.Blob.IListBlobItem blob in blobs) 
            {
                Console.WriteLine(blob.Uri.ToString() + " - " + blob.GetType().ToString());
                if (typeof(Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob) == blob.GetType()) 
                {
                    Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blblob = (Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob)blob;
                    Console.Write(" [MIME: " + blblob.Properties.ContentType + "]");
                    if (blblob.Uri.ToString().ToLower().Contains(".jpg") && blblob.Properties.ContentType != "image/jpeg") 
                    {
                        blblob.FetchAttributes();
                        blblob.Properties.ContentType = "image/jpeg";
                        blblob.SetProperties();
                        Console.Write("Cambiado mime a : " + blblob.Properties.ContentType + "]");
                    }

                }
            }

        }
        
    }

    


}
