// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Generic;

namespace HenFwork.Worlds.Decorative
{
    /// <summary>
    ///     A world that only has decorative elements.
    /// </summary>
    public class DecorativeWorld
    {
        public string Name { get; set; }

        public List<Decoration> Decorations { get; init; } = new();
    }
}