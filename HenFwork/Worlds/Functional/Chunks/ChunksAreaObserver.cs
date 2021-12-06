// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Collections.Generic;

namespace HenFwork.Worlds.Functional.Chunks
{
    /// <summary>
    ///     Provides events about <see cref="Node"/>s and <see cref="Medium"/>s
    ///     entering or leaving the <see cref="ObservedArea"/>.
    /// </summary>
    public class ChunksAreaObserver
    {
        private readonly HashSet<Node> nodesInArea = new();
        private readonly HashSet<Medium> mediumsInArea = new();
        private readonly ISet<Chunk> observedChunks = new HashSet<Chunk>();
        private readonly IList<Chunk> chunksToStopObserving = new List<Chunk>();
        private readonly ChunksManager chunksManager;
        private RectangleF observedArea;

        public event Action<Node> NodeEnteredChunksArea;

        public event Action<Node> NodeLeftChunksArea;

        public event Action<Medium> MediumEnteredChunksArea;

        public event Action<Medium> MediumLeftChunksArea;

        public event Action<Chunk> BeganObservingChunk;

        public event Action<Chunk> StoppedObservingChunk;

        public IReadOnlySet<Node> NodesInArea => nodesInArea;
        public IReadOnlySet<Medium> MediumsInArea => mediumsInArea;

        public RectangleF ObservedArea
        {
            get => observedArea;
            set
            {
                if (observedArea.Equals(value))
                    return;

                observedArea = value;
                OnObservedAreaChanged();
            }
        }

        public ChunksAreaObserver(ChunksManager chunksManager) => this.chunksManager = chunksManager;

        private void OnObservedAreaChanged()
        {
            chunksToStopObserving.Clear();
            foreach (var chunk in observedChunks)
            {
                var intersection = chunk.Coordinates.GetIntersection(ObservedArea);
                if (intersection is null || intersection.Value.Area is 0)
                    chunksToStopObserving.Add(chunk);
            }

            foreach (var chunk in chunksToStopObserving)
                StopObservingChunk(chunk);

            foreach (var chunk in chunksManager.GetChunksForRectangle(ObservedArea))
            {
                if (observedChunks.Contains(chunk))
                    continue;

                BeginObservingChunk(chunk);
            }
        }

        private void StopObservingChunk(Chunk chunk)
        {
            observedChunks.Remove(chunk);
            chunk.NodeAdded -= OnNodeAddedToChunk;
            chunk.NodeRemoved -= OnNodeRemovedFromChunk;

            foreach (var node in chunk.Nodes)
                OnNodeRemovedFromChunk(node);

            foreach (var medium in chunk.Mediums)
                OnMediumRemovedFromChunk(medium);

            StoppedObservingChunk?.Invoke(chunk);
        }

        private void OnNodeAddedToChunk(Node node)
        {
            if (nodesInArea.Contains(node))
                return;

            nodesInArea.Add(node);
            NodeEnteredChunksArea?.Invoke(node);
        }

        private void OnMediumAddedToChunk(Medium medium)
        {
            if (mediumsInArea.Contains(medium))
                return;

            mediumsInArea.Add(medium);
            MediumEnteredChunksArea?.Invoke(medium);
        }

        private void OnNodeRemovedFromChunk(Node node)
        {
            foreach (var chunk in chunksManager.GetChunksForNode(node))
                if (observedChunks.Contains(chunk))
                    return;

            nodesInArea.Remove(node);
            NodeLeftChunksArea?.Invoke(node);
        }

        private void OnMediumRemovedFromChunk(Medium medium)
        {
            foreach (var chunk in chunksManager.GetChunksForMedium(medium))
                if (observedChunks.Contains(chunk))
                    return;

            mediumsInArea.Remove(medium);
            MediumLeftChunksArea?.Invoke(medium);
        }

        private void BeginObservingChunk(Chunk chunk)
        {
            observedChunks.Add(chunk);
            chunk.NodeAdded += OnNodeAddedToChunk;
            chunk.NodeRemoved += OnNodeRemovedFromChunk;

            foreach (var node in chunk.Nodes)
                OnNodeAddedToChunk(node);

            foreach (var medium in chunk.Mediums)
                OnMediumAddedToChunk(medium);

            BeganObservingChunk?.Invoke(chunk);
        }
    }
}