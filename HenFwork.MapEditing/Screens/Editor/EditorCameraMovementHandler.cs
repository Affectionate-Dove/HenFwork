// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Input.UI;
using HenFwork.MapEditing.Input;
using System;
using System.Numerics;

namespace HenFwork.MapEditing.Screens.Editor
{
    public class EditorCameraMovementHandler : Container, IInterfaceComponent<EditorControls>
    {
        private const float camera_speed = 1.2f;
        private readonly Graphics2d.Rectangle focusBorder;
        private Vector3 cameraVelocity;
        private WorldEditViewer worldEditViewer;

        public event Action<IInterfaceComponent<EditorControls>> FocusRequested
        { add { } remove { } }

        public bool AcceptsFocus => true;

        public EditorCameraMovementHandler(WorldEditViewer worldEditViewer)
        {
            this.worldEditViewer = worldEditViewer;
            AddChild(focusBorder = new Graphics2d.Rectangle()
            {
                BorderThickness = 3,
                RelativeSizeAxes = Axes.Both,
                Color = ColorInfo.BLANK
            });
        }

        public bool OnActionPressed(EditorControls action)
        {
            var cameraVelocityDelta = ActionCameraVelocityChange(action);
            if (cameraVelocityDelta != Vector3.Zero)
            {
                cameraVelocity += cameraVelocityDelta;
                return true;
            }
            return false;
        }

        public void OnActionReleased(EditorControls action) => cameraVelocity -= ActionCameraVelocityChange(action);

        public void OnFocus() => focusBorder.BorderColor = ColorInfo.ORANGE;

        public void OnFocusLost() => focusBorder.BorderColor = ColorInfo.BLANK;

        protected override void OnUpdate(float elapsed)
        {
            base.OnUpdate(elapsed);
            worldEditViewer.ObservedPoint += cameraVelocity * elapsed;
        }

        private static Vector3 ActionCameraVelocityChange(EditorControls action)
        {
            return action switch
            {
                EditorControls.MoveForward => new Vector3(0, 0, camera_speed),
                EditorControls.MoveBackward => new Vector3(0, 0, -camera_speed),
                EditorControls.MoveLeft => new Vector3(-camera_speed, 0, 0),
                EditorControls.MoveRight => new Vector3(camera_speed, 0, 0),
                EditorControls.MoveUp => new Vector3(0, camera_speed, 0),
                EditorControls.MoveDown => new Vector3(0, -camera_speed, 0),
                _ => Vector3.Zero
            };
        }
    }
}