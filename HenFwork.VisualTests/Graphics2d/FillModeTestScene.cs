// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.UI;
using System.Collections.Generic;

namespace HenFwork.VisualTests.Graphics2d
{
    public class FillModeTestScene : VisualTestScene
    {
        private readonly Container container1;
        private readonly VisibleContainer container2;

        public override Dictionary<List<SceneControls>, string> ControlsDescriptions { get; } = new()
        {
            [new() { SceneControls.One }] = "Strech",
            [new() { SceneControls.Two }] = "Fill",
            [new() { SceneControls.Three }] = "Fit",
            [new() { SceneControls.Up }] = "Increase proportions",
            [new() { SceneControls.Down }] = "Decrease proportions"
        };

        public FillModeTestScene()
        {
            AddChild(container1 = new VisibleContainer(false, new(255, 140, 0))
            {
                Anchor = new(0.5f),
                Origin = new(0.5f),
                Size = new(400)
            });
            container1.AddChild(container2 = new VisibleContainer(true, new(0, 40, 200))
            {
                RelativeSizeAxes = Axes.Both,
                FillModeProportions = 1.5f,
                Anchor = new(0.5f),
                Origin = new(0.5f),
                Size = new(0.95f)
            });
        }

        public override bool OnActionPressed(SceneControls action)
        {
            switch (action)
            {
                case SceneControls.One:
                    container2.FillMode = FillMode.Stretch;
                    return true;

                case SceneControls.Two:
                    container2.FillMode = FillMode.Fill;
                    return true;

                case SceneControls.Three:
                    container2.FillMode = FillMode.Fit;
                    return true;

                case SceneControls.Down:
                    container2.FillModeProportions -= 0.1f;
                    return true;

                case SceneControls.Up:
                    container2.FillModeProportions += 0.1f;
                    return true;

                default:
                    return base.OnActionPressed(action);
            }
        }

        private class VisibleContainer : Container
        {
            private readonly bool displayInfo;
            private readonly SpriteText spriteText;

            public VisibleContainer(bool displayInfo, ColorInfo color)
            {
                AddChild(new Rectangle { RelativeSizeAxes = Axes.Both, Color = color });
                AddChild(spriteText = new SpriteText { RelativeSizeAxes = Axes.Both, Offset = new(10), FontSize = 20 });
                this.displayInfo = displayInfo;
            }

            protected override void OnLayoutUpdate()
            {
                base.OnLayoutUpdate();
                if (displayInfo)
                    spriteText.Text = $"FillMode: {FillMode}\nFillModeProportions: {FillModeProportions:0.0}";
            }
        }
    }
}