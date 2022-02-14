// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Extensions;
using HenBstractions.Graphics;
using HenBstractions.Numerics;
using HenFwork.Graphics2d;
using HenFwork.Graphics3d;
using HenFwork.Graphics3d.Shapes;
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

        public WorldEditViewer()
        {
            RelativeSizeAxes = Axes.Both;

            Scene scene;
            AddChild(SceneViewer = new SceneViewer(scene = new Scene())
            {
                RelativeSizeAxes = Axes.Both
            });

            var boxX = Box.FromPositionAndSize(Vector3.UnitX * 3, Vector3.One, new(0.5f));
            var boxY = Box.FromPositionAndSize(Vector3.UnitY * 3, Vector3.One, new(0.5f));
            var boxZ = Box.FromPositionAndSize(Vector3.UnitZ * 3, Vector3.One, new(0.5f));
            var cubeX = new BoxSpatial { Box = boxX, Color = ColorInfo.RED };
            var cubeY = new BoxSpatial { Box = boxY, Color = ColorInfo.GREEN };
            var cubeZ = new BoxSpatial { Box = boxZ, Color = ColorInfo.BLUE };

            const string model_path = "Resources/Models/building_dock.obj";
            if (!Game.ModelStore.IsLoaded(model_path))
            {
                Game.ModelStore.Load(model_path);
            }
            var sword = new ModelSpatial
            {
                Model = Game.ModelStore.Get(model_path),
                Scale = new Vector3(4),
                Rotation = new(0, -90, 0)
            };

            scene.Spatials.AddRange(new Spatial[] { cubeX, cubeY, cubeZ, sword });

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