using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloudBuilder.Domain;
using TagCloudBuilder.Infrastructure;
using static System.Math;

namespace TagCloudBuilder.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private CloudDrawer drawer;


        [SetUp]
        public void SetUp()
        {
            drawer = new CloudDrawer(1024, 1024);
            layouter = new CircularCloudLayouter(new Point(0, 0));//drawer.Width / 2, drawer.Height / 2));
        }

        [TearDown]
        public void TearDown()
        {
            if (!Equals(TestContext.CurrentContext.Result.Outcome, ResultState.Failure)) return;
            var path = Path.Combine(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory),
                "TestPictures",
                TestContext.CurrentContext.Test.Name + ".bmp");
            var bitmap = drawer.Draw(layouter);
            bitmap.Save(path);
            Console.WriteLine("Tag cloud visualization saved to file {0}", path);
        }

        [TestCase(1, 1, 0, 0, TestName = "Odd sizes will round x, y to int")]
        [TestCase(4, 4, -1, -1, TestName = "Even sizes ")]
        public void CenterFirstRectangle_Should_Be_InIntegerCloudCenter(
            int width, int height, int expectedX, int expectedY)
        {
            layouter = new CircularCloudLayouter(new Point(1, 1));
            var size = new Size(width, height);
            layouter.PutNextRectangle(size);
            layouter.Cloud
                .Last()
                .Should()
                .Be(new Rectangle(expectedX, expectedY, width, height));
        }

        [Test]
        public void SeveralRectanglesDoesNotIntersect()
        {
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                var r1 = 50 + random.Next() % 50;
                var r2 = 50 + random.Next() % 50;
                layouter.PutNextRectangle(new Size(r1, r2));
            }
            layouter.Cloud.Any(rectangle1 =>
                    layouter.Cloud.Any(rectangle2 => rectangle1 != rectangle2 &&
                    rectangle1.IntersectsWith(rectangle2)))
                .Should().BeFalse();
        }
        
        [TestCase(1,1, ExpectedResult = Quarter.XandYPositive)]
        [TestCase(0,1, ExpectedResult = Quarter.OnlyYPositive)]
        [TestCase(1,0, ExpectedResult = Quarter.OnlyXPositive)]
        [TestCase(-1,1, ExpectedResult = Quarter.OnlyYPositive)]
        [TestCase(-1,0, ExpectedResult = Quarter.XandYNonPositive)]
        [TestCase(-1,-1, ExpectedResult = Quarter.XandYNonPositive)]
        [TestCase(0,-1, ExpectedResult = Quarter.XandYNonPositive)]
        [TestCase(1,-1, ExpectedResult = Quarter.OnlyXPositive)]
        [TestCase(0,0, ExpectedResult = Quarter.XandYNonPositive)]
        public Quarter TestGetQuarter(int x, int y)
        {
            return new Point(x, y).GetQuarter(new Point(0, 0));
        }

        [TestCase(PI / 4, 1, 1)]
        [TestCase(3 * PI / 4, -4, 1)]
        [TestCase(-3 * PI / 4, -4, -3)]
        [TestCase(- PI / 4, 1, -3)]
        public void TestGetNearestToCenterCorner_AllQuarters(double angle, int expectedX, int expectedY)
        {
            layouter.GetNearestToCenterCornerPoint(angle, 1, new Size(3, 2))
                .Should()
                .Be(new Point(expectedX, expectedY));
        }
    }
}