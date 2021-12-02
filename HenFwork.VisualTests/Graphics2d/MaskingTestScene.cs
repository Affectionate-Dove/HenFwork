// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.UI;

namespace HenFwork.VisualTests.Graphics2d
{
    public class MaskingTestScene : VisualTestScene
    {
        private readonly Container container1;
        private readonly VisibleContainer container2;

        public MaskingTestScene()
        {
            AddChild(container1 = new VisibleContainer(1, new(255, 140, 0))
            {
                Anchor = new(0.5f),
                Origin = new(0.5f),
                Size = new(400)
            });
            container1.AddChild(container2 = new VisibleContainer(2, new(0, 40, 200))
            {
                Size = new(300, 250),
                Anchor = new(0.25f, 0.5f),
                Origin = new(0.5f)
            });
            container2.AddChild(new Rectangle { Anchor = new(0.5f), Origin = new(0, 0.5f), Size = new(400, 100) });
        }

        public override bool OnActionPressed(SceneControls action)
        {
            switch (action)
            {
                case SceneControls.One:
                    container1.Masking = !container1.Masking;
                    return true;

                case SceneControls.Two:
                    container2.Masking = !container2.Masking;
                    return true;

                default:
                    return base.OnActionPressed(action);
            }
        }

        private class VisibleContainer : Container
        {
            private readonly int id;
            private readonly SpriteText spriteText;

            public VisibleContainer(int id, ColorInfo color)
            {
                AddChild(new Rectangle { RelativeSizeAxes = Axes.Both, Color = color });
                AddChild(spriteText = new SpriteText { RelativeSizeAxes = Axes.Both, Offset = new(10) });
                this.id = id;
            }

            protected override void OnLayoutUpdate()
            {
                base.OnLayoutUpdate();
                spriteText.Text = $"Masking: {Masking}\nPress {id} to switch";
            }
        }
    }
}