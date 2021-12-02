// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Testing;
using System.Numerics;

namespace HenFwork.VisualTests.Examples
{
    public class ExampleTestScene1 : VisualTestScene
    {
        public ExampleTestScene1() => AddChild(new ExampleDrawable("Sample text 1")
        {
            Offset = new Vector2(50),
            Color = new(0, 60, 160)
        });
    }
}