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
    // TODO: For now just a placeholder displaying an object
    public class WorldEditViewer : Container
    {
        private readonly SceneViewer sceneViewer;
        private float yaw;
        private float pitch;

        public WorldEditViewer()
        {
            RelativeSizeAxes = Axes.Both;

            // by default, the camera should be at Z = -1,
            // so (0, 0, 1) rotated by y180
            yaw = 180;

            // look from a bit up
            pitch = 30;

            Scene scene;
            AddChild(sceneViewer = new SceneViewer(scene = new Scene())
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
                Scale = new Vector3(4)
            };

            scene.Spatials.AddRange(new Spatial[] { cubeX, cubeY, cubeZ, sword });

            sceneViewer.Camera.LookingAt = Vector3.UnitY;
        }

        protected override void OnUpdate(float elapsed)
        {
            base.OnUpdate(elapsed);
            yaw += 0.4f;

            pitch = MathF.Min(89.999f, MathF.Max(-89.999f, pitch));
            sceneViewer.Camera.Position = new Vector3(0, 0, 5).GetRotated(new(pitch, yaw, 0));
        }
    }
}