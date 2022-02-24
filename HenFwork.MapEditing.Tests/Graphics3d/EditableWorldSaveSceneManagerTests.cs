// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics3d;
using HenFwork.MapEditing.Graphics3d;
using HenFwork.MapEditing.Saves;
using HenFwork.MapEditing.Saves.Editable;
using HenFwork.MapEditing.Tests.Saves;
using HenFwork.Worlds.Functional.Mediums;
using NUnit.Framework;

namespace HenFwork.MapEditing.Tests.Graphics3d
{
    [TestOf(typeof(EditableWorldSaveSceneManager))]
    public class EditableWorldSaveSceneManagerTests
    {
        [Test(Description = $"Tests whether a correct amount of {nameof(Spatial)}s is added on initialization.")]
        public void CtorSpatialAmountTest()
        {
            var (chunk, _) = BuildEditableChunkSave();

            var editableWorldSave = new EditableWorldSave("world", new[] { chunk });

            var editableWorldSaveSceneManager = new EditableWorldSaveSceneManager(editableWorldSave);
            Assert.AreEqual(3, editableWorldSaveSceneManager.Scene.Spatials.Count);
        }

        [Test(Description = $"Adding and removing {nameof(NodeSave)}s and/or {nameof(MediumSave)} should increase and decrease {nameof(Spatial)} count.")]
        public void AddRemoveNodeTest()
        {
            var (chunk, nodesSerializer) = BuildEditableChunkSave();

            var editableWorldSave = new EditableWorldSave("world", new[] { chunk });

            var editableWorldSaveSceneManager = new EditableWorldSaveSceneManager(editableWorldSave);

            Assert.AreEqual(3, editableWorldSaveSceneManager.Scene.Spatials.Count);

            var nodeSave = nodesSerializer.Serialize(new TestNodeForSaving());
            chunk.Add(nodeSave);
            Assert.AreEqual(4, editableWorldSaveSceneManager.Scene.Spatials.Count);

            chunk.Remove(nodeSave);
            Assert.AreEqual(3, editableWorldSaveSceneManager.Scene.Spatials.Count);

            var mediumSave = CreateMedium();
            mediumSave = mediumSave with { Type = MediumType.Air };
            chunk.Add(mediumSave);
            Assert.AreEqual(4, editableWorldSaveSceneManager.Scene.Spatials.Count);

            chunk.Remove(mediumSave);
            Assert.AreEqual(3, editableWorldSaveSceneManager.Scene.Spatials.Count);
        }

        private static (EditableChunkSave, NodesSerializer) BuildEditableChunkSave()
        {
            var chunk = new EditableChunkSave(10, (0, 0));

            var nodesSerializer = new NodesSerializer();
            var node1 = new TestNodeForSaving { TestStringField = "h" };
            var node2 = new TestNodeForSaving { TestStringField = "H" };
            chunk.Add(nodesSerializer.Serialize(node1));
            chunk.Add(nodesSerializer.Serialize(node2));
            var medium = CreateMedium();
            chunk.Add(medium);

            return (chunk, nodesSerializer);
        }

        private static MediumSave CreateMedium() => new(new(new(1), new(2), new(3)), MediumType.Water);
    }
}