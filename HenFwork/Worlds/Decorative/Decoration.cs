// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.Worlds.Decorative
{
    public class Decoration : WorldObject
    {
        public Vector3 Scale { get; set; } = Vector3.One;

        public Vector3 Rotation { get; set; }

        public string? ModelName { get; set; }

        public Dictionary<string, string> Properties { get; init; }
    }
}