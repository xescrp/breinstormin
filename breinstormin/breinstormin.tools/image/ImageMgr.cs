using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using Omu.Drawing;


namespace breinstormin.tools.image
{
    public static class ImageMgr
    {

        public static Image CropPng(string filename, string newfilename, int x, int y, int width, int height)
        {
            using (var image = Image.FromFile(filename))
            {
                var img = Imager.Crop(image, new Rectangle(x, y, width, height));

                img.Save(newfilename, System.Drawing.Imaging.ImageFormat.Png);
                return img;
            }
        }

        public static Image CropJpg(string filename, string newfilename, int x, int y, int width, int height)
        {
            using (var image = Image.FromFile(filename))
            {
                var img = Imager.Crop(image, new Rectangle(x, y, width, height));

                Imager.SaveJpeg(newfilename, img);
                return img;
            }
        }

        public static Image SaveJpgImageFromStream(string outputfilename, System.IO.Stream inputStream, int width, int height) 
        {
            using (var image = Image.FromStream(inputStream))
            {

                var resized = Imager.Resize(image, width, height, true);
                Imager.SaveJpeg(outputfilename, resized);

                return resized;
                ;
            }
        }

        public static Image SavePngImageFromStream(string outputfilename, System.IO.Stream inputStream, int width, int height) 
        {
            Image bmp = Bitmap.FromStream(inputStream);
          
            bmp.Save(outputfilename, System.Drawing.Imaging.ImageFormat.Png);
            return bmp;
        
        }

        public static void SaveJpgImage(Image image, string outputfilename)
        {
            Imager.SaveJpeg(outputfilename, image);
        }

        public static void SavePngImage(Image image, string outputfilename) 
        {
            image.Save(outputfilename, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static Image ResizePng(string filename, string outputfilename, int width, int height)
        {

            System.IO.FileStream str = new System.IO.FileStream(filename, System.IO.FileMode.Open);
            using (var image = Image.FromStream(str))
            {

                var resized = Imager.Resize(image, width, height, true);
                resized.Save(outputfilename, System.Drawing.Imaging.ImageFormat.Png);


                return resized;
                ;
            }
        }


        public static Image ResizeJpg(string filename, string outputfilename, int width, int height)
        {

            System.IO.FileStream str = new System.IO.FileStream(filename, System.IO.FileMode.Open);
            using (var image = Image.FromStream(str))
            {

                var resized = Imager.Resize(image, width, height, true);
                Imager.SaveJpeg(outputfilename, resized);


                return resized;
                ;
            }
        }

    }
}
