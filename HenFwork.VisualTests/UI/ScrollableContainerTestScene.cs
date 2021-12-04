// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.UI;
using System.Collections.Generic;

namespace HenFwork.VisualTests.UI
{
    public class ScrollableContainerTestScene : VisualTestScene
    {
        private readonly ScrollableContainer<SceneControls> scrollableContainer;
        private readonly FillFlowContainer flow;

        public override Dictionary<List<SceneControls>, string> ControlsDescriptions { get; } = new Dictionary<List<SceneControls>, string>
        {
            { new List<SceneControls> { SceneControls.Up, SceneControls.Left }, "Scroll backwards" },
            { new List<SceneControls> { SceneControls.Down, SceneControls.Right }, "Scroll forwards" },
            { new List<SceneControls> { SceneControls.One }, "Decrease container size" },
            { new List<SceneControls> { SceneControls.Two }, "Increase container size" },
            { new List<SceneControls> { SceneControls.Three }, "Change scrolling direction" },
        };

        public ScrollableContainerTestScene()
        {
            AddChild(scrollableContainer = new ScrollableContainer<SceneControls>
            {
                Size = new(200),
                Offset = new(200),
                Direction = Direction.Vertical
            });
            flow = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Spacing = 20,
                Direction = Direction.Vertical,
                Padding = new() { Vertical = 20, Horizontal = 20 }
            };
            scrollableContainer.AddChild(flow);
            scrollableContainer.BackgroundContainer.AddChild(new Rectangle { RelativeSizeAxes = Axes.Both, Color = ColorInfo.GRAY });
            var colors = new ColorInfo[] { ColorInfo.BEIGE, ColorInfo.DARKPURPLE, ColorInfo.GOLD, ColorInfo.MAROON };
            foreach (var color in colors)
            {
                flow.AddChild(new ExampleDrawable
                {
                    Size = new(100),
                    Color = color,
                });
            }
        }

        public override bool OnActionPressed(SceneControls action)
        {
            const int scroll_amount = 50;
            switch (action)
            {
                case SceneControls.Down:
                case SceneControls.Right:
                    scrollableContainer.Scroll += scroll_amount;
                    return true;

                case SceneControls.Up:
                case SceneControls.Left:
                    scrollableContainer.Scroll -= scroll_amount;
                    return true;

                case SceneControls.One:
                    scrollableContainer.Size = new(scrollableContainer.Size.X - 50);
                    return true;

                case SceneControls.Two:
                    scrollableContainer.Size = new(scrollableContainer.Size.X + 50);
                    return true;

                case SceneControls.Three:
                    flow.Direction = scrollableContainer.Direction = scrollableContainer.Direction == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal;
                    if (flow.Direction == Direction.Vertical)
                    {
                        flow.AutoSizeAxes = Axes.Y;
                        flow.RelativeSizeAxes = Axes.X;
                    }
                    else
                    {
                        flow.AutoSizeAxes = Axes.X;
                        flow.RelativeSizeAxes = Axes.Y;
                    }
                    return true;

                default:
                    return base.OnActionPressed(action);
            }
        }
    }
}