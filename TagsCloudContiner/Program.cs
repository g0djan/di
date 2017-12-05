using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static void DrawTagsCloud(IContainer container, IEnumerable<string> boringWords)
        {
            var filter = container.Resolve<IWordsEditor>();
            filter.AddBoringWords(boringWords);
            var text = container.Resolve<IFileReader>().ReadFile(container.Resolve<string>());
            var words = container.Resolve<ITextParser>().GetWords(text);
            words = filter.EditWords(words);
            var textRectangles = container.Resolve<ITagsCloudBuilder>().GetTextRectangles(words);
            var drawer = container.Resolve<ITextRectanglesDrawer>();
            drawer.Draw(textRectangles);
            drawer.Save(container.Resolve<Bitmap>());
        }
    }
}