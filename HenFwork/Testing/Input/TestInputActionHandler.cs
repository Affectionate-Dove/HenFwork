﻿// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Input;
using System.Collections.Generic;

namespace HenFwork.Testing.Input
{
    public class TestInputActionHandler : InputActionHandler<TestAction>
    {
        public TestInputActionHandler(Inputs inputs) : base(inputs)
        {
        }

        protected override Dictionary<TestAction, IList<Keybind>> CreateDefaultKeybindings() => new()
        {
            { TestAction.Up, new List<Keybind> { new(KeyboardKey.KEY_UP) } },
            { TestAction.Down, new List<Keybind> { new(KeyboardKey.KEY_DOWN) } },
            { TestAction.Left, new List<Keybind> { new(KeyboardKey.KEY_LEFT) } },
            { TestAction.Right, new List<Keybind> { new(KeyboardKey.KEY_RIGHT) } },
            { TestAction.Next, new List<Keybind> { new(KeyboardKey.KEY_TAB) } },
            { TestAction.Confirm, new List<Keybind> { new(KeyboardKey.KEY_ENTER) } }
        };
    }

    public enum TestAction
    {
        Up,
        Left,
        Down,
        Right,
        Next,
        Confirm
    }
}