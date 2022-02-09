// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Numerics;
using HenFwork.Graphics3d;
using HenFwork.Graphics3d.Shapes;
using System.Numerics;

namespace HenFwork.MapEditing.Screens.Editor
{
    public class AxesViewer : SceneViewer
    {
        public Camera ParentCamera { get; set; }

        public AxesViewer(Camera parentCamera) : this(parentCamera, new AxesScene())
        {
        }

        protected AxesViewer(Camera parentCamera, Scene axesScene) : base(axesScene)
        {
            ParentCamera = parentCamera;
            Camera.LookingAt = Vector3.Zero;
            Camera.Perspective = CameraProjection.Orthographic;
            Camera.FovY = 10;
        }

        protected override void OnUpdate(float elapsed)
        {
            var parentCameraPositionDelta = ParentCamera.Position - ParentCamera.CalculateWhereLookingAt();
            if (parentCameraPositionDelta != Vector3.Zero)
                Camera.Position = Vector3.Normalize(parentCameraPositionDelta) * 50;
            base.OnUpdate(elapsed);
        }

        public class AxesScene : Scene
        {
            public AxesScene()
            {
                var distance = 3;
                AddSpheresForAxis(new(distance, 0, 0), ColorInfo.RED.MultiplyBrightness(0.92f));
                AddSpheresForAxis(new(0, distance, 0), ColorInfo.BLUE.MultiplyBrightness(0.92f));
                AddSpheresForAxis(new(0, 0, distance), ColorInfo.GREEN.MultiplyBrightness(0.92f));
            }

            private void AddSpheresForAxis(Vector3 positiveDirection, ColorInfo color)
            {
                var size = 2.4f;
                Spatials.Add(new SphereSpatial
                {
                    Sphere = new Sphere
                    {
                        CenterPosition = positiveDirection,
                        Diameter = size
                    },
                    Color = color
                });
                Spatials.Add(new SphereSpatial
                {
                    Sphere = new Sphere
                    {
                        CenterPosition = -positiveDirection,
                        Diameter = size
                    },
                    Color = color.WithAlpha(0.2f)
                });
            }
        }
    }
}