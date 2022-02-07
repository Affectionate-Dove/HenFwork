// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Input.UI;
using HenFwork.MapEditing.Input;
using HenFwork.Testing;
using System;

namespace HenFwork.MapEditing.VisualTests
{
    public class MapEditingVisualTestsGame : Game
    {
        private readonly MapEditingInputActionHandler inputActionHandler;
        private readonly PositionalInterfaceInputManager positionalInterfaceInputManager;

        public MapEditingVisualTestsGame()
        {
            var visualTesterScreen = new MapEditingVisualTester(Inputs);

            inputActionHandler = new(Inputs);
            inputActionHandler.Propagator.Listeners.Add(visualTesterScreen);

            positionalInterfaceInputManager = new(Inputs, ScreenStack);

            ScreenStack.Push(visualTesterScreen);
        }

        protected override void OnUpdate()
        {
            inputActionHandler.Update();
            positionalInterfaceInputManager.Update();
            base.OnUpdate();
        }

        private class MapEditingVisualTester : VisualTester, IInterfaceComponent<EditorControls>
        {
            public event Action<IInterfaceComponent<EditorControls>> FocusRequested
            { add { } remove { } }

            public bool AcceptsFocus => false;

            public MapEditingVisualTester(Inputs inputs) : base(inputs)
            {
            }

            public bool OnActionPressed(EditorControls action) => false;

            public void OnActionReleased(EditorControls action)
            { }

            public void OnFocus()
            { }

            public void OnFocusLost()
            { }
        }
    }
}