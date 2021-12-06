// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d.Worlds;
using HenFwork.Testing;
using HenFwork.Testing.Input;
using HenFwork.Testing.Worlds;
using HenFwork.UI;
using HenFwork.Worlds.Functional;
using System.Numerics;

namespace HenFwork.VisualTests.Graphics2d.Worlds
{
    public class WorldViewer2dTestScene : VisualTestScene
    {
        private readonly WorldViewer2d worldViewer2d;
        private readonly NodeWorld world;

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
            AddChild(GenerateInfoText());
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

        private static SpriteText GenerateInfoText()
        {
            var t =
                "1 - decrease grid spacing by 0.5\n" +
                "2 - increase grid spacing by 0.5\n" +
                "↑/↓/←/→ - move view\n" +
                "9/0 - increase/decrease zoom";

            return new()
            {
                Anchor = new(1, 1),
                Origin = new(1, 1),
                Size = new(300, 600),
                Text = t
            };
        }
    }
}