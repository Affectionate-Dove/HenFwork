// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using NUnit.Framework;
using System.Numerics;

namespace HenBstractions.Tests.Numerics
{
    public class Triangle2Tests
    {
        private readonly Triangle2 triangle = new(new(0, 0), new(0, 1.5f), new(3, 0));

        [Test]
        public void CentroidTest() => Assert.AreEqual(new Vector2(1, 0.5f), triangle.Centroid);

        [Test]
        public void ReversedTest() => Assert.AreEqual(new Triangle2(new(3, 0), new(0, 1.5f), new(0, 0)), triangle.Reversed());
    }
}