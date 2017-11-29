using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContiner
{
    class FrequencyWord
    {
        public string Text;
        public double frequency;
        public Font font;

        public FrequencyWord(string text, double frequency)
        {
            Text = text;
            this.frequency = frequency;
            font = new Font(FontFamily.GenericMonospace, (int)(frequency * 4000));
        }

        public Size ToSize(Graphics graphics)
        {
            return Size.Ceiling(graphics.MeasureString(Text, font));

//            var width = (int) (10000 * frequency);
//            var height = width / Text.Length;
//            return new Size(width, height);
        }
    }
}
