// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Input.UI;
using HenFwork.MapEditing.Input;
using HenFwork.Screens;
using System;
using System.Numerics;

namespace HenFwork.MapEditing.Screens.Editor
{
    /// <summary>
    ///     Used for editing a map. Allows the user
    ///     to select various tools and actions, and use
    ///     them on the map.
    /// </summary>
    public class EditorScreen : Screen, IInterfaceComponent<EditorControls>
    {
        private const float ACTIONS_TOOLBAR_SIZE = 40;
        private const float TOOLS_TOOLBAR_SIZE = 42;
        private const float toolbars_margins = 100;
        private const float camera_speed = 1.2f;
        private WorldEditViewer worldEditViewer;
        private Vector3 cameraVelocity;

        public event Action<IInterfaceComponent<EditorControls>> FocusRequested
        { add { } remove { } }

        public bool AcceptsFocus => true;

        public EditorScreen() => CreateLayout();

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

        public void OnFocus()
        { }

        public void OnFocusLost()
        { }

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

        private void CreateLayout()
        {
            AddChild(worldEditViewer = new WorldEditViewer());

            AddChild(new ToolsToolbar
            {
                RelativeSizeAxes = Axes.Y,
                Size = new(TOOLS_TOOLBAR_SIZE, 1),
                Padding = new MarginPadding { Vertical = toolbars_margins }
            });
            AddChild(new ActionsToolbar
            {
                RelativeSizeAxes = Axes.X,
                Size = new(1, ACTIONS_TOOLBAR_SIZE),
                Padding = new MarginPadding { Horizontal = toolbars_margins }
            });
        }
    }
}