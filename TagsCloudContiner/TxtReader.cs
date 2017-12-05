using System.IO;

namespace TagsCloudContainer
{
    interface IFileReader
    {
        string ReadFile(string filename);
    }

    class TxtReader : IFileReader
    {
        public string ReadFile(string filename) => File.ReadAllText(filename);
    }
}
