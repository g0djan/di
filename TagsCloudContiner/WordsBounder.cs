using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public interface IWordsBounder
    {
        IEnumerable<Size> ConvertWordsToSizes(IEnumerable<string> words);
        Font GetFont(IGrouping<string, string> word, IEnumerable<string> words);
    }

    public class WordsBounder : IWordsBounder
    {
        private FontFamily FontFamily { get; }
        private Graphics Graphics { get; }

        public WordsBounder(FontFamily fontFamily, Graphics graphics)
        {
            FontFamily = fontFamily;
            Graphics = graphics;
        }

        public IEnumerable<Size> ConvertWordsToSizes(IEnumerable<string> words) =>
            words
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .Select(word => GetSize(word.Key, GetFont(word, words)));

        public Font GetFont(IGrouping<string, string> word, IEnumerable<string> words) =>
            GetFont(GetFontSize(
                word.Count(),
                words.Count(),
                word.Key.Length,
                GetMinImageDimension()));

        private int GetMinImageDimension() => (int) Math.Min(Graphics.DpiX, Graphics.DpiY);

        private int GetFontSize(int countThisWord, int countAllWords, int wordLength, int diametr)
        {
            if (wordLength == 0)
                return 1;
            var s = Math.PI * diametr * diametr / 4;
            var frequency = (double) countThisWord / countAllWords;
            var vovelHeight = frequency * s / wordLength;
            return (int) Math.Ceiling(vovelHeight);
        }

        private Font GetFont(int fontSize) =>
            new Font(FontFamily, fontSize);

        private Size GetSize(string word, Font font) =>
            Size.Ceiling(Graphics.MeasureString(word, font));
    }
}