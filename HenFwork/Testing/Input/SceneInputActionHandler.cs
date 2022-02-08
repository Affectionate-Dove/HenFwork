// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Input;
using System.Collections.Generic;

namespace HenFwork.Testing.Input
{
    public class SceneInputActionHandler : InputActionHandler<SceneControls>
    {
        public SceneInputActionHandler(Inputs inputs) : base(inputs)
        {
        }

        protected override Dictionary<SceneControls, Keybind> CreateDefaultKeybindings() => new()
        {
            { SceneControls.Back, new(KeyboardKey.KEY_ESCAPE) },
            { SceneControls.Select, new(KeyboardKey.KEY_ENTER) },
            { SceneControls.Down, new(KeyboardKey.KEY_S) },
            { SceneControls.Up, new(KeyboardKey.KEY_W) },
            { SceneControls.Left, new(KeyboardKey.KEY_A) },
            { SceneControls.Right, new(KeyboardKey.KEY_D) },
            { SceneControls.One, new(KeyboardKey.KEY_ONE) },
            { SceneControls.Two, new(KeyboardKey.KEY_TWO) },
            { SceneControls.Three, new(KeyboardKey.KEY_THREE) },
            { SceneControls.Four, new(KeyboardKey.KEY_FOUR) },
            { SceneControls.Five, new(KeyboardKey.KEY_FIVE) },
            { SceneControls.Six, new(KeyboardKey.KEY_SIX) },
            { SceneControls.Seven, new(KeyboardKey.KEY_SEVEN) },
            { SceneControls.Eight, new(KeyboardKey.KEY_EIGHT) },
            { SceneControls.Nine, new(KeyboardKey.KEY_NINE) },
            { SceneControls.Zero, new(KeyboardKey.KEY_ZERO) },
        };
    }
}