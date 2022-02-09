// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Input.UI;
using HenFwork.MapEditing.Input;
using HenFwork.Testing;

namespace HenFwork.MapEditing.VisualTests
{
    public class MapEditingVisualTestsGame : Game
    {
        private readonly MapEditingInputActionHandler inputActionHandler;
        private readonly PositionalInterfaceInputManager positionalInterfaceInputManager;
        private readonly InterfaceInputManager<EditorControls> interfaceInputManager;

        public MapEditingVisualTestsGame()
        {
            interfaceInputManager = new(ScreenStack, EditorControls.Next);

            inputActionHandler = new(Inputs);
            inputActionHandler.Propagator.Listeners.Add(interfaceInputManager);

            positionalInterfaceInputManager = new(Inputs, ScreenStack);

            ScreenStack.Push(new VisualTester(Inputs));
        }

        protected override void OnUpdate()
        {
            inputActionHandler.Update();
            positionalInterfaceInputManager.Update();
            base.OnUpdate();
        }
    }
}