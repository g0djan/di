using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    public interface ITagsCloudBuilder
    {
        Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes);
        IEnumerable<TextRectangle> GetTextRectangles(IEnumerable<string> words);
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

        public Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes)
        {
            foreach (var rectangleShape in rectangleShapes)
                CloudLayouter.PutNextRectangle(rectangleShape);
            return CloudLayouter.Cloud;
        }

        public IEnumerable<TextRectangle> GetTextRectangles(IEnumerable<string> words) =>
            GetTextRectangles(words, BuildCloud(WordsBounder.ConvertWordsToSizes(words)));

        private IEnumerable<TextRectangle> GetTextRectangles(
            IEnumerable<string> words,
            IEnumerable<Rectangle> rectangles) =>
            words
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .Zip(rectangles, (word, rectangle) =>
                    new TextRectangle(word.Key, WordsBounder.GetFont(word, words), rectangle));
    }
}