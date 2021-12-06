// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System.Numerics;

namespace HenFwork.Worlds
{
    public abstract class WorldObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Vector3 Position { get; set; }
    }
}