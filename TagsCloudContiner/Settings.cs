using System.Drawing;

namespace TagsCloudContainer
{
    public class Settings
    {
        public Color Color { get; }
        public Point Center { get; }
        public FontFamily FontFamily { get; }
        public string InputPath { get; }
        public string OutputFile { get; }

        public Settings(Color color, FontFamily fontFamily, Point center, string inputPath, string outputFile)
        {
            Color = color;
            FontFamily = fontFamily;
            Center = center;
            InputPath = inputPath;
            OutputFile = outputFile;
        }
    }
}