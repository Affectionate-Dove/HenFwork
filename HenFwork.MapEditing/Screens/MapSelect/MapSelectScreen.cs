// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.MapEditing.Saves;
using HenFwork.Screens;
using HenFwork.UI;
using System;
using System.Collections.Generic;

namespace HenFwork.MapEditing.Screens.MapSelect
{
    public class MapSelectScreen : Screen
    {
        private SpriteText mapName;
        private Button<H> confirmButton;

        public MapSelectScreen(List<WorldSave> worldSaves, Action<WorldSave> confirmAction)
        {
            AddChild(new SpriteText()
            {
                Text = "Select map",
                Anchor = new(0.5f, 0),
                Origin = new(0.5f, 0),
                Color = new HenBstractions.Graphics.ColorInfo(21, 37, 69),
                FontSize = 69
            });
            CreateLeftSide(worldSaves, confirmAction);
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

            confirmButton = new Button<H>()
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
            flow.AddChild(mapName = new SpriteText()
            {
                FontSize = 35
            });
            flow.AddChild(new SpriteText()
            {
                Text = "Jakas gadka szmatka hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh hhhhhhhhhh hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh",
                FontSize = 20
            });
            AddChild(confirmButton);
        }

        private void CreateLeftSide(List<WorldSave> worldSaves, Action<WorldSave> confirmAction)
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

            foreach (var save in worldSaves)
            {
                flow.AddChild(new Button<H>()
                {
                    Text = save.WorldName,
                    Size = new(1f, 75),
                    RelativeSizeAxes = Axes.X,
                    Action = () =>
                    {
                        mapName.Text = save.WorldName;
                        confirmButton.Action = () => confirmAction(save);
                    },
                });
            }
        }
    }

    public enum H
    {
    }
}