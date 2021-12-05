// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds;
using HenFwork.Worlds.Mediums;

namespace HenFwork.Testing.Worlds
{
    public class SampleWorld : World
    {
        public SampleWorld()
        {
            AddMedium(new Medium
            {
                Type = MediumType.Ground,
                Triangle = new(new(0, 0, 0), new(50, 0, 0), new(0, 0, 50))
            });
            AddMedium(new Medium
            {
                Type = MediumType.Ground,
                Triangle = new(new(50, 0, 50), new(0, 0, 50), new(50, 0, 0))
            });
        }
    }
}