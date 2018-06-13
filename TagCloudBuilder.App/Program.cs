using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TagCloudBuilder.Domain;
using TagCloudBuilder.Infrastructure;
using Autofac;
using Autofac.Core;
using NUnit.Framework.Internal;

namespace TagCloudBuilder.App
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new AppTagCloud());
        }

        public static void DrawTagCloud(IContainer container,
            ImplementationName name,
            IEnumerable<string> boringWords)
        {
            var filter = container.ResolveNamed<IWordsFilter>(name.WordsFilter);
            filter.AddBoringWords(boringWords);
            var namedLayouter = new ResolvedParameter((pi, ctx) => pi.Name == "layouter",
                (pi, ctx) => container.ResolveNamed<ITagCloudLayouter>(name.CloudLayouter));
            var textRectangles = container.ResolveNamed<IFileReader>(name.Reader)
                .ReadFile(container.Resolve<Settings>().InputPath)
                .Then(container.Resolve<ITextParser>().GetWords)
                .Then(filter.FilterWords)
                .Then(container.ResolveNamed<IWordsEditor>(name.WordsEditor).Edit)
                .Then(container.Resolve<ITagCloudBuilder>(namedLayouter).GetTextRectangles);
            var drawer = container.ResolveNamed<ITextRectanglesDrawer>(name.Drawer);
            drawer.Draw(textRectangles.GetValueOrThrow());
            drawer.Save(container.Resolve<Settings>().Bitmap);
        }

        public static Result<IContainer> SetContainer(Settings settings) =>
            Result.Of(() =>
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<TxtReader>().Named<IFileReader>("txt");
                builder.RegisterType<TextParser>().As<ITextParser>();
                builder.RegisterType<WordsFilter>().Named<IWordsFilter>("All");
                builder.RegisterType<WordsEditor>().Named<IWordsEditor>("No format");
                builder.RegisterType<WordsBounder>().As<IWordsBounder>();
                builder.RegisterType<CloudBuilder>().As<ITagCloudBuilder>();
                builder.RegisterType<PngDrawer>().Named<ITextRectanglesDrawer>("png");
                builder.RegisterType<CircularCloudLayouter>().Named<ITagCloudLayouter>("Circular");
                builder.RegisterInstance(new Logger("CloudLogger", InternalTraceLevel.Debug, TextWriter.Null))
                    .As<ILogger>();
                builder.RegisterInstance(settings).As<Settings>();
                builder.Register(_ => settings.Center).As<Point>();
                return builder.Build();
            });
        

        private static Result<ContainerBuilder> Register<TInterface>(
            ContainerBuilder builder,
            IEnumerable<ForRegister> toRegister) =>
            Result.Of(() =>
            {
                foreach (var registring in toRegister.Where(t => t.Implementation == typeof(TInterface)))
                    builder.RegisterType(registring.Implementation).Named<TInterface>(registring.Name);
                return builder;
            });
    }
}