// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;

namespace HenFwork.Graphics3d
{
    public class SceneViewer : Drawable
    {
        private RenderTexture renderTexture;

        public Scene Scene { get; }
        public Camera Camera { get; }

        public SceneViewer(Scene scene)
        {
            Camera = new();
            Scene = scene;
        }

        protected override void OnLayoutUpdate()
        {
            base.OnLayoutUpdate();

            renderTexture = new(LayoutInfo.RenderSize);
        }

        protected override void OnRender()
        {
            Drawing.BeginTextureMode(renderTexture);
            Drawing.ClearBackground(ColorInfo.BROWN);

            Camera.Update();
            Drawing.BeginMode3D(Camera);

            foreach (var spatial in Scene.Spatials)
                spatial.Render();

            Drawing.EndMode3D();

            Drawing.EndTextureMode();
            Drawing.DrawTexture(renderTexture, LayoutInfo.RenderPosition);

            base.OnRender();
        }

        protected override void OnUpdate(float elapsed)
        {
            base.OnUpdate(elapsed);
            foreach (var spatial in Scene.Spatials)
                spatial.Update(elapsed);
        }
    }
}