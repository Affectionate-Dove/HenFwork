// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Graphics3d;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.UI;
using System.Numerics;

namespace HenFwork.VisualTests.Graphics3d
{
    public class ModelSpatialRotationsTestScene : VisualTestScene
    {
        /// <summary>
        ///     In degrees per second.
        /// </summary>
        private const int rotationVelocity = 150;

        private readonly ModelSpatial modelSpatial;
        private int yRotating;
        private int xRotating;
        private int zRotating;

        public ModelSpatialRotationsTestScene()
        {
            var scene = new Scene();
            scene.Spatials.Add(modelSpatial = new ModelSpatial { Model = Game.ModelStore.Get("Resources/Models/building_dock.obj") });
            var sceneViewerContainer = new Container { RelativeSizeAxes = Axes.Both, Padding = new MarginPadding { Top = 100 } };
            var sceneViewer = new SceneViewer(scene) { RelativeSizeAxes = Axes.Both };
            AddChild(sceneViewerContainer);
            sceneViewerContainer.AddChild(sceneViewer);

            sceneViewer.Camera.LookingAt = Vector3.Zero;
            sceneViewer.Camera.Position = new(0, 0.8f, -0.9f);

            AddInstructions();
        }

        public override bool OnActionPressed(SceneControls action)
        {
            switch (action)
            {
                case SceneControls.Left:
                    yRotating -= rotationVelocity;
                    return true;

                case SceneControls.Right:
                    yRotating += rotationVelocity;
                    return true;

                case SceneControls.Up:
                    xRotating -= rotationVelocity;
                    return true;

                case SceneControls.Down:
                    xRotating += rotationVelocity;
                    return true;

                case SceneControls.One:
                    zRotating -= rotationVelocity;
                    return true;

                case SceneControls.Two:
                    zRotating += rotationVelocity;
                    return true;
            }
            return base.OnActionPressed(action);
        }

        public override void OnActionReleased(SceneControls action)
        {
            switch (action)
            {
                case SceneControls.Left:
                    yRotating += rotationVelocity;
                    return;

                case SceneControls.Right:
                    yRotating -= rotationVelocity;
                    return;

                case SceneControls.Up:
                    xRotating += rotationVelocity;
                    return;

                case SceneControls.Down:
                    xRotating -= rotationVelocity;
                    return;

                case SceneControls.One:
                    zRotating += rotationVelocity;
                    return;

                case SceneControls.Two:
                    zRotating -= rotationVelocity;
                    return;
            }
            base.OnActionPressed(action);
        }

        protected override void OnUpdate(float elapsed)
        {
            modelSpatial.Rotation += new Vector3(xRotating, yRotating, zRotating) * elapsed;
            base.OnUpdate(elapsed);
        }

        private void AddInstructions()
        {
            AddChild(new SpriteText
            {
                Text = "Left/Right - Y axis\n" +
                "Up/Down - X axis\n" +
                "1/2 - Z axis",
                RelativeSizeAxes = Axes.Both,
                Offset = new(15)
            });
        }
    }
}