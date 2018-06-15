using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TagCloudBuilder.Domain;

namespace TagCloudBuilder.App
{
    public interface ITextRectanglesDrawer
    {
        void Draw(IEnumerable<TextRectangle> textRectangles, Settings settings);
        void Save(Bitmap bmp);
    }

    public class PngDrawer : ITextRectanglesDrawer
    {
        public void Draw(IEnumerable<TextRectangle> textRectangles, Settings settings)
        {
            const TextFormatFlags flags = TextFormatFlags.WordBreak;
            foreach (var textRectangle in textRectangles)
            {
                IsRectangleInBounds(textRectangle, settings.Graphics);
                TextRenderer.DrawText(settings.Graphics,
                    textRectangle.Word,
                    textRectangle.Font,
                    textRectangle.Rectangle,
                    settings.Color,
                    flags);
            }
        }

        private void IsRectangleInBounds(TextRectangle textRectangle, Graphics graphics)
        {
            if (textRectangle.Rectangle.Left < graphics.VisibleClipBounds.Left ||
                textRectangle.Rectangle.Bottom > graphics.VisibleClipBounds.Bottom ||
                textRectangle.Rectangle.Right > graphics.VisibleClipBounds.Right ||
                textRectangle.Rectangle.Top < graphics.VisibleClipBounds.Top)
                throw new Exception("Cloud is so big for this window");
        }

        public void Save(Bitmap bmp)
        {
            using (var newBmp = new Bitmap(bmp))
                newBmp.Save("cloud.png");
        }
    }
}