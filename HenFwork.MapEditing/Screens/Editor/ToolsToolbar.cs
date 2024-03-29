﻿// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.MapEditing.Screens.Editor.Tools;
using System.Collections.Generic;
using System.Linq;

namespace HenFwork.MapEditing.Screens.Editor
{
    /// <summary>
    ///     Contains selectable mapping tools.
    /// </summary>
    public class ToolsToolbar : Container
    {
        private const string cursor_img_path = @"Resources\Images\Tools\cursor.png";
        private const string add_img_path = @"Resources\Images\Tools\add.png";
        private const string rotate_img_path = @"Resources\Images\Tools\rotate.png";
        private const string resize_img_path = @"Resources\Images\Tools\resize.png";
        private static readonly string[] toolImgs = new[] { cursor_img_path, add_img_path, rotate_img_path, resize_img_path };

        private readonly List<ToolButton> toolbarButtons = new();

        public ToolsToolbar(ToolsManager toolsManager)
        {
            foreach (var toolImg in toolImgs.Where(toolImg => !Game.TextureStore.IsLoaded(toolImg)))
                Game.TextureStore.Load(toolImg).GenerateMipmaps();

            RelativeSizeAxes = Axes.Y;

            // background
            AddChild(new Rectangle
            {
                RelativeSizeAxes = Axes.Both,
                Color = new(64, 64, 64)
            });

            const float spacing = 4;
            var flowContainer = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding
                {
                    Horizontal = spacing,
                    Vertical = spacing,
                },
                Spacing = spacing,
                Direction = Direction.Vertical
            };
            AddChild(flowContainer);

            foreach (var toolImg in toolImgs)
            {
                var toolButton = new ToolButton(new Tool(toolImg), toolsManager);
                flowContainer.AddChild(toolButton);
                toolbarButtons.Add(toolButton);
            }

            toolsManager.SelectedToolChanged += OnSelectionToolChanged;
        }

        private void OnSelectionToolChanged(Tool? tool)
        {
            toolbarButtons.ForEach(b =>
            {
                b.MakeUnselected();
                if (tool is not null && b.Tool == tool)
                    b.MakeSelected();
            });
        }

        private class ToolButton : ToolbarButton
        {
            public Tool Tool { get; }

            public ToolButton(Tool tool, ToolsManager toolsManager)
            {
                Tool = tool;
                Sprite = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    AutoFillModeProportions = true,
                    Anchor = new(0.5f),
                    Origin = new(0.5f),
                    Texture = Game.TextureStore.Get(tool.TextureName),
                };
                Action = () => toolsManager.SelectedTool = tool;
            }
        }
    }
}