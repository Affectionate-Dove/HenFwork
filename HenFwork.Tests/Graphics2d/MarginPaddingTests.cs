// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using NUnit.Framework;
using System.Numerics;

namespace HenFwork.Tests.Graphics2d
{
    [TestOf(typeof(MarginPadding))]
    public class MarginPaddingTests
    {
        [Test]
        public void ParameterlessCtorTest() => Check(0, 0, 0, 0, new MarginPadding());

        [Test]
        public void EachSideCtorTest()
        {
            const int left = 1;
            const int right = 2;
            const int bottom = 3;
            const int top = 4;
            var mp = new MarginPadding(top, left, bottom, right);
            Check(top, left, bottom, right, mp);
        }

        [Test]
        public void HorizontalVerticalCtorTest()
        {
            const float horizontal = 3.5f;
            const float vertical = 2.7f;
            Check(vertical, horizontal, vertical, horizontal, new MarginPadding(horizontal, vertical));
            Check(vertical, horizontal, vertical, horizontal, new MarginPadding(new Vector2(horizontal, vertical)));
        }

        [Test]
        public void AllSidesCtorTest()
        {
            const float allSides = 24.3f;
            Check(allSides, allSides, allSides, allSides, new MarginPadding(allSides));
        }

        private static void Check(float top, float left, float bottom, float right, MarginPadding marginPadding)
        {
            Assert.AreEqual(top, marginPadding.Top);
            Assert.AreEqual(left, marginPadding.Left);
            Assert.AreEqual(bottom, marginPadding.Bottom);
            Assert.AreEqual(right, marginPadding.Right);
            Assert.AreEqual(top + bottom, marginPadding.TotalVertical);
            Assert.AreEqual(left + right, marginPadding.TotalHorizontal);
            Assert.AreEqual(new Vector2(left, top), marginPadding.TopLeft);
            Assert.AreEqual(new Vector2(right, bottom), marginPadding.BottomRight);
        }
    }
}