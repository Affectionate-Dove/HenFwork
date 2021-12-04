// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using NUnit.Framework;

namespace HenBstractions.Tests.Numerics
{
    public class SphereTests
    {
        private Sphere sphere;

        [TestCase(5, 314.159265359f)]
        [TestCase(2.5f, 78.5398163397f)]
        public void SurfaceAreaTest(float radius, float expected)
        {
            sphere.Radius = radius;
            Assert.AreEqual(expected, sphere.SurfaceArea);
        }

        [TestCase(5, 10)]
        [TestCase(2.5f, 5)]
        public void DiameterTest(float radius, float expected)
        {
            sphere.Radius = radius;
            Assert.AreEqual(expected, sphere.Diameter);
        }

        [TestCase(5)]
        [TestCase(2.5f)]
        public void DiameterSetterTest(float diameter)
        {
            sphere.Diameter = diameter;
            Assert.AreEqual(diameter, sphere.Diameter);
        }

        [TestCase(5, 523.598775598f)]
        public void VolumeTest(float radius, float expected)
        {
            sphere.Radius = radius;
            Assert.AreEqual(expected, sphere.Volume);
        }

        [TestCase(4, 2, 0, 69)]
        public void ToTopDownCircleTest(float x, float y, float z, float radius)
        {
            var circle = new Circle
            {
                Radius = radius,
                CenterPosition = new System.Numerics.Vector2(x, z)
            };
            sphere.CenterPosition = new System.Numerics.Vector3(x, y, z);
            sphere.Radius = radius;
            Assert.AreEqual(circle, sphere.ToTopDownCircle());
        }
    }
}