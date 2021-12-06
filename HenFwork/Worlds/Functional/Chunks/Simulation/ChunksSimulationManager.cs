// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.Collisions;
using HenFwork.Worlds.Functional.Chunks;
using HenFwork.Worlds.Functional.Chunks.Simulation;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace HenFwork.Worlds.Chunks.Simulation
{
    /// <summary>
    ///     Manages organized simulation of <see cref="Chunk"/>s.
    /// </summary>
    public partial class ChunksSimulationManager
    {
        private readonly List<NodeTransfer> nodeTransfers = new();

        private readonly SortedSet<ChunksSimulationRing> rings = new();
        private ChunksSimulationStrategy strategy;

        /// <summary>
        ///     The time at the start of the latest simulation.
        /// </summary>
        public double SynchronizedTime { get; private set; }

        public ChunksSimulationStrategy Strategy
        {
            get => strategy;
            set
            {
                strategy = value;
                OnStrategyChanged();
            }
        }

        protected ChunksManager ChunksManager { get; }

        /// <summary>
        ///     Sets <see cref="Strategy"/> with
        ///     <see cref="ChunksSimulationStrategy.Type"/> equal to
        ///     <see cref="ChunksSimulationStrategyType.All"/>.
        /// </summary>
        public ChunksSimulationManager(ChunksManager chunksManager)
        {
            ChunksManager = chunksManager;
            Strategy = new(ChunksSimulationStrategyType.All, null);
        }

        /// <summary>
        ///     Simulates chunks according to <see cref="Strategy"/>.
        /// </summary>
        /// <param name="newTime">
        ///     The value to which <see cref="SynchronizedTime"/>
        ///     will be set at the beginning of this function.
        /// </param>
        /// <param name="origin">
        ///     The point around which the simulation occurs
        ///     according to <see cref="Strategy"/>.
        /// </param>
        public void Simulate(double newTime, Vector2 origin = default)
        {
            SynchronizedTime = newTime;
            foreach (var chunk in GetChunksToSimulate(newTime, origin))
                SimulateChunk(chunk, newTime);
            PerformNodesTransfers();
        }

        private void OnStrategyChanged()
        {
            if (Strategy.Type != ChunksSimulationStrategyType.All && (Strategy.Rings == null || Strategy.Rings.Count == 0))
                throw new Exception($"If the {nameof(Strategy.Type)} is not {nameof(ChunksSimulationStrategyType.All)}, there has to be at least one {nameof(ChunksSimulationRingConfiguration)} defined in {nameof(Strategy.Rings)}.");
            if (Strategy.Type == ChunksSimulationStrategyType.All && Strategy.Rings != null && Strategy.Rings.Count != 0)
                throw new Exception($"There are {nameof(ChunksSimulationRingConfiguration)}s in {nameof(Strategy.Rings)}, but {nameof(Strategy.Type)} is {nameof(ChunksSimulationStrategyType.All)}, making them useless.");

            rings.Clear();
            if (Strategy.Type == ChunksSimulationStrategyType.All)
                return;
            foreach (var ringConfiguration in Strategy.Rings)
                rings.Add(new ChunksSimulationRing(ringConfiguration));
        }

        private IEnumerable<Chunk> GetChunksToSimulate(double newTime, Vector2 origin)
        {
            if (Strategy.Type == ChunksSimulationStrategyType.All)
                return ChunksManager.Chunks.Values;

            var widestRing = rings.Where(ring => ring.AdvanceTimeIfShouldBeSimulated(newTime))
                .LastOrDefault();

            if (widestRing is null)
                return Array.Empty<Chunk>();

            var radius = widestRing.Radius;

            if (Strategy.Type == ChunksSimulationStrategyType.Circle)
                return ChunksManager.GetChunksForCircle(new() { CenterPosition = origin, Radius = radius });
            else if (Strategy.Type == ChunksSimulationStrategyType.Square)
            {
                var square = new RectangleF(origin.X - radius, origin.X + radius, origin.Y - radius, origin.Y + radius);
                return ChunksManager.GetChunksForRectangle(square);
            }

            throw new NotImplementedException($"Handling the value \"{Strategy.Type}\" for {nameof(Strategy.Type)} is not implemented.");
        }

        private void SimulateChunk(Chunk chunk, double newTime)
        {
            foreach (var simulatedNode in chunk.Simulate(newTime))
            {
                ContainNodeInMediums(simulatedNode);
                RegisterNodeTransfers(simulatedNode, chunk);
            }

            CollisionChecker.CheckNodeCollisions(chunk.Nodes, OnNodesCollision);
        }

        private void ContainNodeInMediums(Node node)
        {
            var mediumsToCheck = ChunksManager.GetChunksForNode(node).SelectMany(c => c.Mediums);
            //if (!CollisionChecker.IsNodeContainedInMediums(node, mediumsToCheck))
            //    throw new NotImplementedException("Perform containing Node inside Mediums"); // TODO
        }

        private void RegisterNodeTransfers(Node node, Chunk origin)
        {
            if (node.Disappearing)
            {
                nodeTransfers.Add(new NodeTransfer(node, NodeTransfer.TransferType.From, origin));
                return;
            }

            var stillInOrigin = false;
            foreach (var chunk in ChunksManager.GetChunksForNode(node))
            {
                if (chunk != origin)
                    nodeTransfers.Add(new NodeTransfer(node, NodeTransfer.TransferType.To, chunk));
                else
                    stillInOrigin = true;
            }
            if (!stillInOrigin)
                nodeTransfers.Add(new NodeTransfer(node, NodeTransfer.TransferType.From, origin));
        }

        private void PerformNodesTransfers()
        {
            nodeTransfers.ForEach(transfer => transfer.Perform());
            nodeTransfers.Clear();
        }

        private void OnNodesCollision(Node a, Node b)
        {
            a.OnCollision(b);
            b.OnCollision(a);
        }
    }
}