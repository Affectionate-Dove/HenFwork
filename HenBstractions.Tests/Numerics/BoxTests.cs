// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using NUnit.Framework;
using System.Numerics;

namespace HenBstractions.Tests.Numerics
{
    public class BoxTests
    {
        private Box box;

        [SetUp]
        public void SetUp() => box = new(-2, 4, 3, 7, -5, -2);

        [Test]
        public void FacesTest()
        {
            Assert.AreEqual(-2, box.Left);
            Assert.AreEqual(4, box.Right);

            Assert.AreEqual(3, box.Bottom);
            Assert.AreEqual(7, box.Top);

            Assert.AreEqual(-5, box.Back);
            Assert.AreEqual(-2, box.Front);
        }

        [Test]
        public void CenterTest() => Assert.AreEqual(new Vector3(1, 5, -3.5f), box.Center);

        [Test]
        public void SizeTest() => Assert.AreEqual(new Vector3(6, 4, 3), box.Size);
    }
}