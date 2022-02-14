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
        private bool consideringPositionalInput;
        private bool draggingAllowed;

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
                consideringPositionalInput = true;
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
                consideringPositionalInput = false;
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

        void IPositionalInterfaceComponent.OnMousePress(MouseButton button)
        {
            if (consideringPositionalInput)
                draggingAllowed = true;
        }

        void IPositionalInterfaceComponent.OnMouseRelease(MouseButton button) => draggingAllowed = false;

        void IPositionalInterfaceComponent.OnClick(MouseButton button)
        {
        }

        void IPositionalInterfaceComponent.OnMouseDrag(MouseButton button, Vector2 delta)
        {
            if (!draggingAllowed)
                return;

            // also this maybe should be dependent on time

            var speed = 0.01f;
            var mouseDelta3 = new Vector3(delta.X, delta.Y, 0);
            var moveDelta = Vector3.Transform(mouseDelta3, worldEditViewer.SceneViewer.Camera.Matrix) * speed;
            worldEditViewer.ObservedPoint -= moveDelta;
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
    }
}