// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Extensions;
using HenFwork.Graphics2d;
using HenFwork.Graphics3d;
using HenFwork.MapEditing.Graphics3d;
using HenFwork.MapEditing.Saves.Editable;
using System;
using System.Numerics;

namespace HenFwork.MapEditing.Screens.Editor
{
    /// <summary>
    ///     Displays an editable view of a map.
    /// </summary>
    // TODO: For now just a placeholder displaying an object
    public class WorldEditViewer : Container
    {
        private Vector2 cameraOrbitAngle = new(-30, -45);

        public SceneViewer SceneViewer { get; }

        public Vector3 ObservedPoint { get; set; }

        public Vector2 CameraOrbitAngle
        {
            get => cameraOrbitAngle;
            set => cameraOrbitAngle = value with { X = Math.Clamp(value.X, -89.99f, 89.99f) };
        }

        public WorldEditViewer(EditableWorldSave editableWorldSave)
        {
            RelativeSizeAxes = Axes.Both;

            var editableWorldSaveSceneManager = new EditableWorldSaveSceneManager(editableWorldSave);
            AddChild(SceneViewer = new SceneViewer(editableWorldSaveSceneManager.Scene)
            {
                RelativeSizeAxes = Axes.Both
            });

            SceneViewer.Camera.LookingAt = ObservedPoint;
        }

        protected override void OnUpdate(float elapsed)
        {
            base.OnUpdate(elapsed);
            SceneViewer.Camera.LookingAt = ObservedPoint;

            SceneViewer.Camera.Position = new Vector3(0, 0, -5).GetRotated(new(CameraOrbitAngle, 0));
            SceneViewer.Camera.Position += ObservedPoint;
        }
    }
}