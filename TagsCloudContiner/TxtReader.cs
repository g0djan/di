using System.IO;
using NUnit.Framework.Internal;
using ResultOf;

namespace TagsCloudContainer
{
    interface IFileReader
    {
        Result<string> ReadFile(string filename);
    }

    class TxtReader : IFileReader
    {
        private ILogger Logger { get; }

        public TxtReader(ILogger logger)
        {
            Logger = logger;
        }
        public Result<string> ReadFile(string filename)
        {
            return Result.Of(() => File.ReadAllText(filename));
//            try
//            {
//                return File.ReadAllText(filename);
//            }
//            catch (FileNotFoundException e)
//            {
//                Logger.Debug(e.Message);
//                throw;
//            } 
        }
    }
}
