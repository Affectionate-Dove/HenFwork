// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using System.Numerics;

namespace HenFwork.Graphics2d.Layouts
{
    public readonly struct DrawableLayoutInfo
    {
        /// <summary>
        ///     The position inside a parent container, in pixels.
        /// </summary>
        /// <remarks>
        ///     This is not the position of the top-left corner of a <see cref="Drawable"/>,
        ///     nor the center point, but of the <see cref="Drawable.Origin"/> point.
        ///     For a corner's position, see <see cref="LocalRect"/>.
        /// </remarks>
        public Vector2 LocalPosition { get; init; }

        /// <summary>
        ///     The position on screen, in pixels.
        /// </summary>
        /// <remarks>
        ///     This is not the position of the top-left corner of a <see cref="Drawable"/>,
        ///     nor the center point, but of the <see cref="Drawable.Origin"/> point.
        ///     For a corner's position, see <see cref="LocalRect"/>.
        /// </remarks>
        public Vector2 RenderPosition { get; init; }

        /// <summary>
        ///     The size of a <see cref="Drawable"/> on screen, in pixels.
        /// </summary>
        public Vector2 RenderSize { get; init; }

        /// <summary>
        ///     The part of a <see cref="Drawable"/> that is visible,
        ///     defined by a rectangle on screen, in pixels.
        /// </summary>
        public RectangleF? Mask { get; init; }

        /// <summary>
        ///     Defines where inside the <see cref="Drawable"/>'s
        ///     <see cref="LocalRect"/>/<see cref="RenderRect"/> are the points
        ///     <see cref="LocalPosition"/>/<see cref="RenderPosition"/>,
        ///     in percentage values (0.0 to 1.0).
        /// </summary>
        /// <remarks>
        ///     Let's say we have a <see cref="Drawable"/> with:
        ///     <list type="bullet">
        ///     <item>Position: (100, 100)</item>
        ///     <item>Size: (100, 100)</item>
        ///     </list>
        ///     With <see cref="Origin"/> equal to <c>0.0</c>, the "position
        ///     point" is in the top left corner, so its defining rectangle is as follows:
        ///     <list type="bullet">
        ///         <item>Left: 100</item>
        ///         <item>Right: 200</item>
        ///         <item>Top: 100</item>
        ///         <item>Bottom: 200</item>
        ///     </list>
        ///     But if the <see cref="Origin"/> is equal to <c>0.5</c>, then the
        ///     "position point" is in the center, and the defining rectangle is as follows:
        ///     <list type="bullet">
        ///         <item>Left: 50</item>
        ///         <item>Right: 150</item>
        ///         <item>Top: 50</item>
        ///         <item>Bottom: 150</item>
        ///     </list>
        /// </remarks>
        public Vector2 Origin { get; init; }

        /// <summary>
        ///     The boundaries inside a parent container, in pixels.
        /// </summary>
        public RectangleF LocalRect => RectangleF.FromPositionAndSize(LocalPosition, RenderSize, Origin, CoordinateSystem2d.YDown);

        /// <summary>
        ///     The boundaries on screen, in pixels.
        /// </summary>
        public RectangleF RenderRect => RectangleF.FromPositionAndSize(RenderPosition, RenderSize, Origin, CoordinateSystem2d.YDown);
    }
}