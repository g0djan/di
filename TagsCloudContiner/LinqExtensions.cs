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
    public static class LinqExtensions
    {
        public static IEnumerable<string> PreprocessingWith(this IEnumerable<string> words,
            IWordsProcessing wordProcessor) => 
            wordProcessor.WordsPreprocessing(words);

        public static Cloud<Rectangle> BuildTagCloudWith(this IEnumerable<Size> sizes, ITagsCloudBuilder builder) => 
            builder.BuildCloud(sizes);

        public static void DrawPictureWith(this Cloud<WordRectangle> cloud, 
            ICloudDrawer drawer, Graphics graphics) =>
            drawer.Draw(cloud, graphics);

        public static Cloud<T> ToCloud<T>(this IEnumerable<T> collection, int x = 0, int y = 0)
        {
            var cloud = new Cloud<T>(new Point(x, y));
            cloud.AddRange(collection);
            return cloud;
        }
    }
}
