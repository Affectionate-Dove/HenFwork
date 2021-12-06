// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.Nodes;
using System.Collections.Generic;

namespace HenFwork.Worlds.Functional.PathFinding
{
    /// <summary>
    /// Represents a point in 3D space that
    /// can be connected to other points.
    /// </summary>
    public class PathNode : Node
    {
        private readonly HashSet<PathNode> connections = new(4);

        /// <remarks>Direct connections.</remarks>
        public IReadOnlySet<PathNode> Connections => connections;

        /// <summary>
        /// Creates a 2-way connection between this
        /// <see cref="PathNode"/> and the <paramref name="other"/>.
        /// </summary>
        public void ConnectSymmetrically(PathNode other)
        {
            connections.Add(other);
            other.connections.Add(this);
        }

        /// <summary>
        /// Creates a 1-way connection from this
        /// <see cref="PathNode"/> to the <paramref name="other"/>.
        /// </summary>
        /// <param name="forceAsymmetry">
        /// If true, if <paramref name="other"/> is connected to
        /// this <see cref="PathNode"/>, it will be disconnected
        /// to force a 1-way connection.
        /// If false, it's possible to have a 2-way connection
        /// between these points after this function.
        /// </param>
        public void ConnectAsymmetrically(PathNode other, bool forceAsymmetry = true)
        {
            connections.Add(other);
            if (forceAsymmetry)
                other.connections.Remove(this);
        }

        /// <summary>
        /// Removes a connection from this
        /// <see cref="PathNode"/> to the
        /// <paramref name="other"/>, and vice versa.
        /// </summary>
        public void DisconnectSymmetrically(PathNode other)
        {
            connections.Remove(other);
            other.connections.Remove(this);
        }

        /// <summary>
        /// Removes a connection from this
        /// <see cref="PathNode"/> to the
        /// <paramref name="other"/>, but leaves a
        /// connection from the <paramref name="other"/>
        /// to this <see cref="PathNode"/>, if it exists.
        /// </summary>
        public void DisconnectAsymmetrically(PathNode other) => connections.Remove(other);
    }
}