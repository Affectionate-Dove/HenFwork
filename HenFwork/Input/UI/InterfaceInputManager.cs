// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Screens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HenFwork.Input.UI
{
    /// <summary>
    ///     Handles user interface components'
    ///     (<seealso cref="IInterfaceComponent{TInputAction}"/>)
    ///     focus and input.
    /// </summary>
    public class InterfaceInputManager<TInputAction> : IInputListener<TInputAction> where TInputAction : struct, Enum
    {
        private readonly Stack<ContainerAndEnumerator> containersStack = new();
        private readonly InputPropagator<TInputAction> inputPropagator;
        private readonly List<IInterfaceComponent<TInputAction>> focusedComponents = new();
        private readonly List<IInterfaceComponent<TInputAction>> subscribedTo = new();

        /// <summary>
        ///     The <see cref="Screens.ScreenStack"/>, inside
        ///     of which all handled <see cref="IInterfaceComponent{TInputAction}"/> are.
        /// </summary>
        public ScreenStack ScreenStack { get; }

        /// <summary>
        ///     The <seealso cref="IInterfaceComponent{TInputAction}"/>s
        ///     that currently have focus.
        /// </summary>
        /// <remarks>
        ///     The components that are the deepest
        ///     in the <see cref="Drawable"/> tree are last.
        /// </remarks>
        // TODO: On each get, validate whether the components
        // are still available in the drawable tree.
        public IReadOnlyList<IInterfaceComponent<TInputAction>> FocusedComponents => focusedComponents;

        /// <summary>
        ///     The <typeparamref name="TInputAction"/> that
        ///     triggers the <see cref="FocusNextComponent"/> function.
        /// </summary>
        public TInputAction? NextComponentAction { get; set; }

        public InterfaceInputManager(ScreenStack screenStack)
        {
            ScreenStack = screenStack;
            screenStack.ScreenPushed += OnScreenStackPushOrPop;
            screenStack.ScreenPopped += OnScreenStackPushOrPop;
            inputPropagator = new();
            inputPropagator.Listeners.Add(new NextComponentActionListener(this));
        }

        public InterfaceInputManager(ScreenStack screenStack, TInputAction nextComponentAction) : this(screenStack) => NextComponentAction = nextComponentAction;

        /// <summary>
        ///     Finds the next <see cref="IInterfaceComponent{TInputAction}"/>
        ///     in the <see cref="Drawable"/> tree inside of <see cref="ScreenStack"/>
        ///     that doesn't have a descendant of type <see cref="IInterfaceComponent{TInputAction}"/>,
        ///     and focuses it and all its ancestors
        ///     that are also a <see cref="IInterfaceComponent{TInputAction}"/>.
        /// </summary>
        /// <remarks>
        ///     When a <see cref="Container"/> has <see cref="IInterfaceComponent{TInputAction}"/>
        ///     somewhere down the <see cref="Drawable"/> tree and is also
        ///     a <see cref="IInterfaceComponent{TInputAction}"/> itself,
        ///     it gets focused whenever that descendant gets focused.
        /// </remarks>
        public void FocusNextComponent() => FocusNextComponent(null);

        /// <summary>
        ///     Removes focus from all <see cref="FocusedComponents"/>.
        /// </summary>
        /// <remarks>
        ///     The <see cref="FocusNextComponent"/> function will begin
        ///     seeking from the beginning the next time it's called.
        /// </remarks>
        public void Unfocus()
        {
            containersStack.Clear();
            foreach (var component in FocusedComponents.ToList())
                DoUnfocusTasks(component);
        }

        /// <summary>
        ///     Focuses the <paramref name="component"/>.
        /// </summary>
        /// <remarks>
        ///     Internally calls <see cref="FocusNextComponent"/>
        ///     until it finds the <paramref name="component"/>.
        /// </remarks>
        /// <param name="component">
        ///     The component to focus.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the <paramref name="component"/> isn't inside the
        ///     <see cref="ScreenStack"/>'s <see cref="Drawable"/> tree.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if the <paramref name="component"/> is null.
        /// </exception>
        public void FocusComponent(IInterfaceComponent<TInputAction> component)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));
            FocusNextComponent(component);
        }

        /// <summary>
        ///     Propagates the <paramref name="action"/> press
        ///     to the <see cref="FocusedComponents"/>, starting from the deepest,
        ///     until one handles it.
        /// </summary>
        /// <remarks>
        ///     If the <paramref name="action"/> is equal to <see cref="NextComponentAction"/>,
        ///     and no <see cref="FocusedComponents"/> handled it,
        ///     this <see cref="InterfaceInputManager{TInputAction}"/> will handle this press,
        ///     and on <paramref name="action"/> release,
        ///     the <see cref="FocusNextComponent"/> function will be called.
        /// </remarks>
        /// <returns>
        ///     Whether the <see cref="FocusedComponents"/> or
        ///     this <see cref="InterfaceInputManager{TInputAction}"/>
        ///     handled the <paramref name="action"/>.
        /// </returns>
        public bool OnActionPressed(TInputAction action) => inputPropagator.OnActionPressed(action);

        /// <summary>
        ///     Propagates the <paramref name="action"/> release
        ///     to the <see cref="FocusedComponents"/>,
        ///     or calls the <see cref="FocusNextComponent"/> function.
        /// </summary>
        public void OnActionReleased(TInputAction action) => inputPropagator.OnActionReleased(action);

        /// <summary>
        ///     Looks through the whole <see cref="Drawable"/> tree, and gives focus
        ///     to the first found <see cref="IInterfaceComponent{TInputAction}"/>
        ///     with <see cref="IInterfaceComponent{TInputAction}.RequestsFocus"/>
        ///     set to <see langword="true"/>.
        /// </summary>
        // TODO: This suboptimal, should be addressed when
        // there will be a way to know when a Drawable
        // tree is modified.
        // Then, we can automatically update what's needed
        // instead of looking through everything, manually.
        public void UpdateFocusRequestedSubscriptions()
        {
            foreach (var component in subscribedTo)
                component.FocusRequested -= OnFocusRequested;
            subscribedTo.Clear();

            if (ScreenStack.CurrentScreen is null)
                return;

            SubscribeToComponents(ScreenStack.CurrentScreen);
        }

        private void OnFocusRequested(IInterfaceComponent<TInputAction> component) => FocusComponent(component);

        private void SubscribeToComponents(Drawable drawable)
        {
            if (drawable is Container container)
            {
                foreach (var child in container.Children.AsEnumerable().Reverse())
                    SubscribeToComponents(child);
            }

            if (drawable is IInterfaceComponent<TInputAction> component)
                component.FocusRequested += OnFocusRequested;
        }

        // TODO: remember the focused component for each screen,
        // and refocus that component when coming back to a screen.
        private void OnScreenStackPushOrPop(Screen screen) => Unfocus();

        private void FocusNextComponent(IInterfaceComponent<TInputAction> componentToFocus)
        {
            var restartCount = 0;
            while (true)
            {
                if (RestartStackIfNeeded())
                    restartCount++;
                if (restartCount == 2)
                    break;

                var foundNextFocusableComponent = HandleEnumerator();
                if (foundNextFocusableComponent is not null)
                {
                    if (componentToFocus is null || componentToFocus == foundNextFocusableComponent)
                    {
                        ChangeFocus(foundNextFocusableComponent);
                        return;
                    }
                }
            }

            if (componentToFocus is not null)
                throw new InvalidOperationException($"The requested {nameof(componentToFocus)} is not inside the {nameof(Drawable)} tree of the {nameof(ScreenStack)}.");

            ChangeFocus(null);
        }

        private void ChangeFocus(IInterfaceComponent<TInputAction> newlyFocusedComponent)
        {
            if (newlyFocusedComponent is null)
            {
                Unfocus();
                return;
            }

            var containerComponents = containersStack.Select(ce => ce.Container).OfType<IInterfaceComponent<TInputAction>>();

            var componentsToUnfocus = focusedComponents
                .Except(containerComponents.Append(newlyFocusedComponent))
                .ToList();

            var componentsToFocus = containerComponents
                .Append(newlyFocusedComponent)
                .Except(FocusedComponents)
                .ToList();

            foreach (var component in componentsToUnfocus)
                DoUnfocusTasks(component);

            foreach (var component in componentsToFocus)
                DoFocusTasks(component);

            foreach (var containerEnumerator in containersStack)
            {
                containerEnumerator.HadComponentChildren = true;
            }
        }

        private void DoUnfocusTasks(IInterfaceComponent<TInputAction> component)
        {
            focusedComponents.Remove(component);
            inputPropagator?.Listeners.Remove(component);
            component?.OnFocusLost();
        }

        private void DoFocusTasks(IInterfaceComponent<TInputAction> component)
        {
            focusedComponents.Add(component);
            inputPropagator?.Listeners.Add(component);
            component?.OnFocus();
        }

        /// <summary>
        ///     Advances the <see cref="IEnumerator{Drawable}"/>
        ///     at the top of the <see cref="containersStack"/> to the next <see cref="Drawable"/>.
        ///     If that's successful, calls and returns
        ///     the return value of <see cref="HandleDrawable(Drawable)"/>.
        ///     If not, pops the <see cref="containersStack"/>.
        ///     If the popped <see cref="Container"/> is
        ///     an <see cref="IInterfaceComponent{TInputAction}"/>
        ///     and had no descendants of that interface, it will get returned.
        ///     Otherwise, nothing gets returned.
        /// </summary>
        private IInterfaceComponent<TInputAction> HandleEnumerator()
        {
            var enumerator = containersStack.Peek().Enumerator;
            try
            {
                if (enumerator.MoveNext())
                    return HandleDrawable(enumerator.Current);
            }
            catch (InvalidOperationException)
            {
            }

            var containerEnumerator = containersStack.Pop();
            if (containerEnumerator.Container is IInterfaceComponent<TInputAction> component && !containerEnumerator.HadComponentChildren && component.AcceptsFocus)
                return component;

            return null;
        }

        private bool RestartStackIfNeeded()
        {
            if (containersStack.Count == 0)
            {
                containersStack.Push(new(ScreenStack.CurrentScreen));
                return true;
            }

            return false;
        }

        private IInterfaceComponent<TInputAction> HandleDrawable(Drawable drawable)
        {
            if (drawable is IContainer<Drawable> container)
            {
                // If this is a container that's also a disabled component,
                // it should not be considered at all in the interface tree.
                if (drawable is IInterfaceComponent<TInputAction> component && !component.AcceptsFocus)
                    return null;

                containersStack.Push(new(container));
            }
            else if (drawable is IInterfaceComponent<TInputAction> component && component.AcceptsFocus)
                return component;
            return null;
        }

        private class ContainerAndEnumerator
        {
            public IContainer<Drawable> Container;
            public IEnumerator<Drawable> Enumerator;
            public bool HadComponentChildren;

            public ContainerAndEnumerator(IContainer<Drawable> container)
            {
                Container = container;
                Enumerator = container.Children.GetEnumerator();
                HadComponentChildren = false;
            }
        }

        private class NextComponentActionListener : IInputListener<TInputAction>
        {
            private readonly InterfaceInputManager<TInputAction> interfaceInputManager;

            public NextComponentActionListener(InterfaceInputManager<TInputAction> interfaceInputManager) => this.interfaceInputManager = interfaceInputManager;

            public bool OnActionPressed(TInputAction action) => action.Equals(interfaceInputManager.NextComponentAction);

            public void OnActionReleased(TInputAction action) => interfaceInputManager.FocusNextComponent();
        }
    }
}