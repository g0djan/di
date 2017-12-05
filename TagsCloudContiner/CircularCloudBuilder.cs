﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    public interface ITagsCloudBuilder
    {
        Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes);
        IEnumerable<TextRectangle> GetTextRectangles(IEnumerable<string> words);
    }

    public class CircularCloudBuilder : ITagsCloudBuilder
    {
        private Point Center { get; }
        private IWordsBounder Converter { get; }

        public CircularCloudBuilder(Point center, IWordsBounder converter)
        {
            Center = center;
            Converter = converter;
        }

        public Cloud<Rectangle> BuildCloud(IEnumerable<Size> rectangleShapes)
        {
            var layouter = new CircularCloudLayouter(Center);
            foreach (var rectangleShape in rectangleShapes)
                layouter.PutNextRectangle(rectangleShape);
            return layouter.Cloud;
        }

        public IEnumerable<TextRectangle> GetTextRectangles(IEnumerable<string> words) =>
            GetTextRectangles(words, BuildCloud(Converter.ConvertWordsToSizes(words)));

        private IEnumerable<TextRectangle> GetTextRectangles(
            IEnumerable<string> words,
            IEnumerable<Rectangle> rectangles) =>
            words
                .GroupBy(key => key)
                .OrderByDescending(group => group.Count())
                .Zip(rectangles, (word, rectangle) =>
                    new TextRectangle(word.Key, Converter.GetFont(word, words), rectangle));
    }
}