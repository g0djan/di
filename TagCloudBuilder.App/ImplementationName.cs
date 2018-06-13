namespace TagCloudBuilder.App
{
    class ImplementationName
    {
        public string Reader { get; }
        public string WordsFilter { get; }
        public string WordsEditor { get; }
        public string CloudLayouter { get; }
        public string Drawer { get; }

        public ImplementationName(string reader, string wordsFilter, string wordsEditor, string cloudLayouter, string drawer)
        {
            Reader = reader;
            WordsFilter = wordsFilter;
            WordsEditor = wordsEditor;
            CloudLayouter = cloudLayouter;
            Drawer = drawer;
        }
    }
}