// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Input;
using HenFwork.Graphics2d;
using HenFwork.Input;
using HenFwork.Screens;
using HenFwork.Testing.Input;
using HenFwork.Testing.UI;
using HenFwork.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HenFwork.Testing
{
    public class VisualTester : Screen, IInputListener<VisualTesterControls>
    {
        private readonly FillFlowContainer scenesList;
        private readonly ScreenStack scenesContainer;
        private readonly List<Type> sceneTypes;
        private readonly List<TestSceneButton> buttons;
        private readonly SceneInputActionHandler sceneInputActionHandler;
        private readonly TestSceneInfoOverlay testSceneInfoOverlay;
        private int sceneIndex;

        public VisualTester(Inputs inputs)
        {
            sceneInputActionHandler = new SceneInputActionHandler(inputs);

            RelativeSizeAxes = Axes.Both;

            scenesList = CreateScenesList();
            scenesContainer = new ScreenStack { RelativeSizeAxes = Axes.Both };
            var leftContainer = CreateLeftContainer();
            AddChild(leftContainer);

            sceneTypes = new List<Type>();
            buttons = new List<TestSceneButton>();
            CreateAndAddButtons();

            if (sceneTypes.Count is 0)
            {
                leftContainer.AddChild(new SpriteText
                {
                    Text = "No test scenes.",
                    RelativeSizeAxes = Axes.Both,
                    TextAlignment = new(0.5f)
                });
                return;
            }

            var rightContainer = new Container
            {
                Padding = new MarginPadding { Left = 200 },
                RelativeSizeAxes = Axes.Both
            };
            AddChild(rightContainer);
            rightContainer.AddChild(scenesContainer);

            AddChild(testSceneInfoOverlay = new TestSceneInfoOverlay
            {
                AutoSizeAxes = Axes.Y,
                Size = new(300, 0),
                Anchor = new(1, 0),
                Origin = new(1, 0)
            });

            ChangeScene();

            HotReloadManager.HotReloaded += OnHotReload;
        }

        public bool OnActionPressed(VisualTesterControls action)
        {
            if (action == VisualTesterControls.NextScene)
                sceneIndex++;
            else if (action == VisualTesterControls.PreviousScene)
                sceneIndex--;
            else
                return false;

            ChangeScene();
            return true;
        }

        public void OnActionReleased(VisualTesterControls action)
        {
        }

        protected override void OnUpdate(float elapsed)
        {
            base.OnUpdate(elapsed);
            if (sceneIndex < sceneTypes.Count - 1 && (scenesContainer.CurrentScreen as VisualTestScene).IsSceneDone)
            {
                sceneIndex++;
                ChangeScene();
            }
            sceneInputActionHandler.Update();
        }

        private static FillFlowContainer CreateScenesList() => new()
        {
            RelativeSizeAxes = Axes.Y,
            Size = new System.Numerics.Vector2(200, 1),
            Padding = new MarginPadding { Horizontal = 5, Vertical = 5 },
            Spacing = 5,
            Direction = Direction.Vertical
        };

        private void OnHotReload() => ChangeScene();

        private VisualTestScene CreateVisualTestScene(Type type)
        {
            var visualTestScene = Activator.CreateInstance(type) as VisualTestScene;
            sceneInputActionHandler.Propagator.Listeners.Add(visualTestScene);
            return visualTestScene;
        }

        private void CreateAndAddButtons()
        {
            var visualTestSceneTypes = Assembly.GetEntryAssembly().GetTypes()
                .Where(type => type.IsSubclassOf(typeof(VisualTestScene)));

            foreach (var type in visualTestSceneTypes)
            {
                sceneTypes.Add(type);
                var button = new TestSceneButton(type)
                {
                    RelativeSizeAxes = Axes.X,
                    Size = new System.Numerics.Vector2(1, 20)
                };
                button.FocusRequested += OnButtonFocusRequested;
                scenesList.AddChild(button);
                buttons.Add(button);
            }
        }

        private void OnButtonFocusRequested(HenFwork.Input.UI.IInterfaceComponent<VisualTesterControls> component)
        {
            var button = component as TestSceneButton;
            sceneIndex = sceneTypes.IndexOf(button.Type);
            ChangeScene();
        }

        private Container CreateLeftContainer()
        {
            var leftContainer = new Container
            {
                RelativeSizeAxes = Axes.Y,
                Size = new System.Numerics.Vector2(200, 1),
            };
            leftContainer.AddChild(new Rectangle
            {
                RelativeSizeAxes = Axes.Both,
                Color = new ColorInfo(40, 40, 40)
            });
            leftContainer.AddChild(scenesList);

            return leftContainer;
        }

        private void ChangeScene()
        {
            if (sceneTypes.Count == 0)
                return; // take no action if there are no test scenes

            // make sure the index is valid (loop around)
            if (sceneIndex >= sceneTypes.Count)
                sceneIndex = 0;
            else if (sceneIndex < 0)
                sceneIndex = sceneTypes.Count - 1;

            if (scenesContainer.CurrentScreen is not null)
            {
                scenesContainer.Pop();
                sceneInputActionHandler.Propagator.Listeners.Clear();
            }

            var scene = CreateVisualTestScene(sceneTypes[sceneIndex]);
            scenesContainer.Push(scene);
            testSceneInfoOverlay.ChangeScene(scene);
            UpdateButtonsColors();
        }

        private void UpdateButtonsColors()
        {
            foreach (var button in buttons)
            {
                if (button.Type == sceneTypes[sceneIndex])
                    button.OnFocus();
                else
                    button.OnFocusLost();
            }
        }

        private class TestSceneButton : Button<VisualTesterControls>
        {
            public Type Type { get; }

            public override bool AcceptsPositionalInput => true;

            public TestSceneButton(Type type)
            {
                Type = type;

                var typeInfo = type.GetTypeInfo();
                var testSceneName = typeInfo.GetCustomAttribute<TestSceneNameAttribute>()?.Name ??
                    type.Name.Replace("TestScene", null);

                Text = testSceneName;

                FocusedColors = new(new(100, 100, 100), null, null);
                DisabledColors = new(new(60, 60, 60), null, new(250, 250, 250));
            }
        }
    }
}