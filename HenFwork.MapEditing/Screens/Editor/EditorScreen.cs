// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Screens;
using System.Collections.Generic;

namespace HenFwork.MapEditing.Screens.Editor
{
    public class EditorScreen : Screen
    {
        private const float ACTIONS_TOOLBAR_SIZE = 40;
        private const float TOOLS_TOOLBAR_SIZE = 50;
        private const string cursor_img_path = @"Resources\Images\Tools\cursor.png";
        private const string add_img_path = @"Resources\Images\Tools\add.png";
        private const string rotate_img_path = @"Resources\Images\Tools\rotate.png";
        private const string resize_img_path = @"Resources\Images\Tools\resize.png";
        private static readonly string[] toolImgs = new[] { cursor_img_path, add_img_path, rotate_img_path, resize_img_path };

        public EditorScreen()
        {
            // TODO: this should be simpler + Screen.OnExit() should exist
            foreach (var toolImg in toolImgs)
            {
                if (!Game.TextureStore.IsLoaded(toolImg))
                    Game.TextureStore.Load(toolImg).GenerateMipmaps();
            }
            CreateLayout();
        }

        private static Container CreateToolsToolbar()
        {
            var container = new Container
            {
                RelativeSizeAxes = Axes.Y,
                Size = new(TOOLS_TOOLBAR_SIZE, 1),
                Padding = new() { Top = ACTIONS_TOOLBAR_SIZE }
            };

            // background
            container.AddChild(new Rectangle
            {
                RelativeSizeAxes = Axes.Both,
                Color = new(64, 64, 64)
            });

            const float spacing = 6;
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
            container.AddChild(flowContainer);

            foreach (var sampleButton in CreatePlaceholderTools())
                flowContainer.AddChild(sampleButton);

            return container;
        }

        private static Container CreateActionsToolbar()
        {
            var container = new Container
            {
                RelativeSizeAxes = Axes.X,
                Size = new(1, ACTIONS_TOOLBAR_SIZE)
            };

            // background
            container.AddChild(new Rectangle
            {
                RelativeSizeAxes = Axes.Both,
                Color = new(128, 128, 128)
            });

            const float spacing = 2;
            var flowContainer = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding
                {
                    Horizontal = spacing,
                    Vertical = spacing,
                    Left = TOOLS_TOOLBAR_SIZE + spacing
                },
                Spacing = spacing,
                Direction = Direction.Horizontal
            };
            container.AddChild(flowContainer);

            foreach (var sampleButton in CreatePlaceholderActionButtons())
                flowContainer.AddChild(sampleButton);

            return container;
        }

        private static IEnumerable<Drawable> CreatePlaceholderTools()
        {
            static Drawable createPlaceholderTool(string imgPath)
            {
                var container = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                };

                container.AddChild(new Rectangle
                {
                    RelativeSizeAxes = Axes.Both,
                    Color = new(150, 150, 150, 255),
                    BorderColor = new(255, 255, 255, 90),
                    BorderThickness = 3
                });

                container.AddChild(new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    AutoFillModeProportions = true,
                    Size = new(0.6f),
                    Anchor = new(0.5f),
                    Origin = new(0.5f),
                    Texture = Game.TextureStore.Get(imgPath)
                });

                return container;
            }

            foreach (var toolImg in toolImgs)
                yield return createPlaceholderTool(toolImg);
        }

        private static IEnumerable<Rectangle> CreatePlaceholderActionButtons()
        {
            var colors = new ColorInfo[]
            {
                ColorInfo.ORANGE,
                ColorInfo.VIOLET,
                new(0, 122, 32),
                new(0, 51, 122),
                new(122, 0, 76),
                new(100, 122, 0),
                ColorInfo.MAROON,
                ColorInfo.DARKPURPLE
            };

            foreach (var color in colors)
            {
                yield return new Rectangle
                {
                    FillMode = FillMode.Fit,
                    RelativeSizeAxes = Axes.Both,
                    BorderThickness = 3,
                    BorderColor = new(255, 255, 255, 200), //new(200, 200, 200),
                    Color = color
                };
            }
        }

        private void CreateLayout()
        {
            AddChild(CreateToolsToolbar());
            AddChild(CreateActionsToolbar());
        }
    }
}