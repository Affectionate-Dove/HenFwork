// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Testing;
using System.Numerics;

namespace HenFwork.VisualTests.Graphics2d
{
    public class CircleTestScene : VisualTestScene
    {
        public CircleTestScene()
        {
            AddChild(new Circle
            {
                Size = new Vector2(69, 69),
                Offset = new Vector2(200, 200)
            });

            AddChild(new Circle
            {
                Size = new Vector2(69, 69),
                Offset = new Vector2(400, 400),
                Color = new ColorInfo(69, 69, 240)
            });

            AddChild(new Circle
            {
                Size = new Vector2(69, 420),
                Offset = new Vector2(600, 200),
                Color = new ColorInfo(3, 123, 240)
            });

            AddChild(new Circle
            {
                Size = new Vector2(420, 69),
                Offset = new Vector2(200, 400),
                Color = new ColorInfo(34, 240, 111)
            });
        }
    }
}