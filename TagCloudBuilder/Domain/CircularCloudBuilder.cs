using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Domain
{
    public interface ITagsCloudBuilder
    {
        Result<Cloud<Rectangle>> BuildCloud(IEnumerable<Size> rectangleShapes);
        Result<IEnumerable<TextRectangle>> GetTextRectangles(IEnumerable<string> words);
    }

    public class CircularCloudBuilder : ITagsCloudBuilder
    {
        private Point Center { get; }
        private IWordsBounder WordsBounder { get; }
        private CircularCloudLayouter CloudLayouter { get; }

        public CircularCloudBuilder(Settings settings, IWordsBounder wordsBounder, CircularCloudLayouter layouter)
        {
            Center = settings.Center;
            CloudLayouter = layouter;
            WordsBounder = wordsBounder;
        }

        public Result<Cloud<Rectangle>> BuildCloud(IEnumerable<Size> rectangleShapes)
        {
            return Result.Of(() =>
            {
                foreach (var rectangleShape in rectangleShapes)
                    CloudLayouter.PutNextRectangle(rectangleShape);
                return CloudLayouter.Cloud;
            });
        }

        public Result<IEnumerable<TextRectangle>> GetTextRectangles(IEnumerable<string> words) =>
            Result.Of(() =>
            {
                var rectangles = BuildCloud(WordsBounder.ConvertWordsToSizes(words).GetValueOrThrow());
                return GetTextRectangles(words, rectangles.GetValueOrThrow());
            });

        private IEnumerable<TextRectangle> GetTextRectangles(
            IEnumerable<string> words,
            IEnumerable<Rectangle> rectangles) =>
            words
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .Zip(rectangles, (word, rectangle) =>
                    new TextRectangle(word.Key, WordsBounder.GetFont(word, words).GetValueOrThrow(), rectangle));
    }
}