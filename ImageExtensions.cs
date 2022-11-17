﻿using System.Drawing.Imaging;
using Encoder = System.Drawing.Imaging.Encoder;
namespace RaccoonImageConverter
{
    public static class ImageExtensions
    {
        public static void SaveJpeg(this Image img, string filePath, long quality)
        {
            var ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            img.Save(filePath, GetEncoder(ImageFormat.Jpeg), ep);
        }

        public static void SaveJpeg(this Image img, Stream stream, long quality)
        {
            var ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            img.Save(stream, GetEncoder(ImageFormat.Jpeg), ep);
        }

        public static void SavePng(this Image img, string fileName, long quality)
        {
            var ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            img.Save(fileName, GetEncoder(ImageFormat.Png), ep);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.Single(codec => codec.FormatID == format.Guid);
        }
    }
}
