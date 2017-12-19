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
        public Bitmap Bitmap { get; }
        public Graphics Graphics { get; }

        public Settings(Color color, FontFamily fontFamily, Point center, Bitmap bitmap, string inputPath, string outputFile = "cloud.png")
        {
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