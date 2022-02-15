// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Input;
using HenFwork.Graphics2d;
using HenFwork.Input.UI;
using System;

namespace HenFwork.UI
{
    public class ScrollableContainer<TInputAction> : Container, IInterfaceComponent<TInputAction>, IPositionalInterfaceComponent
    {
        private Direction direction;
        private float scroll;

        public event Action<IInterfaceComponent<TInputAction>> FocusRequested { add => throw new NotImplementedException(); remove => throw new NotImplementedException(); }

        public Direction Direction
        {
            get => direction; set
            {
                direction = value;
                ContentContainer.AutoSizeAxes = direction == Direction.Horizontal ? Axes.X : Axes.Y;
                ContentContainer.RelativeSizeAxes = direction == Direction.Horizontal ? Axes.Y : Axes.X;
                ContentContainer.Size = System.Numerics.Vector2.One;
                ScrollBar.Direction = value;
            }
        }

        public Container BackgroundContainer { get; }

        public TInputAction? ScrollBackAction { get; set; }

        public TInputAction? ScrollForwardAction { get; set; }

        public float Scroll
        {
            get => scroll;
            set
            {
                CalculateScrollInfo(value, out var absoluteScroll, out _);
                if (scroll == absoluteScroll)
                    return;

                scroll = -absoluteScroll;
                LayoutValid = false;
            }
        }

        public bool AcceptsFocus { get; set; } = true;

        public float ScrollOnActionAmount { get; set; }

        public ScrollBarContainer ScrollBar { get; }

        public bool AcceptsPositionalInput => true;
        protected Container ContentContainer { get; }

        public ScrollableContainer()
        {
            base.AddChild(BackgroundContainer = new()
            {
                RelativeSizeAxes = Axes.Both
            });
            base.AddChild(ContentContainer = new());
            base.AddChild(ScrollBar = new ScrollBarContainer());
            Direction = Direction.Vertical;
            Masking = true;
        }

        public override void AddChild(Drawable child) => ContentContainer.AddChild(child);

        public override void RemoveChild(Drawable child) => ContentContainer.RemoveChild(child);

        public void OnFocus()
        { }

        public void OnFocusLost()
        { }

        public bool OnActionPressed(TInputAction action) => action.Equals(ScrollBackAction) || action.Equals(ScrollForwardAction);

        void IPositionalInterfaceComponent.OnMouseScroll(float delta) => Scroll -= delta * 30;

        public void OnActionReleased(TInputAction action)
        {
            if (action.Equals(ScrollForwardAction))
                Scroll += ScrollOnActionAmount;
            else if (action.Equals(ScrollBackAction))
                Scroll -= ScrollOnActionAmount;
        }

        public bool AcceptsPositionalButton(MouseButton button) => false;

        public void OnHover()
        { }

        public void OnHoverLost()
        { }

        public void OnMousePress(MouseButton button)
        { }

        public void OnMouseRelease(MouseButton button)
        { }

        public void OnClick(MouseButton button)
        { }

        protected override void OnLayoutUpdate()
        {
            CalculateScrollInfo(scroll, out var absoluteScroll, out var relativeScroll);

            ScrollBar.UpdateScrollPosition(relativeScroll);

            if (Direction == Direction.Vertical)
                ContentContainer.Offset = new(0, absoluteScroll);
            else
                ContentContainer.Offset = new(absoluteScroll, 0);

            base.OnLayoutUpdate();
        }

        private void CalculateScrollInfo(in float requestedScroll, out float absoluteScroll, out float relativeScroll)
        {
            var contentSize = ContentContainer.LayoutInfo.RenderSize;
            var contentLength = Direction == Direction.Vertical ? contentSize.Y : contentSize.X;
            var maxScroll = Math.Max(0, contentLength - (Direction == Direction.Vertical ? LayoutInfo.RenderSize.Y : LayoutInfo.RenderSize.X));

            absoluteScroll = -Math.Clamp(requestedScroll, 0, maxScroll);

            if (maxScroll == 0)
                relativeScroll = 0;
            else
                relativeScroll = -absoluteScroll / maxScroll;
        }

        public class ScrollBarContainer : Container
        {
            protected const float DEFAULT_THICKNESS = 10;
            protected const float DEFAULT_BAR_HEIGHT = 50;
            private static ColorInfo default_background_color = ColorInfo.DARKGRAY;
            private static ColorInfo default_foreground_color = ColorInfo.LIGHTGRAY;
            private readonly Rectangle background;
            private readonly Rectangle body;
            private ColorInfo backgroundColor;
            private ColorInfo foregroundColor;
            private float thickness;
            private Direction direction;
            private float barHeight;

            public float Thickness
            {
                get => thickness; set
                {
                    if (thickness == value)
                        return;

                    thickness = value;
                    LayoutValid = false;
                }
            }

            public Direction Direction
            {
                get => direction; set
                {
                    if (direction == value)
                        return;

                    direction = value;
                    LayoutValid = false;
                }
            }

            public ColorInfo BackgroundColor
            {
                get => backgroundColor; set
                {
                    backgroundColor = value;
                    UpdateColors();
                }
            }

            public ColorInfo ForegroundColor
            {
                get => foregroundColor; set
                {
                    foregroundColor = value;
                    UpdateColors();
                }
            }

            public float BarHeight
            {
                get => barHeight; private set
                {
                    if (barHeight == value)
                        return;

                    barHeight = value;
                    LayoutValid = false;
                }
            }

            public ScrollBarContainer()
            {
                AddChild(background = new Rectangle()
                {
                    RelativeSizeAxes = Axes.Both,
                });
                AddChild(body = new Rectangle { RelativePositionAxes = Axes.Both });
                backgroundColor = default_background_color;
                ForegroundColor = default_foreground_color;
                barHeight = DEFAULT_BAR_HEIGHT;
                Thickness = DEFAULT_THICKNESS;
                Anchor = Origin = System.Numerics.Vector2.One;
            }

            public void UpdateScrollPosition(float percentage)
            {
                if (Direction == Direction.Horizontal)
                {
                    body.Offset = new(percentage, 0);
                    body.Origin = body.Offset;
                }
                else
                {
                    body.Offset = new(0, percentage);
                    body.Origin = body.Offset;
                }
            }

            protected override void OnLayoutUpdate()
            {
                base.OnLayoutUpdate();
                if (Direction == Direction.Horizontal)
                {
                    RelativeSizeAxes = Axes.X;
                    Size = new(1, Thickness);
                    body.RelativeSizeAxes = Axes.Y;
                    body.Size = new(BarHeight, 1);
                }
                else
                {
                    RelativeSizeAxes = Axes.Y;
                    Size = new(Thickness, 1);
                    body.RelativeSizeAxes = Axes.X;
                    body.Size = new(1, BarHeight);
                }
            }

            private void UpdateColors()
            {
                background.Color = BackgroundColor;
                body.Color = ForegroundColor;
            }
        }
    }
}