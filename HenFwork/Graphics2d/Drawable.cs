// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Numerics;
using HenFwork.Graphics2d.Layouts;
using System;
using System.Numerics;

namespace HenFwork.Graphics2d
{
    public abstract class Drawable
    {
        private IContainer parent;
        private Vector2 offset;
        private Axes relativePositionAxes;
        private Vector2 size = Vector2.One;
        private Axes relativeSizeAxes;
        private Vector2 anchor;
        private Vector2 origin;
        private bool masking;
        private FillMode fillMode = FillMode.Stretch;
        private float fillModeProportions = 1;

        /// <summary>
        ///     Whether this drawable
        ///     is rendered to the screen.
        ///     True by default.
        /// </summary>
        public bool Visible { get; set; } = true;

        public IContainer Parent
        {
            get => parent;
            set
            {
                if (parent == value)
                    return;

                LayoutValid = false;
                parent = value;
            }
        }

        public Vector2 Offset
        {
            get => offset;
            set
            {
                if (offset == value)
                    return;

                offset = value;
                LayoutValid = false;
            }
        }

        public Axes RelativePositionAxes
        {
            get => relativePositionAxes;
            set
            {
                if (relativePositionAxes == value)
                    return;

                relativePositionAxes = value;
                LayoutValid = false;
            }
        }

        public Vector2 Size
        {
            get => size;
            set
            {
                if (size == value)
                    return;

                size = value;
                LayoutValid = false;
            }
        }

        public Axes RelativeSizeAxes
        {
            get => relativeSizeAxes;
            set
            {
                if (relativeSizeAxes == value)
                    return;

                relativeSizeAxes = value;
                LayoutValid = false;
            }
        }

        public Vector2 Anchor
        {
            get => anchor;
            set
            {
                if (anchor == value)
                    return;

                anchor = value;
                LayoutValid = false;
            }
        }

        public Vector2 Origin
        {
            get => origin;
            set
            {
                if (origin == value)
                    return;

                origin = value;
                LayoutValid = false;
            }
        }

        /// <summary>
        ///     Whether anything outside
        ///     the <see cref="DrawableLayoutInfo.RenderRect"/>
        ///     is hidden.
        /// </summary>
        /// <remarks>
        ///     This doesn't have an effect if this is a top-level Drawable,
        ///     in that case masking is always on.
        /// </remarks>
        public bool Masking
        {
            get => masking;
            set
            {
                if (masking == value)
                    return;

                masking = value;
                LayoutValid = false;
            }
        }

        /// <remarks>
        ///     <see cref="FillMode.Stretch"/> by default.
        /// </remarks>
        public FillMode FillMode
        {
            get => fillMode;
            set
            {
                if (fillMode == value)
                    return;

                fillMode = value;
                LayoutValid = false;
            }
        }

        /// <summary>
        ///     When <see cref="RelativeSizeAxes"/> are set to <see cref="Axes.Both"/>,
        ///     and <see cref="FillMode"/> to anything other than <see cref="FillMode.Stretch"/>,
        ///     the width to height proportions of this <see cref="Drawable"/>.
        /// </summary>
        public float FillModeProportions
        {
            get => fillModeProportions;
            set
            {
                if (fillModeProportions == value)
                    return;

                fillModeProportions = value;
                LayoutValid = false;
            }
        }

        public DrawableLayoutInfo LayoutInfo { get; private set; }
        public bool LayoutValid { get; protected set; }

        public void Update(float elapsed)
        {
            if (!LayoutValid)
                UpdateLayout();

            OnUpdate(elapsed);

            if (!LayoutValid)
                UpdateLayout();
        }

        public void Render()
        {
            var _mask = LayoutInfo.Mask;
            if (_mask is null)
                return;

            if (!Visible)
                return;

            var mask = _mask.Value;
            Drawing.BeginScissorMode(mask);

            OnRender();
        }

        public void UpdateLayout()
        {
            var localPos = ComputeLocalPosition();
            var renderPos = ComputeRenderPosition(localPos);
            var renderSize = ComputeRenderSize();
            var renderRect = RectangleF.FromPositionAndSize(renderPos, renderSize, Origin, CoordinateSystem2d.YDown);

            LayoutInfo = new DrawableLayoutInfo
            {
                Origin = Origin,
                LocalPosition = localPos,
                RenderPosition = renderPos,
                RenderSize = renderSize,
                Mask = ComputeMaskArea(renderRect)
            };
            LayoutValid = true;

            OnLayoutUpdate();
        }

        protected virtual Vector2 ComputeRenderSize()
        {
            var size = Size;
            if (Parent is null)
                return size;

            if (RelativeSizeAxes.HasFlag(Axes.X))
                size.X *= Parent.ContainerLayoutInfo.ChildrenRenderArea.Size.X;
            if (RelativeSizeAxes.HasFlag(Axes.Y))
                size.Y *= Parent.ContainerLayoutInfo.ChildrenRenderArea.Size.Y;

            if (RelativeSizeAxes.HasFlag(Axes.Both) && FillMode != FillMode.Stretch)
            {
                var currentProportions = size.X / size.Y;
                if (FillMode == FillMode.Fill)
                {
                    if (currentProportions < FillModeProportions)
                        size.X = size.Y * FillModeProportions;
                    else
                        size.Y = size.X / FillModeProportions;
                }
                else if (FillMode == FillMode.Fit)
                {
                    if (currentProportions < FillModeProportions)
                        size.Y = size.X / FillModeProportions;
                    else
                        size.X = size.Y * FillModeProportions;
                }
            }

            return size;
        }

        protected virtual void OnUpdate(float elapsed)
        {
        }

        protected virtual void OnLayoutUpdate()
        {
        }

        protected virtual void OnRender()
        {
        }

        protected RectangleF? ComputeMaskArea(RectangleF renderArea)
        {
            if (Parent is null) // top level Drawable
                return renderArea;

            var maskArea = Parent.ContainerLayoutInfo.MaskArea;

            if (!Masking)
                return maskArea;

            if (!Visible)
                return null;

            return maskArea?.GetIntersection(renderArea);
        }

        private Vector2 ComputeLocalPosition()
        {
            var pos = Offset;
            if (Parent is null)
                return pos;

            if (RelativePositionAxes.HasFlag(Axes.X))
                pos.X *= Parent.ContainerLayoutInfo.ChildrenRenderArea.Size.X;
            if (RelativePositionAxes.HasFlag(Axes.Y))
                pos.Y *= Parent.ContainerLayoutInfo.ChildrenRenderArea.Size.Y;
            pos += Anchor * Parent.ContainerLayoutInfo.ChildrenRenderArea.Size;

            return pos;
        }

        private Vector2 ComputeRenderPosition(Vector2 localPosition)
        {
            var pos = localPosition;
            if (Parent != null)
                // TODO: check if this is correct
                pos += Parent.ContainerLayoutInfo.ChildrenRenderArea.TopLeft;
            return pos;
        }
    }

    [Flags]
    public enum Axes
    {
        None = 0,
        X = 1,
        Y = 2,

        Both = X | Y
    }

    public enum FillMode
    {
        Stretch,
        Fill,
        Fit
    }
}