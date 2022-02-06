// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Screens;
using HenFwork.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HenFwork.MapEditing.Screens.MapSelect
{
    public class MapSelectScreen : Screen
    {
        public MapSelectScreen()
        {
            AddChild(new SpriteText()
            {
                Text = "Select map",
                Anchor = new(0.5f, 0),
                Origin = new(0.5f, 0),
                Color = new HenBstractions.Graphics.ColorInfo(21, 37, 69),
                FontSize = 69
            });
            CreateLeftSide();
            CreateRightSide();
        }

        private void CreateRightSide()
        {
            var descriptionContainer = new Container()
            {
                Anchor = new(0.6f, 0.2f),
                RelativeSizeAxes = Axes.Both,
                Size = new(0.4f, 0.8f),

            };

            var flow = new FillFlowContainer()
            {
                Size = new(1, 1),
                RelativeSizeAxes = Axes.Both,
                Direction = Direction.Vertical,
                Padding = new MarginPadding()
                {
                    Horizontal = 10,
                    Vertical = 10
                }
            };

            var button = new Button<H>()
            {
                Text = "Confirm",
                Size = new(0.4f, 50),
                RelativeSizeAxes = Axes.X,
                Anchor = new(0.6f, 0.8f),
                RelativePositionAxes = Axes.Both,
                DisabledColors = new ButtonColorSet(null, HenBstractions.Graphics.ColorInfo.GREEN, HenBstractions.Graphics.ColorInfo.WHITE)
            };

            AddChild(descriptionContainer);
            descriptionContainer.AddChild(flow);
            flow.AddChild(new SpriteText()
            {
                Text = "Map 2",
                FontSize = 35
            });
            flow.AddChild(new SpriteText()
            {
                Text = "Jakas gadka szmatka hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh hhhhhhhhhh hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh",
                FontSize = 20
            });
            AddChild(button);
        }

        private void CreateLeftSide()
        {
            var scroll = new ScrollableContainer<H>()
            {
                Anchor = new(0, 0.2f),
                Size = new(0.6f, 1f),
                RelativeSizeAxes = Axes.Both
            };

            var flow = new FillFlowContainer()
            {
                Size = new(1f, 0),
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = Direction.Vertical,
                Padding = new MarginPadding()
                {
                    Horizontal = 20,
                    Vertical = 20
                },
                Spacing = 10
            };


            AddChild(scroll);
            scroll.AddChild(flow);

            for (var i = 0; i < 10; i++)
            {
                flow.AddChild(new Button<H>()
                {
                    Text = $"Map {i + 1}",
                    Size = new(1f, 75),
                    RelativeSizeAxes = Axes.X
                });
            }
        }

    }

    public enum H
    {

    }

}
