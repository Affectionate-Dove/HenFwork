// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.UI;
using System.Collections.Generic;

namespace HenFwork.VisualTests.Input
{
    public class SceneInputTestScene : VisualTestScene
    {
        private readonly List<SceneControls> pressedControls = new();
        private readonly SpriteText spriteText;

        public SceneInputTestScene()
        {
            AddChild(spriteText = new SpriteText { RelativeSizeAxes = Axes.Both, TextAlignment = new(0.5f) });
            UpdateText();
        }

        public override bool OnActionPressed(SceneControls action)
        {
            pressedControls.Add(action);
            UpdateText();
            return true;
        }

        public override void OnActionReleased(SceneControls action)
        {
            pressedControls.Remove(action);
            UpdateText();
        }

        private void UpdateText() => spriteText.Text = pressedControls.Count == 0 ? "Nothing pressed" : string.Join(", ", pressedControls);
    }
}