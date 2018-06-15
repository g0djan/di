using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Domain
{
    public interface IWordsBounder
    {
        Result<IEnumerable<Size>> ConvertWordsToSizes(IEnumerable<string> words, Settings settings);
        Result<Font> GetFont(IGrouping<string, string> word, IEnumerable<string> words, Settings settings);
    }

    public class WordsBounder : IWordsBounder
    {
        public Result<IEnumerable<Size>> ConvertWordsToSizes(IEnumerable<string> words, Settings settings) =>
            Result.Of(() => words
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .Select(word => GetSize(word.Key, GetFont(word, words, settings).GetValueOrThrow(),
                    settings.Graphics)));

        public Result<Font> GetFont(IGrouping<string, string> word,
            IEnumerable<string> words,
            Settings settings) =>
            Result.Of(() => GetFont(GetFontSize(
                word.Count(),
                words.Count(),
                word.Key.Length,
                GetMinImageDimension(settings.Graphics)), settings.FontFamily));

        private int GetMinImageDimension(Graphics graphics) => (int) Math.Min(graphics.DpiX, graphics.DpiY);

        private int GetFontSize(int countThisWord, int countAllWords, int wordLength, int diametr)
        {
            if (wordLength == 0)
                return 1;
            var s = Math.PI * diametr * diametr / 4;
            var frequency = (double) countThisWord / countAllWords;
            var vovelHeight = frequency * s / wordLength;
            return (int) Math.Ceiling(vovelHeight);
        }

        private Font GetFont(int fontSize, FontFamily fontFamily) =>
            new Font(fontFamily, fontSize);

        private Size GetSize(string word, Font font, Graphics graphics) =>
            Size.Ceiling(graphics.MeasureString(word, font));
    }
}