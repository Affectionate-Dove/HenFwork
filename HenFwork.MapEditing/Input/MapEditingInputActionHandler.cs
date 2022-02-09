// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Input;
using System.Collections.Generic;

namespace HenFwork.MapEditing.Input
{
    public class MapEditingInputActionHandler : InputActionHandler<EditorControls>
    {
        public MapEditingInputActionHandler(Inputs inputs) : base(inputs)
        {
        }

        protected override Dictionary<EditorControls, IList<Keybind>> CreateDefaultKeybindings() => new()
        {
            [EditorControls.Select] = new List<Keybind> { new(KeyboardKey.KEY_ENTER) },
            [EditorControls.Back] = new List<Keybind> { new(KeyboardKey.KEY_ESCAPE) },
            [EditorControls.Next] = new List<Keybind> { new(KeyboardKey.KEY_TAB) },
            [EditorControls.Previous] = new List<Keybind> { new(KeyboardKey.KEY_LEFT_SHIFT, KeyboardKey.KEY_TAB) },
            [EditorControls.MoveForward] = new List<Keybind> { new(KeyboardKey.KEY_W) },
            [EditorControls.MoveBackward] = new List<Keybind> { new(KeyboardKey.KEY_S) },
            [EditorControls.MoveLeft] = new List<Keybind> { new(KeyboardKey.KEY_A) },
            [EditorControls.MoveRight] = new List<Keybind> { new(KeyboardKey.KEY_D) },
            [EditorControls.MoveUp] = new List<Keybind> { new(KeyboardKey.KEY_SPACE) },
            [EditorControls.MoveDown] = new List<Keybind> { new(KeyboardKey.KEY_LEFT_SHIFT) }
        };
    }
}