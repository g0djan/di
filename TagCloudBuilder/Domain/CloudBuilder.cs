using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Domain
{
    public interface ITagCloudBuilder
    {
        Result<Cloud<Rectangle>> BuildCloud(IEnumerable<Size> rectangleShapes);
        Result<IEnumerable<TextRectangle>> GetTextRectangles(IEnumerable<string> words);
    }

    public class CloudBuilder : ITagCloudBuilder
    {
        private Point Center { get; }
        private IWordsBounder WordsBounder { get; }
        private ITagCloudLayouter CloudLayouter { get; }

        public CloudBuilder(Settings settings, IWordsBounder wordsBounder, ITagCloudLayouter layouter)
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