// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Input;
using System.Collections.Generic;

namespace HenFwork.Tests.Input
{
    public class TestInputActionHandler : InputActionHandler<TestAction>
    {
        public TestInputActionHandler(Inputs inputs) : base(inputs)
        {
        }

        protected override Dictionary<TestAction, IList<Keybind>> CreateDefaultKeybindings() => new()
        {
            { TestAction.Action1, new List<Keybind> { new(KeyboardKey.KEY_A) } },
            { TestAction.Action2, new List<Keybind> { new(KeyboardKey.KEY_LEFT_CONTROL, KeyboardKey.KEY_S) } },
        };
    }

    public enum TestAction
    {
        Action1,
        Action2
    }
}