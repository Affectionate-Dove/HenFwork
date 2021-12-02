// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Testing;
using System.Numerics;

namespace HenFwork.VisualTests.Graphics2d
{
    public class LineTestScene : VisualTestScene
    {
        public LineTestScene()
        {
            AddChild(new Line
            {
                A = new Vector2(170, 100),
                B = new Vector2(100, 200),
                Color = new ColorInfo(12, 54, 150),
                Thickness = 7
            });

            AddChild(new Line
            {
                A = new Vector2(100, 100),
                B = new Vector2(170, 200),
                Color = new ColorInfo(12, 54, 150),
                Thickness = 7
            });

            AddChild(new Line
            {
                A = new Vector2(200, 100),
                B = new Vector2(200, 200),
                Color = new ColorInfo(12, 54, 150),
                Thickness = 7
            });

            AddChild(new Line
            {
                A = new Vector2(200, 100),
                B = new Vector2(230, 100),
                Color = new ColorInfo(12, 54, 150),
                Thickness = 7
            });

            AddChild(new Line
            {
                A = new Vector2(200, 200),
                B = new Vector2(230, 200),
                Color = new ColorInfo(12, 54, 150),
                Thickness = 7
            });

            AddChild(new Line
            {
                A = new Vector2(260, 133),
                B = new Vector2(230, 100),
                Color = new ColorInfo(12, 54, 150),
                Thickness = 7
            });

            AddChild(new Line
            {
                A = new Vector2(260, 166),
                B = new Vector2(230, 200),
                Color = new ColorInfo(12, 54, 150),
                Thickness = 7
            });

            AddChild(new Line
            {
                A = new Vector2(260, 133),
                B = new Vector2(260, 166),
                Color = new ColorInfo(12, 54, 150),
                Thickness = 7
            });
        }
    }
}