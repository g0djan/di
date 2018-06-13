﻿using System.Collections.Generic;
using System.Drawing;
using Moq;
using NUnit.Framework;
using TagCloudBuilder.Domain;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.Tests
{
    [TestFixture]
    public class CircularCloudBuilder_Should
    {
        private string[] Words { get; set; }
        private Settings Settings { get; set; }
        private CircularCloudLayouter Layouter { get; set; }

        [SetUp]
        public void SetUp()
        {
            Words = new[] {"a"};
            Settings = new Settings(
                Color.AliceBlue, FontFamily.GenericMonospace, new Point(), new Bitmap(10, 10), "");
            Layouter = new CircularCloudLayouter(Settings.Center);
        }

        [Test]
        public void BoundWords_DuringBuildCloud()
        {
            var mock = new Mock<ITagCloudBuilder>();
            var bounder = new Mock<IWordsBounder>();
            mock.Setup(builder => builder.GetTextRectangles(It.IsAny<IEnumerable<string>>()))
                .Returns(It.IsAny<Result<IEnumerable<TextRectangle>>>());
            new CircularCloudBuilder(Settings, bounder.Object, Layouter).GetTextRectangles(Words);
            bounder.Verify(bd => bd.ConvertWordsToSizes(Words));
        }
    }
}