﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudContainer
{
    public interface ITextRectanglesDrawer
    {
        void Draw(IEnumerable<TextRectangle> textRectangles);
        void Save(Bitmap bmp);
    }

    class PngDrawer : ITextRectanglesDrawer
    {
        private Color Color { get; }
        private Graphics Graphics { get; }

        public PngDrawer(Settings settings)
        {
            Color = settings.Color;
            Graphics = settings.Graphics;
        }

        public void Draw(IEnumerable<TextRectangle> textRectangles)
        {
            const TextFormatFlags flags = TextFormatFlags.WordBreak;
            foreach (var textRectangle in textRectangles)
            {
                TextRenderer.DrawText(Graphics,
                    textRectangle.Word,
                    textRectangle.Font,
                    textRectangle.Rectangle,
                    Color,
                    flags);
            }
        }

        public void Save(Bitmap bmp)
        {
            using (var newBmp = new Bitmap(bmp))
                newBmp.Save("cloud.png");
        }
    }
}