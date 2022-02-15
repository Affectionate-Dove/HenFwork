// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.MapEditing.Saves;
using HenFwork.MapEditing.Saves.Editable;
using HenFwork.Worlds.Functional.Mediums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace HenFwork.MapEditing.Tests.Saves.Editable
{
    [TestOf(typeof(EditableChunkSave))]
    public class EditableChunkSaveTests
    {
        private readonly NodesSerializer nodesSerializer = new();
        private List<NodeSave> nodeSaves;
        private EditableChunkSave editableChunkSave;

        [SetUp]
        public void SetUp()
        {
            nodeSaves = new List<NodeSave>();
            var node = new TestNodeForSaving();
            nodeSaves.Add(nodesSerializer.Serialize(node));
            var mediumSaves = new List<MediumSave>
            {
                new MediumSave(new Triangle3(new(1), new(2), new(3)), MediumType.Air)
            };

            editableChunkSave = new EditableChunkSave(nodeSaves, mediumSaves, 10, (1, 2));
        }

        [Test]
        public void ToAndFromChunkSaveTest()
        {
            var chunkSave = editableChunkSave.ToChunkSave();
            var editableChunkSave2 = new EditableChunkSave(chunkSave);

            Assert.AreEqual(editableChunkSave.Count, editableChunkSave2.Count);
            foreach (var nodeSave in editableChunkSave)
            {
                Assert.AreEqual(1, ((IEnumerable<NodeSave>)editableChunkSave2)
                    .Count(nodeSave2 => nodeSave.Equals(nodeSave2)));
            }

            foreach (var mediumSave in (IEnumerable<MediumSave>)editableChunkSave)
            {
                Assert.AreEqual(1, ((IEnumerable<MediumSave>)editableChunkSave2)
                    .Count(mediumSave2 => mediumSave.Equals(mediumSave2)));
            }
        }

        [Test]
        public void EventsTest()
        {
            var nodeAddedFired = false;
            var nodeRemovedFired = false;
            var mediumAddedFired = false;
            var mediumRemovedFired = false;

            editableChunkSave.NodeAdded += n => nodeAddedFired = true;
            editableChunkSave.NodeRemoved += n => nodeRemovedFired = true;

            editableChunkSave.MediumAdded += n => mediumAddedFired = true;
            editableChunkSave.MediumRemoved += n => mediumRemovedFired = true;

            var nodeSave = nodesSerializer.Serialize(new TestNodeForSaving());
            var mediumSave = new MediumSave(new(new(1), new(2), new(3)), MediumType.Water);

            editableChunkSave.Add(nodeSave);
            Assert.True(nodeAddedFired);

            Assert.True(editableChunkSave.Remove(nodeSave));
            Assert.True(nodeRemovedFired);

            nodeRemovedFired = false;
            Assert.False(editableChunkSave.Remove(nodeSave));
            Assert.False(nodeRemovedFired);

            //

            editableChunkSave.Add(mediumSave);
            Assert.True(mediumAddedFired);

            Assert.True(editableChunkSave.Remove(mediumSave));
            Assert.True(mediumRemovedFired);

            mediumRemovedFired = false;
            Assert.False(editableChunkSave.Remove(mediumSave));
            Assert.False(mediumRemovedFired);
        }
    }
}