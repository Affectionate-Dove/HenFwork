// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System;
using System.Numerics;

namespace HenBstractions.Graphics
{
    public struct ColorInfo
    {
        public byte a;
        public byte r;
        public byte b;
        public byte g;

        public static ColorInfo LIGHTGRAY { get; } = Raylib_cs.Color.LIGHTGRAY;
        public static ColorInfo GRAY { get; } = Raylib_cs.Color.GRAY;
        public static ColorInfo DARKGRAY { get; } = Raylib_cs.Color.DARKGRAY;
        public static ColorInfo YELLOW { get; } = Raylib_cs.Color.YELLOW;
        public static ColorInfo GOLD { get; } = Raylib_cs.Color.GOLD;
        public static ColorInfo ORANGE { get; } = Raylib_cs.Color.ORANGE;
        public static ColorInfo PINK { get; } = Raylib_cs.Color.PINK;
        public static ColorInfo RED { get; } = Raylib_cs.Color.RED;
        public static ColorInfo MAROON { get; } = Raylib_cs.Color.MAROON;
        public static ColorInfo GREEN { get; } = Raylib_cs.Color.GREEN;
        public static ColorInfo LIME { get; } = Raylib_cs.Color.LIME;
        public static ColorInfo DARKGREEN { get; } = Raylib_cs.Color.DARKGREEN;
        public static ColorInfo SKYBLUE { get; } = Raylib_cs.Color.SKYBLUE;
        public static ColorInfo BLUE { get; } = Raylib_cs.Color.BLUE;
        public static ColorInfo DARKBLUE { get; } = Raylib_cs.Color.DARKBLUE;
        public static ColorInfo PURPLE { get; } = Raylib_cs.Color.PURPLE;
        public static ColorInfo VIOLET { get; } = Raylib_cs.Color.VIOLET;
        public static ColorInfo DARKPURPLE { get; } = Raylib_cs.Color.DARKPURPLE;
        public static ColorInfo BEIGE { get; } = Raylib_cs.Color.BEIGE;
        public static ColorInfo BROWN { get; } = Raylib_cs.Color.BROWN;
        public static ColorInfo DARKBROWN { get; } = Raylib_cs.Color.DARKBROWN;
        public static ColorInfo WHITE { get; } = Raylib_cs.Color.WHITE;
        public static ColorInfo BLACK { get; } = Raylib_cs.Color.BLACK;
        public static ColorInfo BLANK { get; } = Raylib_cs.Color.BLANK;
        public static ColorInfo MAGENTA { get; } = Raylib_cs.Color.MAGENTA;
        public static ColorInfo RAYWHITE { get; } = Raylib_cs.Color.RAYWHITE;

        public ColorInfo(byte r, byte g, byte b, byte a = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static implicit operator Raylib_cs.Color(ColorInfo c) => new(c.r, c.g, c.b, c.a);

        public static implicit operator ColorInfo(Raylib_cs.Color c) => new(c.r, c.g, c.b, c.a);

        public static ColorInfo FromHSVA(float hue, float saturation, float value, float alpha = 1)
        {
            var c = value * saturation;
            var x = c * (1 - Math.Abs(((hue / 60f) % 2) - 1));
            var m = value - c;

            Vector3 color = hue switch
            {
                >= 0 and < 60 => new(c, x, 0),
                >= 60 and < 120 => new(x, c, 0),
                >= 120 and < 180 => new(0, c, x),
                >= 180 and < 240 => new(0, x, c),
                >= 240 and < 300 => new(x, 0, c),
                >= 300 and < 360 => new(c, 0, x),
                _ => throw new NotImplementedException("Hues outside of the range [0, 360) are not supported.")
            };

            byte adjust(float x) => (byte)Math.Round((x + m) * 255);

            return new ColorInfo(adjust(color.X), adjust(color.Y), adjust(color.Z), (byte)Math.Round(alpha * 255));
        }

        public static ColorInfo FromHSV(Vector3 hsv) => FromHSVA(hsv.X, hsv.Y, hsv.Z, 1);

        /// <param name="hsva">
        ///     <list type="bullet">
        ///         <item>
        ///             <see cref="Vector4.X"/> - hue
        ///         </item>
        ///         <item>
        ///             <see cref="Vector4.Y"/> - saturation
        ///         </item>
        ///         <item>
        ///             <see cref="Vector4.Z"/> - value
        ///         </item>
        ///         <item>
        ///             <see cref="Vector4.W"/> - alpha (0.0 to 1.0)
        ///         </item>
        ///     </list>
        /// </param>
        public static ColorInfo FromHSVA(Vector4 hsva) => FromHSVA(hsva.X, hsva.Y, hsva.Z, hsva.W);

        public Vector3 ToHSV() => Raylib_cs.Raylib.ColorToHSV(this);

        public Vector4 ToHSVA() => new(ToHSV(), a / 255f);

        public ColorInfo WithAlpha(float alpha) => Raylib_cs.Raylib.ColorAlpha(this, alpha);

        public ColorInfo MultiplyBrightness(float percent)
        {
            byte adjust(byte x) => (byte)Math.Clamp(x * percent, 0, 255);
            return new(adjust(r), adjust(g), adjust(b), a);
        }

        public override string ToString() => new Vector4(r, g, b, a).ToString();
    }
}