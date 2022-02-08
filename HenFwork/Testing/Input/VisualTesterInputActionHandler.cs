// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Input;
using System.Collections.Generic;

namespace HenFwork.Testing.Input
{
    public class VisualTesterInputActionHandler : InputActionHandler<VisualTesterControls>
    {
        public VisualTesterInputActionHandler(Inputs inputs) : base(inputs)
        {
        }

        protected override Dictionary<VisualTesterControls, IList<Keybind>> CreateDefaultKeybindings() => new()
        {
            { VisualTesterControls.PreviousScene, new List<Keybind> { new(KeyboardKey.KEY_PAGE_UP) } },
            { VisualTesterControls.NextScene, new List<Keybind> { new(KeyboardKey.KEY_PAGE_DOWN) } }
        };
    }
}