// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

namespace HenFwork.Worlds.Functional.Chunks.Simulation
{
    public enum ChunksSimulationStrategyType
    {
        /// <summary>
        ///     All <see cref="Chunk"/>s in a <see cref="NodeWorld"/>
        ///     will be simulated.
        /// </summary>
        All,

        /// <summary>
        ///     Rings will have the shape of a square with a given radius.
        /// </summary>
        /// <remarks>
        ///     Radius in this context means the shortest distance
        ///     from the center of the square to each of its sides.
        ///     A square with 1 unit long sides has a radius of 0.5 units.
        /// </remarks>
        Square,

        /// <summary>
        ///     Rings will have the shape of a circle with a given radius.
        /// </summary>
        Circle
    }
}