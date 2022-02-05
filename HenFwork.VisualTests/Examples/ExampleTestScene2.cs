// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.VisualTests.Examples
{
    public class ExampleTestScene2 : VisualTestScene
    {
        private readonly ExampleDrawable drawable;

        public override Dictionary<List<SceneControls>, string> ControlsDescriptions { get; } = new()
        {
            [new List<SceneControls> { SceneControls.One }] = "Change the drawable's color"
        };

        public ExampleTestScene2()
        {
            AddChild(drawable = new ExampleDrawable("Sample text 2")
            {
                Offset = new Vector2(150, 50),
                Color = new(200, 60, 30)
            });
        }

        public override bool OnActionPressed(SceneControls action)
        {
            if (action != SceneControls.One)
                return base.OnActionPressed(action);

            if (drawable.Color.Equals((ColorInfo)ColorInfo.DARKGREEN))
                drawable.Color = new(200, 60, 30);
            else
                drawable.Color = ColorInfo.DARKGREEN;

            return true;
        }
    }
}