// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Random;
using System.Collections.Generic;

namespace HenFwork.MapEditing.Screens.Editor
{
    /// <summary>
    ///     Contains (quick?) actions for the currently selected mapping tool.
    /// </summary>
    /// <remarks>
    ///     As an example, for the "add" tool, the actions could be
    ///     recently added objects.
    /// </remarks>
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

        private static IEnumerable<Drawable> CreatePlaceholderActionButtons()
        {
            var baseColors = new ColorInfo[]
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

            const int actions_count = 20;
            var colors = new List<ColorInfo>(actions_count);
            for (var i = 0; i < actions_count; i++)
                colors.Add(baseColors[RNG.GetIntBelow(baseColors.Length)]);

            foreach (var color in colors)
            {
                yield return new ToolbarButton
                {
                    FillMode = FillMode.Fit,
                    RelativeSizeAxes = Axes.Both,
                    BorderThickness = 3,
                    EnabledColors = new(color.MultiplyBrightness(0.5f), new(255, 255, 255, 200), null),
                };
            }
        }
    }
}