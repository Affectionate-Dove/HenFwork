// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Collisions;
using HenFwork.Graphics2d;
using HenFwork.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace HenFwork.Input.UI
{
    /// <summary>
    ///     Propagates positional input events
    ///     to <see cref="IPositionalInterfaceComponent"/>s.
    /// </summary>
    public class PositionalInterfaceInputManager
    {
        private readonly List<ButtonInfo> buttonsInfos = CreateButtonStates();

        private readonly List<IPositionalInterfaceComponent> hoveredComponents = new();
        private Vector2? lastMousePosition;

        public Inputs Inputs { get; }

        /// <summary>
        ///     The <see cref="Screens.ScreenStack"/>, inside
        ///     of which all handled <see cref="IPositionalInterfaceComponent"/> are.
        /// </summary>
        public ScreenStack ScreenStack { get; }

        public PositionalInterfaceInputManager(Inputs inputs, ScreenStack screenStack)
        {
            Inputs = inputs;
            ScreenStack = screenStack;
        }

        /// <summary>
        ///     Checks for events and propagates them.
        /// </summary>
        public void Update()
        {
            foreach (var buttonInfo in buttonsInfos)
                buttonInfo.Pressed = this.Inputs.IsMouseButtonDown((MouseButton)buttonInfo.MouseButton);

            if (ScreenStack.CurrentScreen is not null)
                HandleDrawable(ScreenStack.CurrentScreen);

            HandleLostHovers();

            lastMousePosition = Inputs.MousePosition;
        }

        private static List<ButtonInfo> CreateButtonStates()
        {
            var list = new List<ButtonInfo>();
            foreach (MouseButton button in Enum.GetValues(typeof(MouseButton)))
                list.Add(new(button));
            return list;
        }

        private void HandleDrawable(Drawable drawable)
        {
            if (!ElementaryCollisions.IsPointInRectangle(this.Inputs.MousePosition, drawable.LayoutInfo.RenderRect))
                return;

            if (drawable is IContainer<Drawable> container)
            {
                // elements highest visually, so last in container.Children,
                // should be considered first
                foreach (var child in container.Children.AsEnumerable().Reverse())
                    HandleDrawable(child);
            }

            if (drawable is IPositionalInterfaceComponent component && component.AcceptsPositionalInput)
            {
                if (!hoveredComponents.Contains(component))
                {
                    component.OnHover();
                    hoveredComponents.Add(component);
                }

                foreach (var buttonInfo in buttonsInfos)
                {
                    // the button was just pressed and no descendants handled it,
                    // and this component wants to handle it
                    if (buttonInfo.JustPressed && buttonInfo.CurrentlyHandledBy is null && component.AcceptsPositionalButton(buttonInfo.MouseButton))
                    {
                        buttonInfo.CurrentlyHandledBy = component;
                        component.OnMousePress(buttonInfo.MouseButton);
                    }
                    else if (buttonInfo.JustReleased && buttonInfo.CurrentlyHandledBy == component)
                    {
                        component.OnMouseRelease(buttonInfo.MouseButton);
                        component.OnClick(buttonInfo.MouseButton);
                        buttonInfo.CurrentlyHandledBy = null;
                    }
                    else if (buttonInfo.CurrentlyHandledBy == component && buttonInfo.Pressed && buttonInfo.PressedPreviously)
                    {
                        var mousePositionDelta = Inputs.MousePosition - lastMousePosition.Value;
                        if (mousePositionDelta != Vector2.Zero)
                            component.OnMouseDrag(buttonInfo.MouseButton, mousePositionDelta);
                    }
                }
            }
        }

        private void HandleLostHovers() => hoveredComponents.RemoveAll(TryDoHoverLostTasks);

        /// <returns>
        ///     Whether the <paramref name="component"/> lost hover.
        /// </returns>
        private bool TryDoHoverLostTasks(IPositionalInterfaceComponent component)
        {
            if (ElementaryCollisions.IsPointInRectangle(this.Inputs.MousePosition, component.LayoutInfo.RenderRect))
                return false;

            component.OnHoverLost();
            foreach (var buttonInfo in buttonsInfos.Where(bi => bi.CurrentlyHandledBy == component))
            {
                component.OnMouseRelease(buttonInfo.MouseButton);
                buttonInfo.CurrentlyHandledBy = null;
            }
            return true;
        }

        [DebuggerDisplay("{" + nameof(MouseButton) + "}: {" + nameof(Pressed) + " ? true : false}")]
        private class ButtonInfo
        {
            private bool pressed;

            public MouseButton MouseButton { get; }
            public bool PressedPreviously { get; private set; }

            public bool Pressed
            {
                get => pressed;
                set
                {
                    PressedPreviously = pressed;
                    pressed = value;
                }
            }

            public bool JustChanged => Pressed != PressedPreviously;
            public bool JustPressed => Pressed && !PressedPreviously;
            public bool JustReleased => !Pressed && PressedPreviously;

            public IPositionalInterfaceComponent CurrentlyHandledBy { get; set; }

            public ButtonInfo(MouseButton mouseButton) => MouseButton = mouseButton;
        }
    }
}