using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Domain
{
    public interface ITagCloudBuilder
    {
        Result<Cloud<Rectangle>> BuildCloud(IEnumerable<Size> rectangleShapes, Settings settings);
        Result<IEnumerable<TextRectangle>> GetTextRectangles(IEnumerable<string> words, Settings settings);
    }

    public class CloudBuilder : ITagCloudBuilder
    {
        private IEnumerable<IWordsBounder> WordBounders { get; }
        private IEnumerable<ITagCloudLayouter> CloudLayouters { get; }


        public CloudBuilder(IEnumerable<IWordsBounder> wordBounders, IEnumerable<ITagCloudLayouter> layouter)
        {
            CloudLayouters = layouter;
            WordBounders = wordBounders;
        }

        public Result<IEnumerable<TextRectangle>> GetTextRectangles(
            IEnumerable<string> words, 
            Settings settings)
        {
            return Result.Of(() =>
            {
                var wordsBounder = FinbByClassName(WordBounders, settings.WordsBounder);
                var rectangles = BuildCloud(wordsBounder.ConvertWordsToSizes(words, settings).GetValueOrThrow(),
                    settings);
                return GetTextRectangles(words, rectangles.GetValueOrThrow(), settings, wordsBounder);
            });
        }

        public Result<Cloud<Rectangle>> BuildCloud(
            IEnumerable<Size> rectangleShapes, 
            Settings settings)
        {
            var cloudLayouter = FinbByClassName(CloudLayouters, settings.CloudLayouter);
            cloudLayouter.Setup(settings);
            return Result.Of(() =>
            {
                cloudLayouter.Setup(settings);
                foreach (var rectangleShape in rectangleShapes)
                    cloudLayouter.PutNextRectangle(rectangleShape);
                return cloudLayouter.Cloud;
            });
        }

        private T FinbByClassName<T>(IEnumerable<T> sequence, string name)
        {
            return sequence.FirstOrDefault(p => p.GetType().Name == name);
        }

        private IEnumerable<TextRectangle> GetTextRectangles(
            IEnumerable<string> words,
            IEnumerable<Rectangle> rectangles,
            Settings settings,
            IWordsBounder bounder) =>
            words
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .Zip(rectangles, (word, rectangle) =>
                    new TextRectangle(word.Key, bounder.GetFont(word, words, settings).GetValueOrThrow(),
                        rectangle));
    }
}