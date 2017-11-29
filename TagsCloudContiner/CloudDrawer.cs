using System.Drawing;
using System.Windows.Forms;
using TagsCloudContiner.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudContiner
{
    class CloudDrawer : ICloudDrawer
    {
        private Color color;

        public CloudDrawer(Color color)
        {
            this.color = color;
        }

        public void Draw(Cloud<WordRectangle> cloud, Graphics graphics)
        {
//            var bitmap = new Bitmap(size.Width, size.Height);
//            var graphics = Graphics.FromImage(bitmap);
            const TextFormatFlags flags = TextFormatFlags.WordBreak;
            cloud.ForEach(rectangle =>
                TextRenderer.DrawText(graphics, rectangle.Text, rectangle.font, rectangle.rectangle, color, flags));
                //graphics.DrawString(rectangle.Text, font, pen, rectangle.rectangle));
//            return bitmap;
        }
    }
}