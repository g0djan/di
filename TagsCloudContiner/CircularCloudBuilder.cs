using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudContainer.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    class CircularCloudBuilder : ITagsCloudBuilder
    {
        private Point Center { get; }
        private FontFamily FontFamily { get; }

        public CircularCloudBuilder(Point center, FontFamily fontFamily)
        {
            Center = center;
            FontFamily = fontFamily;
        }

        public Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes)
        {
            var layouter = new CircularCloudLayouter(Center);
            foreach (var rectangleShape in rectangleShapes)
                layouter.PutNextRectangle(rectangleShape);
            return layouter.Cloud;
        }

        public IEnumerable<Size> WordsToSizes(
            IEnumerable<string> words,
            Graphics graphics) =>
            words
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .Select(word =>
                    ToSize(word.Key,
                        GetFont(word, words, graphics),
                        graphics));

        public Cloud<TextRectangle> ToTextRectangles(
            IEnumerable<string> words,
            Graphics graphics) =>
            GetTextRectangles(words, BuildCloud(WordsToSizes(words, graphics)), graphics)
                .ToCloud(Center);

        private IEnumerable<TextRectangle> GetTextRectangles(
            IEnumerable<string> words,
            IEnumerable<Rectangle> rectangles,
            Graphics graphics) =>
            words
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .Zip(rectangles, (word, rectangle) =>
                    new TextRectangle(
                        word.Key,
                        GetFont(word, words, graphics),
                        rectangle));

        private int GetMinDimension(Graphics g) => (int) Math.Min(g.DpiX, g.DpiY);

        private int GetFontSize(int countThisWord, int countAllWords, int wordLength, int diametr)
        {
            if (wordLength == 0)
                return 1;
            var s = Math.PI * diametr * diametr / 4;
            var frequency = (double) countThisWord / countAllWords;
            var vovelHeight = frequency * s / wordLength;
            return (int) Math.Ceiling(vovelHeight);
        }

        private Font GetFont(IGrouping<string, string> word, IEnumerable<string> words, Graphics graphics) =>
            GetFont(GetFontSize(word.Count(), words.Count(), word.Key.Length, GetMinDimension(graphics)));

        private Font GetFont(int fontSize) =>
            new Font(FontFamily, fontSize);

        private Size ToSize(string word, Font font, Graphics graphics) =>
            Size.Ceiling(graphics.MeasureString(word, font));
    }

    [TestFixture]
    public class CircularCloudBuilder_Should
    {
        private Mock<ITagsCloudBuilder> mock;
        private Mock<IEnumerable<Size>> sizesMock;
        private Mock<IEnumerable<string>> wordsMock;
        private Mock<Graphics> graphicsMock;

        [SetUp]
        public void SetUp()
        {
            mock = new Mock<ITagsCloudBuilder>();
            sizesMock = new Mock<IEnumerable<Size>>();
        }

        [Test]
        public void WordsToSize_AreaIncreaseWithWordFrequency()
        {
            sizesMock.Setup(sizes => IsDescendingSizeSequence(sizes)).Returns(true);
            mock.Setup(builder =>
                builder.WordsToSizes(It.IsAny<IEnumerable<string>>(), It.IsAny<Graphics>())).Returns(sizesMock.Object);

            IsDescendingSizeSequence(new CircularCloudBuilder(new Point(0, 0), FontFamily.GenericMonospace)
                .WordsToSizes(new[] {"a", "b", "b"}, Graphics.FromImage(new Bitmap(1, 1)))).Should().BeTrue();
        }

        private bool IsDescendingSizeSequence(IEnumerable<Size> sizes)
        {
            var previous = sizes.First();
            return sizes.Skip(1).All(size =>
            {
                var result = size.Height * size.Width <= previous.Height * previous.Width;
                previous = size;
                return result;
            });
        }
    }
}