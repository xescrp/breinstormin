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
                new web.css.cssReader(@"C:\css\estilos.css");
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
    }
}
