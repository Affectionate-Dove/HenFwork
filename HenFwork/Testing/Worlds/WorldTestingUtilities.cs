// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.Worlds;
using HenFwork.Worlds.Mediums;
using HenFwork.Worlds.Nodes;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.Testing.Worlds
{
    public static class WorldTestingUtilities
    {
        public static World GetExampleWorld()
        {
            var world = new World(new Vector2(10), 2);

            AddNodes(world);
            foreach (var medium in GetSampleMediums())
                world.AddMedium(medium);

            return world;
        }

        private static void AddNodes(World world)
        {
            world.AddNode(new TestNode(new(4, 0, 4)));
            world.AddNode(new TestNode(new(3, 4, 8)));
            world.AddNode(new TestNode(new(7, 2, 3)));
        }

        private static IEnumerable<Medium> GetSampleMediums() => new Medium[]
        {
            new Medium
            {
                Triangle = new Triangle3(new(0, 0, 10), new(5, 0, 0), new(5, 0, 10)),
                Type = MediumType.Water
            },
            new Medium
            {
                Triangle = new Triangle3(new(0, 0, 10), new(0, 0, 0), new(5, 0, 0)),
                Type = MediumType.Water
            },
            new Medium
            {
                Triangle = new Triangle3(new(5, 0, 0), new(7, 0, 0), new(7, 0, 10)),
                Type = MediumType.Ground
            },
            new Medium
            {
                Triangle = new Triangle3(new(5, 0, 0), new(7, 0, 10), new(5, 0, 10)),
                Type = MediumType.Ground
            },
        };

        public class TestNode : Node
        {
            public TestNode(Vector3 position) => Position = position;
        }
    }
}