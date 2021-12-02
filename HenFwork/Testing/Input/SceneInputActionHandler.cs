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

        protected override Dictionary<SceneControls, ISet<KeyboardKey>> CreateDefaultKeybindings() => new()
        {
            { SceneControls.Back, new HashSet<KeyboardKey> { KeyboardKey.KEY_ESCAPE } },
            { SceneControls.Select, new HashSet<KeyboardKey> { KeyboardKey.KEY_ENTER } },
            { SceneControls.Down, new HashSet<KeyboardKey> { KeyboardKey.KEY_S } },
            { SceneControls.Up, new HashSet<KeyboardKey> { KeyboardKey.KEY_W } },
            { SceneControls.Left, new HashSet<KeyboardKey> { KeyboardKey.KEY_A } },
            { SceneControls.Right, new HashSet<KeyboardKey> { KeyboardKey.KEY_D } },
            { SceneControls.One, new HashSet<KeyboardKey> { KeyboardKey.KEY_ONE } },
            { SceneControls.Two, new HashSet<KeyboardKey> { KeyboardKey.KEY_TWO } },
            { SceneControls.Three, new HashSet<KeyboardKey> { KeyboardKey.KEY_THREE } },
            { SceneControls.Four, new HashSet<KeyboardKey> { KeyboardKey.KEY_FOUR } },
            { SceneControls.Five, new HashSet<KeyboardKey> { KeyboardKey.KEY_FIVE } },
            { SceneControls.Six, new HashSet<KeyboardKey> { KeyboardKey.KEY_SIX } },
            { SceneControls.Seven, new HashSet<KeyboardKey> { KeyboardKey.KEY_SEVEN } },
            { SceneControls.Eight, new HashSet<KeyboardKey> { KeyboardKey.KEY_EIGHT } },
            { SceneControls.Nine, new HashSet<KeyboardKey> { KeyboardKey.KEY_NINE } },
            { SceneControls.Zero, new HashSet<KeyboardKey> { KeyboardKey.KEY_ZERO } },
        };
    }
}