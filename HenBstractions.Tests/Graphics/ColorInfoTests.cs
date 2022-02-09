// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using NUnit.Framework;
using System.Collections.Generic;
using System.Numerics;

namespace HenBstractions.Tests.Graphics
{
    [TestOf(typeof(ColorInfo))]
    public class ColorInfoTests
    {
        public record struct HsvRgbCase(ColorInfo Rgba, Vector4 Hsva);

        [TestCaseSource(nameof(HsvRgbCases))]
        public void ConversionTest(HsvRgbCase c)
        {
            Assert.AreEqual(c.Hsva, c.Rgba.ToHSVA());
            Assert.AreEqual(c.Rgba, ColorInfo.FromHSVA(c.Hsva));
        }

        private static IEnumerable<HsvRgbCase> HsvRgbCases()
        {
            yield return new HsvRgbCase(new(255, 51, 255, 255), new(300, .8f, 1, 1));
            yield return new HsvRgbCase(new(255, 51, 255, 127), new(300, .8f, 1, 0.49803922f));
            yield return new HsvRgbCase(new(130, 51, 0, 0), new(23.53846f, 1, .50980395f, 0f));
        }
    }
}