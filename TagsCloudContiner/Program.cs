using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autofac;

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
            var filter = container.ResolveNamed<IWordsEditor>(name.WordsEditor);
            filter.AddBoringWords(boringWords);
            var text = container.ResolveNamed<IFileReader>(name.Reader)
                .ReadFile(container.Resolve<Settings>().InputPath);
            var words = container.Resolve<ITextParser>().GetWords(text);
            words = filter.EditWords(words);
            var textRectangles = container.ResolveNamed<ITagsCloudBuilder>(name.CloudBuilder).GetTextRectangles(words);
            var drawer = container.ResolveNamed<ITextRectanglesDrawer>(name.Drawer);
            drawer.Draw(textRectangles);
            drawer.Save(container.Resolve<Settings>().Bitmap);
        }

        public static IContainer SetContainer(Settings settings, IEnumerable<RegistringImplemetation> toRegister)
        {
            var builder = RegisterImplementations(toRegister);
            builder.RegisterType<TxtReader>().Named<IFileReader>("txt");
            builder.RegisterType<TextParser>().As<ITextParser>();
            builder.RegisterType<WordsEditor>().Named<IWordsEditor>("No format:All");
            builder.RegisterType<WordsBounder>().As<IWordsBounder>();
            builder.RegisterType<CircularCloudBuilder>().Named<ITagsCloudBuilder>("Circular");
            builder.RegisterType<PngDrawer>().Named<ITextRectanglesDrawer>("png");
            builder.RegisterInstance(settings).As<Settings>();            
            return builder.Build();
        }

        private static ContainerBuilder RegisterImplementations(IEnumerable<RegistringImplemetation> toRegister)
        {
            var builder = new ContainerBuilder();
            builder = Registration<IFileReader>(builder, toRegister);
            builder = Registration<IWordsEditor>(builder, toRegister);
            builder = Registration<ITagsCloudBuilder>(builder, toRegister);
            builder = Registration<ITextRectanglesDrawer>(builder, toRegister);
            return builder;
        }

        private static ContainerBuilder Registration<TInterface>(ContainerBuilder builder,
            IEnumerable<RegistringImplemetation> toRegister)
        {
            foreach (var registring in toRegister.Where(t => t.Implementation == typeof(TInterface)))
                builder.RegisterType(registring.Implementation).Named<TInterface>(registring.Name);
            return builder;
        }
    }
}