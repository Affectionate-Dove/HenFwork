// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.Chunks.Simulation;
using NUnit.Framework;

namespace HenFwork.Tests.Worlds.Functional.Chunks.Simulation
{
    public class ChunksSimulationRingTests
    {
        [Test]
        public void AdvanceTimeIfShouldBeSimulatedTest()
        {
            var config = new ChunksSimulationRingConfiguration(2, 5);
            var ring = new ChunksSimulationRing(config);
            var newTime = 2;
            var expectedSynchronizedTime = 0;
            Assert.IsFalse(ring.AdvanceTimeIfShouldBeSimulated(newTime));
            Assert.AreEqual(expectedSynchronizedTime, ring.SynchronizedTime);

            newTime = 4;
            Assert.IsFalse(ring.AdvanceTimeIfShouldBeSimulated(newTime));
            Assert.AreEqual(expectedSynchronizedTime, ring.SynchronizedTime);

            expectedSynchronizedTime = newTime = 5;
            Assert.IsTrue(ring.AdvanceTimeIfShouldBeSimulated(newTime));
            Assert.AreEqual(expectedSynchronizedTime, ring.SynchronizedTime);

            newTime = 6;
            Assert.IsFalse(ring.AdvanceTimeIfShouldBeSimulated(newTime));
            Assert.AreEqual(expectedSynchronizedTime, ring.SynchronizedTime);

            expectedSynchronizedTime = newTime = 11;
            Assert.IsTrue(ring.AdvanceTimeIfShouldBeSimulated(newTime));
            Assert.AreEqual(expectedSynchronizedTime, ring.SynchronizedTime);

            newTime = 12;
            Assert.IsFalse(ring.AdvanceTimeIfShouldBeSimulated(newTime));
            Assert.AreEqual(expectedSynchronizedTime, ring.SynchronizedTime);
        }
    }
}