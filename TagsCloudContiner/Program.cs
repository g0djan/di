using System.Drawing;
using System.Linq;
using Autofac;
using TagsCloudContainer.Interfaces;
using TagsCloudContiner;

namespace TagsCloudContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "text.txt";
            var builder = new ContainerBuilder();
            builder.RegisterType<CircularCloudBuilder>().As<ITagsCloudBuilder>();
            builder.RegisterType<TextFilter>().As<IWordsProcessing>();
            builder.RegisterType<CloudDrawer>().As<ICloudDrawer>();
            builder.RegisterType<TxtReader>().As<IFileReader>();
            builder.RegisterInstance(new Bitmap(1024, 1024)).As<Bitmap>();
            builder.Register(_ => new Size(1024, 1024)).As<Size>();
            builder.Register(_ => Color.Crimson).As<Color>();
            builder.RegisterType<TextParser>().AsSelf();
            var center = new Point(512, 512);
            var fontFamily = FontFamily.GenericMonospace;

            var container = builder.Build();
            var bitmap = container.Resolve<Bitmap>();
            var graphics = Graphics.FromImage(bitmap);

            var textRectanglesCloud = container.Resolve<IFileReader>()
                .ReadFile(fileName)
                .ParseTextWith(container.Resolve<TextParser>())
                .PreprocessingWith(container.Resolve<IWordsProcessing>())
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .ToTextRectanglesWith(container.Resolve<ITagsCloudBuilder>(), graphics, fontFamily, center);
            graphics.DrawWordsCloudWith(container.Resolve<ICloudDrawer>(), textRectanglesCloud);

            bitmap.Save("cloud.png");
        }
    }
}