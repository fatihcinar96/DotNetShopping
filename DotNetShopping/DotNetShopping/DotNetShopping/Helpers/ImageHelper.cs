using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;   
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.Helpers;

namespace DotNetShopping.Helpers 
{
    public class ImageHelper
    {
        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            b.MakeTransparent();
            Graphics g = Graphics.FromImage((Image)b);
            g.CompositingMode = CompositingMode.SourceCopy;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public static void SaveImage(string name, System.Drawing.Image Image)
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 80L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            SaveImageForSize(name + ".jpg", Image, 1000, false, jgpEncoder, myEncoderParameters);
            for(int i=10; i>0; i--)
            {
                SaveImageForSize(name + "-" + i + ".jpg", Image, i * 100, true, jgpEncoder, myEncoderParameters);
            }
        }

        private static void SaveImageForSize(string name, Image Image, int size, bool watermark, ImageCodecInfo jgpEncoder, EncoderParameters myEncoderParameters)
        {
            string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ProductImage"), name);
            Image = resizeImage(Image, new Size(size, size/2*3));
            if(watermark)
            {
                Image = WatermarkText(Image, "DotNet19", size/10);
            }
            Image.Save(path, jgpEncoder, myEncoderParameters);
        }

        public static Image WatermarkText(Image image, string watermark, int size)
        {
            FontFamily family = new FontFamily("Arial");
            Color color = Color.FromArgb(50, 0, 0, 0);
            Color color2 = Color.FromArgb(100, 255, 255, 255);
            Point atpoint = new Point(image.Width / 2, image.Height / 2);
            SolidBrush brush = new SolidBrush(color);
            SolidBrush brush2 = new SolidBrush(color2);
            Graphics graphics = Graphics.FromImage(image);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            GraphicsPath myPath = new GraphicsPath();
            int fontStyle = (int)FontStyle.Italic;
            myPath.AddString(watermark, family, fontStyle, size, atpoint, sf);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(brush, myPath);
            graphics.DrawPath(new Pen(brush2, 2), myPath);
            graphics.Dispose();
            return image;
        }
    }
}