// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.Worlds.Chunks.Simulation;
using HenFwork.Worlds.Functional.Chunks;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace HenFwork.Worlds.Functional
{
    public class NodeWorld
    {
        public ChunksSimulationManager ChunksSimulationManager { get; }
        public ChunksManager ChunksManager { get; }
        public IEnumerable<Node> Nodes => ChunksManager.Chunks.Values.SelectMany(chunk => chunk.Nodes).Distinct();

        public double SynchronizedTime => ChunksSimulationManager.SynchronizedTime;

        public Vector2 Size => ChunksManager.ChunkCount * ChunksManager.ChunkSize;

        public NodeWorld() : this(new Vector2(100))
        {
        }

        public NodeWorld(Vector2 minimumSize) : this(minimumSize, 16)
        {
        }

        public NodeWorld(Vector2 minimumSize, float chunkSize)
        {
            var chunkCount = new Vector2(MathF.Ceiling(minimumSize.X / chunkSize), MathF.Ceiling(minimumSize.Y / chunkSize));
            ChunksManager = new(chunkCount, chunkSize);
            ChunksSimulationManager = new ChunksSimulationManager(ChunksManager);
        }

        public void AddNode(Node node)
        {
            ChunksManager.AddNode(node);
            node.NodeEjected += OnNodeEjected;
        }

        public void AddMedium(Medium medium) => ChunksManager.AddMedium(medium);

        public void Simulate(double newTime) => ChunksSimulationManager.Simulate(newTime);

        /// <summary>
        ///     Returns <see cref="Medium"/> mediums that
        ///     are in chunks that are inside
        ///     the specified <paramref name="area"/>.
        /// </summary>
        public IEnumerable<Medium> GetMediumsAroundArea(RectangleF area) => ChunksManager.GetChunksForRectangle(area).SelectMany(chunk => chunk.Mediums);

        public IEnumerable<Node> GetNodesAroundArea(RectangleF area) => ChunksManager.GetChunksForRectangle(area).SelectMany(chunk => chunk.Nodes);

        public IEnumerable<Chunk> GetChunksAroundArea(RectangleF area) => ChunksManager.GetChunksForRectangle(area);

        private void OnNodeEjected(Node node) => AddNode(node);
    }
}