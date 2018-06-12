using System.IO;
using NUnit.Framework.Internal;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Domain
{
    public interface IFileReader
    {
        Result<string> ReadFile(string filename);
    }

    public class TxtReader : IFileReader
    {
        private ILogger Logger { get; }

        public TxtReader(ILogger logger)
        {
            Logger = logger;
        }
        public Result<string> ReadFile(string filename)
        {
            return Result.Of(() => File.ReadAllText(filename));
        }
    }
}
