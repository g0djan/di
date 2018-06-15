using System.Collections.Generic;
using System.Drawing;
using Moq;
using NUnit.Framework;
using TagCloudBuilder.Domain;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Tests
{
    [TestFixture]
    public class CloudBuilder_Should
    {
        private string[] Words { get; set; }
        private Settings Settings { get; set; }
        private CircularCloudLayouter Layouter { get; set; }

        [SetUp]
        public void SetUp()
        {
            Words = new[] {"a"};
            Settings = new Settings("txt", "All", "No format", "Circular", "png", "WordsBounder",
                Color.AliceBlue, FontFamily.GenericMonospace, new Point(), new Bitmap(10, 10), "");
            Layouter = new CircularCloudLayouter();
            Layouter.Setup(new Point(0, 0));
        }

        [Test]
        public void BoundWords_DuringBuildCloud()
        {
            var mock = new Mock<ITagCloudBuilder>();
            var bounder = new Mock<IWordsBounder>();
            mock.Setup(builder => builder.GetTextRectangles(It.IsAny<IEnumerable<string>>(), Settings))
                .Returns(It.IsAny<Result<IEnumerable<TextRectangle>>>());
            new CloudBuilder(new[] {bounder.Object}, new []{Layouter}).GetTextRectangles(Words, Settings);
            bounder.Verify(bd => bd.ConvertWordsToSizes(Words, Settings));
        }
    }
}