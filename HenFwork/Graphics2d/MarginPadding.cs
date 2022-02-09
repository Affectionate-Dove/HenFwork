// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System.Numerics;

namespace HenFwork.Graphics2d
{
    public struct MarginPadding
    {
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }

        public Vector2 TopLeft
        {
            get => new(Left, Top);
            set
            {
                Top = value.Y;
                Left = value.X;
            }
        }

        public Vector2 BottomRight
        {
            get => new(Right, Bottom);
            set
            {
                Bottom = value.Y;
                Right = value.X;
            }
        }

        /// <summary>
        ///     The sum of <see cref="Left"/> and <see cref="Right"/>.
        ///     Upon setting, sets each of them to half the provided value.
        /// </summary>
        public float TotalHorizontal
        {
            get => Left + Right;
            set => Left = Right = value * 0.5f;
        }

        /// <summary>
        ///     The sum of <see cref="Top"/> and <see cref="Bottom"/>.
        ///     Upon setting, sets each of them to half the provided value.
        /// </summary>
        public float TotalVertical
        {
            get => Top + Bottom;
            set => Top = Bottom = value * 0.5f;
        }

        /// <summary>
        ///     Sets <see cref="Left"/> and <see cref="Right"/> to the provided value.
        /// </summary>
        public float Horizontal
        {
            set => Left = Right = value;
        }

        /// <summary>
        ///     Sets <see cref="Top"/> and <see cref="Bottom"/> to the provided value.
        /// </summary>
        public float Vertical
        {
            set => Top = Bottom = value;
        }

        /// <summary>
        ///     Returns the total padding for each axis.
        ///     Upon setting, sets each side on an axis to half the provided value,
        ///     as if <see cref="TotalHorizontal"/> and <see cref="TotalVertical"/> were set.
        /// </summary>
        public Vector2 Total
        {
            get => new(TotalHorizontal, TotalVertical);
            set
            {
                TotalHorizontal = value.X;
                TotalVertical = value.Y;
            }
        }

        public MarginPadding(Vector2 horizontalVertical) : this(horizontalVertical.X, horizontalVertical.Y)
        {
        }

        /// <param name="horizontal">
        ///     The value of <see cref="Left"/> and <see cref="Right"/> to be set.
        /// </param>
        /// <param name="vertical">
        ///     The value of <see cref="Top"/> and <see cref="Bottom"/> to be set.
        /// </param>
        public MarginPadding(float horizontal, float vertical) : this()
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public MarginPadding(float top, float left, float bottom, float right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }

        public MarginPadding(float allSides) : this(allSides, allSides, allSides, allSides)
        {
        }
    }
}