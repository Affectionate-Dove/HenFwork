// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Generic;
using System.Linq;

namespace HenFwork.Graphics2d
{
    public class FillFlowContainer : Container
    {
        private Direction direction;
        private float spacing;

        public float Spacing
        {
            get => spacing;
            set
            {
                if (spacing == value)
                    return;

                spacing = value;
                LayoutValid = false;
            }
        }

        public Direction Direction
        {
            get => direction;
            set
            {
                direction = value;
                foreach (var container in base.Children.OfType<ChildContainer>())
                {
                    container.AutoSizeAxes = AxisFromDirection();
                    container.RelativeSizeAxes = AxisPerpendicularToDirection();
                }
                LayoutValid = false;
            }
        }

        public new IEnumerable<Drawable> Children => base.Children.OfType<ChildContainer>().Select(c => c.Child);

        public int Count => base.Children.Count;

        public override void AddChild(Drawable child)
        {
            var container = new ChildContainer();
            container.AddChild(child);
            base.AddChild(container);
            LayoutValid = ContainerLayoutValid = false;
        }

        public override void RemoveChild(Drawable child)
        {
            var container = FindChildContainer(child);
            if (container == null)
                return;
            container.RemoveChild(child);
            base.RemoveChild(container);
            LayoutValid = ContainerLayoutValid = false;
        }

        public override int RemoveAll(System.Predicate<Drawable> match) => base.Children
        .RemoveAll(_childContainer =>
        {
            var childContainer = _childContainer as ChildContainer;
            var child = childContainer.Child;
            if (match(child))
            {
                childContainer.RemoveChild(child);
                LayoutValid = ContainerLayoutValid = false;
                return true;
            }
            return false;
        });

        public override void Clear() => RemoveAll(_ => true);

        public virtual bool Contains(Drawable drawable) => base.Children.OfType<ChildContainer>().Any(childContainer => childContainer.Child == drawable);

        protected override void OnLayoutUpdate()
        {
            base.OnLayoutUpdate();

            foreach (var childContainer in base.Children)
                childContainer.UpdateLayout();

            // TODO: call this after layout change or children layout change
            UpdateChildrenPositions();

            foreach (var childContainer in base.Children)
                childContainer.UpdateLayout();
        }

        private Axes AxisFromDirection() => Direction switch
        {
            Direction.Horizontal => Axes.X,
            Direction.Vertical => Axes.Y,
            _ => throw new System.NotImplementedException()
        };

        private Axes AxisPerpendicularToDirection() => Direction switch
        {
            Direction.Horizontal => Axes.Y,
            Direction.Vertical => Axes.X,
            _ => throw new System.NotImplementedException()
        };

        private void UpdateChildrenPositions()
        {
            var maxPos = 0f;
            foreach (var childContainer in base.Children.OfType<Container>())
            {
                if (Direction == Direction.Horizontal)
                {
                    childContainer.Offset = new System.Numerics.Vector2(maxPos, 0);
                    maxPos += childContainer.LayoutInfo.RenderSize.X;
                }
                else
                {
                    childContainer.Offset = new System.Numerics.Vector2(0, maxPos);
                    maxPos += childContainer.LayoutInfo.RenderSize.Y;
                }

                maxPos += Spacing;
            }
        }

        private Container FindChildContainer(Drawable child)
        {
            foreach (var drawable in base.Children)
            {
                var container = drawable as Container;
                if (container.Children[0] == child)
                    return container;
            }
            return null;
        }

        private class ChildContainer : Container
        {
            public Drawable Child => Children[0];
            public new FillFlowContainer Parent => base.Parent as FillFlowContainer;

            protected override void OnLayoutUpdate()
            {
                if (Child.FillMode == FillMode.Stretch)
                {
                    AutoSizeAxes = Parent.AxisFromDirection();
                    RelativeSizeAxes = Parent.AxisPerpendicularToDirection();
                }
                else
                {
                    AutoSizeAxes = Axes.None;
                    RelativeSizeAxes = Axes.Both;
                    FillMode = Child.FillMode;
                    FillModeProportions = Child.FillModeProportions;
                }
                base.OnLayoutUpdate();
            }
        }
    }

    public enum Direction
    {
        Horizontal, Vertical
    }
}