// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Input;
using HenFwork.Graphics2d;
using HenFwork.Input.UI;
using HenFwork.MapEditing.Input;
using System;
using System.Numerics;

namespace HenFwork.MapEditing.Screens.Editor
{
    public class EditorCameraMovementHandler : Container, IInterfaceComponent<EditorControls>, IPositionalInterfaceComponent
    {
        private readonly Rectangle focusBorder;
        private readonly WorldEditViewer worldEditViewer;
        private Vector3 cameraVelocity;
        private DraggingMode nextDraggingMode = DraggingMode.Orbit;
        private DraggingMode? currentDraggingMode;

        public event Action<IInterfaceComponent<EditorControls>> FocusRequested
        { add { } remove { } }

        public bool AcceptsFocus => true;

        bool IPositionalInterfaceComponent.AcceptsPositionalInput => true;

        public EditorCameraMovementHandler(WorldEditViewer worldEditViewer)
        {
            this.worldEditViewer = worldEditViewer;
            AddChild(focusBorder = new Rectangle()
            {
                BorderThickness = 3,
                RelativeSizeAxes = Axes.Both,
                Color = ColorInfo.BLANK
            });
        }

        public bool OnActionPressed(EditorControls action)
        {
            if (action is EditorControls.MoveCameraWithPositionalInput)
            {
                nextDraggingMode = DraggingMode.Move;
                return true;
            }

            var cameraVelocityDelta = ActionCameraVelocityChange(action);
            if (cameraVelocityDelta != Vector3.Zero)
            {
                cameraVelocity += cameraVelocityDelta;
                return true;
            }
            return false;
        }

        public void OnActionReleased(EditorControls action)
        {
            if (action is EditorControls.MoveCameraWithPositionalInput)
            {
                nextDraggingMode = DraggingMode.Orbit;
                return;
            }

            cameraVelocity -= ActionCameraVelocityChange(action);
        }

        public void OnFocus() => focusBorder.BorderColor = ColorInfo.ORANGE;

        public void OnFocusLost() => focusBorder.BorderColor = ColorInfo.BLANK;

        bool IPositionalInterfaceComponent.AcceptsPositionalButton(MouseButton button) => button is MouseButton.Middle;

        void IPositionalInterfaceComponent.OnHover()
        {
        }

        void IPositionalInterfaceComponent.OnHoverLost()
        {
        }

        void IPositionalInterfaceComponent.OnMousePress(MouseButton button) => currentDraggingMode = nextDraggingMode;

        void IPositionalInterfaceComponent.OnMouseRelease(MouseButton button) => currentDraggingMode = null;

        void IPositionalInterfaceComponent.OnClick(MouseButton button)
        {
        }

        void IPositionalInterfaceComponent.OnMouseDrag(MouseButton button, Vector2 delta)
        {
            // this maybe should be dependent on time
            if (currentDraggingMode is DraggingMode.Orbit)
            {
                var speed = 0.4f;
                worldEditViewer.CameraOrbitAngle += new Vector2(-delta.Y, delta.X) * speed;
            }
            else if (currentDraggingMode is DraggingMode.Move)
            {
                var speed = 0.01f;
                var mouseDelta3 = new Vector3(delta.X, delta.Y, 0);
                var moveDelta = Vector3.Transform(mouseDelta3, worldEditViewer.SceneViewer.Camera.Matrix) * speed;
                worldEditViewer.ObservedPoint -= moveDelta;
            }
        }

        protected override void OnUpdate(float elapsed)
        {
            base.OnUpdate(elapsed);
            worldEditViewer.ObservedPoint += cameraVelocity * elapsed;
        }

        private static Vector3 ActionCameraVelocityChange(EditorControls action)
        {
            var camera_speed = 5f;
            return action switch
            {
                EditorControls.MoveCameraForward => new Vector3(0, 0, camera_speed),
                EditorControls.MoveCameraBackward => new Vector3(0, 0, -camera_speed),
                EditorControls.MoveCameraLeft => new Vector3(-camera_speed, 0, 0),
                EditorControls.MoveCameraRight => new Vector3(camera_speed, 0, 0),
                EditorControls.MoveCameraUp => new Vector3(0, camera_speed, 0),
                EditorControls.MoveCameraDown => new Vector3(0, -camera_speed, 0),
                _ => Vector3.Zero
            };
        }

        private enum DraggingMode
        {
            Orbit,
            Move
        }
    }
}