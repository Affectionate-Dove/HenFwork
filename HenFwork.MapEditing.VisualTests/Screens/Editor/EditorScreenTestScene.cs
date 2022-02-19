// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.MapEditing.Saves;
using HenFwork.MapEditing.Saves.Editable;
using HenFwork.MapEditing.Screens.Editor;
using HenFwork.Testing;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using System.Numerics;

namespace HenFwork.MapEditing.VisualTests.Screens.Editor
{
    public class EditorScreenTestScene : VisualTestScene
    {
        public EditorScreenTestScene()
        {
            var nodesSerializer = new NodesSerializer();

            var posX = Vector3.UnitX * 3;
            var posY = Vector3.UnitY * 3;
            var posZ = Vector3.UnitZ * 3;

            var nodeX = nodesSerializer.Serialize(new TestNode { Position = posX });
            var nodeY = nodesSerializer.Serialize(new TestNode { Position = posY });
            var nodeZ = nodesSerializer.Serialize(new TestNode { Position = posZ });

            var medium = new MediumSave(new(posX, posY, posZ), MediumType.Water);

            var chunkSave = new EditableChunkSave(new[] { nodeX, nodeY, nodeZ }, new[] { medium }, 10, (0, 0));
            var worldSave = new EditableWorldSave("Example world", new[] { chunkSave });

            AddChild(new EditorScreen(worldSave) { RelativeSizeAxes = Axes.Both });
        }

        private class TestNode : Node
        {
        }
    }
}