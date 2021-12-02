// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Input;
using HenFwork.Graphics2d;
using HenFwork.Input.UI;
using HenFwork.Screens;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.UI;
using System.Numerics;

namespace HenFwork.VisualTests.Input.UI
{
    public class InterfaceInputManagerTestScene : VisualTestScene
    {
        private readonly InterfaceInputManager<TestAction> interfaceInputManager;
        private readonly TestInputActionHandler inputActionHandler;
        private readonly PositionalInterfaceInputManager positionalInterfaceInputManager;

        public InterfaceInputManagerTestScene()
        {
            inputActionHandler = new TestInputActionHandler(new RealInputs());
            var screenStack = new ScreenStack
            {
                Size = new Vector2(500, 400),
                Offset = new(100)
            };
            AddChild(screenStack);
            var screen1 = new Screen();
            screenStack.Push(screen1);
            screen1.AddChild(new Rectangle { RelativeSizeAxes = Axes.Both, Color = new(100, 200, 50) });
            var mainFillFlowContainer = new TestFillFlowContainer(0.6f, 400, 0);
            screen1.AddChild(mainFillFlowContainer);
            var header = new TestFillFlowContainer(0.5f, 70, 2);
            mainFillFlowContainer.AddChildToFlowContainer(header);

            var horizontalContainer = new TestFillFlowContainer(0.5f, 300, 0);
            horizontalContainer.FillFlowContainer.Direction = Direction.Horizontal;
            var leftContainer = new TestFillFlowContainer(0.4f, 1, 3) { RelativeSizeAxes = Axes.Y, Size = new(100, 1) };
            var rightContainer = new TestFillFlowContainer(0.4f, 1, 3) { RelativeSizeAxes = Axes.Y, Size = new(100, 1) };
            mainFillFlowContainer.AddChildToFlowContainer(horizontalContainer);
            horizontalContainer.AddChildToFlowContainer(leftContainer);
            horizontalContainer.AddChildToFlowContainer(rightContainer);

            interfaceInputManager = new InterfaceInputManager<TestAction>(screenStack, TestAction.Next);
            inputActionHandler.Propagator.Listeners.Add(interfaceInputManager);

            positionalInterfaceInputManager = new PositionalInterfaceInputManager(new RealInputs(), screenStack);
            interfaceInputManager.UpdateFocusRequestedSubscriptions();
        }

        protected override void OnUpdate(float elapsed)
        {
            inputActionHandler.Update();
            positionalInterfaceInputManager.Update();
            base.OnUpdate(elapsed);
        }

        private class TestFillFlowContainer : Container, IInterfaceComponent<TestAction>
        {
            private readonly Rectangle background;
            private readonly byte v;

            public virtual event System.Action<IInterfaceComponent<TestAction>> FocusRequested
            {
                add { }
                remove { }
            }

            public FillFlowContainer FillFlowContainer { get; }

            public bool AcceptsFocus => true;

            public TestFillFlowContainer(float brightness, int height, int buttonAmount)
            {
                Size = new(1, height);
                FillFlowContainer = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = Direction.Vertical,
                    Padding = new() { Horizontal = 10, Vertical = 10 },
                    Spacing = 10
                };
                RelativeSizeAxes = Axes.X;
                v = (byte)(brightness * 255);
                AddChild(background = new Rectangle { RelativeSizeAxes = Axes.Both, Color = new(v, v, v) });
                AddChild(FillFlowContainer);
                for (var i = 0; i < buttonAmount; i++)
                    FillFlowContainer.AddChild(new TestButton(i + 1, brightness - 0.1f));
            }

            public void AddChildToFlowContainer(Drawable drawable) => FillFlowContainer.AddChild(drawable);

            public void OnFocus() => background.Color = new(v, v, 0);

            public void OnFocusLost() => background.Color = new(v, v, v);

            public bool OnActionPressed(TestAction action) => false;

            public void OnActionReleased(TestAction action)
            {
            }
        }

        private class TestButton : Button<TestAction>, IPositionalInterfaceComponent
        {
            private readonly byte v;

            private int counter;

            public override bool AcceptsFocus => true;

            public override bool AcceptsPositionalInput => true;

            public TestButton(int id, float brightness)
            {
                v = (byte)(brightness * 255);
                RelativeSizeAxes = Axes.X;
                Size = new(1, 20);

                FocusedColors = new(new(0, v, v), null, null);
                var h = (byte)((brightness + 0.2) * 255);
                HoveredColors = new(new(h, h, h), null, null);
                var p = (byte)((brightness - 0.1) * 255);
                PressedColors = new(new(p, p, p), null, null);
                DisabledColors = new(new(v, v, v), null, ColorInfo.WHITE);
                counter = id;

                UpdateText();
            }

            public override bool AcceptsPositionalButton(MouseButton button) => button is MouseButton.Left or MouseButton.Right;

            public override void OnClick(MouseButton button)
            {
                counter += button == MouseButton.Left ? 1 : -1;
                UpdateText();
                base.OnClick(button);
            }

            public override bool OnActionPressed(TestAction action)
            {
                if (action is TestAction.Up)
                {
                    counter++;
                    UpdateText();
                    return true;
                }
                else if (action is TestAction.Down)
                {
                    counter--;
                    UpdateText();
                    return true;
                }
                return base.OnActionPressed(action);
            }

            private void UpdateText() => Text = counter.ToString();
        }
    }
}