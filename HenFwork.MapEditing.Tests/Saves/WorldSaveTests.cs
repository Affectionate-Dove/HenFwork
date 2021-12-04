// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.MapEditing.Saves;
using HenFwork.Worlds.Mediums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HenFwork.MapEditing.Tests.Saves
{
    [TestOf(typeof(WorldSave))]
    public class WorldSaveTests
    {
        public static IEnumerable<ChunkSave[]> Cases()
        {
            var nodesSerializer = new NodesSerializer();
            var node1 = new TestNodeForSaving { TestStringField = "node1" };
            var node2 = new TestNodeForSaving { TestStringProperty = "node2" };
            var node3 = new TestNodeForSaving { TestStringField = "node3" };
            var node4 = new TestNodeForSaving { TestStringProperty = "node4" };
            var nodeSave1 = nodesSerializer.Serialize(node1);
            var nodeSave2 = nodesSerializer.Serialize(node2);
            var nodeSave3 = nodesSerializer.Serialize(node3);
            var nodeSave4 = nodesSerializer.Serialize(node4);

            var nodeSaves1 = new[] { nodeSave1, nodeSave2 };
            var nodeSaves2 = new[] { nodeSave3 };
            var nodeSaves3 = new[] { nodeSave4 };
            var nodeSaves4 = Array.Empty<NodeSave>();

            var mediumSave1 = new MediumSave(new(new(1, 0, 0), new(0, 0, 0), new(0, 0, 1)), MediumType.Ground);
            var mediumSave2 = new MediumSave(new(new(1, 3, 4), new(0, 2, 5), new(0, 1, 1)), MediumType.Air);
            var mediumSaves = new[] { mediumSave1, mediumSave2 };

            var chunkSizes = 10;
            var chunk1 = new ChunkSave(nodeSaves1, mediumSaves, (0, 0), chunkSizes);
            var chunk2 = new ChunkSave(nodeSaves2, new List<MediumSave>(), (0, 1), chunkSizes);
            var chunk3 = new ChunkSave(nodeSaves3, new List<MediumSave>(), (1, 0), chunkSizes);
            var chunk4 = new ChunkSave(nodeSaves4, new List<MediumSave>(), (1, 1), chunkSizes);
            yield return new[] { chunk1, chunk2, chunk3, chunk4 };

            yield return Array.Empty<ChunkSave>();
        }

        [TestCaseSource(nameof(Cases))]
        public void FromPropertiesTest(ChunkSave[] chunkSaves)
        {
            var worldSave = new WorldSave(chunkSaves);
            ValidateProperties(worldSave, chunkSaves);
        }

        [TestCaseSource(nameof(Cases))]
        public void FromDataStringTest(ChunkSave[] chunkSaves)
        {
            var dataString = new WorldSave(chunkSaves).ToDataString();
            var worldSave = new WorldSave(dataString);
            ValidateProperties(worldSave, chunkSaves);
        }

        private static void ValidateProperties(WorldSave worldSave, ChunkSave[] chunkSaves)
        {
            for (var i = 0; i < chunkSaves.Length; i++)
                Assert.AreEqual(chunkSaves[i].ToDataString(), worldSave.ChunkSaves[i].ToDataString());
        }
    }
}