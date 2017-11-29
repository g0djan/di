using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TagsCloudContiner.Interfaces;

namespace TagsCloudContiner
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "text.txt";
            var builder = new ContainerBuilder();
            builder.RegisterType<TxtReader>().As<IFileReader>();
            builder.RegisterType<TextFilter>().As<IWordsProcessing>();
            builder.RegisterType<CircularCloudBuilder>().As<ITagsCloudBuilder>();
            builder.RegisterType<CloudDrawer>().As<ICloudDrawer>();
            builder.Register(_ => new Size(1024, 1024)).As<Size>();
            builder.RegisterInstance(new Font(FontFamily.GenericMonospace, 1)).As<Font>();
            builder.Register(_ => Color.Crimson).As<Color>();
            builder.RegisterInstance(new Bitmap(1024, 1024)).As<Bitmap>();
            var container = builder.Build();

            var lexemes = container.Resolve<IFileReader>()
                .ReadFile(fileName)
                .PreprocessingWith(container.Resolve<IWordsProcessing>());
            var frequencyWords = lexemes
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .ThenByDescending(group => group.Key.Length)
                .Select(group => new FrequencyWord(group.Key, (double)group.Count() / lexemes.Count()));
            var bitmap = container.Resolve<Bitmap>();
            var graphics = Graphics.FromImage(bitmap);
            frequencyWords
                .Select(word => word.ToSize(graphics))
                .BuildTagCloudWith(container.Resolve<ITagsCloudBuilder>())
                .Zip(frequencyWords, (rectangle, s) => new WordRectangle(rectangle, s.Text, s.font))
                .ToCloud(x: 512, y: 512)
                .DrawPictureWith(container.Resolve<ICloudDrawer>(), graphics);
            bitmap.Save("cloud.png");
        }
    }
}