using System.Collections.Generic;
using System.Drawing;

namespace TagCloudBuilder.Domain
{
    public class Settings
    {
        public string Reader { get; }
        public string WordsFilter { get; }
        public string WordsEditor { get; }
        public string CloudLayouter { get; }
        public string Drawer { get; }
        public string WordsBounder { get; }
        public Color Color { get; }
        public Point Center { get; }
        public FontFamily FontFamily { get; }
        public string InputPath { get; }
        public string OutputFile { get; }
        public Bitmap Bitmap { get; }
        public Graphics Graphics { get; }

        private static readonly Dictionary<string, string> NameToImplementationName =
            new Dictionary<string, string>()
            {
                {"txt", "TxtReader"},
                {"All", "WordsFilter"},
                {"No format", "WordsEditor"},
                {"Circular", "CircularCloudLayouter"},
                {"png", "PngDrawer"},
                {"WordsBounder", "WordsBounder"}
            };

        public Settings(string reader, string wordsFilter, string wordsEditor,
            string cloudLayouter, string drawer, string bounder, Color color, FontFamily fontFamily, 
            Point center, Bitmap bitmap, string inputPath, string outputFile = "cloud.png")
        {
            Reader = NameToImplementationName[reader];
            WordsFilter = NameToImplementationName[wordsFilter];
            WordsEditor = NameToImplementationName[wordsEditor];
            CloudLayouter = NameToImplementationName[cloudLayouter];
            Drawer = NameToImplementationName[drawer];
            WordsBounder = NameToImplementationName[bounder];
            Color = color;
            FontFamily = fontFamily;
            Center = center;
            InputPath = inputPath;
            Bitmap = bitmap;
            OutputFile = outputFile;
            Graphics = Graphics.FromImage(Bitmap);
        }
    }
}