// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System;

namespace HenFwork.Worlds.Functional.Chunks.Simulation
{
    public class ChunksSimulationRing : IComparable<ChunksSimulationRing>
    {
        public float Radius { get; }

        /// <summary>
        ///     The duration to wait before simulating
        ///     <see cref="Chunk"/>s inside this ring again.
        /// </summary>
        public double SimulationInterval { get; }

        /// <summary>
        ///     The time set on the last call of
        ///     <see cref="AdvanceTimeIfShouldBeSimulated(double)"/>
        ///     when it returned true.
        /// </summary>
        public double SynchronizedTime { get; private set; }

        public ChunksSimulationRing(ChunksSimulationRingConfiguration configuration)
        {
            Radius = configuration.Radius;
            SimulationInterval = configuration.SimulationInterval;
        }

        /// <summary>
        ///     Advances <see cref="SynchronizedTime"/> to
        ///     <paramref name="newTime"/> if the time difference
        ///     is greater than or equal to <see cref="SimulationInterval"/>.
        /// </summary>
        /// <returns>
        ///     Whether the <see cref="SynchronizedTime"/> was advanced.
        ///     That means the <see cref="Chunk"/>s inside this ring
        ///     should be simulated.
        /// </returns>
        public bool AdvanceTimeIfShouldBeSimulated(double newTime)
        {
            var timeDelta = newTime - SynchronizedTime;
            if (timeDelta < SimulationInterval)
                return false;

            SynchronizedTime = newTime;
            return true;
        }

        public int CompareTo(ChunksSimulationRing other) => Math.Sign(Radius - other.Radius);
    }
}