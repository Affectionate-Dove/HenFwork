// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.PathFinding;
using NUnit.Framework;

namespace HenFwork.Tests.Worlds.Functional.PathFinding
{
    public class PathNodeTests
    {
        [Test]
        public void ConnectSymmetricallyTest()
        {
            var a = new PathNode();
            var b = new PathNode();
            a.ConnectSymmetrically(b);
            Assert.IsTrue(a.Connections.Contains(b));
            Assert.IsTrue(b.Connections.Contains(a));
        }

        [Test]
        public void ConnectAsymmetricallyTest()
        {
            var a = new PathNode();
            var b = new PathNode();
            a.ConnectAsymmetrically(b, true);
            Assert.IsTrue(a.Connections.Contains(b));
            Assert.IsFalse(b.Connections.Contains(a));

            a.DisconnectAsymmetrically(b);
            b.ConnectAsymmetrically(a);
            a.ConnectAsymmetrically(b, true);
            Assert.IsTrue(a.Connections.Contains(b));
            Assert.IsFalse(b.Connections.Contains(a));

            a.DisconnectAsymmetrically(b);
            a.ConnectAsymmetrically(b, false);
            Assert.IsTrue(a.Connections.Contains(b));
            Assert.IsFalse(b.Connections.Contains(a));

            a.DisconnectAsymmetrically(b);
            b.ConnectAsymmetrically(a);
            a.ConnectAsymmetrically(b, false);
            Assert.IsTrue(a.Connections.Contains(b));
            Assert.IsTrue(b.Connections.Contains(a));
        }

        [Test]
        public void DisconnectSymmetricallyTest()
        {
            var a = new PathNode();
            var b = new PathNode();
            a.ConnectSymmetrically(b);
            Assert.IsTrue(a.Connections.Contains(b));
            Assert.IsTrue(b.Connections.Contains(a));
            a.DisconnectSymmetrically(b);
            Assert.IsFalse(a.Connections.Contains(b));
            Assert.IsFalse(b.Connections.Contains(a));
        }

        [Test]
        public void DisconnectAsymmetricallyTest()
        {
            var a = new PathNode();
            var b = new PathNode();
            a.ConnectSymmetrically(b);
            Assert.IsTrue(a.Connections.Contains(b));
            Assert.IsTrue(b.Connections.Contains(a));
            a.DisconnectAsymmetrically(b);
            Assert.IsFalse(a.Connections.Contains(b));
            Assert.IsTrue(b.Connections.Contains(a));
        }
    }
}