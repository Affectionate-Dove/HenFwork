// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d.Worlds;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.Testing.Worlds;
using HenFwork.Worlds.Functional;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.VisualTests.Graphics2d.Worlds
{
    public class WorldViewer2dTestScene : VisualTestScene
    {
        private readonly WorldViewer2d worldViewer2d;
        private readonly NodeWorld world;

        public override Dictionary<List<SceneControls>, string> ControlsDescriptions { get; } = new()
        {
            [new() { SceneControls.One }] = "decrease grid spacing by 0.5",
            [new() { SceneControls.Two }] = "increase grid spacing by 0.5",
            [new() { SceneControls.Up, SceneControls.Down, SceneControls.Left, SceneControls.Right }] = "move view",
            [new() { SceneControls.Nine, SceneControls.Zero }] = "increase/decrease zoom",
        };

        public WorldViewer2dTestScene()
        {
            world = WorldTestingUtilities.GetExampleWorld();
            worldViewer2d = new WorldViewer2d(world)
            {
                Size = new Vector2(200),
                Anchor = new Vector2(0.5f),
                Origin = new Vector2(0.5f)
            };
            AddChild(worldViewer2d);
        }

        public override bool OnActionPressed(SceneControls action)
        {
            switch (action)
            {
                case SceneControls.One:
                    worldViewer2d.GridDistance -= 0.5f;
                    return true;

                case SceneControls.Two:
                    worldViewer2d.GridDistance += 0.5f;
                    return true;

                case SceneControls.Up:
                    worldViewer2d.Target += new Vector2(0, 1);
                    return true;

                case SceneControls.Down:
                    worldViewer2d.Target -= new Vector2(0, 1);
                    return true;

                case SceneControls.Left:
                    worldViewer2d.Target -= new Vector2(1, 0);
                    return true;

                case SceneControls.Right:
                    worldViewer2d.Target += new Vector2(1, 0);
                    return true;

                case SceneControls.Nine:
                    worldViewer2d.FieldOfView -= 1;
                    return true;

                case SceneControls.Zero:
                    worldViewer2d.FieldOfView += 1;
                    return true;

                default:
                    return base.OnActionPressed(action);
            }
        }
    }
}