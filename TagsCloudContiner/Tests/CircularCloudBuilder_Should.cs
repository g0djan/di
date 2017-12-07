using System.Collections.Generic;
using System.Drawing;
using Moq;
using NUnit.Framework;

namespace TagsCloudContainer
{
    [TestFixture]
    public class CircularCloudBuilder_Should
    {
        private string[] Words { get; set; }
        private Settings Settings { get; set; }

        [SetUp]
        public void SetUp()
        {
            Words = new[] { "a" };
            Settings = new Settings(
                Color.AliceBlue, FontFamily.GenericMonospace, new Point(), new Bitmap(10, 10), "");
        }

        [Test]
        public void BoundWords_DuringBuildCloud()
        {
            var mock = new Mock<ITagsCloudBuilder>();
            var bounder = new Mock<IWordsBounder>();
            mock.Setup(builder => builder.GetTextRectangles(It.IsAny<IEnumerable<string>>()))
                .Returns(It.IsAny<IEnumerable<TextRectangle>>());
            new CircularCloudBuilder(Settings, bounder.Object).GetTextRectangles(Words);
            bounder.Verify(bd => bd.ConvertWordsToSizes(Words));

        }
    }
}