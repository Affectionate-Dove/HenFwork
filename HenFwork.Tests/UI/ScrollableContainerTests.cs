// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Tests.Input;
using HenFwork.UI;
using NUnit.Framework;
using System.Collections.Generic;

namespace HenFwork.Tests.UI
{
    [TestFixture(TestOf = typeof(ScrollableContainer<TestAction>))]
    public class ScrollableContainerTests
    {
        public static IEnumerable<Case> Cases => new Case[]
        {
            new(0, 0, new (float, float)[] { (-1, 1), (0, 1), (1, 0) }),
            new(5, 0, new (float, float)[] { (-1, 1), (0, 1), (1, 1) }),
            new(5, 3, new (float, float)[]{ (1, 1), (4, 1) }),
            new(1, 10, new (float, float)[]{ (0, 1), (1,0), (4,-3), (15, -9) }),
        };

        [TestCaseSource(nameof(Cases))]
        public void ScrollTest(Case c)
        {
            var scrollableContainer = new ScrollableContainer<TestAction>
            {
                Size = new(c.ContainerSize),
            };
            var drawable = new Rectangle { Offset = new(1), Size = new(c.DrawableSize) };
            scrollableContainer.AddChild(drawable);
            scrollableContainer.Update(0);

            Assert.AreEqual(drawable.Offset.Y, drawable.LayoutInfo.RenderRect.Top);

            foreach (var (scroll, expectedTop) in c.ScrollCases)
                ScrollAndTest(scroll, expectedTop);

            void ScrollAndTest(float scroll, float expectedTop)
            {
                scrollableContainer.Scroll = scroll;
                var failHint = $"Scroll: {scroll}";
                scrollableContainer.Direction = Direction.Vertical;
                scrollableContainer.Update(0);
                Assert.AreEqual(1, drawable.LayoutInfo.RenderRect.Left, "The non-scrollable axis is off.");
                Assert.AreEqual(expectedTop, drawable.LayoutInfo.RenderRect.Top, failHint);
                scrollableContainer.Direction = Direction.Horizontal;
                scrollableContainer.Update(0);
                Assert.AreEqual(1, drawable.LayoutInfo.RenderRect.Top, "The non-scrollable axis is off.");
                Assert.AreEqual(expectedTop, drawable.LayoutInfo.RenderRect.Left, failHint);
            }
        }

        public record Case(float ContainerSize, float DrawableSize, (float scroll, float expectedTop)[] ScrollCases);
    }
}