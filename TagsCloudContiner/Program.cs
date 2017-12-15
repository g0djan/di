using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using NUnit.Framework.Internal;

namespace TagsCloudContainer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new AppTagsCloud());
        }

        public static void DrawTagsCloud(IContainer container, ImplementationName name, IEnumerable<string> boringWords)
        {
            var filter = container.ResolveNamed<IWordsFilter>(name.WordsFilter);
            filter.AddBoringWords(boringWords);
            var text = container.ResolveNamed<IFileReader>(name.Reader)
                .ReadFile(container.Resolve<Settings>().InputPath);
            var words = container.Resolve<ITextParser>().GetWords(text);
            words = filter.FilterWords(words);
            words = container.ResolveNamed<IWordsEditor>(name.WordsEditor).Edit(words);
            var textRectangles = container.ResolveNamed<ITagsCloudBuilder>(name.CloudBuilder).GetTextRectangles(words);
            var drawer = container.ResolveNamed<ITextRectanglesDrawer>(name.Drawer);
            drawer.Draw(textRectangles);
            drawer.Save(container.Resolve<Settings>().Bitmap);
        }

        public static IContainer SetContainer(Settings settings, IEnumerable<ForRegister> toRegister)
        {
            var builder = RegisterImplementations(toRegister);
            builder.RegisterType<TxtReader>().Named<IFileReader>("txt");
            builder.RegisterType<TextParser>().As<ITextParser>();
            builder.RegisterType<WordsFilter>().Named<IWordsFilter>("All");
            builder.RegisterType<WordsEditor>().Named<IWordsEditor>("No format");
            builder.RegisterType<WordsBounder>().As<IWordsBounder>();
            builder.RegisterType<CircularCloudBuilder>().Named<ITagsCloudBuilder>("Circular");
            builder.RegisterType<PngDrawer>().Named<ITextRectanglesDrawer>("png");
            builder.RegisterInstance(settings).As<Settings>();
            builder.Register(_ => settings.Center).As<Point>();
            builder.RegisterInstance(new Logger("CloudLogger", InternalTraceLevel.Debug, TextWriter.Null))
                .As<ILogger>();
            return builder.Build();
        }

        private static ContainerBuilder RegisterImplementations(IEnumerable<ForRegister> toRegister)
        {
            var builder = new ContainerBuilder();
            Register<IFileReader>(builder, toRegister);
            Register<IWordsFilter>(builder, toRegister);
            Register<IWordsEditor>(builder, toRegister);
            Register<ITagsCloudBuilder>(builder, toRegister);
            Register<ITextRectanglesDrawer>(builder, toRegister);
            return builder;
        }

        private static void Register<TInterface>(
            ContainerBuilder builder,
            IEnumerable<ForRegister> toRegister)
        {
            foreach (var registring in toRegister.Where(t => t.Implementation == typeof(TInterface)))
                builder.RegisterType(registring.Implementation).Named<TInterface>(registring.Name);
        }
    }
}