using System.Drawing;

namespace TagCloudBuilder.Domain
{
    public class TextRectangle
    {
        public string Word { get; }
        public Font Font { get; }
        public Rectangle Rectangle { get; }

        public TextRectangle(string word, Font font, Rectangle rectangle)
        {
            Word = word;
            Font = font;
            Rectangle = rectangle;
        }
    }
}