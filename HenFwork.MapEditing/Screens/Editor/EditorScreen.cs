// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Screens;

namespace HenFwork.MapEditing.Screens.Editor
{
    public class EditorScreen : Screen
    {
        private const float ACTIONS_TOOLBAR_SIZE = 40;
        private const float TOOLS_TOOLBAR_SIZE = 42;
        private const float toolbars_margins = 80;

        public EditorScreen() => CreateLayout();

        private void CreateLayout()
        {
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