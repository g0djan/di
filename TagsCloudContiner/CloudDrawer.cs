using System.Drawing;
using System.Windows.Forms;
using TagsCloudContainer.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    class CloudDrawer : ICloudDrawer
    {
        private Color color;

        public CloudDrawer(Color color)
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
    }
}