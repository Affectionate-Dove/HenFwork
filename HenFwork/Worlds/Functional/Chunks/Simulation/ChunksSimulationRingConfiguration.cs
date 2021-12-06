// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

namespace HenFwork.Worlds.Functional.Chunks.Simulation
{
    public record ChunksSimulationRingConfiguration
    {
        public float Radius { get; }

        /// <summary>
        ///     The duration to wait before simulating
        ///     <see cref="Chunk"/>s inside this ring again.
        /// </summary>
        public double SimulationInterval { get; }

        public ChunksSimulationRingConfiguration(float radius, double simulationInterval)
        {
            Radius = radius;
            SimulationInterval = simulationInterval;
        }
    }
}