// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Testing;

namespace HenFwork.VisualTests.Graphics2d
{
    public class FillFlowContainerTestScene : VisualTestScene
    {
        public FillFlowContainerTestScene()
        {
            var backgroundContainer = new Container { RelativeSizeAxes = Axes.Both };
            var actualContainer = new Container { RelativeSizeAxes = Axes.Both };

            AddChild(backgroundContainer);
            AddChild(actualContainer);

            var fillFlowContainer1 = new TestFillFlowContainer(Direction.Vertical, 4, new(20, 20, 100))
            {
                Offset = new(100),
                Size = new(200, 500)
            };
            actualContainer.AddChild(fillFlowContainer1);
            backgroundContainer.AddChild(new MimickingRectangle(fillFlowContainer1, new(100, 100, 0)));
        }

        private class TestFillFlowContainer : FillFlowContainer
        {
            public TestFillFlowContainer(Direction direction, int amountOfRectangles, ColorInfo rectanglesColor)
            {
                Direction = direction;
                Spacing = 10;
                Padding = new MarginPadding { Vertical = 10, Horizontal = 10 };
                for (var i = 0; i < amountOfRectangles; i++)
                {
                    AddChild(new Rectangle
                    {
                        Color = rectanglesColor,
                        RelativeSizeAxes = direction == Direction.Horizontal ? Axes.Y : Axes.X,
                        Size = direction == Direction.Horizontal ? new(20, 1) : new(1, 20)
                    });
                }
            }
        }

        private class MimickingRectangle : Rectangle
        {
            private readonly FillFlowContainer fillFlowContainer;

            public MimickingRectangle(FillFlowContainer fillFlowContainer, ColorInfo color)
            {
                this.fillFlowContainer = fillFlowContainer;
                Color = color;
            }

            protected override void OnUpdate(float elapsed)
            {
                base.OnUpdate(elapsed);
                Size = fillFlowContainer.Size;
                Offset = fillFlowContainer.Offset;
            }
        }
    }
}