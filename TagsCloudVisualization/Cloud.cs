using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Cloud<T> : List<T>
    {
        public readonly Point Center;

        public Cloud(Point center)
        {
            Center = center;
        }
    }
}