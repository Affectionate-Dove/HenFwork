﻿using HenHen.Framework.Graphics2d;
using NUnit.Framework;

namespace HenHen.Framework.Tests.Graphics2d
{
    public class CircleTests
    {
        [TestCase(5, 10 * System.MathF.PI)]
        [TestCase(2.5f, 5 * System.MathF.PI)]
        public void CircumferenceTest(float radius, float expected)
        {
            var kolo = new Circle
            {
                Radius = radius
            };
            Assert.AreEqual(expected, kolo.Circumference);
        }

        [TestCase(5, 10)]
        [TestCase(2.5f, 5)]
        public void DiameterTest(float radius, float expected)
        {
            var kolo1 = new Circle
            {
                Radius = radius
            };
            Assert.AreEqual(expected, kolo1.Diameter);
        }

        [TestCase(5)]
        [TestCase(2.5f)]
        public void DiameterSetterTest(float diameter)
        {
            var kolo2 = new Circle
            {
                Diameter = diameter
            };
            Assert.AreEqual(diameter, kolo2.Diameter);
        }

        [TestCase(5, 78.5398163397f)]
        public void AreaTest(float radius, float expected)
        {
            var kolo3 = new Circle()
            {
                Radius = radius
            };
            Assert.AreEqual(expected, kolo3.Area);
        }
    }
}