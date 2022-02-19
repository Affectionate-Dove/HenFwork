// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Collisions;
using System;

namespace HenFwork.Worlds.Functional.Nodes
{
    /// <summary>
    ///     Represents anything in a <see cref="NodeWorld"/>,
    ///     from a simple information, to a physical object.
    /// </summary>
    public abstract class Node : WorldObject
    {
        /// <summary>
        ///     Called when a new Node was created
        ///     that should be placed into environment.
        /// </summary>
        public event Action<Node> NodeEjected;

        [Saveable]
        public CollisionBody CollisionBody { get; set; }

        /// <summary>
        ///     The time at the beginning of
        ///     the latest started simulation.
        /// </summary>
        [Saveable]
        public double SynchronizedTime { get; private set; }

        public virtual Action Interaction { get; }

        /// <summary>
        ///     Whether this node is in the process
        ///     of disappearing from a <see cref="NodeWorld"/>.
        ///     This means that after the next simulation step of a chunk,
        ///     it will be removed from it.
        /// </summary>
        [Saveable]
        public bool Disappearing { get; private set; }

        public virtual void OnCollision(Node other)
        {
        }

        /// <summary>
        ///     Updates <see cref="SynchronizedTime"/>
        ///     and calls <see cref="Simulation(double)"/>
        ///     if <paramref name="newTime"/> is greater than
        ///     <see cref="SynchronizedTime"/>.
        ///     If these are equal, does nothing.
        /// </summary>
        /// <param name="newTime">
        ///     Has to be greater than or equal to
        ///     <see cref="SynchronizedTime"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="newTime"/> is lesser than
        ///     <see cref="SynchronizedTime"/>.
        /// </exception>
        public void Simulate(double newTime)
        {
            if (newTime < SynchronizedTime)
                throw new ArgumentOutOfRangeException(nameof(newTime), $"Must be greater than or equal to {SynchronizedTime}");

            if (newTime == SynchronizedTime)
                return;

            var duration = newTime - SynchronizedTime;
            SynchronizedTime = newTime;
            Simulation(duration);
        }

        public override string ToString() => $"{GetType().Name}:{{" +
            $"{nameof(Id)}:{Id}," +
            $"{nameof(Position)}:{Position}," +
            $"{nameof(CollisionBody)}:{CollisionBody}" +
            $"}}";

        /// <summary>
        ///     Calls <see cref="NodeEjected"/>.
        /// </summary>
        /// <param name="node">
        ///     The node that should be ejected.
        /// </param>
        protected void EjectNode(Node node) => NodeEjected?.Invoke(node);

        /// <summary>
        ///     Sets the <see cref="Disappearing"/> property to true.
        ///     This doesn't mean that this node is immediately gone
        ///     from a <see cref="NodeWorld"/>/<see cref="Chunks.Chunk"/>.
        /// </summary>
        protected void Disappear() => Disappearing = true;

        protected virtual void Simulation(double duration)
        {
        }
    }
}