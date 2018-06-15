using System.Collections.Generic;

namespace TagCloudBuilder.App
{
    public class ImplementationName
    {
        private static readonly Dictionary<string, string> nameToImplementationName = new Dictionary<string, string>()
        {
            {"txt", "TxtReader"},
            {"All", "WordsFilter"},
            {"No format", "WordsEditor"},
            {"Circular", "CircularCloudLayouter"},
            {"png", "PngDrawer"}
        };

        public string Reader { get; }
        public string WordsFilter { get; }
        public string WordsEditor { get; }
        public string CloudLayouter { get; }
        public string Drawer { get; }

        public ImplementationName(string reader, string wordsFilter, string wordsEditor, string cloudLayouter, string drawer)
        {
            Reader = nameToImplementationName[reader];
            WordsFilter = nameToImplementationName[wordsFilter];
            WordsEditor = nameToImplementationName[wordsEditor];
            CloudLayouter = nameToImplementationName[cloudLayouter];
            Drawer = nameToImplementationName[drawer];
        }
    }
}