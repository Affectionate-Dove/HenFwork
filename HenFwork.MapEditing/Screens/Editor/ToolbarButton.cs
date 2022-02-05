// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.UI;

namespace HenFwork.MapEditing.Screens.Editor
{
    public class ToolbarButton<TInputAction> : Button<TInputAction>
    {
        private readonly Container spriteContainer;
        private Sprite sprite;

        public Sprite Sprite
        {
            get => sprite;
            set
            {
                spriteContainer.Clear();

                sprite = value;

                if (sprite is not null)
                    spriteContainer.AddChild(sprite);
            }
        }

        public ToolbarButton()
        {
            // TODO
            Action = () => System.Console.WriteLine("H");

            FillMode = FillMode.Fit;
            RelativeSizeAxes = Axes.Both;
            DisabledColors = new ButtonColorSet(ColorInfo.GRAY, new(255, 255, 255, 150), null);
            HoveredColors = new ButtonColorSet(null, new(255, 255, 255, 255), null);
            BorderThickness = 3;
            AddChild(spriteContainer = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Size = new(0.64f),
                Anchor = new(0.5f),
                Origin = new(0.5f)
            });
        }
    }
}