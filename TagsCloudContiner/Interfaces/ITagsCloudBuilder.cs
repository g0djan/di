using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudContainer.Interfaces
{
    public interface ITagsCloudBuilder
    {
        Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes, Point cloudCenter);
    }
}
