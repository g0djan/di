using System.IO;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TagsCloudContainer
{
    [TestFixture]
    public class TxtReader_Should
    {
        [Test]
        public void ThrowFileNotFoundException()
        {
            var mock = new Mock<IFileReader>();
            mock.Setup(reader => reader.ReadFile("unknown.txt")).Throws<FileNotFoundException>();
            var logger = new Mock<ILogger>();
            try
            {
                new TxtReader(logger.Object).ReadFile("unknown.txt");
            }
            catch(FileNotFoundException e)
            {
                logger.Verify(l => l.Debug(e.Message));
            }
        }
    }
}