using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    public static class LinqExtensions
    {
        public static IEnumerable<string> PreprocessingWith(this IEnumerable<string> words,
           TextFilter wordProcessor) =>
            wordProcessor.WordsPreprocessing(words);


        public static Cloud<T> ToCloud<T>(this IEnumerable<T> collection, Point center)
        {
            var cloud = new Cloud<T>(center);
            cloud.AddRange(collection);
            return cloud;
        }

        public static Cloud<TextRectangle> ToTextRectanglesWith(
            this IEnumerable<IGrouping<string, string>> countedWords,
            ITagsCloudBuilder cloudBuilder,
            Graphics graphics,
            FontFamily fontFamily,
            Point center) =>
            countedWords
                .Zip(GetRectanglesCloud(countedWords, cloudBuilder, graphics, fontFamily, center),
                    (countedWord, rectangle) => new TextRectangle(countedWord.Key,
                        GetFont(countedWord.Count(), fontFamily, countedWords.Count(), graphics, countedWord.Key.Length), rectangle))
                .ToCloud(center);

        private static Cloud<Rectangle> GetRectanglesCloud(
            IEnumerable<IGrouping<string, string>> countedWords,
            ITagsCloudBuilder cloudBuilder,
            Graphics graphics,
            FontFamily fontFamily,
            Point center) =>
            countedWords
                .Select(countedWord =>
                    ToSize(countedWord.Key,
                        GetFont(countedWord.Count(), fontFamily, countedWords.Count(), graphics, countedWord.Key.Length), graphics))
                .BuildTagCloudWith(cloudBuilder, center);

        private static Font GetFont(int countedWordCount, FontFamily fontFamily, int countedWordsCount, Graphics graphics, int wordLength) =>
            new Font(fontFamily, GetFontSize(countedWordCount, countedWordsCount, graphics, wordLength));

        private static Cloud<Rectangle> BuildTagCloudWith(this IEnumerable<Size> sizes, ITagsCloudBuilder builder,
            Point center) =>
            builder.BuildCloud(sizes, center);

        private static int GetFontSize(int countThisWord, int countAllWords, Graphics graphics, int wordLength)
        {
            if (wordLength == 0)
                return 1;
            var d = Math.Min(graphics.DpiX, graphics.DpiY);
            var s = Math.PI * d * d / 4;
            var frequency = (double) countThisWord / countAllWords;
            var vovelHeight = frequency * s / wordLength;
            return (int)Math.Ceiling(vovelHeight);
        }

        private static Size ToSize(string word, Font font, Graphics graphics) =>
            Size.Ceiling(graphics.MeasureString(word, font));
    }
}