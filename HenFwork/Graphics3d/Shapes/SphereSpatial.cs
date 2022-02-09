// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Numerics;
using HenFwork.Graphics2d;

namespace HenFwork.Graphics3d.Shapes
{
    public class SphereSpatial : Spatial, IHasColor
    {
        public Sphere Sphere { get; set; }

        public ColorInfo? Color { get; set; } = ColorInfo.RAYWHITE;

        ColorInfo IHasColor.Color => Color.GetValueOrDefault(new(0, 0, 0, 0));

        protected override void OnRender()
        {
            base.OnRender();
            if (Color.HasValue)
                Drawing.DrawSphere(Sphere + Position, Color.Value);
        }
    }
}