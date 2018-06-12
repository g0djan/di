using System.Drawing;
using TagCloudBuilder.Domain;

namespace TagCloudBuilder.Tests
{
    public class CloudDrawer
    {
        public int Width { get; }
        public int Height { get; }
        public CloudDrawer(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public Bitmap Draw(CircularCloudLayouter cloudLayouter)
        {
            var bitmap = new Bitmap(Width, Height);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new SolidBrush(Color.DarkRed);
            cloudLayouter.Cloud.ForEach(rectangle => graphics.FillRectangle(pen, rectangle));
            return bitmap;
        }
    }
}
