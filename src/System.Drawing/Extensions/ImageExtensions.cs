using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Patchwork.Framework;
using Patchwork.Framework.Environment;

namespace System.Drawing.Extensions
{
    public static class ImageExtensions
    {
        public static Image FromFile(this Image image, string filename, bool useEmbeddedColorManagement = false)
        {
            return image.FromFile(filename, useEmbeddedColorManagement);
        }

        public static Bitmap FromHbitmap(this Image image, IntPtr hbitmap, IntPtr hpalette = new IntPtr())
        {
            return image.FromHbitmap(hbitmap, hpalette);
        }

        internal static Image LoadFromStream(this Image image, Stream stream, bool keepAlive)
        {
            return image.LoadFromStream(stream, keepAlive);
        }

        // note: FromStream can return either a Bitmap or Metafile instance
        // See http://support.microsoft.com/default.aspx?scid=kb;en-us;831419 for performance discussion	
        public static Image FromStream(this Image image, Stream stream, bool useEmbeddedColorManagement = false, bool validateImageData = false)
        {
            return image.LoadFromStream(stream, false);
        }
    }
}
