// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.MapEditing.Saves;
using HenFwork.Worlds.Functional.Mediums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HenFwork.MapEditing.Tests.Saves
{
    [TestOf(typeof(ChunkSave))]
    public class ChunkSaveTests
    {
        public static IEnumerable<Case> Cases()
        {
            var nodesSerializer = new NodesSerializer();
            var node1 = new TestNodeForSaving { TestStringField = "node1" };
            var node2 = new TestNodeForSaving { TestStringProperty = "node2" };
            var nodeSave1 = nodesSerializer.Serialize(node1);
            var nodeSave2 = nodesSerializer.Serialize(node2);
            var nodeSaves = new[] { nodeSave1, nodeSave2 };
            var mediumSave1 = new MediumSave(new(new(1, 0, 0), new(0, 0, 0), new(0, 0, 1)), MediumType.Ground);
            var mediumSave2 = new MediumSave(new(new(1, 3, 4), new(0, 2, 5), new(0, 1, 1)), MediumType.Air);
            var mediumSaves = new[] { mediumSave1, mediumSave2 };
            yield return (new((3, 2), 10, nodeSaves, mediumSaves));
            //test empty nodes or mediums
            yield return (new((0, 0), 5, Array.Empty<NodeSave>(), Array.Empty<MediumSave>()));
        }

        public static void FromPropertiesTest(Case c)
        {
            var chunkSave = new ChunkSave(c.NodeSaves, c.MediumSaves, c.Index, c.Size);
            ValidateProperties(chunkSave, c);
        }

        public static void FromDataStringTest(Case c)
        {
            var dataString = new ChunkSave(c.NodeSaves, c.MediumSaves, c.Index, c.Size).ToDataString();
            var chunkSave = new ChunkSave(dataString);
            ValidateProperties(chunkSave, c);
        }

        [TestCaseSource(nameof(Cases))]
        public void Test(Case c)
        {
            FromPropertiesTest(c);
            FromDataStringTest(c);
        }

        private static void ValidateProperties(ChunkSave chunkSave, Case c)
        {
            for (var i = 0; i < c.NodeSaves.Length; i++)
                Assert.AreEqual(c.NodeSaves[i].ToStringData(), chunkSave.NodeSaves[i].ToStringData());

            for (var i = 0; i < c.MediumSaves.Length; i++)
                Assert.AreEqual(c.MediumSaves[i], chunkSave.MediumSaves[i]);

            Assert.AreEqual(c.Index, chunkSave.Index);
            Assert.AreEqual(c.Size, chunkSave.Size);
        }

        public class Case
        {
            public (int x, int y) Index;
            public int Size;
            public NodeSave[] NodeSaves;
            public MediumSave[] MediumSaves;

            public Case((int x, int y) index, int size, NodeSave[] nodeSaves, MediumSave[] mediumSaves)
            {
                Index = index;
                Size = size;
                NodeSaves = nodeSaves;
                MediumSaves = mediumSaves;
            }
        }
    }
}