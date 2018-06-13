﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudBuilder.Infrastructure;
using static System.Math;

namespace TagCloudBuilder.Domain
{
    public interface ITagCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
        Cloud<Rectangle> Cloud { get; }
    }

    public class CircularCloudLayouter : ITagCloudLayouter
    {
        public Cloud<Rectangle> Cloud { get; }
        private int radius;
        private readonly Dictionary<Quarter, Func<Rectangle, Point>> shifts;
        private double angle;

        public CircularCloudLayouter(Point center)
        {
            Cloud = new Cloud<Rectangle>(center);
            radius = 1;
            shifts = InitShifts();
            angle = 0d;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (!Cloud.Any())
            {
                var rectPlaced = new Point((int) (Cloud.Center.X - rectangleSize.Width / 2d),
                    (int) (Cloud.Center.Y - rectangleSize.Height / 2d));
                var rectangle = new Rectangle(rectPlaced, rectangleSize);
                Cloud.Add(rectangle);
                return rectangle;
            }
            Cloud.Add(FindNextRectangle(rectangleSize));
            return Cloud.Last();
        }

        private Dictionary<Quarter, Func<Rectangle, Point>> InitShifts()
        {
            return new Dictionary<Quarter, Func<Rectangle, Point>>
            {
                [Quarter.XandYPositive] = GetShift(false, false),
                [Quarter.OnlyYPositive] = GetShift(true, false),
                [Quarter.XandYNonPositive] = GetShift(true, true),
                [Quarter.OnlyXPositive] = GetShift(false, true)
            };
        }

        private static Func<Rectangle, Point> GetShift(bool isDx, bool isDy)
        {
            return rectangle => new Point(
                rectangle.X - (isDx ? 1 : 0) * rectangle.Width, 
                rectangle.Y - (isDy ? 1 : 0) * rectangle.Height);
        }

        private Rectangle FindNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var placeForRectangle = GetPointsCircleWithoutCollisions(radius, rectangleSize)
                    .Select(point => new { point, rectangle = new Rectangle(point, rectangleSize) })
                    .FirstOrDefault(namedTuple => 
                        Cloud.All(cloudRectangle => !cloudRectangle.IntersectsWith(namedTuple.rectangle)))?
                    .point;
                if (placeForRectangle.HasValue)
                    return new Rectangle(placeForRectangle.Value, rectangleSize);
                radius++;
            }
        }

        private IEnumerable<Point> GetPointsCircleWithoutCollisions(int r, Size rectangleSize)
        {
            return GetPointsCircle(r, rectangleSize)
                .GroupBy(p => p)
                .Select(group => group.Key);
        }

        private IEnumerable<Point> GetPointsCircle(int r, Size rectangleSize)
        {
            if (r < 0)
                throw new ArgumentException();
            if (r == 0)
            {
                yield return Cloud.Center;
                yield break;
            }
            var shiftAngle = 1d / r;
            for (; angle < 2 * PI; angle += shiftAngle)
                yield return GetNearestToCenterCornerPoint(r, rectangleSize);
            angle %= 2 * PI;
        }

        private Point GetNearestToCenterCornerPoint(int r, Size size)
        {
            var point = Cloud.Center.Shift((int) Round(Cos(angle) * r), (int) Round(Sin(angle) * r));
            return shifts[point.GetQuarter(Cloud.Center)](new Rectangle(point, size));
        }

        //For test

        public Point GetNearestToCenterCornerPoint(double angle, int r, Size size)
        {
            this.angle = angle;
            return GetNearestToCenterCornerPoint(r, size);
        }
    }
}

    