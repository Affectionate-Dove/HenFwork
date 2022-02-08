﻿// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
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

        protected override Dictionary<EditorControls, Keybind> CreateDefaultKeybindings() => new()
        {
            [EditorControls.Select] = new(KeyboardKey.KEY_ENTER),
            [EditorControls.Back] = new(KeyboardKey.KEY_ESCAPE),
            [EditorControls.Next] = new(KeyboardKey.KEY_TAB),
            [EditorControls.Previous] = new(KeyboardKey.KEY_LEFT_SHIFT, KeyboardKey.KEY_TAB),
        };
    }
}