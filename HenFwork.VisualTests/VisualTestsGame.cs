// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Input.UI;
using HenFwork.Testing;
using HenFwork.Testing.Input;

namespace HenFwork.VisualTests
{
    public class VisualTestsGame : Game
    {
        private readonly VisualTesterInputActionHandler inputActionHandler;
        private readonly PositionalInterfaceInputManager positionalInterfaceInputManager;

        public VisualTestsGame()
        {
            LoadResources();

            var visualTesterScreen = new VisualTester(Inputs);

            inputActionHandler = new VisualTesterInputActionHandler(Inputs);
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

        private static void LoadResources()
        {
            ModelStore.Load("Resources/Models/building_dock.obj");
            ModelStore.Load("Resources/Models/house_type01.obj");
            ModelStore.Load("Resources/Models/driveway_long.obj");
        }
    }
}