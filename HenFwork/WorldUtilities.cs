// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics3d;
using HenFwork.Worlds;

namespace HenFwork
{
    public static class WorldUtilities
    {
        public static void CreateVisualWorldPackage(in World world, out WorldSceneManager worldSceneManager, out SceneViewer sceneViewer)
        {
            worldSceneManager = new WorldSceneManager(world);
            sceneViewer = new SceneViewer(worldSceneManager.Scene);
        }
    }
}