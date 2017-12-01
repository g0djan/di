using System.IO;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer
{
    class TxtReader : IFileReader
    {
        public string ReadFile(string filename) => File.ReadAllText(filename);
    }
}
