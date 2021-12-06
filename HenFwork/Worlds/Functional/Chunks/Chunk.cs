// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.Worlds.Functional.Chunks
{
    /// <summary>
    /// Represents a part of a <see cref="NodeWorld"/>
    /// that can be simulated on its own.
    /// </summary>
    public class Chunk
    {
        private readonly List<Medium> mediumsList = new();

        // Nodes need to be stored both in a list and in a hashset.
        // Publicly exposing them in a list is needed for
        // collision checking functions, to avoid casting hashset to list.
        // HashSet is needed to avoid having to check whether a node
        // is already present using List.Contains, which is slow,
        // and it also avoids having to store the presence of each node
        // elsewhere.
        private readonly List<Node> nodesList = new();

        private readonly HashSet<Node> nodesHashSet = new();

        public event Action<Node> NodeAdded;

        public event Action<Node> NodeRemoved;

        /// <summary>
        /// All <see cref="Medium"/>s that are at least
        /// partly contained inside this <see cref="Chunk"/>'s
        /// <see cref="Coordinates"/>.
        /// </summary>
        public IReadOnlyList<Medium> Mediums => mediumsList;

        /// <summary>
        /// All <see cref="Node"/>s with <see cref="Node.Position"/>
        /// inside this <see cref="Chunk"/>'s <see cref="Coordinates"/>.
        /// </summary>
        public IReadOnlyList<Node> Nodes => nodesList;

        public Vector2 Index { get; }
        public RectangleF Coordinates { get; }

        public double SynchronizedTime { get; private set; }

        public Chunk(Vector2 index, float size)
        {
            Index = index;
            Coordinates = RectangleF.FromPositionAndSize(index * size, new(size), CoordinateSystem2d.YUp);
        }

        /// <summary>
        /// Simulates <see cref="Nodes"/> in this <see cref="Chunk"/>.
        /// </summary>
        /// <returns>
        /// Each simulated <see cref="Node"/>
        /// after the process of simulation.
        /// </returns>
        public IEnumerable<Node> Simulate(double newTime)
        {
            if (newTime < SynchronizedTime)
                throw new ArgumentOutOfRangeException(nameof(newTime), $"New time has to be greater than or equal to {nameof(SynchronizedTime)}");

            foreach (var node in Nodes)
            {
                node.Simulate(newTime);
                yield return node;
            }
            SynchronizedTime = newTime;
        }

        public void AddMedium(Medium medium) => mediumsList.Add(medium);

        public void AddNode(Node node)
        {
            if (nodesHashSet.Add(node))
                nodesList.Add(node);
            // only add node to list if it isn't already in this chunk

            NodeAdded?.Invoke(node);
        }

        public void RemoveMedium(Medium medium)
        {
            if (!mediumsList.Remove(medium))
                throw new InvalidOperationException($"The {nameof(medium)} wasn't in this chunk.");
        }

        public void RemoveNode(Node node)
        {
            if (!nodesList.Remove(node))
                throw new InvalidOperationException($"The {nameof(node)} wasn't in this chunk.");
            if (!nodesHashSet.Remove(node))
                throw new InvalidOperationException($"The {nameof(node)} was in {nameof(nodesList)} but wasn't in {nameof(nodesHashSet)}.");

            NodeRemoved?.Invoke(node);
        }
    }
}