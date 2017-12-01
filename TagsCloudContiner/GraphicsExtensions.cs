using System.Drawing;
using TagsCloudContainer;
using TagsCloudContainer.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudContiner
{
    public static class GraphicsExtensions
    {
        public static void DrawWordsCloudWith(this Graphics graphics, ICloudDrawer drawer,
            Cloud<TextRectangle> cloud) =>
            drawer.Draw(cloud, graphics);

    }
}
