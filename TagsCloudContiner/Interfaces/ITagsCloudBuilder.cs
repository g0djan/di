using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization;

namespace TagsCloudContiner.Interfaces
{
    public interface ITagsCloudBuilder
    {
        Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes);
    }
}
