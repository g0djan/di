using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudContainer.Interfaces
{
    public interface ITagsCloudBuilder
    {
        Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes);
        IEnumerable<Size> WordsToSizes(IEnumerable<string> words, Graphics graphics);
        Cloud<TextRectangle> ToTextRectangles(IEnumerable<string> words, Graphics graphics);
    }
}
