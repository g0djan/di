using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContiner.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudContiner
{
    class CircularCloudBuilder : ITagsCloudBuilder
    {
        public Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes)
        {
            var layouter = new CircularCloudLayouter(new Point(512, 512));
            foreach (var rectangleShape in rectangleShapes)
                layouter.PutNextRectangle(rectangleShape);
            return layouter.Cloud;
        }
    }
}
