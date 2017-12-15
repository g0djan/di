using System.IO;
using NUnit.Framework.Internal;

namespace TagsCloudContainer
{
    interface IFileReader
    {
        string ReadFile(string filename);
    }

    class TxtReader : IFileReader
    {
        private ILogger Logger { get; }

        public TxtReader(ILogger logger)
        {
            Logger = logger;
        }
        public string ReadFile(string filename)
        {
            try
            {
                return File.ReadAllText(filename);
            }
            catch (FileNotFoundException e)
            {
                Logger.Debug(e.Message);
                throw;
            } 
        }
    }
}
