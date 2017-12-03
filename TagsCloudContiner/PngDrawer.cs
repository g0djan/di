using System.Drawing;
using System.Windows.Forms;
using TagsCloudContainer.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    class PngDrawer : ICloudDrawer
    {
        private Color color;

        public PngDrawer(Color color)
        {
            this.color = color;
        }

        public void Draw(Cloud<TextRectangle> cloud, Graphics graphics)
        {
            const TextFormatFlags flags = TextFormatFlags.WordBreak;
            cloud.ForEach(textRectangle =>
                TextRenderer.DrawText(graphics,
                    textRectangle.Word,
                    textRectangle.Font, 
                    textRectangle.Rectangle,
                    color,
                    flags));
        }

        public void Save(Bitmap bmp)
        {
            using (var newBmp = new Bitmap(bmp))
                newBmp.Save("cloud.png");
        }
    }
}