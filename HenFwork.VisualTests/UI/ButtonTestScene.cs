// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Input.UI;
using HenFwork.Screens;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.UI;

namespace HenFwork.VisualTests.UI
{
    public class ButtonTestScene : VisualTestScene
    {
        private readonly TestInputActionHandler actionHandler;
        private readonly SpriteText actionCountSpriteText;
        private PositionalInterfaceInputManager positionalInterfaceManager;
        private int actionCount;

        public ButtonTestScene()
        {
            AddChild(actionCountSpriteText = new SpriteText
            {
                Offset = new(320, 10),
                Size = new(50, 50),
                Text = 0.ToString()
            });

            var enabledButton1 = CreateButton();
            enabledButton1.Action = () =>
            {
                actionCount++;
                actionCountSpriteText.Text = actionCount.ToString();
            };
            enabledButton1.Text = "Increase counter";
            enabledButton1.Offset = new(10, 10);
            AddChild(enabledButton1);

            var disabledButton = CreateButton();
            disabledButton.Text = "Disabled button";
            disabledButton.Offset = new(10, 50);
            AddChild(disabledButton);

            var enabledButton2 = CreateButton();
            enabledButton2.Action = () =>
            {
                actionCount--;
                actionCountSpriteText.Text = actionCount.ToString();
            };
            enabledButton2.Text = "Decrease counter";
            enabledButton2.Offset = new(10, 90);
            AddChild(enabledButton2);

            actionHandler = new TestInputActionHandler(new RealInputs());
        }

        protected override void OnUpdate(float elapsed)
        {
            if (actionHandler.Propagator.Listeners.Count is 0 && Parent is not null)
            {
                var interfaceManager = new InterfaceInputManager<TestAction>(Parent as ScreenStack, TestAction.Next);
                actionHandler.Propagator.Listeners.Add(interfaceManager);
                positionalInterfaceManager = new PositionalInterfaceInputManager(new RealInputs(), Parent as ScreenStack);
                interfaceManager.UpdateFocusRequestedSubscriptions();
            }
            base.OnUpdate(elapsed);
            positionalInterfaceManager.Update();
            actionHandler.Update();
        }

        private static Button<TestAction> CreateButton()
        {
            var button = new Button<TestAction> { Size = new(300, 30) };
            button.AcceptedActions.Add(TestAction.Confirm);
            return button;
        }
    }
}