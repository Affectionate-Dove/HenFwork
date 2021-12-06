// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Generic;

namespace HenFwork.Worlds.Functional.Chunks.Simulation
{
    public record ChunksSimulationStrategy
    {
        public ChunksSimulationStrategyType Type { get; }

        /// <summary>
        ///     Sorted set, with the smallest
        ///     <see cref="ChunksSimulationRingConfiguration"/> first.
        /// </summary>
        public IReadOnlySet<ChunksSimulationRingConfiguration> Rings { get; } = new SortedSet<ChunksSimulationRingConfiguration>();

        public ChunksSimulationStrategy(ChunksSimulationStrategyType type, IReadOnlySet<ChunksSimulationRingConfiguration> rings)
        {
            Type = type;
            Rings = rings;
        }
    }
}