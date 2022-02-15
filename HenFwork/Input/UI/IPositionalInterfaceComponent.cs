// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using HenFwork.Graphics2d.Layouts;
using System.Numerics;

namespace HenFwork.Input.UI
{
    public interface IPositionalInterfaceComponent
    {
        /// <summary>
        ///     If returns <see langword="true"/>,
        ///     will be considered for positional input
        ///     and will receive hover and positional button events.
        /// </summary>
        bool AcceptsPositionalInput { get; }

        DrawableLayoutInfo LayoutInfo { get; }

        /// <summary>
        ///     If returns <see langword="true"/> for a <paramref name="button"/>,
        ///     mouse presses and releases with this button
        ///     will be considered for this component.
        /// </summary>
        bool AcceptsPositionalButton(MouseButton button);

        /// <summary>
        ///     Called when the cursor enters the boundaries of this
        ///     <see cref="IPositionalInterfaceComponent"/>.
        /// </summary>
        void OnHover();

        /// <summary>
        ///     Called when the cursor leaves the boundaries of this
        ///     <see cref="IPositionalInterfaceComponent"/> after entering them.
        /// </summary>
        void OnHoverLost();

        void OnMousePress(MouseButton button);

        /// <summary>
        ///     Called when either the <paramref name="button"/> is released
        ///     or the cursor leaves the <see cref="IPositionalInterfaceComponent"/>
        ///     after the <paramref name="button"/> was
        ///     pressed on this <see cref="IPositionalInterfaceComponent"/>
        /// </summary>
        void OnMouseRelease(MouseButton button);

        /// <summary>
        ///     Called when a <paramref name="button"/> is pressed down
        ///     on this <see cref="IPositionalInterfaceComponent"/> and the mouse moves.
        /// </summary>
        void OnMouseDrag(MouseButton button, Vector2 delta) { }

        /// <summary>
        ///     Called after the <paramref name="button"/>
        ///     is both pressed and released on this
        ///     <see cref="IPositionalInterfaceComponent"/>,
        ///     without the cursor ever leaving its boundaries.
        /// </summary>
        void OnClick(MouseButton button);

        void OnMouseScroll(float delta) { }
    }
}