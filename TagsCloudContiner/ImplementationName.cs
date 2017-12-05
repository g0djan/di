namespace TagsCloudContainer
{
    class ImplementationName
    {
        public string Reader { get; }
        public string WordsEditor { get; }
        public string CloudBuilder { get; }
        public string Drawer { get; }

        public ImplementationName(string reader, string wordsEditor, string cloudBuilder, string drawer)
        {
            Reader = reader;
            WordsEditor = wordsEditor;
            CloudBuilder = cloudBuilder;
            Drawer = drawer;
        }
    }
}