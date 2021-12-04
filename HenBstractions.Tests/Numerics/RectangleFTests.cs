// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using NUnit.Framework;
using System;
using System.Numerics;

namespace HenBstractions.Tests.Numerics
{
    public class RectangleFTests
    {
        private RectangleF rect;

        [Test]
        public void Test()
        {
            rect = new RectangleF(2, 4, 4, 2);

            Assert.AreEqual(new Vector2(2, 2), rect.Size);
            Assert.AreEqual(new Vector2(3), rect.Center);
            Assert.AreEqual(4, rect.Area);
            Assert.AreEqual(CoordinateSystem2d.YDown, rect.CoordinateSystem);
        }

        [Test]
        public void Test2()
        {
            rect = new RectangleF(-4, -6, 3, -2);

            Assert.AreEqual(-2, rect.Width);
            Assert.AreEqual(5, rect.Height);
            Assert.AreEqual(10, rect.Area);
            Assert.AreEqual(CoordinateSystem2d.YDown, rect.CoordinateSystem);
        }

        [Test]
        public void Test3()
        {
            rect = new RectangleF(3, -2, -6, -4);

            Assert.AreEqual(-5, rect.Width);
            Assert.AreEqual(2, rect.Height);
            Assert.AreEqual(10, rect.Area);
            Assert.AreEqual(CoordinateSystem2d.YUp, rect.CoordinateSystem);
        }

        [Test]
        public void GetIntersectionTest()
        {
            var rect1 = new RectangleF(1, 2, 1, 2);
            var rect2 = new RectangleF(1.5f, 2.5f, 1.5f, 2.5f);
            var rect1YD = new RectangleF(1, 2, 2, 1);
            var rect2YD = new RectangleF(1.5f, 2.5f, 2.5f, 1.5f);
            var rect3 = new RectangleF(3, 4, 3, 4);

            Assert.Throws<ArgumentException>(() => rect1.GetIntersection(rect1YD));
            Assert.Throws<ArgumentException>(() => rect1.GetIntersection(rect2YD));
            Assert.Throws<ArgumentException>(() => rect2.GetIntersection(rect1YD));
            Assert.Throws<ArgumentException>(() => rect2.GetIntersection(rect2YD));

            RectangleF? expected = new RectangleF(1.5f, 2, 1.5f, 2);
            Assert.AreEqual(expected, rect1.GetIntersection(rect2));
            Assert.AreEqual(expected, rect2.GetIntersection(rect1));

            expected = new RectangleF(1.5f, 2, 2, 1.5f);
            Assert.AreEqual(expected, rect1YD.GetIntersection(rect2YD));
            Assert.AreEqual(expected, rect2YD.GetIntersection(rect1YD));

            expected = null;
            Assert.AreEqual(expected, rect1.GetIntersection(rect3));
            Assert.AreEqual(expected, rect3.GetIntersection(rect1));
        }

        [Test]
        public void FromPositionAndSizeTest()
        {
            var rect = RectangleF.FromPositionAndSize(new(0), new(10), CoordinateSystem2d.YDown);
            Assert.AreEqual(new RectangleF(0, 10, 10, 0), rect);

            rect = RectangleF.FromPositionAndSize(new(0), new(10), CoordinateSystem2d.YUp);
            Assert.AreEqual(new RectangleF(0, 10, 0, 10), rect);

            rect = RectangleF.FromPositionAndSize(new(0), new(10), new(0.5f), CoordinateSystem2d.YUp);
            Assert.AreEqual(new RectangleF(-5, 5, -5, 5), rect);
        }
    }
}