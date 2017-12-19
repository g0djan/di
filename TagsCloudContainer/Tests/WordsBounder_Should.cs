using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace TagsCloudContainer
{
    [TestFixture]
    public class WordsBounder_Should
    {
        private string[] words;
        private Settings settings;

        [SetUp]
        public void SetUp()
        {
            words = new[] {"a", "b", "b"};
            settings = new Settings(
                Color.AliceBlue, FontFamily.GenericMonospace, new Point(), new Bitmap(10, 10), "");
        }

        //такое вообще надо? если да то как сделать чтоб работало)
        [Test]
        public void ConvertsWordsToSize_UseMeasureString()
        {
            var bounder = new Mock<IWordsBounder>();
            
            new WordsBounder(settings).ConvertWordsToSizes(words);
            bounder.Verify(bd => bd.GetFont(It.IsAny<IGrouping<string, string>>(), It.IsAny<IEnumerable<string>>()));
        }

        [Test]
        public void SizesDescendingWithWordsFrequency()
        {
            var wordBounder = new WordsBounder(settings);
            var sizes = wordBounder.ConvertWordsToSizes(words).GetValueOrThrow().ToArray();
            GetArea(sizes[0]).Should().BeGreaterOrEqualTo(GetArea(sizes[1]));
        }

        private long GetArea(Size size) => (long)size.Width * size.Height;
    }
}