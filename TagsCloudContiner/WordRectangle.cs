using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContiner
{
    public class WordRectangle
    {
        public Rectangle rectangle;
        public string Text;
        public Font font;

        public WordRectangle(Rectangle rectangle, string text, Font font)
        {
            this.rectangle = rectangle;
            Text = text;
            this.font = font;
        }
    }
}
