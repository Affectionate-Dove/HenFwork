// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using System.Collections.Generic;

namespace HenFwork.MapEditing.Screens.Editor
{
    public class ActionsToolbar : Container
    {
        public ActionsToolbar()
        {
            RelativeSizeAxes = Axes.X;

            // background
            AddChild(new Rectangle
            {
                RelativeSizeAxes = Axes.Both,
                Color = new(128, 128, 128)
            });

            const float spacing = 3;
            var flowContainer = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding
                {
                    Horizontal = spacing,
                    Vertical = spacing,
                },
                Spacing = spacing,
                Direction = Direction.Horizontal
            };
            AddChild(flowContainer);

            foreach (var sampleButton in CreatePlaceholderActionButtons())
                flowContainer.AddChild(sampleButton);
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
    }
}