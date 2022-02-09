// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Graphics2d;
using HenFwork.Input.UI;
using HenFwork.Testing;
using HenFwork.UI;
using System.Numerics;

namespace HenFwork.VisualTests.Input.UI
{
    public class MouseDraggingTestScene : VisualTestScene
    {
        private readonly DraggingManager draggingManager;
        private readonly SpriteText text;

        public MouseDraggingTestScene()
        {
            AddChild(draggingManager = new DraggingManager { RelativeSizeAxes = Axes.Both });
            AddChild(text = new SpriteText());
        }

        protected override void OnUpdate(float elapsed)
        {
            base.OnUpdate(elapsed);
            text.Text = $"Dragging: {draggingManager.Dragging}\nDelta: {draggingManager.Delta}";
        }

        private class DraggingManager : Drawable, IPositionalInterfaceComponent
        {
            public bool AcceptsPositionalInput => true;

            public bool Dragging { get; private set; }

            public Vector2 Delta { get; private set; }

            public bool AcceptsPositionalButton(MouseButton button) => button is MouseButton.Left;

            public void OnClick(MouseButton button)
            { }

            public void OnHover()
            { }

            public void OnHoverLost()
            { }

            public void OnMouseDrag(MouseButton button, Vector2 delta) => Delta = delta;

            public void OnMousePress(MouseButton button) => Dragging = true;

            public void OnMouseRelease(MouseButton button) => Dragging = false;
        }
    }
}