// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Numerics;
using HenFwork.Graphics2d;

namespace HenFwork.Worlds.Functional.Mediums
{
    public class Medium : IHasColor
    {
        public MediumType Type { get; set; }
        public Triangle3 Triangle { get; set; }

        public ColorInfo Color => GetTypeColor(Type);

        public static ColorInfo GetTypeColor(MediumType type) => type switch
        {
            MediumType.Ground => new(0, 255, 0),
            MediumType.Water => new(0, 100, 255),
            MediumType.Air => new(255, 255, 255, 50),
            _ => new(0, 0, 0, 50)
        };

        public override string ToString() => $"{{{nameof(Type)}={Type},{nameof(Triangle)}={Triangle}}}";
    }

    public enum MediumType
    {
        Ground,
        Water,
        Air
    }
}