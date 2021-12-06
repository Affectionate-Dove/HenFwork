// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Numerics;
using HenFwork.Graphics2d;
using HenFwork.Graphics3d;
using HenFwork.Graphics3d.Shapes;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.UI;
using HenFwork.Worlds.Functional;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Numerics;

namespace HenFwork.VisualTests.Graphics3d
{
    public class WorldSceneManagerTestScene : VisualTestScene
    {
        private readonly WorldSceneManager worldSceneManager;
        private readonly SceneViewer sceneViewer;
        private readonly NodeWorld world;
        private readonly ChunksAreaObserverVisualizer observerVisualizer;

        public WorldSceneManagerTestScene()
        {
            world = new NodeWorld(new(50), 1);
            world.AddNode(new TestNode(5, -2, ColorInfo.BLUE));
            const float medium_height = -0.5f;
            world.AddNode(new TestNode(15, 0, ColorInfo.GREEN));
            world.AddNode(new TestNode(24.5f, 2, ColorInfo.RED));
            world.AddMedium(new Medium
            {
                Triangle = new(new(10, medium_height, 10), new(25, medium_height, 25), new(40, medium_height, 10)),
                Type = MediumType.Ground
            });
            world.AddMedium(new Medium
            {
                Triangle = new(new(10, medium_height, 40), new(25, medium_height, 25), new(10, medium_height, 10)),
                Type = MediumType.Water
            });
            world.AddMedium(new Medium
            {
                Triangle = new(new(40, medium_height, 40), new(25, medium_height, 25), new(10, medium_height, 40)),
                Type = MediumType.Air
            });

            worldSceneManager = new WorldSceneManager(world)
            {
                ViewPoint = new Vector2(25),
                ViewDistance = 10
            };
            worldSceneManager.NodeSpatialCreator = (Node node) =>
            {
                var n = node as TestNode;
                var s = new BoxSpatial { Box = Box.FromPositionAndSize(new(0), new(1), new(0.5f)), Color = n.Color };
                return s;
            };

            AddChild(sceneViewer = new SceneViewer(worldSceneManager.Scene)
            {
                RelativeSizeAxes = Axes.Both
            });
            sceneViewer.Camera.Position = new(0, 30, 0);
            sceneViewer.Camera.LookingAt = new(25, -20, 25);
            AddChild(sceneViewer);

            observerVisualizer = new ChunksAreaObserverVisualizer(worldSceneManager.Observer, worldSceneManager.Scene);

            AddChild(new SpriteText
            {
                RelativeSizeAxes = Axes.Both,
                Text = "W/A/S/D - move loaded area\n" +
                "1/2 - decrease/increase loaded area\n" +
                "3 - toggle loaded chunks visibility\n" +
                "4 - toggle loaded area visibility"
            });
        }

        public override bool OnActionPressed(SceneControls action) => action is SceneControls.One or SceneControls.Two or SceneControls.Left or SceneControls.Right or SceneControls.Up or SceneControls.Down or SceneControls.Three or SceneControls.Four;

        public override void OnActionReleased(SceneControls action)
        {
            switch (action)
            {
                case SceneControls.Left:
                    worldSceneManager.ViewPoint -= Vector2.UnitX;
                    break;

                case SceneControls.Right:
                    worldSceneManager.ViewPoint += Vector2.UnitX;
                    break;

                case SceneControls.Up:
                    worldSceneManager.ViewPoint += Vector2.UnitY;
                    break;

                case SceneControls.Down:
                    worldSceneManager.ViewPoint -= Vector2.UnitY;
                    break;

                case SceneControls.One:
                    if (worldSceneManager.ViewDistance >= 2)
                        worldSceneManager.ViewDistance -= 2;
                    break;

                case SceneControls.Two:
                    worldSceneManager.ViewDistance += 2;
                    break;

                case SceneControls.Three:
                    observerVisualizer.ChunksVisible = !observerVisualizer.ChunksVisible;
                    break;

                case SceneControls.Four:
                    observerVisualizer.ObservedAreaVisible = !observerVisualizer.ObservedAreaVisible;
                    break;
            }
        }

        protected override void OnUpdate(float elapsed)
        {
            world.Simulate(world.SynchronizedTime + elapsed);
            worldSceneManager.Update();
            base.OnUpdate(elapsed);
        }

        private class TestNode : Node
        {
            private readonly float sinMultiplier;
            private readonly int offset;

            public ColorInfo Color { get; }

            public TestNode(float sinMultiplier, int offset, ColorInfo color)
            {
                this.sinMultiplier = sinMultiplier;
                this.offset = offset;
                Color = color;
            }

            protected override void Simulation(double duration)
            {
                Position = new(25 + offset, 0, 25 + (MathF.Sin((float)SynchronizedTime * 1.5f) * sinMultiplier));
                base.Simulation(duration);
            }
        }
    }
}