using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var container = SetContainer();
            Application.Run(new AppTagCloud(container.GetValueOrThrow()));
        }

        public static void DrawTagCloud(IContainer container,
            ImplementationName name,
            IEnumerable<string> boringWords,
            Settings settings)
        {
            var filter = container.ResolveNamed<IWordsFilter>(name.WordsFilter);
            filter.AddBoringWords(boringWords);
            var parameterSettings = new ResolvedParameter((pi, ctx) => pi.Name == "settings",
                (pi, ctx) => settings);
            var parameterLayouter = new ResolvedParameter((pi, ctx) => pi.Name == "layouter",
                (pi, ctx) => container.ResolveNamed<ITagCloudLayouter>(name.CloudLayouter, parameterSettings));
            var parameterBounder = new ResolvedParameter((pi, ctx) => pi.Name == "wordsBounder",
                (pi, ctx) => container.Resolve<IWordsBounder>(parameterSettings));
            var textRectangles = container.ResolveNamed<IFileReader>(name.Reader, parameterSettings)
                .ReadFile(settings.InputPath)
                .Then(container.Resolve<ITextParser>().GetWords)
                .Then(filter.FilterWords)
                .Then(container.ResolveNamed<IWordsEditor>(name.WordsEditor).Edit)
                .Then(container.Resolve<ITagCloudBuilder>(parameterBounder, parameterLayouter, parameterSettings)
                    .GetTextRectangles);
            var drawer = container.ResolveNamed<ITextRectanglesDrawer>(name.Drawer, parameterSettings);
            drawer.Draw(textRectangles.GetValueOrThrow());
            drawer.Save(settings.Bitmap);
        }

        private static Result<IContainer> SetContainer() => Result.Of(() =>
        {
            var builder = new ContainerBuilder();
            var assm = Assembly.Load("TagCloudBuilder");
            foreach (var type in assm.GetTypes().Where(t => t.IsInterface))
                builder.RegisterAssemblyTypes(assm)
                    .Named(t => t.Name, type)
                    .AsImplementedInterfaces();
            builder.RegisterInstance(new Logger("CloudLogger", InternalTraceLevel.Debug, TextWriter.Null))
                .As<ILogger>();
            return builder.Build();
        });
    }
}