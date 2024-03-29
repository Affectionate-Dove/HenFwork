﻿// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Numerics;
using System.Numerics;

namespace HenFwork.Graphics2d
{
    /// <summary>
    ///     Draws a provided texture.
    /// </summary>
    public class Sprite : Drawable
    {
        private Texture texture;
        private bool autoFillModeProportions;

        public Texture Texture
        {
            get => texture;
            set
            {
                texture = value;
                if (AutoFillModeProportions)
                    SetFillProportionsToTextureProportions();
            }
        }

        public ColorInfo Color { get; set; } = new ColorInfo(255, 255, 255);

        /// <summary>
        ///     Whether to automatically set
        ///     <see cref="Drawable.FillModeProportions"/>
        ///     to <see cref="Texture"/>'s proportions.
        /// </summary>
        /// <remarks>
        ///     If <see langword="true"/>, sets
        ///     <see cref="Drawable.FillModeProportions"/> when
        ///     this property or <see cref="Texture"/> changes.
        /// </remarks>
        public bool AutoFillModeProportions
        {
            get => autoFillModeProportions;
            set
            {
                autoFillModeProportions = value;
                if (autoFillModeProportions)
                    SetFillProportionsToTextureProportions();
            }
        }

        protected override void OnRender()
        {
            base.OnRender();
            var sourceRec = RectangleF.FromPositionAndSize(Vector2.Zero, Texture.Size, CoordinateSystem2d.YDown);
            var destRec = LayoutInfo.RenderRect;
            Drawing.DrawTexture(Texture, sourceRec, destRec, Vector2.Zero, 0, Color);
        }

        /// <summary>
        ///     Sets <see cref="Drawable.FillModeProportions"/>
        ///     to proportions of <see cref="Texture"/>.
        /// </summary>
        private void SetFillProportionsToTextureProportions()
        {
            if (Texture?.Size.Y is not (0 or null))
                FillModeProportions = Texture.Size.X / Texture.Size.Y;
        }
    }
}