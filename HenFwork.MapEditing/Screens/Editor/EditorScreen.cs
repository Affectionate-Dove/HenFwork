﻿// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Screens;

namespace HenFwork.MapEditing.Screens.Editor
{
    /// <summary>
    ///     Used for editing a map. Allows the user
    ///     to select various tools and actions, and use
    ///     them on the map.
    /// </summary>
    public class EditorScreen<TInputAction> : Screen
    {
        private const float ACTIONS_TOOLBAR_SIZE = 40;
        private const float TOOLS_TOOLBAR_SIZE = 42;
        private const float toolbars_margins = 100;

        public EditorScreen() => CreateLayout();

        private void CreateLayout()
        {
            AddChild(new WorldEditViewer());

            AddChild(new ToolsToolbar<TInputAction>
            {
                RelativeSizeAxes = Axes.Y,
                Size = new(TOOLS_TOOLBAR_SIZE, 1),
                Padding = new MarginPadding { Vertical = toolbars_margins }
            });
            AddChild(new ActionsToolbar<TInputAction>
            {
                RelativeSizeAxes = Axes.X,
                Size = new(1, ACTIONS_TOOLBAR_SIZE),
                Padding = new MarginPadding { Horizontal = toolbars_margins }
            });
        }
    }
}