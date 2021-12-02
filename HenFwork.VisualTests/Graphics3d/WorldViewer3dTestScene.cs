// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Extensions;
using HenBstractions.Graphics;
using HenBstractions.Numerics;
using HenFwork.Graphics3d;
using HenFwork.Graphics3d.Shapes;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using System;
using System.Numerics;

namespace HenFwork.VisualTests.Graphics3d
{
    public class WorldViewer3dTestScene : VisualTestScene
    {
        private readonly SceneViewer sceneViewer;
        private readonly Scene scene;
        private float yaw;
        private float targetYaw;
        private float pitch;
        private float targetPitch;

        public WorldViewer3dTestScene()
        {
            // by default, the camera should be at Z = -1,
            // so (0, 0, 1) rotated by y180
            targetYaw = yaw = 180;

            // look from a bit up
            targetPitch = pitch = 10;

            Padding = new HenFwork.Graphics2d.MarginPadding { Vertical = 50, Horizontal = 50 };
            AddChild(sceneViewer = new SceneViewer(scene = new Scene())
            {
                RelativeSizeAxes = HenFwork.Graphics2d.Axes.Both
            });

            var boxX = Box.FromPositionAndSize(Vector3.UnitX * 3, Vector3.One, new(0.5f));
            var boxY = Box.FromPositionAndSize(Vector3.UnitY * 3, Vector3.One, new(0.5f));
            var boxZ = Box.FromPositionAndSize(Vector3.UnitZ * 3, Vector3.One, new(0.5f));
            var cubeX = new BoxSpatial { Box = boxX, Color = ColorInfo.RED };
            var cubeY = new BoxSpatial { Box = boxY, Color = ColorInfo.GREEN };
            var cubeZ = new BoxSpatial { Box = boxZ, Color = ColorInfo.BLUE };

            var sword = new ModelSpatial
            {
                Model = Game.ModelStore.Get("Resources/Models/building_dock.obj"),
                Scale = new Vector3(4)
            };

            scene.Spatials.AddRange(new Spatial[] { cubeX, cubeY, cubeZ, sword });

            sceneViewer.Camera.LookingAt = Vector3.Zero;
            sceneViewer.Camera.Position = new(0, 1, -5);
        }

        public override bool OnActionPressed(SceneControls action)
        {
            targetYaw += action switch
            {
                SceneControls.Left => 30,
                SceneControls.Right => -30,
                _ => 0
            };
            targetPitch += action switch
            {
                SceneControls.Up => 30,
                SceneControls.Down => -30,
                _ => 0
            };
            targetPitch = MathF.Min(90, MathF.Max(-90, targetPitch));
            return base.OnActionPressed(action);
        }

        protected override void OnUpdate(float elapsed)
        {
            base.OnUpdate(elapsed);
            if (targetYaw > yaw)
                yaw += 1;
            else if (targetYaw < yaw)
                yaw -= 1;

            if (targetPitch > pitch + 1)
                pitch += 1;
            else if (targetPitch < pitch - 1)
                pitch -= 1;
            pitch = MathF.Min(89.999f, MathF.Max(-89.999f, pitch));
            sceneViewer.Camera.Position = new Vector3(0, 0, 5).GetRotated(new(pitch, yaw, 0));
        }
    }
}