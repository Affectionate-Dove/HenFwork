// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Testing;
using HenFwork.Testing.Input;

namespace HenFwork.VisualTests.Graphics2d
{
    [TestSceneName("Drawable visibility")]
    public class DrawableVisibilityTestScene : VisualTestScene
    {
        private readonly ExampleDrawable drawable;

        public DrawableVisibilityTestScene()
        {
            AddChild(drawable = new ExampleDrawable("Press 1 to change visibility")
            {
                Visible = true
            });
        }

        public override bool OnActionPressed(SceneControls action)
        {
            if (action != SceneControls.One)
                return base.OnActionPressed(action);

            drawable.Visible = !drawable.Visible;

            return true;
        }
    }
}