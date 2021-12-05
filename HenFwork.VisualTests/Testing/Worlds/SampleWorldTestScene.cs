// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Graphics3d;
using HenFwork.Testing;
using HenFwork.Testing.Worlds;

namespace HenFwork.VisualTests.Testing.Worlds
{
    public class SampleWorldTestScene : VisualTestScene
    {
        public SampleWorldTestScene()
        {
            WorldUtilities.CreateVisualWorldPackage(new SampleWorld(), out var worldSceneManager, out var sceneViewer);
            _ = new ChunksAreaObserverVisualizer(worldSceneManager.Observer, worldSceneManager.Scene)
            {
                ChunksVisible = true
            };
            sceneViewer.RelativeSizeAxes = Axes.Both;
            AddChild(sceneViewer);
            sceneViewer.Camera.Position = new(25, 50, 25);
            sceneViewer.Camera.Rotation = new(-89, 0, 0);
            worldSceneManager.ViewDistance = 100;
            worldSceneManager.Update();
        }
    }
}