using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Interfaces;
using TagsCloudVisualization;
using Moq;

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

        public static Cloud<TextRectangle> GetCloudWith(
            this IEnumerable<string> words,
            ITagsCloudBuilder cloudBuilder,
            Graphics graphics) =>
            cloudBuilder.ToTextRectangles(words, graphics);
    }
}