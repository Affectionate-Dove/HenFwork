// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.MapEditing.Input;
using HenFwork.UI;

namespace HenFwork.MapEditing.Screens.Editor
{
    public class ToolbarButton : Button<EditorControls>
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
            EnabledColors = new ButtonColorSet(null, null, null);
            HoveredColors = new ButtonColorSet(null, ColorInfo.WHITE, null);
            FocusedColors = new ButtonColorSet(null, ColorInfo.ORANGE, null);
            PressedColors = new ButtonColorSet(ColorInfo.BLACK, null, null);

            AcceptedActions.Add(EditorControls.Select);
            FillMode = FillMode.Fit;
            RelativeSizeAxes = Axes.Both;
            BorderThickness = 3;
            AddChild(spriteContainer = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Size = new(0.64f),
                Anchor = new(0.5f),
                Origin = new(0.5f)
            });
            MakeUnselected();
        }

        public void MakeUnselected() => EnabledColors = EnabledColors with 
        {
            fill = ColorInfo.GRAY,
            border = ColorInfo.GRAY.MultiplyBrightness(1.4f)
        };

        public void MakeSelected() => EnabledColors = EnabledColors with 
        {
            fill = ColorInfo.GRAY.MultiplyBrightness(0.3f),
            border = ColorInfo.RAYWHITE.MultiplyBrightness(0.95f)
        };
    }
}