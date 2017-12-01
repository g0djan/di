using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    class CircularCloudBuilder : ITagsCloudBuilder
    {
        public Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes, Point point)
        {
            var layouter = new CircularCloudLayouter(point);
            foreach (var rectangleShape in rectangleShapes)
                layouter.PutNextRectangle(rectangleShape);
            return layouter.Cloud;
        }
    }
}
