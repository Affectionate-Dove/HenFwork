// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.Input.UI;
using System;

namespace HenFwork.UI
{
    public class ScrollableContainer<TInputAction> : Container, IInterfaceComponent<TInputAction>
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
                ScrollBarContainer.Direction = value;
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

        public ScrollBarContainer ScrollBarContainer { get; }

        protected Container ContentContainer { get; }

        public ScrollableContainer()
        {
            base.AddChild(BackgroundContainer = new()
            {
                RelativeSizeAxes = Axes.Both
            });
            base.AddChild(ContentContainer = new());
            base.AddChild(ScrollBarContainer = new ScrollBarContainer());
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

        public void OnActionReleased(TInputAction action)
        {
            if (action.Equals(ScrollForwardAction))
                Scroll += ScrollOnActionAmount;
            else if (action.Equals(ScrollBackAction))
                Scroll -= ScrollOnActionAmount;
        }

        protected override void OnLayoutUpdate()
        {
            CalculateScrollInfo(scroll, out var absoluteScroll, out var relativeScroll);

            ScrollBarContainer.UpdateScrollPosition(relativeScroll);

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
    }

    public class ScrollBarContainer : Container
    {
        private readonly Rectangle background;
        private readonly Rectangle body;
        private ColorInfo backgroundColor;
        private ColorInfo foregroundColor;
        private float thickness;
        private Direction direction;
        private float barHeight;
        public static float DEFAULT_THICKNESS { get; set; } = 10;
        public static float DEFAULT_BAR_HEIGHT { get; set; } = 50;
        public static ColorInfo DEFAULT_BACKGROUND_COLOR { get; set; } = ColorInfo.DARKGRAY;
        public static ColorInfo DEFAULT_FOREGROUND_COLOR { get; set; } = ColorInfo.LIGHTGRAY;

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
            backgroundColor = DEFAULT_BACKGROUND_COLOR;
            ForegroundColor = DEFAULT_FOREGROUND_COLOR;
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