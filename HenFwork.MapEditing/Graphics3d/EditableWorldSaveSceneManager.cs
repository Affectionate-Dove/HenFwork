// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Numerics;
using HenFwork.Graphics3d;
using HenFwork.Graphics3d.Shapes;
using HenFwork.MapEditing.Saves;
using HenFwork.MapEditing.Saves.Editable;
using HenFwork.MapEditing.Saves.PropertySerializers;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.MapEditing.Graphics3d
{
    /// <summary>
    ///     Provides and manages a <see cref="HenFwork.Graphics3d.Scene"/>
    ///     for a given <see cref="Saves.Editable.EditableWorldSave"/>.
    /// </summary>
    /// <remarks>
    ///     Automatically creates (and removes) <see cref="Spatial"/>s
    ///     for <see cref="NodeSave"/>s and <see cref="MediumSave"/>s
    ///     in the <see cref="EditableWorldSave"/> using functions assigned to
    ///     <see cref="NodeSpatialCreator"/> and <see cref="MediumSpatialCreator"/>.
    /// </remarks>
    public class EditableWorldSaveSceneManager
    {
        private static readonly Vector3Serializer vector3Serializer = new();
        private readonly Dictionary<NodeSave, Spatial> nodeSpatials = new();
        private readonly Dictionary<MediumSave, Spatial> mediumSpatials = new();

        public Scene Scene { get; } = new();

        public EditableWorldSave EditableWorldSave { get; }

        public Func<NodeSave, Spatial> NodeSpatialCreator { get; set; } = CreateNodeSpatial;
        public Func<MediumSave, Spatial> MediumSpatialCreator { get; set; } = CreateMediumSpatial;

        public EditableWorldSaveSceneManager(EditableWorldSave editableWorldSave)
        {
            EditableWorldSave = editableWorldSave;
            EditableWorldSave.ChunkAdded += OnChunkAdded;
            EditableWorldSave.ChunkRemoved += OnChunkRemoved;

            foreach (var chunk in EditableWorldSave)
                OnChunkAdded(chunk);
        }

        private static Spatial CreateMediumSpatial(MediumSave mediumSave)
        {
            var color = Medium.GetTypeColor(mediumSave.Type);
            return new TriangleSpatial
            {
                Triangle = mediumSave.Triangle,
                Color = color.WithAlpha(0.1f),
                WireColor = color
            };
        }

        private static Spatial CreateNodeSpatial(NodeSave node) => new BoxSpatial
        {
            Box = Box.FromPositionAndSize(Vector3.Zero, Vector3.One, new(0.5f)),
            Color = ColorInfo.RED,
            Position = (Vector3)vector3Serializer.Deserialize(node.MembersValues[nameof(Node.Position)])
        };

        private void OnChunkRemoved(EditableChunkSave editableChunkSave)
        {
            editableChunkSave.NodeAdded -= OnNodeAdded;
            editableChunkSave.NodeRemoved -= OnNodeRemoved;
            editableChunkSave.MediumAdded -= OnMediumAdded;
            editableChunkSave.MediumRemoved -= OnMediumRemoved;

            foreach (var nodeSave in (IEnumerable<NodeSave>)editableChunkSave)
                OnNodeRemoved(nodeSave);
            foreach (var mediumSave in (IEnumerable<MediumSave>)editableChunkSave)
                OnMediumRemoved(mediumSave);
        }

        private void OnChunkAdded(EditableChunkSave editableChunkSave)
        {
            editableChunkSave.NodeAdded += OnNodeAdded;
            editableChunkSave.NodeRemoved += OnNodeRemoved;
            editableChunkSave.MediumAdded += OnMediumAdded;
            editableChunkSave.MediumRemoved += OnMediumRemoved;

            foreach (var node in editableChunkSave.Nodes)
                OnNodeAdded(node);

            foreach (var medium in editableChunkSave.Mediums)
                OnMediumAdded(medium);
        }

        private void OnMediumRemoved(MediumSave mediumSave)
        {
            var spatial = mediumSpatials[mediumSave];
            mediumSpatials.Remove(mediumSave);
            Scene.Spatials.Remove(spatial);
        }

        private void OnMediumAdded(MediumSave mediumSave)
        {
            var spatial = CreateMediumSpatial(mediumSave);
            mediumSpatials.Add(mediumSave, spatial);
            Scene.Spatials.Add(spatial);
        }

        private void OnNodeRemoved(NodeSave nodeSave)
        {
            var spatial = nodeSpatials[nodeSave];
            nodeSpatials.Remove(nodeSave);
            Scene.Spatials.Remove(spatial);
        }

        private void OnNodeAdded(NodeSave nodeSave)
        {
            var spatial = CreateNodeSpatial(nodeSave);
            nodeSpatials.Add(nodeSave, spatial);
            Scene.Spatials.Add(spatial);
        }
    }
}