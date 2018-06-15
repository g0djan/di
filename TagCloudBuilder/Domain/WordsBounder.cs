using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Domain
{
    public interface IWordsBounder
    {
        Result<IEnumerable<Size>> ConvertWordsToSizes(IEnumerable<string> words);
        Result<Font> GetFont(IGrouping<string, string> word, IEnumerable<string> words);
    }

    public class WordsBounder : IWordsBounder
    {
        private FontFamily FontFamily { get; set; }
        private Graphics Graphics { get; set; }

        public WordsBounder(Settings settings)
        {
            FontFamily = settings.FontFamily;
            Graphics = settings.Graphics;
        }
        
        public Result<IEnumerable<Size>> ConvertWordsToSizes(IEnumerable<string> words) =>
            Result.Of(() => words
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .Select(word => GetSize(word.Key, GetFont(word, words).GetValueOrThrow())));

        public Result<Font> GetFont(IGrouping<string, string> word, IEnumerable<string> words) =>
            Result.Of(() => GetFont(GetFontSize(
                word.Count(),
                words.Count(),
                word.Key.Length,
                GetMinImageDimension())));

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