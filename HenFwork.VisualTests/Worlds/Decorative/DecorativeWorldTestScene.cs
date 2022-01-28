// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Graphics3d;
using HenFwork.Testing;
using HenFwork.Worlds.Decorative;
using System.Numerics;

namespace HenFwork.VisualTests.Worlds.Decorative
{
    public class DecorativeWorldTestScene : VisualTestScene
    {
        public DecorativeWorldTestScene()
        {
            var road = new Decoration
            {
                Position = new(0, 0, 0),
                ModelName = "Resources/Models/driveway_long.obj",
            };
            var house = new Decoration
            {
                Position = new(.76f, 0, 0),
                ModelName = "Resources/Models/house_type01.obj",
                Rotation = new(0, 0, 0),
                Scale = new(1)
            };
            var decorativeWorld = new DecorativeWorld();
            decorativeWorld.Decorations.Add(road);
            decorativeWorld.Decorations.Add(house);

            var wsm = new WorldSceneManager(decorativeWorld);
            var sceneViewer = new SceneViewer(wsm.Scene) { RelativeSizeAxes = Axes.Both };
            sceneViewer.Camera.Position = new Vector3(0, 1, -2);
            sceneViewer.Camera.LookingAt = new Vector3(0);
            AddChild(sceneViewer);
        }
    }
}